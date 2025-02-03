using Models.AppModels;

namespace UI.Pages.MasterScreen;

public partial class PartyManagement
{
    [Inject]
    ISnackbar _Snackbar { get; set; }

    [Inject]
    IPartyRepoUI _partyRepoUI { get; set; }
    [Inject]
    NavigationManager Navigate { get; set; }


    #region Variables
    private bool _processing = false, AddPartyVisible = false;
    #endregion


    #region List
    List<Party> customerInfoList = new List<Party>();
    #endregion


    #region Object
    AppUsers UserSession;
    Party Model = new Party();
    #endregion

    private DialogOptions dialogOptions = new() { FullScreen = true, CloseButton = true, NoHeader = true, DisableBackdropClick = true, CloseOnEscapeKey = false };

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
            customerInfoList = await _partyRepoUI.GetAll("Party/GetParties") ?? new List<Party>();

            if (UserSession != null)
            {

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
        if (string.IsNullOrEmpty(Model.CompanyName) || string.IsNullOrEmpty(Model.CompanyContact) || string.IsNullOrEmpty(Model.CompanyEmail) || string.IsNullOrEmpty(Model.CompanyAddress)
            || string.IsNullOrEmpty(Model.FocalPersonName) || string.IsNullOrEmpty(Model.FocalPersonContact) || string.IsNullOrEmpty(Model.FocalPersonEmail) || string.IsNullOrEmpty(Model.PartyType))
        {
            _Snackbar.Add("Please fill all fields.", Severity.Error);
        }
        else
        {
            var result = Model.Id > 0 ? await _partyRepoUI.Update("Party/Update", Model) : await _partyRepoUI.Create("Party/Create", Model) ?? new Party();

            if (result.Id > 0)
            {
                _Snackbar.Add("Saved successfully", Severity.Success);
                Navigate.NavigateTo("/Party", forceLoad: true);
            }
            else
            {
                _Snackbar.Add("There is some thing Went worng While Creating New Recoard", Severity.Error);
            }

        }

    }

    void CloseParty()
    {
        Model = new Party();
        AddPartyVisible = false;
    }

    private void GridtoCall(Party data)
    {
        Model = data;
        AddPartyVisible = true;
    }
}