using Models.AppModels;
using MudBlazor;
using System.Reflection;
using System.Transactions;
using UI.Repositories;

namespace UI.Pages.Production
{
    partial class ProductionIssuance
    {

        #region DI

        [Inject]
        public ISnackbar snackbar { get; set; }
        [Inject]
        public IJSRuntime _jS { get; set; }
        [Inject]
        public ProtectedLocalStorage _localStorage { get; set; }
        [Inject]
        public NavigationManager navigation { get; set; }
        [Inject]
        public IPartyRepoUI _partyRepoUI { get; set; }
        [Inject]
        public IPurchaseOrderRepoUI _ProductionOrderRepoUI { get; set; }
        [Inject]
        public IOrderRepoUI _orderRepoUI { get; set; }
        [Inject]
        public IProductDetailsRepoUI _ProductDetailsRepoUI { get; set; }
        [Inject]
        public IStockTransactionsRepoUI _stockTransactionsRepoUI { get; set; }

        #endregion

        #region Variables

        Order Model = new Order();
        Party party = new Party();
        List<Party> parties = new List<Party>();
        List<Party> Vanders = new List<Party>();

        List<Party> Brands = new List<Party>();

        Party Brand = new Party();


        List<ProductDetails> ProDetalilList = new List<ProductDetails>();
        List<ProductDetails> CompanyDetalilList = new List<ProductDetails>();
        OrderDetail ProductionOrderDetail = new OrderDetail();
        List<ProductionOrderDetail> ProductionOrderDetailList = new List<ProductionOrderDetail>();
        AppUsers UserSession = new AppUsers();
        private bool _saving = false;
        OrderTransactions transactions = new OrderTransactions();

        ProductDetails ProDetalil = new ProductDetails();
        List<ProductDetails> CompanyProducts = new List<ProductDetails>();

        private bool _processing = false, _productdetails = true, _Party;

		int liveStock = 0;

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
                        navigation.NavigateTo("/signin");
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

                if (UserSession.Id > 0)
                {
					var uri = new Uri(navigation.Uri);
					var query = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(uri.Query);

					if (query.TryGetValue("data", out var jsonData))
					{
						var decoded = Uri.UnescapeDataString(jsonData);
						party = JsonSerializer.Deserialize<Party>(decoded);
					}



					Brands = await _partyRepoUI.GetAll("Party/GetPartiesByType?partyType=Vendor") ?? new List<Party>();
                }
                _processing = false;
            }
            catch (Exception ex)
            {
                UILogger.WriteLog(ex);
            }

