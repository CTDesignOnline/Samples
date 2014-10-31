using System.Globalization;
using Merchello.Core;

namespace BrambleBerry.Kitchen.Models.Workflow
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Web.Mvc;

    /// <summary>
    /// The address model.
    /// </summary>
    public class AddressModel
    {
        public enum AddressRole
        {
            Residential,
            Business
        }

        /// <summary>
        /// TODO: This should not be editable really, need a way to get this in possibly on the 
        /// constructor but we might not always know the address key when we need an address object?
        /// </summary>
        public Guid Key { get; set; }

        public AddressModel(Guid customerKey)
        {
            CustomerKey = customerKey;
        }

        public AddressModel(Guid customerKey, Guid addressKey)
        {
            CustomerKey = customerKey;
            Key = addressKey;
        }

        public Boolean IsExpressCheckoutEnabled { get; set; }

        /// <summary>
        /// Is this the users perferred address for checkouts etc.
        /// </summary>
        public Boolean IsDefaultAddress { get; set; }

        /// <summary>
        /// The full display name for this recipient
        /// </summary>
        public string Fullname { get; set; }

        /// <summary>
        /// Gets or sets the customer key.
        /// </summary>
        public Guid CustomerKey { get; private set; }

        /// <summary>
        /// The human friendly name for this address for reuse in the UI
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// Gets or sets the address 1.
        /// </summary>
        public string Address1 { get; set; }

        /// <summary>
        /// Gets or sets the address 2.
        /// </summary>
        public string Address2 { get; set; }

        /// <summary>
        /// Gets or sets the country code.
        /// </summary>
        public string CountryCode { get; set; }

        /// <summary>
        /// Gets or sets the email address of the recipient.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Sets the usage role of this address (ie is it a business or residential?)
        /// TODO: RSS - no this is for the shipping provider.  But in the UI they want this as a drop down.
        /// </summary>
        public AddressRole AddressUsageRole { get; set; }
        
        /// <summary>
        /// Gets or sets the locality.
        /// </summary>
        [DisplayName( "City" )]
        public string City { get; set; }
        /*
         * 
         * Not used for shipping address so commented out for now
        /// <summary>
        /// Gets or sets the company name or the recipient if there is one
        /// </summary>
        [DisplayName("Company Name")]
        public string Organization { get; set; }
        */
        /// <summary>
        /// Gets or sets the phone number of the recipient that can be used by the delivery driver in case of problems.
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Gets or sets the postal code.
        /// </summary>
        [DisplayName( "Postal Code" )]
        public string PostalCode { get; set; }

        /// <summary>
        /// Gets or sets the region.
        /// </summary>
        [DisplayName( "State" )]
        public string Region { get; set; }

        public Boolean IsDefault { get; set; }

        public IEnumerable<SelectListItem> Countries { get; set; }


        public IEnumerable<SelectListItem> AddressUsageRoles { get; set; }

        /// <summary>
        /// Gets or sets if this address is a billing, shipping address etc.
        /// </summary>


        /// <remarks>
        /// TODO: RSS - This is/was shipping or billing. 
        /// TODO: PD - Rusty, see TypeOfAddress below
        /// </remarks>
       // public string AddressType { get; set; }

        /// <remarks>
        /// The type of address that this is (ie billing or shipping)
        /// TODO: PD - Can't call this AddressType as it clashes with the enum of the same name?
        /// </remarks>
        public AddressType TypeOfAddress { get; set; }

        #region Helper booleans

        /// <summary>
        /// Is the deliver for a commercial business?
        /// </summary>
        public bool IsCommercial { get { return AddressUsageRole == AddressRole.Business; } }

        /// <summary>
        /// Is the deliver for a residential home?
        /// </summary>
        public bool IsResidential { get { return AddressUsageRole == AddressRole.Residential; } }

        /// <summary>
        /// Is this address a billing address?
        /// </summary>
        public bool IsBillingAddress { get { return TypeOfAddress == AddressType.Billing; } }

        /// <summary>
        /// Is this address a shipping address?
        /// </summary>
        public bool IsShippingAddress { get { return TypeOfAddress == AddressType.Shipping; } }


        public string CountryName
        {
            get { return new RegionInfo(CountryCode).DisplayName; }
        }

        #endregion
    }
}