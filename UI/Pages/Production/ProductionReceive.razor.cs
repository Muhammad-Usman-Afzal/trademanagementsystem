using Models.AppModels;
using MudBlazor;
using System.Reflection;
using System.Transactions;
using UI.Repositories;
using static MudBlazor.Icons.Custom;

namespace UI.Pages.Production
{
    partial class ProductionReceive
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
        List<PrdRecItems> RecItems = new List<PrdRecItems>();

        private bool _processing = false, _productdetails = true, _Party;

        [Parameter]
        public int ContractorId { get; set; }

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
					Model = await _orderRepoUI.GetSingleByCondition($"Order/GetOrdersByContractor?ContractorId={ContractorId}") ?? new Order();

					RecItems = Model.OrderDetail.Select(od => new PrdRecItems
					{
						OrderId = 0,
						ItemId = od.ItemId,
						Item = od.Item,
						RecQty = 0,
						Warehouse = od.Warehouse,
						Weight = 0
					}).ToList();

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

        void OnProductChanged(ProductDetails Value)
        {
            try
            {
                if (Value != null)
                {
                    ProDetalil = Value;
                    ProductionOrderDetail.ItemId = Value.Id;
                    ProductionOrderDetail.Item = Value;
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
                if (POdetail.Qty > 0 && POdetail.Rate > 0 && !String.IsNullOrEmpty(POdetail.Unit))
                {
                    var existingItem = Model.OrderDetail.FirstOrDefault(x => x.ItemId == POdetail.ItemId);

                    if (existingItem != null && existingItem.Unit == POdetail.Unit)
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
                    var stockTransactionsList = new List<StockTransactions>();

                    foreach (var prd in RecItems.DistinctBy(x => x.ItemId))
                    {
                        stockTransactionsList.Add(new StockTransactions
                        {
                            StockIn = prd.RecQty,
                            StockType = StockTransectionTypes.ProdReceipt,
                            ItemId = Convert.ToInt32(prd.ItemId),
                            Warehouse = prd.Warehouse
                        });
                    }

                    await _stockTransactionsRepoUI.BulkInsert("StockTransactions/BulkInsert", stockTransactionsList);

                    snackbar.Add("Saved successfully", Severity.Success);
                    navigation.NavigateTo("/saleorder", forceLoad: true);
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

        void CalcWeight(PrdRecItems recItems)
        {
            var itemToUpdate = RecItems.FirstOrDefault(x => x.Id == recItems.Id);

            if (itemToUpdate != null)
            {
                itemToUpdate.PerBagQty = Convert.ToInt32(recItems.Weight / recItems.Item.StandardWeight);
            }
        }

    }
}
