using Models;
using Models.AppModels;
using MudBlazor;
using UI.Repositories;

namespace UI.Pages.Purchase
{
    public partial class PurchaseOrderReceive
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
        public int PONo { get; set; }
        private int _PONo, TotalReciving, RemaningQty;
        private bool _processing = false, DisableContolle = false;
        #endregion

        #region Objects
        AppUsers UserSession;
        Party party = new Party();
        Order PO = new Order();
        OrderDetail Model = new OrderDetail();
        OrderTransactions transactions = new OrderTransactions();
        StockTransactions stockTransactions = new StockTransactions();

        #endregion

        #region Lists
        List<Party> parties = new List<Party>();
        List<OrderDetail> PODetail = new List<OrderDetail>();
        List<OrderTransactions> POItemsReceive = new List<OrderTransactions>();
        List<OrderDetail> ModelList = new List<OrderDetail>();
        List<OrderTransactions> POtransectionList = new List<OrderTransactions>();
        List<StockTransactions> stockTransactionsList = new List<StockTransactions>();
        #endregion

        protected override Task OnParametersSetAsync()
        {
            try
            {
                _PONo = PONo;

                return base.OnParametersSetAsync();
            }
            catch (Exception ex)
            {
                UILogger.WriteLog(ex);
                return base.OnParametersSetAsync();
            }
        }

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
                    PO = await _OrderRepoUI.GetSingleByCondition($"Order/GetOrderById?id={PONo}") ?? new Order();
                    parties = await _partyRepoUI.GetAll("Party/GetParties") ?? new List<Party>(); // ye api theek krna hai

                    parties = parties.Where(x => x.PartyType == "Customer").ToList(); // ye bhi theek krna hai

                    POItemsReceive = PO.OrderDetail.Select(ot => new OrderTransactions
                    {
                        ItemId = ot.ItemId,
                        ItemName = ot.Item?.ItemName,
                        TType = TransectionTypes.PurchaseReceive,
                        TransectionDate = DateTime.Now,
                        POQty = ot.Qty,
                        TotalRecQty = PO.OT?.Where(x => x.ItemId == ot.ItemId)?.Sum(s => s.RecQty) ?? 0
                    }).ToList();

                    //foreach (var item in PO.OT)
                    //{
                    //    //POItemsReceive.Where(x => x.ItemId == item.ItemId).ToList().ForEach(x => x.Warehouse = item.Warehouse);
                    //    POItemsReceive.Where(x => x.ItemId == item.ItemId).ToList().ForEach(x => x.TotalRecQty = item.TotalRecQty);
                    //}

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
                return PO.OrderDetail;
            return PO.OrderDetail.Where(x => x.Item.ItemName.Contains(value, StringComparison.InvariantCultureIgnoreCase));
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
                transactions.TType = TransectionTypes.PurchaseReceive;

                // Check if the item already exists in PODetail
                var existingItem = PODetail.FirstOrDefault(x => x.ItemId == Model.ItemId);

                if (existingItem != null)
                {
                    // If item exists, update its transaction details
                    var existingTransaction = existingItem.OT.FirstOrDefault(t => t.TType == TransectionTypes.PurchaseReceive);

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
                            ReciverParty = transactions.ReciverParty,
                            RecivingLocation = transactions.RecivingLocation
                            // Add other required properties
                        });
                    }
                }
                else
                {
                    // If item doesn't exist, create a new record in PODetail
                    Model.OT = new List<OrderTransactions>
                            {
                                new OrderTransactions
                                    {
                                        POQty = transactions.POQty,
                                        TType = transactions.TType,
                                        ReciverParty = transactions.ReciverParty,
                                        RecivingLocation = transactions.RecivingLocation
                                        // Add other required properties
                                    }
                            };

                    PODetail.Add(Model);
                }

                // Reset transaction object
                transactions = new OrderTransactions();
                Model = new OrderDetail();
                TotalReciving = new int();
                RemaningQty = new int();
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
                bool validate = false;
                DisableContolle = true;
                await InvokeAsync(StateHasChanged);
                await Task.Delay(1);

                foreach (var a in POItemsReceive)
                {
                    if (!string.IsNullOrEmpty(a.Warehouse)) 
                    {
                        validate = true;
                    }
                }

                if (validate)
                {
                    foreach (var item in POItemsReceive)
                    {
                        item.TotalRecQty += item.RecQty;
                    }

                    PO.OT = POItemsReceive;

                    await _OrderRepoUI.Update("Order/Update", PO);
                    //await _OrderDetailRepoUI.UpdateDetail("OrderDetail/BulkUpdate", PODetail);

                    var stockTransactionsList = new List<StockTransactions>();

                    foreach (var po in POItemsReceive.DistinctBy(x => x.ItemId))
                    {
                        stockTransactionsList.Add(new StockTransactions
                        {
                            StockIn = po.RecQty,
                            StockType = StockTransectionTypes.Purchase,
                            ItemId = Convert.ToInt32(po.ItemId),
                            Warehouse = po.Warehouse,
                            ReferenceNumber = PO.OrderNo,
                        });
                    }

                    await _stockTransactionsRepoUI.BulkInsert("StockTransactions/BulkInsert", stockTransactionsList);

                    _Snackbar.Add("Saved successfully", Severity.Success);
                    Navigate.NavigateTo("/PurchaseOrder", forceLoad: true);
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

        //private async Task<bool> InsurtValidateAsync()
        //{
        //    if (Model.Id > 0 && (!String.IsNullOrEmpty(Model.NTN) || !String.IsNullOrEmpty(Model.STRNo)) && Model.IsRegisterd == false)
        //    {
        //        showRegConfirmDialog = true;
        //        bool confirm = await ShowConfirmationDialog();
        //        if (!confirm)
        //        {
        //            return false;
        //        }
        //    }
        //    if (Model.IsRegisterd)
        //    {
        //        if (isContactInvalid || isEmailInvalid || (isNTNInvalid && isSTRInvalid) || isCompanyContactInvalid || isCompanyEmailInvalid || String.IsNullOrEmpty(Model.PartyType)
        //            || String.IsNullOrEmpty(Model.CompanyName) || String.IsNullOrEmpty(Model.CompanyAddress) || String.IsNullOrEmpty(Model.FocalPersonName))
        //        {
        //            return false;
        //        }
        //        else
        //        {
        //            return true;
        //        }

        //    }
        //    else
        //    {
        //        Model.NTN = null;
        //        Model.STRNo = null;
        //        if (isContactInvalid || isEmailInvalid || isCompanyContactInvalid || isCompanyEmailInvalid || String.IsNullOrEmpty(Model.PartyType)
        //            || String.IsNullOrEmpty(Model.CompanyName) || String.IsNullOrEmpty(Model.CompanyAddress) || String.IsNullOrEmpty(Model.FocalPersonName))
        //        {
        //            return false;
        //        }
        //        else
        //        {
        //            return true;
        //        }
        //    }
        //    return true;
        //}

        void RemoveItem(OrderDetail pOdetail)
        {
            PODetail.Remove(pOdetail);
        }
    }
}
