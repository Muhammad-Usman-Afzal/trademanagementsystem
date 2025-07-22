namespace UI.Pages.Sale;

public partial class SaleOrderExecution
{
    [Inject]
    ISnackbar _Snackbar { get; set; }

    [Inject]
    IPartyRepoUI _partyRepoUI { get; set; }

    [Inject]
    NavigationManager Navigate { get; set; }
    [Inject]
    IOrderRepoUI _OrderRepoUI { get; set; }
    [Inject]
    IOrderDetailRepoUI _OrderDetailRepoUI { get; set; }
    [Inject]
    IOrderTransactionsRepoUI _orderTransactionsRepoUI { get; set; }
    [Inject]
    public ProtectedLocalStorage _localStorage { get; set; }
    [Inject]
   

    IStockTransactionsRepoUI _stockTransactionsRepoUI { get; set; }
   


    #region Variables
    [Parameter]
    public int SONo { get; set; }
    private int _PONo, TotalReciving, RemaningQty;
    private bool _processing = false, DisableContolle = false;
    


    #endregion

    #region Objects
    AppUsers UserSession;
    Party party = new Party();
    Order SaleOrder = new Order();
    OrderDetail Model = new OrderDetail();
    OrderTransactions transactions = new OrderTransactions();
    StockTransactions stockTransactions = new StockTransactions();
    

    #endregion

    #region Lists
    List<Order> SOList = new List<Order>();


    List<Party> parties = new List<Party>();
    List<OrderDetail> SODetail = new List<OrderDetail>();
    List<OrderTransactions> SaleItems = new List<OrderTransactions>();

    List<OrderDetail> ModelList = new List<OrderDetail>();
    List<OrderTransactions> POtransectionList = new List<OrderTransactions>();
    List<StockTransactions> stockTransactionsList = new List<StockTransactions>();
    
    List<string> Warehouses = new List<string>();

    #endregion


    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        try
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                var userSession = await _localStorage.GetAsync<AppUsers>("User");
                UserSession = userSession.Value ?? new AppUsers();

