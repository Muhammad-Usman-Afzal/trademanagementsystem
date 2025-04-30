using UI.Repositories;

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
        OrderDetail purchaseOrderDetail = new OrderDetail();

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
                    parties = await _partyRepoUI.GetAll("Party/GetPartiesByType?partyType=Vendor") ?? new List<Party>();

                    //parties = parties.Where(x => x.PartyType == "Vendor").ToList();

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













    }
}
