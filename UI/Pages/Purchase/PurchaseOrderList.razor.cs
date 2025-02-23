using Models;
using UI.Repositories;

namespace UI.Pages.Purchase
{
    public partial class PurchaseOrderList
    {
        [Inject]
        ISnackbar _Snackbar { get; set; }

        [Inject]
        IPurchaseOrderRepoUI _purchaseOrderRepoUI { get; set; }
        [Inject]
        NavigationManager Navigate { get; set; }

        #region Variables
        private bool _processing = false, CreatePODiloag = false;
        #endregion


        #region List
        List<PurchaseOrder> POList = new List<PurchaseOrder>();
        List<PurchaseOrderDetail> PODetailList = new List<PurchaseOrderDetail>();
        #endregion


        #region Object
        AppUsers UserSession;
        PurchaseOrder Model = new PurchaseOrder();
        #endregion

        private DialogOptions dialogOptions = new() { FullScreen = true, CloseButton = true, NoHeader = true, DisableBackdropClick = true, CloseOnEscapeKey = false };

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            try
            {
                await base.OnAfterRenderAsync(firstRender);

                if (firstRender)
                {
                    //UserSession = await _localStorage.GetItemAsync<UserDetails>("User");
                    //if (UserSession == null)
                    //{
                    //    navigation.NavigateTo("/signin");
                    //}
                    //else
                    //{
                    //    await OnInitializedAsync();
                    //    StateHasChanged();
                    //}
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
                POList = await _purchaseOrderRepoUI.GetAll("PurchaseOrder/GetPurchaseOrders") ?? new List<PurchaseOrder>();

                if (UserSession != null)
                {

                }
                _processing = false;
            }
            catch (Exception ex)
            {
                UILogger.WriteLog(ex);
            }
            return;
        }

        void CreatePO()
        {
            Navigate.NavigateTo("CreatePurchaseOrder");
        }
    }
}
