using System.ComponentModel.DataAnnotations.Schema;

namespace Models.AppModels;

public class ProductionOrder : BaseEntity
{
    [Column(TypeName = "nvarchar(15)")]
    public string PONo { get; set; }
    public DateTime PODate { get; set; }
    [Column(TypeName = "varchar(50)")]
    public string ContractorName { get; set; } //Contractor
    [Column(TypeName = "nvarchar(15)")]
    public int ContactNo { get; set; }
    [Column(TypeName = "nvarchar(125)")]
    public string Address { get; set; }
    [Column(TypeName = "nvarchar(125)")]
    public string Details { get; set; }
    [Column(TypeName = "nvarchar(15)")]
    public int Rate { get; set; }
    [Column(TypeName = "varchar(20)")]
    public string PaymentMethod { get; set; }
    [Column(TypeName = "varchar(20)")]
    public string TransactionType { get; set; }
    [Column(TypeName = "varchar(20)")]
    public string Status { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    [Column(TypeName = "nvarchar(25)")]
    public bool UntilNextContract { get; set; }
    [Column(TypeName = "nvarchar(125)")]
    public string Remarks { get; set; }
}
