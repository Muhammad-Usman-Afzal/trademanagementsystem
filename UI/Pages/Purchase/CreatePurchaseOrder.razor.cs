using Microsoft.AspNetCore.Components;
using Models;
using Models.AppModels;
using MudBlazor;
using System.Reflection.PortableExecutable;

namespace UI.Pages.Purchase
{
    public partial class CreatePurchaseOrder
    {
        #region DI

        [Inject]
        public ISnackbar snackbar { get; set; }
        [Inject]
        public IJSRuntime _jS { get; set; }
        [Inject]
        public ProtectedLocalStorage _localStorage { get; set; }
        [Inject]
        public NavigationManager navigation { get; set; }
        [Inject]
        public IPartyRepoUI _partyRepoUI { get; set; }
        [Inject]
        public IPurchaseOrderRepoUI _purchaseOrderRepoUI { get; set; }
        [Inject]
        public IOrderRepoUI _orderRepoUI { get; set; }
        [Inject]
        public IProductDetailsRepoUI _ProductDetailsRepoUI { get; set; }
        #endregion

        #region Variables

        //PurchaseOrder Model = new PurchaseOrder();
        Order Model = new Order();
        Party party = new Party();
        List<Party> parties = new List<Party>();
        List<Party> Vanders = new List<Party>();
        ProductDetails ProDetalil = new ProductDetails();
        List<ProductDetails> ProDetalilList = new List<ProductDetails>();
        List<ProductDetails> CompanyDetalilList = new List<ProductDetails>();
        //PurchaseOrderDetail purchaseOrderDetail = new PurchaseOrderDetail();
        OrderDetail purchaseOrderDetail = new OrderDetail();
        List<PurchaseOrderDetail> purchaseOrderDetailList = new List<PurchaseOrderDetail>();
        AppUsers UserSession = new AppUsers();

        private bool _processing = false, _productdetails = true, _Party;

        #endregion

        #region Functions

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            try
            {
                await base.OnAfterRenderAsync(firstRender);

                if (firstRender)
                {
                    var userSession = await _localStorage.GetAsync<AppUsers>("User");
                    UserSession = userSession.Value ?? new AppUsers();

                    if (UserSession.Id == 0)
                    {
                        navigation.NavigateTo("/signin");
                    }
                    else
                    {
                        await OnInitializedAsync();
                        StateHasChanged();
                    }
                }
            }
            catch (Exception ex) { UILogger.WriteLog(ex); }
        }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                _processing = true;
                _ = InvokeAsync(StateHasChanged);

