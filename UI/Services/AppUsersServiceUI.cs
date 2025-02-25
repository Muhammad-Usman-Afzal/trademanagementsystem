namespace UI.Services;

public class AppUsersServiceUI : BaseServiceUI<AppUsers>, IAppUsersRepoUI
{
    private readonly HttpClient _httpClient;
    private readonly ProtectedLocalStorage oStorageService;
    private readonly AuthenticationStateProvider oAuth;

    public AppUsersServiceUI(HttpClient httpClient, ProtectedLocalStorage localStorage, AuthenticationStateProvider authenticationState)
        : base(httpClient, localStorage)
    {
        _httpClient = httpClient;
        oAuth = authenticationState;
        oStorageService = localStorage;
    }

    public async Task<AppUsers> ValidateLogin(AppUsers oModel)
    {
        try
        {
            var result = await _httpClient.PostAsJsonAsync("AppUsers/ValidateLogin", oModel)
                .Result.Content.ReadFromJsonAsync<AppUsers>();

            if (!string.IsNullOrEmpty(result.BearerToken))
            {
                await oStorageService.SetAsync("User", result);
                ((AuthStateProvider)oAuth).NotifyUserAuthentication(result.BearerToken);
                return result;
            }
            return result;
        }
        catch (Exception ex)
        {
            UILogger.WriteLog(ex);
            return new AppUsers();
        }
    }
}