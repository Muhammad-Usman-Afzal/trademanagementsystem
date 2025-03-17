namespace API.Services;

public class StockTransactionsService : BaseService<StockTransactions>, IStockTransactionsRepo
{
    private readonly IBaseRepo<StockTransactions> _baseRepo;

    public StockTransactionsService(TMSContext context, IBaseRepo<StockTransactions> baseRepo)
        : base(context)
    {
        _baseRepo = baseRepo;
    }
}