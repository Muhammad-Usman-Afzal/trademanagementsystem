namespace API.Services;

public class OrderTransectionService : BaseService<OrderTransactions>, IOrderTransactionsRepo
{
    private readonly IBaseRepo<OrderTransactions> _baseRepo;

    public OrderTransectionService(TMSContext context, IBaseRepo<OrderTransactions> baseRepo)
        : base(context)
    {
        _baseRepo = baseRepo;
    }
}