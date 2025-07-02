using Models.AppModels;
using UI.Repositories;
using static MudBlazor.Icons.Custom;

namespace UI.Pages.Production
{
    partial class CreateProductionOrder
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
         public IOrderRepoUI _orderRepoUI { get; set; }

        #endregion

        #region Variables

        Party party = new Party();
        Order Model = new Order();
        OrderDetail ProductionOrderDetail = new OrderDetail();
        ProductDetails ProdDetalil = new ProductDetails();
        List<ProductionOrderDetail> ProductionOrderDetailList = new List<ProductionOrderDetail>();
        List<ProductDetails> CompanyDetalilList = new List<ProductDetails>();
        Party Customer = new Party();
        Party Brand = new Party();
        List<Party> Brands = new List<Party>();
        List<Party> Customers = new List<Party>();
                
        List<ProductDetails> CompanyProducts = new List<ProductDetails>();


        int recivingQty;
        #endregion

        #region Functions

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
                    parties = await _partyRepoUI.GetAll("Party/GetPartiesByType?partyType=Contractor") ?? new List<Party>();

                    Brands = await _partyRepoUI.GetAll("Party/GetPartiesByType?partyType=vendor") ?? new List<Party>();



                    // ProDetalilList = await _ProductDetailsRepoUI.GetAll("ProductDetails/GetProductDetails") ?? new List<ProductDetails>();

                    Model.TaxRate = 18;
                    Model.OrderNo = await CreatePONumberAsync();
                }
                _processing = false;
            }
            catch (Exception ex)
            {
                UILogger.WriteLog(ex);
            }

            return;
        }

        public async Task<string> CreatePONumberAsync()
        {
            var lastOrderNo = await _orderRepoUI.GetSingleByColumnAsync($"Order/GetOrderNumberByType?orderType={OrderTypes.PurchaseOrder}");

            if (!string.IsNullOrEmpty(lastOrderNo) && lastOrderNo.Contains("PO-"))
            {
                string[] parts = lastOrderNo.Split('-');

                if (parts.Length == 2 && int.TryParse(parts[1], out int number))
                {
                    number++;
                    string newNumber = number.ToString($"D{parts[1].Length}");

                    return $"{parts[0]}-{newNumber}";
                }
            }

            return "PO-0001"; // First order case
        }

        private bool _processing = false, _productdetails = true, _Party;

        AppUsers UserSession = new AppUsers();

        List<Party> parties = new List<Party>();

        List<ProductDetails> ProDetalilList = new List<ProductDetails>();

        public IProductDetailsRepoUI _ProductDetailsRepoUI { get; set; }

        private async Task<IEnumerable<Party>> SearchVendor(string value)
        {
            await Task.Delay(0);
            if (string.IsNullOrEmpty(value))
                return parties;
            return parties.Where(x => !string.IsNullOrEmpty(x.FocalPersonName) ? x.FocalPersonName.Contains(value, StringComparison.InvariantCultureIgnoreCase) : false);
        }

        private async Task<IEnumerable<Party>> SearchBrands(string value)
        {
            await Task.Delay(0);
            if (string.IsNullOrEmpty(value))
                return Brands;
            return Brands.Where(x => !string.IsNullOrEmpty(x.FocalPersonName) ? x.FocalPersonName.Contains(value, StringComparison.InvariantCultureIgnoreCase) : false);
        }
        void OnPartyChanged(Party Value)
        {
            try
            {
                if (Value != null)
                {
                    party = Value;
                    Model.PartyId = Value.Id;
                    Model.Party = Value;
                    //CompanyDetalilList = ProDetalilList.Where(x => x.PartyId == Model.PartyId && x.Isprocessed == false).ToList();

                    //if (CompanyDetalilList.Count > 0)
                    //{
                    //    _productdetails = false;
                    //}
                    //else
                    //{
                    //    _productdetails = true;
                    //    ProDetalil = new ProductDetails();
                    //}

                    //_productdetails = CompanyDetalilList.Count > 0 ? false : true;

                }
            }
            catch (Exception ex) { UILogger.WriteLog(ex); }
        }

        #endregion

        bool IsValidate()
        {
            return
                string.IsNullOrEmpty(Model.OrderDate.ToString()) || string.IsNullOrEmpty(Model.OrderNo) ||
                Model.PartyId < 0 || Model.OrderDetail.Count < 0
                ? false : true;
        }

        async void Save()
        {
            try
            {
                _processing = true;
                StateHasChanged();

                if (IsValidate())
                {
                    Model.OType = OrderTypes.ProductionOrder;
                    Model.Status = OrderStatus.Opened;
                    var res = Model.Id > 0 ? await _orderRepoUI.Create("Order/Update", Model) : await _orderRepoUI.Create("Order/Create", Model) ?? new Order();

                    if (res != null && res.Id > 0)
                    {
                        snackbar.Add("Production Order has been saved successfully", Severity.Success);
                        Model = new Order();
                        navigation.NavigateTo("/ProductionOrder");
                    }
                    else
                    {
                        snackbar.Add("An error occurred while creating Production Order", Severity.Error);
                    }
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


        private async Task<IEnumerable<ProductDetails>> SearchProDetails(string value)
        {
            await Task.Delay(0);
            if (string.IsNullOrEmpty(value))
                return CompanyDetalilList;
            return CompanyDetalilList.Where(x => !string.IsNullOrEmpty(x.ItemName) ? x.ItemName.Contains(value, StringComparison.InvariantCultureIgnoreCase) : false);
        }

        void OnProductChanged(ProductDetails Value)
        {
            try
            {
                if (Value != null)
                {
                    ProdDetalil = Value;
                    ProductionOrderDetail.ItemId = Value.Id;
                    ProductionOrderDetail.Item = Value;
                }
            }
            catch (Exception ex) { UILogger.WriteLog(ex); }
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
                    ProdDetalil = new ProductDetails();
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
        void RemoveItem(OrderDetail POdetail)
        {
            Model.OrderDetail.Remove(POdetail);
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
            OrderDetail purchaseOrderDetail = new OrderDetail();
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
                        ProdDetalil = new ProductDetails();
                    }

                }
            }
            catch (Exception ex) { UILogger.WriteLog(ex); }
        }





    }
}
