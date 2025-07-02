namespace UI.Repositories;

public interface IStockTransactionsRepoUI : IBaseRepoUI<StockTransactions>
{
    Task<List<ItemStockSummaryDTO>> GetItemWiseStock(string APIName);
}