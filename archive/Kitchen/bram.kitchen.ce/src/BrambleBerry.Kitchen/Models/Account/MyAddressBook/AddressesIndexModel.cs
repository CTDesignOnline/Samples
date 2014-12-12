using System.Collections.Generic;
using BrambleBerry.Kitchen.Models.ViewModels;
using BrambleBerry.Kitchen.Models.Workflow;

namespace BrambleBerry.Kitchen.Models.Account.MyAddressBook
{
    public class AddressesIndexModel : ViewModelBase
    {
        public IEnumerable<AddressModel> BillingAddresses { get; set; }
        public IEnumerable<AddressModel> ShippingAddresses { get; set; }
        public IEnumerable<AddressModel> AllAddresses { get; set; }
        public bool IsExpressCheckoutEnabled { get; set; }
    }
}
