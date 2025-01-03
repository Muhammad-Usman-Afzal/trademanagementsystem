namespace Models;

public class ProductionOrder
{
    public string PONo { get; set; }
    public DateTime PODate { get; set; }
    public string ContractorName { get; set; } //Contractor
    public int ContactNo { get; set; }
    public string Address { get; set; }
    public string Details { get; set; }
    public int Rate { get; set; }
    public string PaymentMethod { get; set; }
    public string TransactionType { get; set; }
    public string Status { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool UntilNextContract { get; set; }
    public string Remarks { get; set; }
}
