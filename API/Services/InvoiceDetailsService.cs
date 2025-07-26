namespace API.Services;

public class InvoiceDetailsService : BaseService<InvoiceDetails>, IInvoiceDetailsRepo
{
    private readonly IBaseRepo<InvoiceDetails> _baseRepo;

    public InvoiceDetailsService(TMSContext context, IBaseRepo<InvoiceDetails> baseRepo)
        : base(context)
    {
        _baseRepo = baseRepo;
    }
}