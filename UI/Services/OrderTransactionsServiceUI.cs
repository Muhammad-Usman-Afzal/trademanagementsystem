namespace UI.Services;

public class OrderTransactionsServiceUI : BaseServiceUI<OrderTransactions>, IOrderTransactionsRepoUI
{
    private readonly HttpClient _httpClient;

    public OrderTransactionsServiceUI(HttpClient httpClient)
        : base(httpClient)
    {
        _httpClient = httpClient;
    }
}