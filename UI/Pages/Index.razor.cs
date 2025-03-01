namespace UI.Pages;

public partial class Index
{
    #region DI

    [Inject]
    public ProtectedLocalStorage _localStorage { get; set; }
    [Inject]
    public NavigationManager _navigation { get; set; }
    [Inject]
    IPartyRepoUI _partyRepoUI { get; set; }
    [Inject]
    IProductDetailsRepoUI _ProductDetailsRepoUI { get; set; }
    [Inject]
    public IAppUsersRepoUI _appUsersRepoUI { get; set; }

    #endregion

    #region Variables

    bool _processing = false;
    int partcount, vendorcount, SupplyerCount, ContractorCount;
    int ProcessedCount, UnprocessedCount;
    AppUsers UserSession = new AppUsers();
    List<Party> PartyCount = new List<Party>();
    List<ProductDetails> ProductCount = new List<ProductDetails>();
    List<AppUsers> userCount = new List<AppUsers>();

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
                    _navigation.NavigateTo("/signin");
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
                await Task.Delay(1000);
                PartyCount = await _partyRepoUI.GetAll("Party/GetParties") ?? new List<Party>();
                ProductCount = await _ProductDetailsRepoUI.GetAll("ProductDetails/GetProductDetails") ?? new List<ProductDetails>();
                userCount = await _appUsersRepoUI.GetAll("AppUsers/GetAppUsers") ?? new List<AppUsers>();

                partcount = PartyCount.Count();
                vendorcount = PartyCount.Where(x=>x.PartyType == "Vendor").Count();
                SupplyerCount = PartyCount.Where(x=>x.PartyType == "Supplier").Count();
                ContractorCount = PartyCount.Where(x=>x.PartyType == "Contractor").Count();

                ProcessedCount = ProductCount.Where(x => x.Isprocessed == true).Count();
                UnprocessedCount = ProductCount.Where(x => x.Isprocessed == false).Count();



            }

            _processing = false;
        }
        catch (Exception ex)
        {
            UILogger.WriteLog(ex);
        }

        return;
    }

    #endregion
}
