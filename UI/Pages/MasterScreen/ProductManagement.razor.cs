using Models.AppModels;

namespace UI.Pages.MasterScreen;

public partial class ProductManagement
{
    [Inject]
    ISnackbar _Snackbar { get; set; }

    [Inject]
    IPartyRepoUI _partyRepoUI { get; set; }
    [Inject]
    IProductDetailsRepoUI _ProductDetailsRepoUI { get; set; }
    [Inject]
    NavigationManager Navigate { get; set; }


    #region Variables
    private bool _processing = false, AddPartyVisible = false;
    #endregion


    #region List
    List<Party> BrandList = new List<Party>();
    List<ProductDetails> ProductDetailsList = new List<ProductDetails>();
    #endregion


    #region Object
    AppUsers UserSession;
    Party party = new Party();
    ProductDetails Model = new ProductDetails();
    #endregion

    private DialogOptions dialogOptions = new() { MaxWidth = MaxWidth.Large, FullWidth = true, CloseButton = true, NoHeader = true, DisableBackdropClick = true, CloseOnEscapeKey = false };

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        try
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                //UserSession = await _localStorage.GetItemAsync<UserDetails>("User");
                //if (UserSession == null)
                //{
                //    navigation.NavigateTo("/signin");
                //}
                //else
                //{
                //    await OnInitializedAsync();
                //    StateHasChanged();
                //}
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

            if (UserSession != null || true)
            {
                BrandList = await _partyRepoUI.GetAll("Party/GetParties") ?? new List<Party>();
                BrandList = BrandList.Where(x => x.PartyType == "Vendor").ToList();

                ProductDetailsList = await _ProductDetailsRepoUI.GetAll("ProductDetails/GetProductDetails") ?? new List<ProductDetails>();

            }
            _processing = false;
        }
        catch (Exception ex)
        {
            UILogger.WriteLog(ex);
        }
        return;
    }

    async Task SaveAsync()
    {
        if (string.IsNullOrEmpty(Model.ItemName) || Model.PartyId <= 0)
        {
            _Snackbar.Add("Please fill all fields.", Severity.Error);
        }
        else
        {
            var result = Model.Id > 0 ? await _ProductDetailsRepoUI.Update("ProductDetails/Update", Model) : await _ProductDetailsRepoUI.Create("ProductDetails/Create", Model) ?? new ProductDetails();

            if (result.Id > 0)
            {
                await Task.Delay(3000);
                _Snackbar.Add("Saved successfully", Severity.Success);
                Navigate.NavigateTo("/ProductManagement", forceLoad: true);
            }
            else
            {
                _Snackbar.Add("There is some thing Went worng While Creating New Recoard", Severity.Error);
            }

        }
    }

    void CloseParty()
    {
        Model = new ProductDetails();
        party = new Party();
        AddPartyVisible = false;
    }

    private void GridtoCall(ProductDetails data)
    {
        Model = data;
        AddPartyVisible = true;
    }

    void onBrandSelect(Party pty)
    {
        try
        {
            if (pty != null)
            {
                party = pty;
                Model.PartyId = pty.Id;
            }
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    private async Task<IEnumerable<Party>> SearchBrand(string value)
    {
        // In real life use an asynchronous function for fetching data from an api.
        await Task.Delay(0);

        // if text is null or empty, don't return values (drop-down will not open)
        if (string.IsNullOrEmpty(value))
            return BrandList;
        return BrandList.Where(x => x.CompanyName.Contains(value, StringComparison.InvariantCultureIgnoreCase));
    }
}