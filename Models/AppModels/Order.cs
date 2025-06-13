using System.ComponentModel.DataAnnotations.Schema;

namespace Models.AppModels;

public class Order : BaseEntity
{
    [Column(TypeName = "varchar(15)")]
    public string OrderNo { get; set; }
    
    public DateTime? OrderDate { get; set; } = DateTime.Now;
    
    public int? PartyId { get; set; } //Vendor
    public Party Party { get; set; } = new Party();
    
    [Column(TypeName = "varchar(25)")]
    public string PaymentMode { get; set; }
    public OrderTypes OType { get; set; } = new OrderTypes();
    public OrderStatus Status { get; set; } = new OrderStatus();
    public string OrderMode { get; set; }  //Local, Import, Export
    public double GrossAmount { get; set; }
    
    public double TaxRate { get; set; }
    
    public double TaxAmount { get; set; }
    
    public double NetAmount { get; set; }

    public bool IspaymentClear { get; set; }
    public string WalkinCustomer { get; set; }
    public List<OrderDetail> OrderDetail { get; set; } = new List<OrderDetail>();
    public List<OrderTransactions> OT { get; set; } = new List<OrderTransactions>();
}

public enum OrderTypes
{
    PurchaseOrder = 1,
    SaleOrder,
    ProductionOrder
}

public enum OrderStatus
{
    Opened = 1,
    Closed
}