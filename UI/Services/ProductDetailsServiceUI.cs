namespace UI.Services;

public class ProductDetailsServiceUI : BaseServiceUI<ProductDetails>, IProductDetailsRepoUI
{
    private readonly HttpClient _httpClient;

    public ProductDetailsServiceUI(HttpClient httpClient)
        : base(httpClient)
    {
        _httpClient = httpClient;
    }
}