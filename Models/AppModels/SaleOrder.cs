using System.ComponentModel.DataAnnotations.Schema;

namespace Models.AppModels;

public class SaleOrder
{
    [Column(TypeName = "nvarchar(15)")]
    public string SRNo { get; set; }
    public DateTime SRDate { get; set; }
    public DateTime ExpectedDeliveryDate { get; set; }
    [Column(TypeName = "varchar(30)")]
    public string SupplierName { get; set; } //Supplier
    [Column(TypeName = "nvarchar(20)")]
    public string PaymentMethod { get; set; }
    [Column(TypeName = "nvarchar(125)")]
    public string Remarks { get; set; }
    [Column(TypeName = "nvarchar(15)")]
    public int GrossAmount { get; set; }
    [Column(TypeName = "nvarchar(15)")]
    public int DiscountAmount { get; set; }
    [Column(TypeName = "nvarchar(15)")]
    public int TaxRate { get; set; }
    [Column(TypeName = "nvarchar(15)")]
    public int TaxAmount { get; set; }
    [Column(TypeName = "nvarchar(15)")]
    public int NetAmount { get; set; }
}
