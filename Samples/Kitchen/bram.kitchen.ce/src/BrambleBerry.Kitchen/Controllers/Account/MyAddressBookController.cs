using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BrambleBerry.Kitchen.Extensions;
using BrambleBerry.Kitchen.Models.Account.MyAddressBook;
using BrambleBerry.Kitchen.Models.Workflow;
using Merchello.Core;
using Merchello.Core.Formatters;
using Merchello.Core.Gateways.Notification;
using Merchello.Core.Models;
using Merchello.Core.Services;
using Umbraco.Web.Models;


namespace BrambleBerry.Kitchen.Controllers.Account
{
    /// <summary>
    /// The contact page controller.
    /// </summary>
    /// <remarks>
    /// This controller is dependent on Merchello's <see cref="INotificationContext"/> so that we can use the <see cref="IPatternReplaceFormatter"/> and
    /// the back office configured SMTP settings
    /// </remarks>
    public class MyAddressBookController : BaseAccountController
    {
        private ICustomerService _customerService;

        public const string AddressErrorMessageKey = "AddressModel.Error";

        public MyAddressBookController() : this(MerchelloContext.Current.Services.CustomerService)
        {
        }

        public MyAddressBookController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        /// <summary>
        /// The default controller method
        /// </summary>
        /// <param name="model">
        /// The default <see cref="RenderModel"/>
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/> to render the contact page template view
        /// </returns>        
        public override ActionResult Index( RenderModel model )
        {
            var addresses = BuildModel<AddressesIndexModel>();

            var allAddresses = new List<AddressModel>();

            // On first load when developing the Customer is sometimes null?
            if (Customer == null || Customer.IsAnonymous) return RedirectToAction("Index", "Authorization");

            foreach (var address in Customer.Addresses)
            {
                allAddresses.Add( address.ToAddress() );
            }
            addresses.AllAddresses = allAddresses;
            
            return View( addresses );
        }

        public ActionResult DummyAddresses()
        {
            /* TODO: Needed some duff data, this does the trick nicely, needs removing though */
            if ( !Customer.Addresses.Any() )
            {
                var home = new AddressModel( Customer.Key )
                {
                    Alias = "Home",
                    Fullname = "Cheryl Wallace",
                    Address1 = "5555 Anywhere St",
                    Address2 = "",
                    City = "Bellingham",
                    Region = "WA",
                    PostalCode = "98225",
                    CountryCode = "US",
                    Phone = "(360) 867-5309",
                    IsExpressCheckoutEnabled = true
                };

                var work = new AddressModel( Customer.Key )
                {
                    Alias = "Work",
                    Fullname = "Cheryl Wallace",
                    Address1 = "222 Anywhere St",
                    Address2 = "Suite 22",
                    City = "Bellingham",
                    Region = "WA",
                    CountryCode = "US",
                    PostalCode = "98226",
                    Phone = "(360) 867-5310",
                    IsExpressCheckoutEnabled = false
                };

                Customer.SaveCustomerAddress( home.ToCustomerAddress() );
                Customer.SaveCustomerAddress( work.ToCustomerAddress() );
            }

            return RedirectToAction( "Index" );
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View( new AddressModel( Customer.Key ) );
        }

        [HttpGet]
        public ActionResult Edit( Guid id)
        {
            AddressModel address = Customer.Addresses.FirstOrDefault( x => x.Key == id ).ToAddress();

            if (address != null)
            {
                address.Countries =
                    Shipping.GetAllowedShipmentDestinationCountries().ToSelectListItems(address.CountryCode);
                var addressTypes = new List<SelectListItem>();
                addressTypes.Add(new SelectListItem()
                {
                    Selected = false,
                    Text = "Residential",
                    Value = AddressModel.AddressRole.Residential.ToString()
                });
                addressTypes.Add(new SelectListItem()
                {
                    Selected = false,
                    Text = "Business",
                    Value = AddressModel.AddressRole.Business.ToString()
                });
                address.AddressUsageRoles = addressTypes;
            }
            else
            {
                // TODO: What to do if no address found?
                TempData.Add( AddressErrorMessageKey, "No address could be found with that id" );
            }

            return View( address );
        }

        [HttpPost]
        public ActionResult Edit(AddressModel model)
        {
            if (!ModelState.IsValid)
            {
                return CurrentUmbracoPage();
            }
            
            Customer.SaveCustomerAddress(model.ToCustomerAddress());

            return View( model );
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Guid key)
        {
            var addresses = Customer.Addresses.ToList();
            addresses.RemoveAll(x => x == null);
            ICustomerAddress address = addresses.FirstOrDefault(x => x.Key == key);
            if (address != null)
            {
                Customer.DeleteCustomerAddress(address);
            }
            return RedirectToAction( "Index" );
        }

        /// <summary>
        /// Sets an address as the default address for the user
        /// </summary>
        /// <param name="id">The unique guid Key for this address</param>
        /// <returns></returns>
        public ActionResult SetDefaultAddress( Guid id )
        {
            var address = Services.CustomerService.GetAddressByKey( id );
            // Make sure we have an address and it belongs to this user
            if ( address != null && Customer.Addresses.FirstOrDefault( x=>x.Key == id ) != null ) {
                address.IsDefault = true;
                Services.CustomerService.Save(address);
            }

            return CurrentUmbracoPage();
        }
    }
}