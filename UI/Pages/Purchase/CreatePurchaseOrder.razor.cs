using Microsoft.AspNetCore.Components;
using Models;
using Models.AppModels;
using MudBlazor;
using System.Reflection.PortableExecutable;

namespace UI.Pages.Purchase
{
    public partial class CreatePurchaseOrder
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
        public IPurchaseOrderRepoUI _purchaseOrderRepoUI { get; set; }

        #endregion

        #region Variables

        PurchaseOrder Model = new PurchaseOrder();
        Party party = new Party();
        List<Party> parties = new List<Party>();
        PurchaseOrderDetail purchaseOrderDetail = new PurchaseOrderDetail();
        List<PurchaseOrderDetail> purchaseOrderDetailList = new List<PurchaseOrderDetail>();
        AppUsers UserSession = new AppUsers();

        private bool _processing = false;

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

                    if (/*UserSession.Id == 0*/ false)
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

                if (/*UserSession.Id > 0*/ true)
                {
                    parties = await _partyRepoUI.GetAll("Party/GetParties") ?? new List<Party>();
                }

                _processing = false;
            }
            catch (Exception ex)
            {
                UILogger.WriteLog(ex);
            }

            return;
        }

        async void Save()
        {
            if (IsValidate())
            {
                var res = Model.Id > 0 ? await _purchaseOrderRepoUI.Create("PurchaseOrder/Update", Model) : await _purchaseOrderRepoUI.Create("PurchaseOrder/Create", Model) ?? new PurchaseOrder();

                if (res != null && res.Id > 0)
                {
                    snackbar.Add("Purchase Order has been saved successfully", Severity.Success);
                    Model = new PurchaseOrder();
                    navigation.NavigateTo("/CreatePurchaseOrder", forceLoad: true);
                }
                else
                {
                    snackbar.Add("An error occurred while creating Purchase Order", Severity.Error);
                }
            }
            else
            {
                snackbar.Add("Please fill all required field(s)", Severity.Error);
            }
        }

        bool IsValidate()
        {
            return
                string.IsNullOrEmpty(Model.PODate.ToString()) ||
                string.IsNullOrEmpty(Model.VendorName) ||
                string.IsNullOrEmpty(Model.DeliveryFrom) ||
                string.IsNullOrEmpty(Model.PaymentMode)
                ? false : true;
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
                    Model.VendorName = Value.FocalPersonName;
                    Model.DeliveryFrom = Value.CompanyAddress;
                }
            }
            catch (Exception ex) { UILogger.WriteLog(ex); }
        }

        void AddItem(PurchaseOrderDetail POdetail)
        {
            Model.purchaseOrderDetail.Add(POdetail);
            purchaseOrderDetail = new PurchaseOrderDetail();

            CalculateTotal();
        }

        void CalculateTotal()
        {
            if (Model.purchaseOrderDetail.Count > 0)
            {
                Model.GrossAmount = Model.purchaseOrderDetail.Sum(x => x.Rate * x.Qty);
                Model.NetAmount = Model.GrossAmount - Model.TaxAmount;
            }
            if (Model.TaxRate > 0 && Model.GrossAmount > 0)
            {
                Model.TaxAmount = Model.GrossAmount * Model.TaxRate / 100;
                Model.NetAmount = Model.GrossAmount - Model.TaxAmount;
            }
        }

        #endregion
    }
}
