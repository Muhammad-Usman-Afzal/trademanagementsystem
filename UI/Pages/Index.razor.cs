namespace UI.Pages;

public partial class Index
{
    #region DI

    [Inject]
    public ProtectedLocalStorage _localStorage { get; set; }
    [Inject]
    public NavigationManager _navigation { get; set; }

    #endregion

    #region Variables

    bool _processing = false;
    AppUsers UserSession = new AppUsers();

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
