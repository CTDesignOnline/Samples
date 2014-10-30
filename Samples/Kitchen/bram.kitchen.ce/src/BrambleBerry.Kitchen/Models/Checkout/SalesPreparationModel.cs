namespace BrambleBerry.Kitchen.Models.Checkout
{
    using System;
    using System.Collections.Generic;
    using BrambleBerry.Kitchen.Models.Workflow;

    /// <summary>
    /// Model to fill in Basket().SalePreparation()
    /// </summary>
    public class SalesPreparationModel
    {
        /// <summary>
        /// Gets or sets the contact information
        /// </summary>
        public ContactInfoModel ContactInfo { get; set; }

        /// <summary>
        /// Gets or sets the shipping address.
        /// </summary>
        public CheckoutAddress ShippingAddress { get; set; }

        /// <summary>
        /// Gets or sets the billing address.
        /// </summary>
        public CheckoutAddress BillingAddress { get; set; }

        /// <summary>
        /// Gets or sets the payment method key.
        /// </summary>
        public Guid PaymentMethodKey { get; set; }

        /// <summary>
        /// Gets or sets the payment args.
        /// </summary>
        public IEnumerable<KeyValuePair<string, string>> PaymentArgs { get; set; }

        /// <summary>
        /// Gets or sets the ship method key.
        /// </summary>
        public Guid ShipMethodKey { get; set; }       
    }
}