                if (UserSession.Id > 0)
                {
                    parties = await _partyRepoUI.GetAll("Party/GetPartiesByType?partyType=Vendor") ?? new List<Party>();

                    //parties = parties.Where(x => x.PartyType == "Vendor").ToList();

                    ProDetalilList = await _ProductDetailsRepoUI.GetAll("ProductDetails/GetProductDetails") ?? new List<ProductDetails>();

                    Model.TaxRate = 18;
                    Model.OrderNo = await CreatePONumberAsync();
                }
                _processing = false;
            }
            catch (Exception ex)
            {
                UILogger.WriteLog(ex);
            }

            return;
        }

        async void Save()
        {
            if (IsValidate())
            {
                Model.OType = OrderTypes.PurchaseOrder;
                Model.Status = OrderStatus.Opened;
                var res = Model.Id > 0 ? await _orderRepoUI.Create("Order/Update", Model) : await _orderRepoUI.Create("Order/Create", Model) ?? new Order();

                if (res != null && res.Id > 0)
                {
                    snackbar.Add("Purchase Order has been saved successfully", Severity.Success);
                    Model = new Order();
                    navigation.NavigateTo("/PurchaseOrder");
                }
                else
                {
                    snackbar.Add("An error occurred while creating Purchase Order", Severity.Error);
                }
            }
            else
            {
                snackbar.Add("Please fill all required field(s)", Severity.Error);
            }
        }

        bool IsValidate()
        {
            return
                string.IsNullOrEmpty(Model.OrderDate.ToString()) || string.IsNullOrEmpty(Model.OrderNo) ||
                Model.PartyId < 0 || string.IsNullOrEmpty(Model.PaymentMode) || string.IsNullOrEmpty(Model.OrderMode) ||
                Model.OrderDetail.Count < 0
                ? false : true;
        }

        private async Task<IEnumerable<Party>> SearchVendor(string value)
        {
            await Task.Delay(0);
            if (string.IsNullOrEmpty(value))
                return parties;
            return parties.Where(x => !string.IsNullOrEmpty(x.FocalPersonName) ? x.FocalPersonName.Contains(value, StringComparison.InvariantCultureIgnoreCase) : false);
        }
        private async Task<IEnumerable<ProductDetails>> SearchProDetails(string value)
        {
            await Task.Delay(0);
            if (string.IsNullOrEmpty(value))
                return CompanyDetalilList;
            return CompanyDetalilList.Where(x => !string.IsNullOrEmpty(x.ItemName) ? x.ItemName.Contains(value, StringComparison.InvariantCultureIgnoreCase) : false);
        }

        void OnPartyChanged(Party Value)
        {
            try
            {
                if (Value != null)
                {
                    party = Value;
                    Model.PartyId = Value.Id;
                    Model.Party = Value;
                    CompanyDetalilList = ProDetalilList.Where(x => x.PartyId == Model.PartyId && x.Isprocessed == false).ToList();

                    if(CompanyDetalilList.Count > 0)
                    {
                        _productdetails = false;
                    }
                    else
                    {
                        _productdetails = true;
                        ProDetalil = new ProductDetails();
                    }

                    _productdetails = CompanyDetalilList.Count > 0 ?  false : true;

                }
            }
            catch (Exception ex) { UILogger.WriteLog(ex); }
        }

        void OnProductChanged(ProductDetails Value)
        {
            try
            {
                if (Value != null)
                {
                    ProDetalil = Value;
                    purchaseOrderDetail.ItemId = Value.Id;
                    purchaseOrderDetail.Item = Value;
                }
            }
            catch (Exception ex) { UILogger.WriteLog(ex); }
        }


        void AddItem(OrderDetail POdetail)
        {
            if (_productdetails == false)
            {
                if (POdetail.Qty > 0 && POdetail.Rate > 0 && !String.IsNullOrEmpty(POdetail.Unit))
                {
                    var existingItem = Model.OrderDetail.FirstOrDefault(x => x.ItemId == POdetail.ItemId);
                    
                    if (existingItem != null && existingItem.Unit == POdetail.Unit)
                    {
                        // Update the quantity
                        existingItem.Qty += POdetail.Qty;

                        // Update the rate if it is different
                        if (existingItem.Rate != POdetail.Rate)
                        {
                            existingItem.Rate = POdetail.Rate;
                        }
                    }
                    else
                    {
                        // Add new item if it does not exist
                        Model.OrderDetail.Add(POdetail);
                    }

                    purchaseOrderDetail = new OrderDetail();
                    ProDetalil = new ProductDetails();
                    CalculateTotal();

                    StateHasChanged();

                    if (Model.OrderDetail.Count > 0)
                    {
                        _Party = true;
                    }
                }
                else
                {
                    snackbar.Add("Kindly fill all Required Data", Severity.Warning);
                }
            }
            else
            {
                snackbar.Add("Please Select Vendor first", Severity.Warning);
            }
        }

        void RemoveItem(OrderDetail POdetail)
        {
                Model.OrderDetail.Remove(POdetail);
        }

        //void UpdateItem(OrderDetail POD)
        //{
        //    purchaseOrderDetail.Qty = POD.Qty;
        //    purchaseOrderDetail.Rate = POD.Rate;
        //    purchaseOrderDetail.Unit = POD.Unit;
        //    ProDetalil = POD.Item;
        //}

        void CalculateTotal()
        {
            if (Model.OrderDetail.Count > 0)
            {
                Model.GrossAmount = Model.OrderDetail.Sum(x => x.Rate * x.Qty);
                Model.NetAmount = Model.GrossAmount - Model.TaxAmount;
            }
            if (Model.TaxRate > 0 && Model.GrossAmount > 0)
            {
                Model.TaxAmount = Model.GrossAmount * Model.TaxRate / 100;
                Model.NetAmount = Model.GrossAmount + Model.TaxAmount;
            }
        }

        public async Task<string> CreatePONumberAsync()
        {
            var lastOrderNo = await _orderRepoUI.GetSingleByColumnAsync($"Order/GetOrderNumberByType?orderType={OrderTypes.PurchaseOrder}");

            if (!string.IsNullOrEmpty(lastOrderNo) && lastOrderNo.Contains("PO-"))
            {
                string[] parts = lastOrderNo.Split('-');

                if (parts.Length == 2 && int.TryParse(parts[1], out int number))
                {
                    number++;
                    string newNumber = number.ToString($"D{parts[1].Length}");

                    return $"{parts[0]}-{newNumber}";
                }
            }

            return "PO-0001"; // First order case
        }


        #endregion
    }
}
