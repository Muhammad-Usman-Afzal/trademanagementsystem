using Models;

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


        #region Variables
        private bool _processing = false;
        #endregion

        #region Objects
        AppUsers UserSession;
        Party party = new Party();
        PurchaseOrder purchaseOrder = new PurchaseOrder();

        #endregion

        #region Lists
        List<Party> parties = new List<Party>();
        #endregion

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

        void onVanderSelect(Party value)
        {
            try
            {
                if (value != null)
                {
                    // = value;
                    //FarwordManagerId = value.Id;
                    //complainTrack.DepartmentManagerId = departmang.Id;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task<IEnumerable<Party>> SearchVander(string value)
        {
            await Task.Delay(0);
            if (string.IsNullOrEmpty(value))
                return parties;
            return parties.Where(x => x.CompanyName.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
