using Microsoft.AspNetCore.Components.Web;

namespace UI.Pages.General;

public partial class Signin
{
    #region DI

    [Inject]
    public ISnackbar _snackbar { get; set; }
    [Inject]
    public NavigationManager navigation { get; set; }
    [Inject]
    public IAppUsersRepoUI _appUsersRepoUI { get; set; }

    #endregion

    #region Variables

    private bool _processing = false;

    AppUsers Model = new AppUsers();

    #endregion

    #region Functions

    async Task<AppUsers> ValidateLogin()
    {
        try
        {
            _processing = true;
            _ = InvokeAsync(StateHasChanged);
            await Task.Delay(1);

            if (Model == null || string.IsNullOrEmpty(Model.UserName) || string.IsNullOrEmpty(Model.Password))
            {
                _snackbar.Add("Please provide Username & Password both", Severity.Error);
                _processing = false;
                return null;
            }
            else
            {
                var result = await _appUsersRepoUI.ValidateLogin(Model);

                if (result != null && !string.IsNullOrEmpty(result.BearerToken))
                {
                    navigation.NavigateTo("/index");
                    _processing = false;
                    return Model;
                }
                else
                {
                    _snackbar.Add("Credentials doesn't match. Please try again", Severity.Error);
                    _processing = false;
                    StateHasChanged();
                    return null;
                }
            }
        }
        catch (Exception ex)
        {
            UILogger.WriteLog(ex);
            _snackbar.Add(ex.Message, Severity.Error, (options) => { options.Icon = Icons.Material.Rounded.Error; });
            _processing = false;
            return null;
        }

    }

    public void Enter(KeyboardEventArgs e)
    {
        if (e.Code == "Enter" || e.Code == "NumpadEnter")
        {
            ValidateLogin();
        }
    }

    #endregion

}