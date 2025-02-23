using System.ComponentModel.DataAnnotations.Schema;

namespace Models.AppModels;

public class SaleOrder : BaseEntity
{
    [Column(TypeName = "nvarchar(15)")]
    public string SRNo { get; set; }
    public DateTime SRDate { get; set; }
    public DateTime ExpectedDeliveryDate { get; set; }
    public int SupplierId { get; set; } //Supplier
    public Party Supplier { get; set; } = new Party();
    [Column(TypeName = "nvarchar(20)")]
    public string PaymentMethod { get; set; }
    [Column(TypeName = "nvarchar(125)")]
    public string Remarks { get; set; }
    public int GrossAmount { get; set; }
    public int DiscountAmount { get; set; }
    public int TaxRate { get; set; }
    public int TaxAmount { get; set; }
    public int NetAmount { get; set; }
    public bool IspaymentClear { get; set; }
    public List<SaleOrderDetail> SODetails { get; set; } = new List<SaleOrderDetail>();

}
