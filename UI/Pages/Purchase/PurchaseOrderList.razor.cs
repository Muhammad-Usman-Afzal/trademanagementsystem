using Models;
using UI.Repositories;

namespace UI.Pages.Purchase
{
    public partial class PurchaseOrderList
    {
        [Inject]
        ISnackbar _Snackbar { get; set; }

        [Inject]
        IOrderRepoUI _OrderRepoUI { get; set; }
        [Inject]
        NavigationManager Navigate { get; set; }

        #region Variables
        private bool _processing = false, CreatePODiloag = false;
        #endregion


        #region List
        List<Order> POList = new List<Order>();
        List<OrderDetail> PODetailList = new List<OrderDetail>();
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
                POList = await _OrderRepoUI.GetAll("Order/GetOrders") ?? new List<Order>();

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
