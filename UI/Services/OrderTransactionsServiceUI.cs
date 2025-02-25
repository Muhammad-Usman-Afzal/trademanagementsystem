namespace UI.Services;

public class OrderTransactionsServiceUI : BaseServiceUI<OrderTransactions>, IOrderTransactionsRepoUI
{
    private readonly HttpClient _httpClient;
    private readonly ProtectedLocalStorage _localStorage;

    public OrderTransactionsServiceUI(HttpClient httpClient, ProtectedLocalStorage localStorage)
        : base(httpClient, localStorage)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
    }
}