                if (UserSession.Id == 0)
                {
                    Navigate.NavigateTo("/signin");
                }
                else
                {
                    await OnInitializedAsync();
                    StateHasChanged();
                }
            }
        }
        catch (Exception ex) { UILogger.WriteLog(ex); }
    }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            _processing = true;
            _ = InvokeAsync(StateHasChanged);
            if (UserSession != null)
            {
                SaleOrder = await _OrderRepoUI.GetSingleByCondition($"Order/GetOrderById?id={SONo}") ?? new Order();
                SOList = await _OrderRepoUI.GetAll($"Order/GetOrdersByType?orderType={OrderTypes.SaleOrder}") ?? new List<Order>();
                parties = await _partyRepoUI.GetAll("Party/GetParties") ?? new List<Party>();

                parties = parties.Where(x => x.PartyType == "Customer").ToList();

                //SaleItems = (await Task.WhenAll(SaleOrder.OrderDetail.Select(async ot => new OrderTransactions
                //{
                //    ItemId = ot.ItemId,
                //    ItemName = ot.Item?.ItemName,
                //    TType = TransectionTypes.Dispatch,
                //    TransectionDate = DateTime.Now,
                //    SOQty = ot.Qty,
                //    TotalSoldQty = SaleOrder.OT?.Where(x => x.ItemId == ot.ItemId)?.Sum(s => s.SaleQty) ?? 0,
                //    LiveStock = Convert.ToInt32(await _partyRepoUI.GetSingleByColumnAsync($"Order/GetLiveStockByItem?ItemId={ot.ItemId}")),
                //    Warehouses = await _partyRepoUI.GetStringList($"Order/GetWarehousesByItem?ItemId={ot.ItemId}") ?? new List<string>()
                //}))).ToList();

                var saleItemsTasks = SaleOrder.OrderDetail.Select(async ot =>
                {
                    var liveStockResponse = await _partyRepoUI.GetSingleByColumnAsync($"StockTransactions/GetLiveStockByItem?ItemId={ot.ItemId}");
                    int liveStock = 0;
                    int.TryParse(liveStockResponse, out liveStock);

                    var warehouses = await _partyRepoUI.GetStringList($"Order/GetWarehousesByItem?ItemId={ot.ItemId}")
                                      ?? new List<string>();

                    return new OrderTransactions
                    {
                        ItemId = ot.ItemId,
                        ItemName = ot.Item?.ItemName,
                        TType = TransectionTypes.Dispatch,
                        TransectionDate = DateTime.Now,
                        SOQty = ot.Qty,
                        TotalSoldQty = SaleOrder.OT?.Where(x => x.ItemId == ot.ItemId)?.Sum(s => s.SaleQty) ?? 0,
                        LiveStock = liveStock,
                        Warehouses = warehouses
                    };
                });

                SaleItems = (await Task.WhenAll(saleItemsTasks)).ToList();
            }
            _processing = false;
        }
        catch (Exception ex)
        {
            UILogger.WriteLog(ex);
        }
        return;
    }

    void onItemSelect(OrderDetail value)
    {
        try
        {
            if (value != null)
            {
                Model = value;
                TotalReciving = Model.OT.Sum(odt => odt.POQty);
                RemaningQty = Model.Qty - TotalReciving;
                //FarwordManagerId = value.Id;
                //complainTrack.DepartmentManagerId = departmang.Id;
            }
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    private async Task<IEnumerable<OrderDetail>> SearchItem(string value)
    {
        await Task.Delay(0);
        if (string.IsNullOrEmpty(value))
            return SaleOrder.OrderDetail;
        return SaleOrder.OrderDetail.Where(x => x.Item.ItemName.Contains(value, StringComparison.InvariantCultureIgnoreCase));
    }

    private async Task<IEnumerable<Party>> SearchVendor(string value)
    {
        await Task.Delay(0);
        if (string.IsNullOrEmpty(value))
            return parties;
        return parties.Where(x => !string.IsNullOrEmpty(x.FocalPersonName) ? x.FocalPersonName.Contains(value, StringComparison.InvariantCultureIgnoreCase) : false);
    }

    void OnPartyChanged(Party Value)
    {
        try
        {
            if (Value != null)
            {
                party = Value;
                transactions.ReciverParty = Value.CompanyName;
                transactions.RecivingLocation = Value.CompanyAddress;
            }
        }
        catch (Exception ex) { UILogger.WriteLog(ex); }
    }

    void AddItem()
    {
        if (transactions.POQty > 0)
        {
            if (TotalReciving < Model.Qty && (TotalReciving + transactions.POQty) <= Model.Qty)
            {
                transactions.TType = TransectionTypes.Dispatch;

                // Check if the item already exists in SODetail
                var existingItem = SODetail.FirstOrDefault(x => x.ItemId == Model.ItemId);

                if (existingItem != null)
                {
                    // If item exists, update its transaction details
                    var existingTransaction = existingItem.OT.FirstOrDefault(t => t.TType == TransectionTypes.Dispatch);

                    if (existingTransaction != null)
                    {
                        existingTransaction.POQty += transactions.POQty;
                    }
                    else
                    {
                        // If no transaction exists, add new transaction for this item
                        existingItem.OT.Add(new OrderTransactions
                        {
                            POQty = transactions.POQty,
                            TType = transactions.TType,
                            ReciverParty = SaleOrder.Party.CompanyName,
                            CreateBy = UserSession.Id,
                            CreateOn = DateTime.Now
                            // Add other required properties
                        });
                    }
                }
                else
                {
                    // If item doesn't exist, create a new record in PODetail
                    //Model.OT = new List<OrderTransactions>
                    //        {
                    //            new OrderTransactions
                    //                {
                    //                    Qty = transactions.Qty,
                    //                    TType = transactions.TType,
                    //                    ReciverParty = SaleOrder.Party.CompanyName,
                    //            RecivingLocation = SaleOrder.Party.CompanyAddress,
                    //            CreateBy = UserSession.Id,
                    //            CreateOn = DateTime.Now
                    //                    // Add other required properties
                    //                }
                    //        };

                    OrderTransactions obj = new OrderTransactions();

                    obj.POQty = transactions.POQty;
                    obj.TType = transactions.TType;
                    obj.ReciverParty = SaleOrder.Party.CompanyName;
                    obj.CreateBy = UserSession.Id;
                    obj.CreateOn = DateTime.Now;

                    Model.OT.Add(obj);

                    SODetail.Add(Model);
                }

                // Reset transaction object
                transactions = new OrderTransactions();
                Model = new OrderDetail();
                TotalReciving = new int();

            RemaningQty = new int();
            }
            else
            {
                _Snackbar.Add("Dispatch Qty must be less then or equal to the Total Sale order Qty", Severity.Warning);

            }
        }
        else
        {
            _Snackbar.Add("Kindly fill all Required Data", Severity.Warning);
        }
    }

    async Task SaveAsync()
    {
        try
        {
            bool res;
            DisableContolle = true;
            await InvokeAsync(StateHasChanged);
            await Task.Delay(1);

            if (await InsurtValidateAsync())
            {
                foreach (var item in SaleItems)
                {
                    item.TotalSoldQty += item.SaleQty;
                }

                SaleOrder.OT = SaleItems;

                await _OrderRepoUI.Update("Order/Update", SaleOrder);
                //await _OrderDetailRepoUI.UpdateDetail("OrderDetail/BulkUpdate", SODetail);
                //await _stockTransactionsRepoUI.BulkInsert("StockTransactions/BulkInsert", stockTransactionsList);

                var stockTransactionsList = new List<StockTransactions>();

                foreach (var so in SaleItems.DistinctBy(x => x.ItemId))
                {
                    stockTransactionsList.Add(new StockTransactions
                    {
                        StockOut = so.SaleQty,
                        StockType = StockTransectionTypes.Sale,
                        ItemId = Convert.ToInt32(so.ItemId),
                        Warehouse = so.Warehouse,
                        ReferenceNumber = SaleOrder.OrderNo,
                    });
                }

                await _stockTransactionsRepoUI.BulkInsert("StockTransactions/BulkInsert", stockTransactionsList);

                _Snackbar.Add("Saved successfully", Severity.Success);
                Navigate.NavigateTo("/saleorder", forceLoad: true);
            }
            else
            {
                _Snackbar.Add("Please fill all fields properly.", Severity.Warning);
            }
            DisableContolle = false;
            StateHasChanged();
        }
        catch (Exception ex)
        {
            _Snackbar.Add($"{ex}", Severity.Error);
            _Snackbar.Add("There is some thing Went worng, Kindly contact to support ", Severity.Error);
            throw;
        }
    }

    private async Task<bool> InsurtValidateAsync()
    {
        //if (Model.Id > 0 && (!String.IsNullOrEmpty(Model.NTN) || !String.IsNullOrEmpty(Model.STRNo)) && Model.IsRegisterd == false)
        //{
        //    showRegConfirmDialog = true;
        //    bool confirm = await ShowConfirmationDialog();
        //    if (!confirm)
        //    {
        //        return false;
        //    }
        //}
        //if (Model.IsRegisterd)
        //{
        //    if (isContactInvalid || isEmailInvalid || (isNTNInvalid && isSTRInvalid) || isCompanyContactInvalid || isCompanyEmailInvalid || String.IsNullOrEmpty(Model.PartyType)
        //        || String.IsNullOrEmpty(Model.CompanyName) || String.IsNullOrEmpty(Model.CompanyAddress) || String.IsNullOrEmpty(Model.FocalPersonName))
        //    {
        //        return false;
        //    }
        //    else
        //    {
        //        return true;
        //    }

        //}
        //else
        //{
        //    Model.NTN = null;
        //    Model.STRNo = null;
        //    if (isContactInvalid || isEmailInvalid || isCompanyContactInvalid || isCompanyEmailInvalid || String.IsNullOrEmpty(Model.PartyType)
        //        || String.IsNullOrEmpty(Model.CompanyName) || String.IsNullOrEmpty(Model.CompanyAddress) || String.IsNullOrEmpty(Model.FocalPersonName))
        //    {
        //        return false;
        //    }
        //    else
        //    {
        //        return true;
        //    }
        //}
        return true;
    }

    private void OnSaleOrderSelected(Order selectedOrder)
    {
        SaleOrder = selectedOrder;
    }

    Func<Order, string> convertSO = p => p?.OrderNo;

    private async Task<IEnumerable<Order>> SearchSO(string value)
    {
        await Task.Delay(0);
        if (string.IsNullOrEmpty(value))
            return SOList;
        return SOList.Where(x => !string.IsNullOrEmpty(x.OrderNo) ? x.OrderNo.Contains(value, StringComparison.InvariantCultureIgnoreCase) : false);
    }

    void RemoveItem(OrderDetail POdetail)
    {
        SODetail.Remove(POdetail);

        TotalReciving = Model.OT.Sum(odt => odt.POQty);
        RemaningQty = Model.Qty - TotalReciving;
    }

    Func<OrderDetail, object> _groupBy = x =>
    {
        return x.Item.ItemName;
    };
}
