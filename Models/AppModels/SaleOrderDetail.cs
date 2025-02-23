using System.ComponentModel.DataAnnotations.Schema;

namespace Models.AppModels;

public class SaleOrderDetail : BaseEntity
{
    public int ItemId { get; set; }
    public ProductDetails Item { get; set; } = new ProductDetails();
    public int Qty { get; set; }
    public int Rate { get; set; }
    public int Unit { get; set; }
    public int Discount { get; set; }
}
