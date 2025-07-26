namespace API.Services;

public class InvoiceService : BaseService<Invoice>, IInvoiceRepo
{
    private readonly IBaseRepo<Invoice> _baseRepo;

    public InvoiceService(TMSContext context, IBaseRepo<Invoice> baseRepo)
        : base(context)
    {
        _baseRepo = baseRepo;
    }
}