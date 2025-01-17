using System.ComponentModel.DataAnnotations.Schema;

namespace Models.AppModels;

public class ProductionOrderDetail
{
    [Column(TypeName = "nvarchar(15)")]
    public string DeliveredItem { get; set; }
    [Column(TypeName = "nvarchar(15)")]
    public string ReceivingItem { get; set; }
    [Column(TypeName = "nvarchar(15)")]
    public int ProductionQty { get; set; }
    [Column(TypeName = "nvarchar(15)")]
    public string NotSpeciffiedYet { get; set; }
}
