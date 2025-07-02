using UI.Repositories;

namespace UI.Pages.Production
{
    partial class ProductionPlan
    {
        [Inject]
        ISnackbar _Snackbar { get; set; }
        [Inject]
        ProtectedLocalStorage _localStorage { get; set; }
        [Inject]
        IOrderRepoUI _OrderRepoUI { get; set; }
        [Inject]
        NavigationManager navigation { get; set; }
        IDialogService dialogService { get; set; }

     


        #region Variables
        private bool _processing = false, CreatePODiloag = false;
        #endregion


        #region List
        List<Order> POList = new List<Order>();
        List<OrderDetail> PODetailList = new List<OrderDetail>();
        #endregion








    }
}
