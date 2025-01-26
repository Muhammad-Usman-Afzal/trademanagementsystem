namespace API.Services;

public class PurchaseOrderService : BaseService<PurchaseOrder>, IPurchaseOrderRepo
{
    private readonly IBaseRepo<PurchaseOrder> _baseRepo;

    public PurchaseOrderService(TMSContext context, IBaseRepo<PurchaseOrder> baseRepo)
        : base(context)
    {
        _baseRepo = baseRepo;
    }
}