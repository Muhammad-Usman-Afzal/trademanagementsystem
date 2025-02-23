using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.AppModels
{
    internal class OrderTransactions : BaseEntity
    {
        [Column(TypeName = "nvarchar(15)")]
        public string TransectionType { get; set; }
        
        [Column(TypeName = "nvarchar(200)")]
        
        public string Remaeks { get; set; }
        
        public int Qty { get; set; }
    }
}


public enum TransectionTypes
{
    WorkInProgress = 1,
    Resolved,
    Rejected,
    Withdraw,
    Farworded
}