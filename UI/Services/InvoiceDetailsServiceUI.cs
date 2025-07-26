namespace UI.Services;

public class InvoiceDetailsServiceUI : BaseServiceUI<InvoiceDetails>, IInvoiceDetailsRepoUI
{
    private readonly HttpClient _httpClient;
    private readonly ProtectedLocalStorage _localStorage;

    public InvoiceDetailsServiceUI(HttpClient httpClient, ProtectedLocalStorage localStorage)
        : base(httpClient, localStorage)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
    }
}