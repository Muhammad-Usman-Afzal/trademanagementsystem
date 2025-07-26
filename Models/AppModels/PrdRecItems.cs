using System.ComponentModel.DataAnnotations.Schema;

namespace Models.AppModels;

public class PrdRecItems : BaseEntity
{
    public int OrderId { get; set; }
    public int ItemId { get; set; }
    public ProductDetails Item { get; set; } = new ProductDetails();
    public int RecQty { get; set; }
    public double Rate { get; set; }
    public decimal? Weight { get; set; }
    public int PerBagQty { get; set; }

    [Column(TypeName = "nvarchar(15)")]
    public string Unit { get; set; }
    public string Warehouse { get; set; }
    [NotMapped]
    public List<string> Warehouses { get; set; } = new List<string>();
}
