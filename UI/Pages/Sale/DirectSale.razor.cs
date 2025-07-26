using Models.AppModels;
using UI.Repositories;

namespace UI.Pages.Sale
{
	public partial class DirectSale
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
		public IInvoiceRepoUI _invoiceRepoUI { get; set; }
		[Inject]
		public IInvoiceDetailsRepoUI _invoiceDetailsRepoUI { get; set; }
		[Inject]
		public IProductDetailsRepoUI _ProductDetailsRepoUI { get; set; }
		[Inject]
		public IOrderTransactionsRepoUI _transactionsRepoUI { get; set; }
		[Inject]
		IStockTransactionsRepoUI _stockTransactionsRepoUI { get; set; }

		#endregion


		AppUsers UserSession = new AppUsers();
		Invoice Model = new Invoice();
		InvoiceDetails ModelDetails = new InvoiceDetails();
		StockTransactions STransaction = new StockTransactions();
		Party Customer = new Party();
		Party Brand = new Party();

		List<Party> Brands = new List<Party>();
		List<Party> Customers = new List<Party>();

		ProductDetails ProDetalil = new ProductDetails();
		List<ProductDetails> CompanyProducts = new List<ProductDetails>();
		List<StockTransactions> STransactionList = new List<StockTransactions>();
		List<ItemStockSummaryDTO> StockSummary = new List<ItemStockSummaryDTO>();
		List<ItemStockSummaryDTO> FilteredStockSummary = new List<ItemStockSummaryDTO>();

