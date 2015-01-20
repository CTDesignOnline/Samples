namespace BrambleBerry.Kitchen.Models.Checkout
{
    using System;

    /// <summary>
    /// Represents a quote from the shipping provider
    /// </summary>
    public class ShipMethodQuote
    {
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        public Guid Key { get; set; }

        /// <summary>
        /// Gets or sets the shipping method name.
        /// </summary>
        public string ShippingMethodName { get; set; }

        /// <summary>
        /// Gets or sets the rate.
        /// </summary>
        public decimal Rate { get; set; }
    }
}