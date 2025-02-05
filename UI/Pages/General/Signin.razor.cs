namespace UI.Pages.General;

public partial class Signin
{
    private bool _processing = false;

    //[Inject]
    //public IAppUsersRepoUI _userRepo { get; set; }
    //[Inject]
    //public ITerminalConfigRepoUI _terminalConfigRepoUI { get; set; }
    [Inject]
    public ISnackbar snackbar { get; set; }
    //[Inject]
    //public ProtectedLocalStorage Storage { get; set; }
    //[Inject]
    //public IConfiguration Configuration { get; set; }
    //[Inject]
    //public IJSRuntime _jSRuntime { get; set; }
    //[Inject]
    //public ProtectedLocalStorage _localStorage { get; set; }
    [Inject]
    public NavigationManager navigation { get; set; }

    //AppUsers UserSession = new AppUsers();
    //AppUsers oModel = new AppUsers();

    //List<TerminalConfig> TerminalList = new List<TerminalConfig>();

    //bool IsValidate()
    //{
    //    return
    //        string.IsNullOrEmpty(oModel.UserName) ||
    //        string.IsNullOrEmpty(oModel.Password) ||
    //        (oModel.UserName!="admin" && (oModel.TerminalId == null || oModel.TerminalId < 1))
    //        ? false : true;
    //}

    //async Task<AppUsers> ValidateLogin()
    //{
    //    try
    //    {
    //        _processing = true;
    //        _ = InvokeAsync(StateHasChanged);
    //        await Task.Delay(1);

    //        if (!IsValidate())
    //        {
    //            Snackbar.Add("Please fill all fields", Severity.Error, (options) => { options.Icon = Icons.Sharp.Error; });
    //            _processing = false;
    //            StateHasChanged();
    //            return null;
    //        }
    //        else
    //        {
    //            if (!IsLicenseExpired())
    //            {
    //                if (_terminalConfigRepoUI.GetBooleanByCondition($"TerminalConfig/IsTerminalBusy?TerminalId={(oModel.UserName=="admin" ? 0 : oModel.TerminalId)}"))
    //                {
    //                    Snackbar.Add("Selected Terminal already busy. Please try different Terminal", Severity.Warning);
    //                    oModel = new AppUsers();
    //                    _processing = false;
    //                    StateHasChanged();
    //                    return null;
    //                }
    //                else
    //                {
    //                    var result = await _userRepo.ValidateLogin(oModel);
    //                    if (result != null && !string.IsNullOrEmpty(result.BearerToken))
    //                    {
    //                        var userSession = await _localStorage.GetAsync<AppUsers>("User");
    //                        UserSession = userSession.Value ?? new AppUsers();

    //                        if (!UserSession.IsSuperUser)
    //                        {
    //                            oModel.Terminal.IsBusy = true;
    //                            await _terminalConfigRepoUI.Update("TerminalConfig/Update", oModel.Terminal); 
    //                        }

    //                        if (UserSession.Department == "Delivery")
    //                        {
    //                            navigation.NavigateTo("/delivery");
    //                        }
    //                        else if (UserSession.Department == "Unrolling")
    //                        {
    //                            navigation.NavigateTo("/unrolling");
    //                        }
    //                        else if (UserSession.Department == "Cutting")
    //                        {
    //                            navigation.NavigateTo("/cutting");
    //                        }
    //                        else if (UserSession.Department == "Stitching")
    //                        {
    //                            navigation.NavigateTo("/stitching");
    //                        }
    //                        else if (UserSession.Department == "Packing")
    //                        {
    //                            if (UserSession.Designation == "Manager")
    //                            {
    //                                navigation.NavigateTo("/packing");
    //                            }
    //                            else
    //                            {
    //                                Snackbar.Add("You are not authorized for Packing. Only Manager can access this page", Severity.Info);
    //                                _processing = false;
    //                                StateHasChanged();
    //                            }
    //                        }
    //                        else if (UserSession.Department == "Master Packing")
    //                        {
    //                            navigation.NavigateTo("/cartonpacking");
    //                        }
    //                        else if (UserSession.Department == "admin")
    //                        {
    //                            navigation.NavigateTo("/Dashboard");
    //                        }
    //                        else
    //                        {
    //                            navigation.NavigateTo("/signin");
    //                        }

