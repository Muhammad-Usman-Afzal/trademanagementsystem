using Models;
using Models.AppModels;
using UI.Repositories;

namespace UI.Pages.Purchase
{
    public partial class PurchaseOrderReceive
    {
        [Inject]
        ISnackbar _Snackbar { get; set; }

        [Inject]
        IPartyRepoUI _partyRepoUI { get; set; }

        [Inject]
        NavigationManager Navigate { get; set; }
        [Inject]
        IOrderRepoUI _OrderRepoUI { get; set; }


        #region Variables
        [Parameter]
        public int PONo { get; set; }
        private int _PONo;
        private bool _processing = false;
        #endregion

        #region Objects
        AppUsers UserSession;
        Party party = new Party();
        Order PO = new Order();
        OrderDetail Model = new OrderDetail();
        OrderTransactions transactions = new OrderTransactions();
        PurchaseOrder purchaseOrder = new PurchaseOrder();

        #endregion

        #region Lists
        List<Party> parties = new List<Party>();
        List<OrderDetail> PODetail = new List<OrderDetail>();
        #endregion

        protected override Task OnParametersSetAsync()
        {
            try
            {
                _PONo = PONo;

                return base.OnParametersSetAsync();
            }
            catch (Exception ex)
            {
                UILogger.WriteLog(ex);
                return base.OnParametersSetAsync();
            }
        }

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
                parties = await _partyRepoUI.GetAll("Party/GetParties") ?? new List<Party>();
                PO = await _OrderRepoUI.GetSingleByCondition($"Order/GetOrderById?id={PONo}") ?? new Order();
                parties = await _partyRepoUI.GetAll("Party/GetParties") ?? new List<Party>();

                parties = parties.Where(x => x.PartyType == "Supplier").ToList();
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

        void onVanderSelect(OrderDetail value)
        {
            try
            {
                if (value != null)
                {
                    Model = value;
                    //FarwordManagerId = value.Id;
                    //complainTrack.DepartmentManagerId = departmang.Id;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task<IEnumerable<OrderDetail>> SearchVander(string value)
        {
            await Task.Delay(0);
            if (string.IsNullOrEmpty(value))
                return PO.OrderDetail;
            return PO.OrderDetail.Where(x => x.Item.ItemName.Contains(value, StringComparison.InvariantCultureIgnoreCase));
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
                    transactions.ReciverParty = Value.CompanyName;
                    transactions.RecivingLocation = Value.CompanyAddress;
                }
            }
            catch (Exception ex) { UILogger.WriteLog(ex); }
        }
    }
}
