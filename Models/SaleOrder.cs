namespace Models;

public class SaleOrder
{
    public string SRNo { get; set; }
    public DateTime SRDate { get; set; }
    public DateTime ExpectedDeliveryDate { get; set; }
    public string SupplierName { get; set; } //Supplier
    public string PaymentMethod { get; set; }
    public string Remarks { get; set; }
    public int GrossAmount { get; set; }
    public int DiscountAmount { get; set; }
    public int TaxRate { get; set; }
    public int TaxAmount { get; set; }
    public int NetAmount { get; set; }
}
