using Microsoft.EntityFrameworkCore;

namespace API.Services;

public class StockTransactionsService : BaseService<StockTransactions>, IStockTransactionsRepo
{
    private readonly IBaseRepo<StockTransactions> _baseRepo;

    public StockTransactionsService(TMSContext context, IBaseRepo<StockTransactions> baseRepo)
        : base(context)
    {
        _baseRepo = baseRepo;
    }

    public async Task<List<ItemStockSummaryDTO>> GetItemWiseStock()
    {
        return await _baseRepo.GetAll()
            .Include(st => st.Item)
            .GroupBy(st => new { st.ItemId, st.Item.ItemName, st.Warehouse, st.Item.Party.CompanyName })
            .Select(g => new ItemStockSummaryDTO
            {
                Brand = g.Key.CompanyName,
                ItemId = g.Key.ItemId,
                ItemName = g.Key.ItemName,
                Warehouse = g.Key.Warehouse,
                TotalStockIn = g.Sum(x => x.StockIn),
                TotalStockOut = g.Sum(x => x.StockOut),
                CurrentStock = g.Sum(x => x.StockIn) - g.Sum(x => x.StockOut),
                StockStatus = (g.Sum(x => x.StockIn) - g.Sum(x => x.StockOut)) < 10 ? "Stock Low"
                        : g.Sum(x => x.StockIn) > g.Sum(x => x.StockOut) ? "Stock In"
                        : g.Sum(x => x.StockIn) < g.Sum(x => x.StockOut) ? "Stock Out"
                        : "Balanced"
            })
            .Where(s => s.CurrentStock != 0)
            .OrderBy(s => s.ItemId)
            .ThenBy(s => s.Warehouse)
            .ToListAsync();
    }

    public async Task<int> GetLiveStockByItem(int itemId)
    {
        var result = await _baseRepo.GetAll()
            .Where(x => x.ItemId == itemId)
            .GroupBy(x => x.ItemId)
            .Select(g => g.Sum(x => x.StockIn) - g.Sum(x => x.StockOut))
            .FirstOrDefaultAsync();

        return result;
    }

}