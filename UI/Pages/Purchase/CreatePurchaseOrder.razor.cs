using Models;

namespace UI.Pages.Purchase
{
    public partial class CreatePurchaseOrder
    {
        PurchaseOrder Model = new PurchaseOrder();
        
        VendorInfo vendorInfo = new VendorInfo();
        List<VendorInfo> vendorInfoList = new List<VendorInfo>();

        PurchaseOrderDetail purchaseOrderDetail = new PurchaseOrderDetail();
        List<PurchaseOrderDetail> purchaseOrderDetailList = new List<PurchaseOrderDetail>();
    }
}
