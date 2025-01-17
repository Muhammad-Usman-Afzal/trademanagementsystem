using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.AppModels;

public class PurchaseOrder:BaseEntity
{
    [Column(TypeName = "varchar(15)")]
    public string PONo { get; set; }
    public DateTime? PODate { get; set; } = DateTime.Now;
    [Column(TypeName = "varchar(30)")]
    public string VendorName { get; set; } //Vendor
    [Column(TypeName = "varchar(30)")] 
    public string DeliveryFrom { get; set; }
    [Column(TypeName = "varchar(20)")] 
    public string PaymentMode { get; set; }
    [Column(TypeName = "varchar(20)")] 
    public string ShipmentType { get; set; }
    [Column(TypeName = "nvarchar(15)")]
    public int GrossAmount { get; set; }
    [Column(TypeName = "nvarchar(15)")]
    public int TaxRate { get; set; }
    [Column(TypeName = "nvarchar(15)")]
    public int TaxAmount { get; set; }
    [Column(TypeName = "nvarchar(15)")]
    public int NetAmount { get; set; }
    public List<PurchaseOrderDetail> purchaseOrderDetail { get; set; }
}
