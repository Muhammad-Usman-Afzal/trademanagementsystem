namespace UI.Services;

public class PurchaseOrderServiceUI : BaseServiceUI<PurchaseOrder>, IPurchaseOrderRepoUI
{
    private readonly HttpClient _httpClient;
    private readonly ProtectedLocalStorage _localStorage;

    public PurchaseOrderServiceUI(HttpClient httpClient, ProtectedLocalStorage localStorage)
        : base(httpClient, localStorage)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
    }
}