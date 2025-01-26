namespace UI.Pages.MasterScreen;

public partial class PartyManagement
{
    [Inject]
    ISnackbar _Snackbar { get; set; }

    [Inject]
    IPartyRepoUI _partyRepoUI { get; set; }

    #region Variables

    #endregion

    Party Model = new Party();
    List<Party> customerInfoList = new List<Party>();

    void Save()
    {
        if (string.IsNullOrEmpty(Model.CompanyName) || string.IsNullOrEmpty(Model.CompanyContact) || string.IsNullOrEmpty(Model.CompanyEmail) || string.IsNullOrEmpty(Model.CompanyAddress)
            || string.IsNullOrEmpty(Model.FocalPersonName) || string.IsNullOrEmpty(Model.FocalPersonContact) || string.IsNullOrEmpty(Model.FocalPersonEmail))
        {
            _Snackbar.Add("Please fill all fields.", Severity.Error);
        }
        else
        {
            var result = _partyRepoUI.Create("Party/Create", Model);

            if (result.Id > 0)
            {
                _Snackbar.Add("Saved successfully", Severity.Success);
                Model = new Party();
            }
            else
            {
                _Snackbar.Add("There is some thing Went worng While Creating New Recoard", Severity.Error);
            }

        }

    }

}