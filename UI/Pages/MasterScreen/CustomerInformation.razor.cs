using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Models;
using MudBlazor;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace UI.Pages.MasterScreen
{
    public partial class CustomerInformation
    {
        [Inject]
        ISnackbar _Snackbar { get; set; }

        [Inject]
        ICustomerInfoRepoUI _ICustomerInfoRepoUI { get; set; }

        #region Variables

        #endregion

        CustomerInfo Model = new CustomerInfo();
        List<CustomerInfo> customerInfoList = new List<CustomerInfo>();

        void Save()
        {
            if (string.IsNullOrEmpty(Model.CompanyName) || string.IsNullOrEmpty(Model.CompanyContact) || string.IsNullOrEmpty(Model.CompanyEmail) || string.IsNullOrEmpty(Model.CompanyAddress)
                || string.IsNullOrEmpty(Model.FocalPersonName) || string.IsNullOrEmpty(Model.FocalPersonContact) || string.IsNullOrEmpty(Model.FocalPersonEmail))
            {
                _Snackbar.Add("Please fill all fields.", Severity.Error);
            }
            else
            {
                var result = _ICustomerInfoRepoUI.Create("CustomerInfo/Create", Model);

                if (result.Id > 0)
                {
                    _Snackbar.Add("Saved successfully", Severity.Success);
                    Model = new CustomerInfo();
                }
                else
                {
                    _Snackbar.Add("There is some thing Went worng While Creating New Recoard", Severity.Error);
                }

            }

        }

    }
}