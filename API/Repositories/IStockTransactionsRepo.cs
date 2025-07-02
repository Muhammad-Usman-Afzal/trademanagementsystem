namespace API.Repositories;

public interface IStockTransactionsRepo : IBaseRepo<StockTransactions> 
{
    Task<List<ItemStockSummaryDTO>> GetItemWiseStock();
    Task<int> GetLiveStockByItem(int itemId);
}