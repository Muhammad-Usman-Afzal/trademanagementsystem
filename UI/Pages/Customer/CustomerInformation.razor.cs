using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Models;
using MudBlazor;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace UI.Pages.Customer
{
    public partial class CustomerInformation
    {
        [Inject]
        ISnackbar _Snackbar { get; set; }

        [Inject]
        ICustomerInfoRepoUI _ICustomerInfoRepoUI { get; set; }

        CustomerInfo Model = new CustomerInfo();
        List<CustomerInfo> customerInfoList = new List<CustomerInfo>();

        void Save()
        {
            if (string.IsNullOrEmpty(Model.NameOfCustomer) ||
                string.IsNullOrEmpty(Model.Contact) ||
                string.IsNullOrEmpty(Model.Email) ||
                string.IsNullOrEmpty(Model.Address))
            {
                _Snackbar.Add("Please fill all fields.", Severity.Error);
            }
            else
            {
                var result = _ICustomerInfoRepoUI.Create("CustomerInfo/Create", Model);
                _Snackbar.Add("Saved successfully", Severity.Success);
                Model = new CustomerInfo();
         
            }

        }

    }
}