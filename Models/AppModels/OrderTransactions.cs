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
        public TransectionTypes TType { get; set; } = new TransectionTypes();
        public DateTime TransectionDate { get; set; } = DateTime.Now;
        public int Qty { get; set; }
        public string ReciverParty { get; set; }
        public string RecivingLocation { get; set; }
        public bool IsDirectDelivery { get; set; }

        [Column(TypeName = "nvarchar(200)")]
        public string Remaeks { get; set; }

    }
}


public enum TransectionTypes
{
    PurchaseReceive = 1,
    Dispatch,
    GoodsIssuance,
    GoodsReceipt
}