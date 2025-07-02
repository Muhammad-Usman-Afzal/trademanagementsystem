using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.AppModels
{
    public class OrderTransactions : BaseEntity
    {
        public string ItemName { get; set; }
        public ProductDetails Item { get; set; }
        public int? ItemId { get; set; }
        public TransectionTypes TType { get; set; } = new TransectionTypes();
        public DateTime TransectionDate { get; set; } = DateTime.Now;
        public int POQty { get; set; }
        public int SOQty { get; set; }
        public string ReciverParty { get; set; }
        public string RecivingLocation { get; set; }
        public bool IsDirectDelivery { get; set; }
        
        [Column(TypeName = "nvarchar(200)")]
        public string Remaeks { get; set; }
        public int RecQty { get; set; }
        public int SaleQty { get; set; }
        public int TotalRecQty { get; set; }
        public int TotalSoldQty { get; set; }
        public string Warehouse { get; set; }
        [NotMapped]
        public List<string> Warehouses { get; set; } = new List<string>();
    }
}


public enum TransectionTypes
{
    PurchaseReceive = 1,
    Dispatch,
    GoodsIssuance,
    GoodsReceipt
}