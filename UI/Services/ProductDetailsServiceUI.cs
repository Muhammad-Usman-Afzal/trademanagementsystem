namespace UI.Services;

public class ProductDetailsServiceUI : BaseServiceUI<ProductDetails>, IProductDetailsRepoUI
{
    private readonly HttpClient _httpClient;
    private readonly ProtectedLocalStorage _localStorage;

    public ProductDetailsServiceUI(HttpClient httpClient, ProtectedLocalStorage localStorage)
        : base(httpClient, localStorage)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
    }
}