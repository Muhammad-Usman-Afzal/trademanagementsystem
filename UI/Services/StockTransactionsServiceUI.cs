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
}