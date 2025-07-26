namespace UI.Services;

public class InvoiceServiceUI : BaseServiceUI<Invoice>, IInvoiceRepoUI
{
    private readonly HttpClient _httpClient;
    private readonly ProtectedLocalStorage _localStorage;

    public InvoiceServiceUI(HttpClient httpClient, ProtectedLocalStorage localStorage)
        : base(httpClient, localStorage)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
    }
}