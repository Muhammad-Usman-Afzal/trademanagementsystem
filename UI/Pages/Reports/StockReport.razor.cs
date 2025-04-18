using Models;
using UI.Repositories;

namespace UI.Pages.Reports
{
    public partial class StockReport
    {
        [Inject]
        ISnackbar _Snackbar { get; set; }
        [Inject]
        public ProtectedLocalStorage _localStorage { get; set; }
        [Inject]
        IStockTransactionsRepoUI  _stockTransactionsRepoUI { get; set; }
        [Inject]
        NavigationManager Navigate { get; set; }

        #region Variables
        private bool _processing = false, CreatePODiloag = false;
        #endregion


        #region List
        List<StockTransactions> STList = new List<StockTransactions>();
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
                STList = await _stockTransactionsRepoUI.GetAll("StockTransactions/GetStockTransactionss") ?? new List<StockTransactions>();

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
    }
}
