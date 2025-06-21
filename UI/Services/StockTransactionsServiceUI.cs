namespace UI.Services;

public class StockTransactionsServiceUI : BaseServiceUI<StockTransactions>, IStockTransactionsRepoUI
{
    private readonly HttpClient _httpClient;
    private readonly ProtectedLocalStorage _localStorage;

    public StockTransactionsServiceUI(HttpClient httpClient, ProtectedLocalStorage localStorage)
        : base(httpClient, localStorage)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
    }

    private async Task SetAuthorizationHeader()
    {
        var userSession = await _localStorage.GetAsync<AppUsers>("User");
        string token = userSession.Value?.BearerToken ?? string.Empty;

        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }

    public async Task<List<ItemStockSummaryDTO>> GetItemWiseStock(string APIName)
    {
        try
        {
            await SetAuthorizationHeader();
            return await _httpClient.GetFromJsonAsync<List<ItemStockSummaryDTO>>(APIName) ?? new List<ItemStockSummaryDTO>();
        }
        catch (Exception ex)
        {
            UILogger.WriteLog(ex);
            return new List<ItemStockSummaryDTO>();
        }
    }

}