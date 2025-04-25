using Models.AppModels;
using UI.Repositories;

namespace UI.Pages.Sale
{
    public partial class CreateSaleOrder
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
        [Inject]
        public IOrderDetailRepoUI _OrderDetailRepoUI { get; set; }
        [Inject]
        public IProductDetailsRepoUI _ProductDetailsRepoUI { get; set; }
        #endregion

        AppUsers UserSession = new AppUsers();
        Order Model = new Order();

        Party Customer = new Party();
        Party Brand = new Party();

        List<Party> Brands = new List<Party>();
        List<Party> Customers = new List<Party>();

        OrderDetail SaleOrderDetail = new OrderDetail();
        List<PurchaseOrderDetail> SaleOrderDetailList = new List<PurchaseOrderDetail>();

        ProductDetails ProDetalil = new ProductDetails();
        List<ProductDetails> CompanyProducts = new List<ProductDetails>();

        private bool _processing = false, _productdetails = true, _Party;


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
                    Customers = await _partyRepoUI.GetAll("Party/GetPartiesByType?partyType=Customer") ?? new List<Party>();
                    Brands = await _partyRepoUI.GetAll("Party/GetPartiesByType?partyType=Vendor") ?? new List<Party>();
                    Model.TaxRate = 18;
                    Model.OrderDate = DateTime.Now;
                    Model.OrderNo = await CreateSONumberAsync();
                }
                _processing = false;
            }
            catch (Exception ex)
            {
                UILogger.WriteLog(ex);
            }

            return;
        }

        public async Task<string> CreateSONumberAsync()
        {
            var lastOrderNo = await _orderRepoUI.GetSingleByColumnAsync($"Order/GetOrderNumberByType?orderType={OrderTypes.SaleOrder}");

            if (!string.IsNullOrEmpty(lastOrderNo) && lastOrderNo.Contains("SO-"))
            {
                string[] parts = lastOrderNo.Split('-');

                if (parts.Length == 2 && int.TryParse(parts[1], out int number))
                {
                    number++;
                    string newNumber = number.ToString($"D{parts[1].Length}");

                    return $"{parts[0]}-{newNumber}";
                }
            }

            return "SO-0001"; // First order case
        }

        private async Task<IEnumerable<Party>> SearchCustomers(string value)
        {
            await Task.Delay(0);
            if (string.IsNullOrEmpty(value))
                return Customers;
            return Customers.Where(x => !string.IsNullOrEmpty(x.FocalPersonName) ? x.FocalPersonName.Contains(value, StringComparison.InvariantCultureIgnoreCase) : false);
        }

        Task OnCustomerChanged(Party Value)
        {
            try
            {
                if (Value != null)
                {
                    Customer = Value;
                    Model.PartyId = Value.Id;
                    Model.Party = Value;

                    //CompanyProducts = await _ProductDetailsRepoUI.GetAll($"Party/GetPartiesByType?partyId={party.Id}") ?? new List<ProductDetails>();
                    //if (CompanyProducts.Count > 0)
                    //{
                    //    _productdetails = false;
                    //}
                    //else
                    //{
                    //    _productdetails = true;
                    //    ProDetalil = new ProductDetails();
                    //}

                }
            }
            catch (Exception ex) { UILogger.WriteLog(ex); }

            return Task.CompletedTask;
        }

        private async Task<IEnumerable<Party>> SearchBrands(string value)
        {
            await Task.Delay(0);
            if (string.IsNullOrEmpty(value))
                return Brands;
            return Brands.Where(x => !string.IsNullOrEmpty(x.FocalPersonName) ? x.FocalPersonName.Contains(value, StringComparison.InvariantCultureIgnoreCase) : false);
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

        private async Task<IEnumerable<ProductDetails>> SearchProDetails(string value)
        {
            await Task.Delay(0);
            if (string.IsNullOrEmpty(value))
                return CompanyProducts;
            return CompanyProducts.Where(x => !string.IsNullOrEmpty(x.ItemName) ? x.ItemName.Contains(value, StringComparison.InvariantCultureIgnoreCase) : false);
        }

        void OnProductChanged(ProductDetails Value)
        {
            try
            {
                if (Value != null)
                {
                    ProDetalil = Value;
                    SaleOrderDetail.ItemId = Value.Id;
                    SaleOrderDetail.Item = Value;
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

                    SaleOrderDetail = new OrderDetail();
                    ProDetalil = new ProductDetails();
                    Brand = new Party();
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
            if (IsValidate())
            {
                Model.OType = OrderTypes.SaleOrder;
                Model.Status = OrderStatus.Opened;
                var res = Model.Id > 0 ? await _orderRepoUI.Create("Order/Update", Model) : await _orderRepoUI.Create("Order/Create", Model) ?? new Order();

                if (res != null && res.Id > 0)
                {
                    snackbar.Add("Sale Order has been saved successfully", Severity.Success);
                    Model = new Order();
                    navigation.NavigateTo("/SaleOrder");
                }
                else
                {
                    snackbar.Add("An error occurred while creating Sale Order", Severity.Error);
                }
            }
            else
            {
                snackbar.Add("Please fill all required field(s)", Severity.Error);
            }
        }



    }
}
