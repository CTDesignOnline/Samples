namespace BrambleBerry.Kitchen.Models.Checkout
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// An address model specifically for the checkout API.
    /// </summary>
    public class CheckoutAddress
    {
        /// <summary>
        /// Gets or sets the full display name for this recipient
        /// </summary>
        public string Fullname { get; set; }

        /// <summary>
        /// Gets or sets the customer key.
        /// </summary>
        public Guid CustomerKey { get; set; }
        
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
        /// Gets or sets a value indicating whether the deliver is for a commercial business or not.
        /// TODO: Does this not link to AddressType below?
        /// TODO: RSS - no this is for the shipping provider.  But in the UI they want this as a drop down.
        /// </summary>
        public bool IsCommercial { get; set; }

        /// <summary>
        /// Gets or sets the locality.
        /// </summary>
        [DisplayName("City")]
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the company name or the recipient if there is one
        /// </summary>
        [DisplayName("Company Name")]
        public string Organization { get; set; }

        /// <summary>
        /// Gets or sets the phone number of the recipient that can be used by the delivery driver in case of problems.
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Gets or sets the postal code.
        /// </summary>
        [DisplayName("Postal Code")]
        public string PostalCode { get; set; }

        /// <summary>
        /// Gets or sets the region.
        /// </summary>
        [DisplayName("State")]
        public string Region { get; set; }
    }
}