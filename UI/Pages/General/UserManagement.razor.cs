namespace UI.Pages.General;

public partial class UserManagement
{
    #region DI

    [Inject]
    public ProtectedLocalStorage _localStorage { get; set; }
    [Inject]
    public NavigationManager _navigation { get; set; }
    [Inject]
    public ISnackbar _snackbar { get; set; }
    [Inject]
    public IAppUsersRepoUI _appUsersRepoUI { get; set; }
    [Inject]
    public IMasterMenuRepoUI _masterMenuRepoUI { get; set; }

    #endregion

    #region Variables

    bool _dlgvisible = false;
    bool _pageloading = false;
    bool _gridloading = false;
    bool _processing = false;
    bool _isDisplay = false;

    Models.AppModels.UserPermission CurrentPermission;

    int activeIndex = 0;
    string searchString1 = "";

    readonly DialogOptions _dialogOptions = new() { FullScreen = true, CloseButton = true };
    AppUsers selectedItem1 = null;
    AppUsers UserSession = new AppUsers();
    AppUsers Model = new AppUsers();
    List<AppUsers> lstUser = new List<AppUsers>();
    MasterMenu oScreen = new MasterMenu();

    List<MasterMenu> lstScreens = new List<MasterMenu>();
    List<MasterMenu> lstAssignedScreens = new List<MasterMenu>();

    #endregion

    #region Functions

    private bool FilterFunc1(AppUsers element) => FilterFunc(element, searchString1);

    private bool FilterFunc(AppUsers element, string searchString)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(searchString))
                return true;
            if (!string.IsNullOrEmpty(element.FullName) ? element.FullName.Contains(searchString, StringComparison.OrdinalIgnoreCase) : false)
                return true;
            return false;
        }
        catch (Exception ex)
        {
            UILogger.WriteLog(ex);
            return false;
        }
    }

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
            _pageloading = true;
            _ = InvokeAsync(StateHasChanged);

            if (UserSession.Id > 0)
            {
                await Task.Delay(1);

                Uri uri = new Uri(_navigation.Uri);
                string finalURI = uri.AbsolutePath.TrimStart('/');
                if (UserSession.AuthorizedScreens.Count > 0)
                {
                    CurrentPermission = UserSession.AuthorizedScreens.FirstOrDefault(x => x.MasterMenu?.URL == finalURI).AuthorizedPermission;
                }

                lstScreens = await _masterMenuRepoUI.GetAll("MasterMenu/GetMasterMenus") ?? new List<MasterMenu>();

                lstScreens.ForEach(x => x.AuthorizedPermission = Models.AppModels.UserPermission.FullAccess);
            }

            _pageloading = false;
        }
        catch (Exception ex)
        {
            UILogger.WriteLog(ex);
        }

        return;
    }

    private async void OpenDialog()
    {
        _dlgvisible = true;
        _gridloading = true;

        await Task.Delay(1);

        lstUser = await _appUsersRepoUI.GetAll("AppUsers/GetAppUsers") ?? new List<AppUsers>();

        _gridloading = false;
        _isDisplay = true;
        StateHasChanged();
    }

    private async Task<IEnumerable<AppUsers>> SearchUser(string value, CancellationToken token)
    {
        await Task.Delay(0);
        if (string.IsNullOrEmpty(value))
            return lstUser;
        return lstUser.Where(x => !string.IsNullOrEmpty(x.FullName) ? x.FullName.Contains(value, StringComparison.InvariantCultureIgnoreCase) : false);
    }

    void WhenUserSelect(AppUsers Value)
    {
        try
        {
            if (Value != null)
            {
                Model = Value;
                Model.FullName = Value.FullName;
            }
        }
        catch (Exception ex) { UILogger.WriteLog(ex); }
    }

    private async Task<IEnumerable<MasterMenu>> SearchScreen(string value)
    {
        await Task.Delay(0);
        if (string.IsNullOrEmpty(value))
            return lstScreens;
        return lstScreens.Where(x => !string.IsNullOrEmpty(x.MenuName) ? x.MenuName.Contains(value, StringComparison.InvariantCultureIgnoreCase) : false);
    }

    void WhenScreenSelect(MasterMenu Value)
    {
        try
        {
            if (Value != null)
            {
                oScreen.MenuName = Value.MenuName;
                lstAssignedScreens.Add(Value);
            }
        }
        catch (Exception ex) { UILogger.WriteLog(ex); }
    }

    void WhenScreenChipClosed(MasterMenu Value)
    {
        lstAssignedScreens.Remove(Value);
    }

    void Next(int index)
    {
        activeIndex = index;
    }

    bool IsValidate()
    {
        try
        {
            return
                string.IsNullOrEmpty(Model.UserName) ||
                string.IsNullOrEmpty(Model.Password) ||
                string.IsNullOrEmpty(Model.FullName) ||
                Model.AuthorizedScreens.Count == 0
                   ? false : true;
        }
        catch (Exception ex)
        {
            UILogger.WriteLog(ex);
            return false;
        }
    }

    void FillModel()
    {
        //Model.AuthorizedScreens = lstScreens.Select(screen => new UserAuthorizedScreens
        //{
        //    MenuName = screen.MenuName,
        //    URL = screen.URL,
        //    IconURL = screen.IconURL,
        //    AuthorizedPermission = screen.AuthorizedPermission
        //}).ToList();
    }

    async void Save()
    {
        FillModel();

        if (IsValidate())
        {
            var result = Model.Id > 0 ? await _appUsersRepoUI.Update($"AppUsers/Update", Model) : await _appUsersRepoUI.Create("AppUsers/Create", Model) ?? new AppUsers();
            
            if (result != null && result.Id > 0)
            {
                _snackbar.Add("User has been saved", Severity.Success);
                await Task.Delay(1000);
                _navigation.NavigateTo("/um", true);
            }
            //else
            //{
            //    _snackbar.Add("An error occurred while User saving", Severity.Error);
            //}
        }
        else
        {
            _snackbar.Add("Please fill all required information", Severity.Error);
        }
    }

    void Edit(AppUsers Row)
    {
        Model = Row;
        Model.AuthorizedScreens = Row.AuthorizedScreens;

        _dlgvisible = false;
    }

    #endregion
}