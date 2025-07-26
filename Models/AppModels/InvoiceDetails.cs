using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.AppModels;

public class InvoiceDetails : BaseEntity
{
    public int ProductId { get; set; }
    public ProductDetails Product { get; set; }

    [Required]
    public int Quantity { get; set; }

    [Required]
    public double UnitPrice { get; set; }

    public double Discount { get; set; }

    public double TotalPrice { get; set; }

    public string DispatchLocation { get; set; }

    public ItemReturnStatus ReturnStatus { get; set; }
}
