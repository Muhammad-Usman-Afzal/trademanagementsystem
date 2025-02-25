namespace UI.Services;

public class OrderDetailServiceUI : BaseServiceUI<OrderDetail>, IOrderDetailRepoUI
{
    private readonly HttpClient _httpClient;

    public OrderDetailServiceUI(HttpClient httpClient)
        : base(httpClient)
    {
        _httpClient = httpClient;
    }
}