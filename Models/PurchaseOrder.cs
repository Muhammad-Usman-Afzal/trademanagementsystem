namespace Models;

public class PurchaseOrder
{
    public string PONo { get; set; }
    public DateTime PODate { get; set; }
    public DateTime ExpectedDeliveryDate { get; set; }
    public string VendorName { get; set; } //Vendor
    public string DeliveryFrom { get; set; }
    public string DeliveryTo { get; set; } //WareHouse
    public string PaymentMode { get; set; } 
    public string ShipmentType { get; set; }
    public int GrossAmount { get; set; }
    public int TaxRate { get; set; }
    public int TaxAmount { get; set; }
    public int NetAmount { get; set; }
}
