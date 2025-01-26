namespace UI.Services;

public class PartyServiceUI : BaseServiceUI<Party>, IPartyRepoUI
{
    private readonly HttpClient _httpClient;

    public PartyServiceUI(HttpClient httpClient)
        : base(httpClient)
    {
        _httpClient = httpClient;
    }
}