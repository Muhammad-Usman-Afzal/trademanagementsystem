namespace UI.Services;

public class PurchaseOrderServiceUI : BaseServiceUI<PurchaseOrder>, IPurchaseOrderRepoUI
{
    private readonly HttpClient _httpClient;

    public PurchaseOrderServiceUI(HttpClient httpClient)
        : base(httpClient)
    {
        _httpClient = httpClient;
    }
}