    //                        _processing = false;
    //                        return oModel;
    //                    }
    //                    else
    //                    {
    //                        Snackbar.Add("Credentials doesn't match. If you are new please Signup first", Severity.Error, (options) => { options.Icon = Icons.Sharp.Error; });
    //                        oModel = new AppUsers();
    //                        _processing = false;
    //                        StateHasChanged();
    //                        return null;
    //                    }
    //                }
    //            }
    //            else
    //            {
    //                Snackbar.Add("Something went wrong", Severity.Error, (options) => { options.Icon = Icons.Sharp.Error; });
    //                oModel = new AppUsers();
    //                _processing = false;
    //                StateHasChanged();
    //                return null;
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        UILogger.WriteLog(ex);
    //        Snackbar.Add(ex.Message, Severity.Error, (options) => { options.Icon = Icons.Sharp.Error; });
    //        _processing = false;
    //        return null;
    //    }
    //}

    //public void Enter(KeyboardEventArgs e)
    //{
    //    if (e.Code == "Enter" || e.Code == "NumpadEnter")
    //    {
    //        ValidateLogin();
    //    }
    //}

    //#region License Work

    //public bool IsLicenseExpired()
    //{
    //    try
    //    {
    //        var _configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

    //        string HKey = UILogger.HKey;
    //        string AppKey = _configuration.GetValue<string>("AppURL");

    //        if (!string.IsNullOrEmpty(AppKey))
    //        {
    //            if (/*AppKey.Length == 44*/ true)
    //            {
    //                DateTime starttime, endtime;
    //                string Decoded, StTime, EnTime;
    //                Decoded = _userRepo.Decryption(AppKey);
    //                StTime = Decoded;
    //                starttime = DateTime.ParseExact(StTime, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None);
    //                //EnTime = Decoded.Substring(12, 8);
    //                //endtime = DateTime.ParseExact(EnTime, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None);
    //                if (DateTime.Now.Date > starttime)
    //                {
    //                    Snackbar.Add("Something went wrong", Severity.Error, (options) => { options.Icon = Icons.Sharp.Error; });
    //                    return true;
    //                }
    //                else
    //                    return false;
    //            }
    //            else
    //            {
    //                Snackbar.Add("Invalid license.", Severity.Error, (options) => { options.Icon = Icons.Sharp.Error; });
    //                return true;
    //            }
    //        }
    //        else
    //        {
    //            return true;
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        UILogger.WriteLog(ex);
    //        return true;
    //    }
    //}

    //#endregion

    //private async Task<IEnumerable<TerminalConfig>> SearchTerminal(string value)
    //{
    //    await Task.Delay(0);
    //    if (string.IsNullOrEmpty(value))
    //        return TerminalList;
    //    return TerminalList.Where(x => x.TerminalName.Contains(value, StringComparison.InvariantCultureIgnoreCase));
    //}

    //void OnValueChange(TerminalConfig Value)
    //{
    //    try
    //    {
    //        if (Value != null)
    //        {
    //            oModel.TerminalId = Value.Id;
    //            oModel.Terminal = Value;
    //        }
    //    }
    //    catch (Exception ex) { UILogger.WriteLog(ex); }
    //}

    //protected override async Task OnInitializedAsync()
    //{
    //    TerminalList = await _terminalConfigRepoUI.GetAll("TerminalConfig/GetTerminalConfigs") ?? new List<TerminalConfig>();

    //    return;
    //}
    string u;
    string p;
    void ValidateLogin()
    {
        if (/*u=="admin" && p=="admin"*/ true)
        {
        navigation.NavigateTo("/index");
        }
        else
        {
            snackbar.Add("User not found with this Username or Password", Severity.Error);
        }
    }

}