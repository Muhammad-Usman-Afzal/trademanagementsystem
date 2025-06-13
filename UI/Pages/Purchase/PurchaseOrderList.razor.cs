using Models;
using UI.Repositories;

namespace UI.Pages.Purchase
{
    public partial class PurchaseOrderList
    {
        [Inject]
        ISnackbar _Snackbar { get; set; }
        [Inject]
        ProtectedLocalStorage _localStorage { get; set; }
        [Inject]
        IOrderRepoUI _OrderRepoUI { get; set; }
        [Inject]
        NavigationManager navigation { get; set; }

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
                    var userSession = await _localStorage.GetAsync<AppUsers>("User");
                    UserSession = userSession.Value ?? new AppUsers();

                    if (UserSession == null)
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

                if (UserSession != null)
                {
                    POList = await _OrderRepoUI.GetAll($"Order/GetOrdersByType?orderType={OrderTypes.PurchaseOrder}") ?? new List<Order>();
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
            navigation.NavigateTo("CreatePurchaseOrder");
        }
    }
}
