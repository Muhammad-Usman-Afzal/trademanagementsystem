using System.ComponentModel.DataAnnotations.Schema;

namespace Models.AppModels
{
    [NotMapped]
    public class ItemStockSummaryDTO
    {
        public string Brand { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string Warehouse { get; set; }
        public int TotalStockIn { get; set; }
        public int TotalStockOut { get; set; }
        public int CurrentStock { get; set; }
        public string StockStatus { get; set; } // "IN" / "OUT" / "BALANCED"
    }
}