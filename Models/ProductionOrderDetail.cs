namespace Models;

public class ProductionOrderDetail
{
    public string DeliveredItem { get; set; }
    public string ReceivingItem { get; set; }
    public int ProductionQty { get; set; }
    public string NotSpeciffiedYet { get; set; }
}
