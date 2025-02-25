using System.ComponentModel.DataAnnotations.Schema;

namespace Models.AppModels;

public class OrderDetail : BaseEntity
{
    public int ItemId { get; set; }
    public ProductDetails Item { get; set; } = new ProductDetails();
    public int Qty { get; set; }
    public double Rate { get; set; }
    [Column(TypeName = "nvarchar(15)")]
    public string Unit { get; set; }
    public List<OrderTransactions> OT { get; set; } =  new List<OrderTransactions>();
}