		private bool _processing = false, _productdetails = true, _IsItemSelected = false, Iswharehouseenable = true;

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
					Customers = await _partyRepoUI.GetAll("Party/GetPartiesByType?partyType=Customer") ?? new List<Party>();
					Brands = await _partyRepoUI.GetAll("Party/GetPartiesByType?partyType=Vendor") ?? new List<Party>();
					StockSummary = await _stockTransactionsRepoUI.GetItemWiseStock("StockTransactions/GetItemWiseStock") ?? new List<ItemStockSummaryDTO>();
					Model.TaxRate = 0;
					Model.InvoiceDate = DateTime.Now;
					Model.InvoiceNumber = await CreateSONumberAsync();
				}
				_processing = false;
			}
			catch (Exception ex)
			{
				UILogger.WriteLog(ex);
			}

			return;
		}

		public async Task<string> CreateSONumberAsync()
		{
			var lastOrderNo = await _invoiceRepoUI.GetSingleByColumnAsync($"Invoice/GetInvoicesByType?InvoiceType={InvoiceType.Sale}");

			if (!string.IsNullOrEmpty(lastOrderNo) && lastOrderNo.Contains("INV-"))
			{
				string[] parts = lastOrderNo.Split('-');

				if (parts.Length == 2 && int.TryParse(parts[1], out int number))
				{
					number++;
					string newNumber = number.ToString($"D{parts[1].Length}");

					return $"{parts[0]}-{newNumber}";
				}
			}

			return "INV-0001"; // First order case
		}

		private async Task<IEnumerable<Party>> SearchCustomers(string value)
		{
			await Task.Delay(0);
			if (string.IsNullOrEmpty(value))
				return Customers;
			return Customers.Where(x => !string.IsNullOrEmpty(x.FocalPersonName) ? x.FocalPersonName.Contains(value, StringComparison.InvariantCultureIgnoreCase) : false);
		}

		Task OnCustomerChanged(Party Value)
		{
			try
			{
				if (Value != null)
				{
					Customer = Value;
					Model.CustomerId = Value.Id;
					Model.Customer = Value;

					//CompanyProducts = await _ProductDetailsRepoUI.GetAll($"Party/GetPartiesByType?partyId={party.Id}") ?? new List<ProductDetails>();
					//if (CompanyProducts.Count > 0)
					//{
					//    _productdetails = false;
					//}
					//else
					//{
					//    _productdetails = true;
					//    ProDetalil = new ProductDetails();
					//}

				}
			}
			catch (Exception ex) { UILogger.WriteLog(ex); }

			return Task.CompletedTask;
		}

		private async Task<IEnumerable<Party>> SearchBrands(string value)
		{
			await Task.Delay(0);
			if (string.IsNullOrEmpty(value))
				return Brands;
			return Brands.Where(x => !string.IsNullOrEmpty(x.FocalPersonName) ? x.FocalPersonName.Contains(value, StringComparison.InvariantCultureIgnoreCase) : false);
		}

		async Task OnBrandsChangedAsync(Party Value)
		{
			try
			{
				if (Value != null)
				{
					Brand = Value;

					CompanyProducts = await _ProductDetailsRepoUI.GetAll($"ProductDetails/GetProductDetailsByParty?partyId={Brand.Id}") ?? new List<ProductDetails>();
					if (CompanyProducts.Count > 0)
					{
						_productdetails = false;
					}
					else
					{
						_productdetails = true;
						ProDetalil = new ProductDetails();
					}

				}
			}
			catch (Exception ex) { UILogger.WriteLog(ex); }
		}

		private async Task<IEnumerable<ProductDetails>> SearchProDetails(string value)
		{
			await Task.Delay(0);
			if (string.IsNullOrEmpty(value))
				return CompanyProducts;
			return CompanyProducts.Where(x => !string.IsNullOrEmpty(x.ItemName) ? x.ItemName.Contains(value, StringComparison.InvariantCultureIgnoreCase) : false);
		}

		void OnProductChanged(ProductDetails Value)
		{
			try
			{
				if (Value != null)
				{
					ProDetalil = Value;
					ModelDetails.ProductId = Value.Id;
					ModelDetails.Product = Value;
					FilteredStockSummary = StockSummary.Where(x => x.ItemId == Value.Id).ToList();
					if (FilteredStockSummary.Count > 0)
					{
						_IsItemSelected = true;
						Iswharehouseenable = false;
					}
					else
					{
						_IsItemSelected = false;
						Iswharehouseenable = true;
						FilteredStockSummary = new List<ItemStockSummaryDTO>();
						ModelDetails.DispatchLocation = "";
					}
					_ = InvokeAsync(StateHasChanged);
				}
				else
				{
					_IsItemSelected = false;
				}
			}
			catch (Exception ex) { UILogger.WriteLog(ex); }
		}

		void AddItem(InvoiceDetails InvDetails)
		{
			if (_productdetails == false)
			{
				if (ModelDetails.Quantity > 0 && ModelDetails.UnitPrice > 0)
				{
					if (ModelDetails.Quantity > FilteredStockSummary.Where(x => x.ItemId == ModelDetails.ProductId && x.Warehouse == ModelDetails.DispatchLocation).Select(a => a.CurrentStock).FirstOrDefault())
					{
						snackbar.Add("You are not allowed to exceed the limit.", Severity.Warning);
					}
					else
					{
						var existingItem = Model.InvoiceDetails.FirstOrDefault(x => x.ProductId == ModelDetails.ProductId);

						if (existingItem != null && existingItem.UnitPrice == ModelDetails.UnitPrice)
						{
							// Update the quantity
							existingItem.Quantity += ModelDetails.Quantity;

							// Update the rate if it is different
							if (existingItem.UnitPrice != ModelDetails.UnitPrice)
							{
								existingItem.UnitPrice = ModelDetails.UnitPrice;
							}
						}
						else
						{
							// Add new item if it does not exist
							Model.InvoiceDetails.Add(ModelDetails);
						}

						ModelDetails = new InvoiceDetails();
						ProDetalil = new ProductDetails();
						Brand = new Party();
						_IsItemSelected = false;
						CalculateTotal();
						StateHasChanged();

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
		void RemoveItem(InvoiceDetails invDetails)
		{
			Model.InvoiceDetails.Remove(invDetails);
		}
		void CalculateTotal()
		{
			if (Model.InvoiceDetails.Count > 0)
			{

				Model.TotalAmount = Model.InvoiceDetails.Sum(x => x.UnitPrice * x.Quantity);


				if (Model.TaxRate > 0)
				{
					Model.TaxAmount = Model.TotalAmount * Model.TaxRate / 100;
				}
				else
				{
					Model.TaxAmount = 0;
				}

				Model.NetAmount = Model.TotalAmount + Model.TaxAmount;
				
				if (Model.PaidAmount > 0)
				{
					Model.Discount = Model.NetAmount - Model.PaidAmount;
				}
			}
		}

		bool IsValidate()
		{
			return
				string.IsNullOrEmpty(Model.InvoiceDate.ToString()) || string.IsNullOrEmpty(Model.InvoiceNumber) ||
				string.IsNullOrEmpty(Model.CustomerName) || string.IsNullOrEmpty(Model.PaymentMode) ||
				Model.InvoiceDetails.Count < 0
				? false : true;
		}

		async void Save()
		{
			try
			{
				_processing = true;
				StateHasChanged();

				if (IsValidate())
				{
					Model.InvoiceType = InvoiceType.Sale;
					Model.PaymentStatus = PaymentStatus.Paid;
					var res = Model.Id > 0 ? await _invoiceRepoUI.Create("Invoice/Update", Model) : await _invoiceRepoUI.Create("Invoice/Create", Model) ?? new Invoice();

					if (res.Id > 0)
					{
						var stockTransactionsList = new List<StockTransactions>();

						foreach (var trans in Model.InvoiceDetails.DistinctBy(x => x.ProductId))
						{
							stockTransactionsList.Add(new StockTransactions
							{
								TransectionDate = (DateTime)Model.InvoiceDate,
								StockOut = trans.Quantity,
								StockType = StockTransectionTypes.Sale,
								ItemId = trans.ProductId,
								Warehouse = trans.DispatchLocation,
								ReferenceNumber = Model.InvoiceNumber,
							});
						}

						await _stockTransactionsRepoUI.BulkInsert("StockTransactions/BulkInsert", stockTransactionsList);

						snackbar.Add("Invoice has been saved successfully", Severity.Success);
						Model = new Invoice();
						navigation.NavigateTo("/DirectSale", forceLoad: true);

					}
					else
					{
						snackbar.Add("An error occurred while creating Sale Order", Severity.Error);
					}
				}
				else
				{
					snackbar.Add("Please fill all required field(s)", Severity.Error);
				}
			}

			catch (Exception ex)
			{
				snackbar.Add($"Error: {ex.Message}", Severity.Error);
			}
			finally
			{
				_processing = false;
				StateHasChanged();
			}
		}

		private void AmountRecive()
		{

			//Model.Discount = Model.NetAmount - Model.PaidAmount;
			CalculateTotal();
		}
	}
}
