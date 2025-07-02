namespace API.Repositories;

public interface IStockTransactionsRepo : IBaseRepo<StockTransactions> 
{
    Task<List<ItemStockSummaryDTO>> GetItemWiseStock();
}