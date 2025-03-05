using Microsoft.AspNetCore.Components.Web;
using Models.AppModels;

namespace UI.Pages.MasterScreen;

public partial class PartyManagement
{
    [Inject]
    ISnackbar _Snackbar { get; set; }
    [Inject]
    public ProtectedLocalStorage _localStorage { get; set; }
    [Inject]
    IPartyRepoUI _partyRepoUI { get; set; }
    [Inject]
    NavigationManager Navigate { get; set; }
    [Inject]
    IDialogService dialogService { get; set; }


    #region Variables
    private bool _processing = false, AddPartyVisible = false, DisableContolle = false, isContactInvalid = false, isEmailInvalid = false,
        isNTNInvalid = false, isSTRInvalid = false, isCompanyContactInvalid = false, isCompanyEmailInvalid = false, showRegConfirmDialog = false;
    int compNumber, FPersonNumber, NTN, STRN;
    //private string contactPattern = @"^\+\d{1,3}\d{7,14}$";
    private string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
    private string ntnPattern = @"^\d{7}-\d{1}$";
    private string strPattern = @"^\d{2}-\d{2}-\d{4}-\d{3}-\d{2}$";
    private string contactPattern = @"^\+?[0-9\s\-()]{7,13}$";  // Allows international formats (7-20 digits, optional + at start)

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
                var userSession = await _localStorage.GetAsync<AppUsers>("User");
                UserSession = userSession.Value ?? new AppUsers();

                if (UserSession.Id == 0)
                {
                    Navigate.NavigateTo("/signin");
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

            if (UserSession != null)
            {
                customerInfoList = await _partyRepoUI.GetAll("Party/GetParties") ?? new List<Party>();
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
        try
        {
            DisableContolle = true;
            await InvokeAsync(StateHasChanged);
            await Task.Delay(1);

            if (await InsurtValidateAsync())
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
            else
            {
                _Snackbar.Add("Please fill all fields properly.", Severity.Warning);
            }
            DisableContolle = false;
            StateHasChanged();
        }
        catch (Exception ex)
        {
            _Snackbar.Add($"{ex}", Severity.Error);
            _Snackbar.Add("There is some thing Went worng, Kindly contact to support ", Severity.Error);
            throw;
        }
    }

    void CloseParty()
    {
        Model = new Party();
        AddPartyVisible = false;
    }

    private async Task<bool> InsurtValidateAsync()
    {
        if (Model.Id>0 && !String.IsNullOrEmpty(Model.NTN) && !String.IsNullOrEmpty(Model.STRNo) && Model.IsRegisterd == false)
        {
            showRegConfirmDialog = true;
            bool confirm = await ShowConfirmationDialog();
            if (!confirm)
            {
                return false;
            }
        }
        if (Model.IsRegisterd)
        {
            if (isContactInvalid || isEmailInvalid || isNTNInvalid || isSTRInvalid || isCompanyContactInvalid || isCompanyEmailInvalid || String.IsNullOrEmpty(Model.PartyType)
                || String.IsNullOrEmpty(Model.CompanyName) || String.IsNullOrEmpty(Model.CompanyAddress) || String.IsNullOrEmpty(Model.FocalPersonName))
            {
                return false;
            }
            else
            {
                return true;
            }

        }
        else
        {
            Model.NTN = null;
            Model.STRNo = null;
            if (isContactInvalid || isEmailInvalid || isCompanyContactInvalid || isCompanyEmailInvalid || String.IsNullOrEmpty(Model.PartyType)
                || String.IsNullOrEmpty(Model.CompanyName) || String.IsNullOrEmpty(Model.CompanyAddress) || String.IsNullOrEmpty(Model.FocalPersonName))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
    private void GridtoCall(Party data)
    {
        Model = data;
        AddPartyVisible = true;
    }

    private void ValidateContact(FocusEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(Model.FocalPersonContact) ||
            !System.Text.RegularExpressions.Regex.IsMatch(Model.FocalPersonContact, @"^\+\d{1,3}\d{7,14}$"))
        {
            isContactInvalid = true;
        }
        else
        {
            isContactInvalid = false;
        }
    }

    private void ValidateEmail(FocusEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(Model.FocalPersonEmail) ||
            !System.Text.RegularExpressions.Regex.IsMatch(Model.FocalPersonEmail, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
        {
            isEmailInvalid = true;
        }
        else
        {
            isEmailInvalid = false;
        }
    }

    private void ValidateNTN(FocusEventArgs e)
    {
        isNTNInvalid = string.IsNullOrWhiteSpace(Model.NTN) ||
                      !System.Text.RegularExpressions.Regex.IsMatch(Model.NTN, ntnPattern);
    }

    private void ValidateSTR(FocusEventArgs e)
    {
        isSTRInvalid = string.IsNullOrWhiteSpace(Model.STRNo) ||
                      !System.Text.RegularExpressions.Regex.IsMatch(Model.STRNo, strPattern);
    }
    private void ValidateCompanyContact(FocusEventArgs e)
    {
        isCompanyContactInvalid = string.IsNullOrWhiteSpace(Model.CompanyContact) ||
                                  !System.Text.RegularExpressions.Regex.IsMatch(Model.CompanyContact, contactPattern);
    }

    private void ValidateCompanyEmail(FocusEventArgs e)
    {
        isCompanyEmailInvalid = string.IsNullOrWhiteSpace(Model.CompanyEmail) ||
                        !System.Text.RegularExpressions.Regex.IsMatch(Model.CompanyEmail, emailPattern);
    }

    private async Task<bool> ShowConfirmationDialog()
    {
        var parameters = new DialogParameters
        {
            { "ContentText", $"Are you sure that {Model.CompanyName} is not registered with the FBR? If you confirm, the NTN and STRN will be removed. Do you want to proceed?" },
            { "ButtonText", "Proceed" },
            { "CancelButtonText", "Wait" }
        };

        var dialog = dialogService.Show<ConfirmationDialog>("Confirm Action", parameters);
        var result = await dialog.Result;

        return !result.Cancelled; // Return true if user confirmed, false if canceled
    }


}