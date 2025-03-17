using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.AppModels
{
    public class StockTransactions : BaseEntity
    {
        public DateTime TransectionDate { get; set; } = DateTime.Now;
        public int ItemId { get; set; }
        public ProductDetails Item { get; set; }
        public StockTransectionTypes StockType { get; set; } = new StockTransectionTypes();
        public int Qty { get; set; }
        [Column(TypeName = "nvarchar(15)")]
        public string Warehouse { get; set; }
        [Column(TypeName = "nvarchar(10)")]
        public string ReferenceNumber { get; set; } // Invoice No, Order No, etc.
        [Column(TypeName = "nvarchar(200)")]
        public string Remaeks { get; set; }
    }
}


public enum StockTransectionTypes
{
    Purchase = 1,
    Sale,
    GoodIssuence,
    GoodsReceipt,
    Return,
    Transfer
}