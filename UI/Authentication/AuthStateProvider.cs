namespace UI.Authentication;

public class AuthStateProvider : AuthenticationStateProvider
{
    private readonly HttpClient oClient;
    private readonly ProtectedLocalStorage oService;
    private readonly AuthenticationState oAnonymus;

    public AuthStateProvider(HttpClient restClient, ProtectedLocalStorage StorageService)
    {
        oClient = restClient;
        oService = StorageService;
        oAnonymus = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            AppUsers user = oService.GetAsync<AppUsers>("User").Result.Value;
            if (user is not null)
            {
                string jwtTokenString = user.BearerToken;
                if (string.IsNullOrEmpty(jwtTokenString))
                {
                    return oAnonymus;
                }
                //UILog.oUser = user;
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(JwtParser.ParseClaimsFromJwt(jwtTokenString), "Bearer")));
            }
            else
            {
                return oAnonymus;
            }
        }
        catch (Exception ex)
        {
            UILogger.WriteLog(ex);
            return oAnonymus;
        }
    }

    public void NotifyUserAuthentication(string token)
    {
        try
        {
            var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(JwtParser.ParseClaimsFromJwt(token), "jwtAuthType"));
            var authState = Task.FromResult(new AuthenticationState(authenticatedUser));
            NotifyAuthenticationStateChanged(authState);
        }
        catch (Exception ex)
        {
            UILogger.WriteLog(ex);
        }
    }

    public void NotifyUserLogout()
    {
        try
        {
            var authState = Task.FromResult(oAnonymus);
            NotifyAuthenticationStateChanged(authState);
        }
        catch (Exception ex)
        {
            UILogger.WriteLog(ex);
        }
    }
}