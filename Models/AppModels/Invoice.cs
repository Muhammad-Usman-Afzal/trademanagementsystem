using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.AppModels;

public class Invoice : BaseEntity
{
    [Required]
    [MaxLength(50)]
    public string InvoiceNumber { get; set; }

    public DateTime? InvoiceDate { get; set; } = DateTime.Now;

    [Required]
    public InvoiceType InvoiceType { get; set; }

    public PaymentStatus PaymentStatus { get; set; }
	[Column(TypeName = "varchar(25)")]
	public string PaymentMode { get; set; } //Cash, Online, Check

	public double TotalAmount { get; set; }

    public double Discount { get; set; }

    public double TaxAmount { get; set; }

    public double NetAmount { get; set; }
    public int PaidAmount { get; set; }
    public int TaxRate { get; set; }

	public string CustomerName { get; set; }
    public int? CustomerId { get; set; }
    public Party Customer { get; set; }

    public virtual List<InvoiceDetails> InvoiceDetails { get; set; } = new List<InvoiceDetails>();
}

public enum InvoiceType
{
    Sale = 1,
    Return = 2
}

public enum PaymentStatus
{
    Pending = 0,
    Paid = 1,
    Partial = 2
}

public enum ItemReturnStatus
{
    NotReturned = 0,
    Returned = 1,
    Damaged = 2
}