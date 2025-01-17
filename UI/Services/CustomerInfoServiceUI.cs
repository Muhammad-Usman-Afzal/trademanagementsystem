namespace UI.Services;

public class CustomerInfoServiceUI : BaseServiceUI<CustomerInfo>, ICustomerInfoRepoUI
{
    private readonly HttpClient _httpClient;

    public CustomerInfoServiceUI(HttpClient httpClient)
        : base(httpClient)
    {
        _httpClient = httpClient;
    }
}