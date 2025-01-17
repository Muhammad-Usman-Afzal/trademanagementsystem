using System.ComponentModel.DataAnnotations.Schema;

namespace Models.AppModels;

public class PurchaseOrderDetail
{
    [Column(TypeName = "varchar(30)")]
    public string ItemName { get; set; }
    [Column(TypeName = "nvarchar(15)")]
    public int Qty { get; set; }
    [Column(TypeName = "nvarchar(15)")]
    public int Rate { get; set; }
    [Column(TypeName = "nvarchar(15)")]
    public int Unit { get; set; }  
}
