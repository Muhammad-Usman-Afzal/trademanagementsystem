namespace UI.Services;

public class OrderServiceUI : BaseServiceUI<Order>, IOrderRepoUI
{
    private readonly HttpClient _httpClient;

    public OrderServiceUI(HttpClient httpClient)
        : base(httpClient)
    {
        _httpClient = httpClient;
    }
}