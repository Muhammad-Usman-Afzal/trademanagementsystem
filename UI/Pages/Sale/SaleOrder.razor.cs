using UI.Repositories;

namespace UI.Pages.Sale
{
    public partial class SaleOrder
    {
        [Inject]
        ISnackbar _Snackbar { get; set; }
        [Inject]
        public ProtectedLocalStorage _localStorage { get; set; }
        [Inject]
        IOrderRepoUI _orderRepoUI { get; set; }
        [Inject]
        NavigationManager Navigate { get; set; }
        [Inject]
        IDialogService dialogService { get; set; }


        private bool _processing = false;

        List<Order> SOList = new List<Order>();
        List<OrderDetail> SODetailList = new List<OrderDetail>();

        AppUsers UserSession;


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
                    SOList = await _orderRepoUI.GetAll($"Order/GetOrdersByType?orderType={OrderTypes.SaleOrder}") ?? new List<Order>();

                }
                _processing = false;
            }
            catch (Exception ex)
            {
                UILogger.WriteLog(ex);
            }
            return;
        }

    }
}