            return;
        }

        private async Task<IEnumerable<ProductDetails>> SearchProDetails(string value)
        {
            await Task.Delay(0);
            if (string.IsNullOrEmpty(value))
                return CompanyProducts;
            return CompanyProducts.Where(x => !string.IsNullOrEmpty(x.ItemName) ? x.ItemName.Contains(value, StringComparison.InvariantCultureIgnoreCase) : false);
        }
        async Task OnBrandsChangedAsync(Party Value)
        {
            try
            {
                if (Value != null)
                {
                    Brand = Value;

                    CompanyProducts = await _ProductDetailsRepoUI.GetAll($"ProductDetails/GetProductDetailsByParty?partyId={Brand.Id}") ?? new List<ProductDetails>();
                    if (CompanyProducts.Count > 0)
                    {
                        _productdetails = false;
                    }
                    else
                    {
                        _productdetails = true;
                        ProDetalil = new ProductDetails();
                    }

                }
            }
            catch (Exception ex) { UILogger.WriteLog(ex); }
        }

        async void OnProductChanged(ProductDetails Value)
        {
            try
            {
                if (Value != null)
                {
                    ProDetalil = Value;
                    ProductionOrderDetail.ItemId = Value.Id;
                    ProductionOrderDetail.Item = Value;

					var liveStockResponse = await _partyRepoUI.GetSingleByColumnAsync($"StockTransactions/GetLiveStockByItem?ItemId={ProductionOrderDetail.ItemId}");
					int.TryParse(liveStockResponse, out liveStock);

                    ProductionOrderDetail.Warehouses = await _partyRepoUI.GetStringList($"Order/GetWarehousesByItem?ItemId={ProductionOrderDetail.ItemId}")
                                      ?? new List<string>();

                    StateHasChanged();
				}
            }
            catch (Exception ex) { UILogger.WriteLog(ex); }
        }

        private async Task<IEnumerable<Party>> SearchBrands(string value)
        {
            await Task.Delay(0);
            if (string.IsNullOrEmpty(value))
                return Brands;
            return Brands.Where(x => !string.IsNullOrEmpty(x.FocalPersonName) ? x.FocalPersonName.Contains(value, StringComparison.InvariantCultureIgnoreCase) : false);
        }
        void AddItem(OrderDetail POdetail)
        {
            if (_productdetails == false)
            {
                if (POdetail.Qty > 0 /*&& POdetail.Rate > 0*/)
                {
                    var existingItem = Model.OrderDetail.FirstOrDefault(x => x.ItemId == POdetail.ItemId);

                    if (existingItem != null)
                    {
                        // Update the quantity
                        existingItem.Qty += POdetail.Qty;

                        // Update the rate if it is different
                        if (existingItem.Rate != POdetail.Rate)
                        {
                            existingItem.Rate = POdetail.Rate;
                        }
                    }
                    else
                    {
                        // Add new item if it does not exist
                        Model.OrderDetail.Add(POdetail);
                    }

                    ProductionOrderDetail = new OrderDetail();
                    ProDetalil = new ProductDetails();
                    CalculateTotal();

                    StateHasChanged();

                    if (Model.OrderDetail.Count > 0)
                    {
                        _Party = true;
                    }
                }
                else
                {
                    snackbar.Add("Kindly fill all Required Data", Severity.Warning);
                }
            }
            else
            {
                snackbar.Add("Please Select Vendor first", Severity.Warning);
            }
        }
        void CalculateTotal()
        {
            if (Model.OrderDetail.Count > 0)
            {
                Model.GrossAmount = Model.OrderDetail.Sum(x => x.Rate * x.Qty);
                Model.NetAmount = Model.GrossAmount - Model.TaxAmount;
            }
            if (Model.TaxRate > 0 && Model.GrossAmount > 0)
            {
                Model.TaxAmount = Model.GrossAmount * Model.TaxRate / 100;
                Model.NetAmount = Model.GrossAmount + Model.TaxAmount;
            }

        }
        bool IsValidate()
        {
            return
                string.IsNullOrEmpty(Model.OrderDate.ToString()) || string.IsNullOrEmpty(Model.OrderNo) ||
                Model.PartyId < 0 || string.IsNullOrEmpty(Model.PaymentMode) ||
                Model.OrderDetail.Count < 0
                ? false : true;
        }

        async void Save()
        {
            try
            {
                _processing = true;
                StateHasChanged();

                if (/*IsValidate()*/ true)
                {
                    Model.PartyId = party.Id;
                    Model.OType = OrderTypes.ProductionOrder;
                    Model.Status = OrderStatus.Opened;
                    var res = Model.Id > 0 ? await _orderRepoUI.Create("Order/Update", Model) : await _orderRepoUI.Create("Order/Create", Model) ?? new Order();

                    var stockTransactionsList = new List<StockTransactions>();

                    foreach (var prd in Model.OrderDetail.DistinctBy(x => x.ItemId))
                    {
                        stockTransactionsList.Add(new StockTransactions
                        {
                            StockOut = prd.Qty,
                            StockType = StockTransectionTypes.ProdIssuance,
                            ItemId = Convert.ToInt32(prd.ItemId),
                            Warehouse = prd.Warehouse,
                            ReferenceNumber = Model.OrderNo,
                        });
                    }

                    await _stockTransactionsRepoUI.BulkInsert("StockTransactions/BulkInsert", stockTransactionsList);

                    snackbar.Add("Saved successfully", Severity.Success);
                    navigation.NavigateTo("/prdiss", forceLoad: true);

                    //if (res.Id > 0)
                    //{
                    //    transactions.TType = TransectionTypes.GoodsIssuance;
                    //    transactions.TransectionDate = DateTime.Now;
                    //    transactions.POQty = Model.OrderDetail.Sum(x => x.Qty);
                    //    transactions.ReciverParty = Model.WalkinCustomer;
                    //    transactions.RecivingLocation = "Walk-In Customer";
                    //    transactions.IsDirectDelivery = true;

                        //    if (res.Id > 0)
                        //    {
                        //        var resTrns = await _transactionsRepoUI.Create("OrderTransactions/Create", transactions) ?? new OrderTransactions();

                        //        if (resTrns.Id > 0)
                        //        {
                        //            snackbar.Add("Sale Order has been saved successfully", Severity.Success);
                        //            Model = new Order();
                        //            navigation.NavigateTo("/SaleOrder");
                        //        }
                        //    }
                        //}
                }
                else
                {
                    snackbar.Add("Please fill all required field(s)", Severity.Error);
                }
            }

            catch (Exception ex)
            {
                snackbar.Add($"Error: {ex.Message}", Severity.Error);
            }
            finally
            {
                _processing = false;
                StateHasChanged();
            }
        }
    }
}
