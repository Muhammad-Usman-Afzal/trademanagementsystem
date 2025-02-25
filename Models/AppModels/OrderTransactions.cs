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
        [Column(TypeName = "nvarchar(15)")]
        public TransectionTypes TType { get; set; } = new TransectionTypes();
        [Column(TypeName = "nvarchar(200)")]
        public string Remaeks { get; set; }
        public DateTime TransectionDate { get; set; }
        public int Qty { get; set; }
    }
}


public enum TransectionTypes
{
    PurchaseReceive = 1,
    Dispatch,
    GoodsIssuance,
    GoodsReceipt
}