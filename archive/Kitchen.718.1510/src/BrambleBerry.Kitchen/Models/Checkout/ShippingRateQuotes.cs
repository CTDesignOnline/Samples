namespace BrambleBerry.Kitchen.Models.Checkout
{
    using System.Collections.Generic;

    /// <summary>
    /// Model for filling a Ship Rate Quote drop down
    /// </summary>
    public class ShippingRateQuotes
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ShippingRateQuotes"/> class.
        /// </summary>
        public ShippingRateQuotes()
        {
            Quotes = new List<ShipMethodQuote>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShippingRateQuotes"/> class.
        /// </summary>
        /// <param name="quotes">
        /// The quotes.
        /// </param>
        public ShippingRateQuotes(IEnumerable<ShipMethodQuote> quotes)
        {
            Quotes = quotes;
        }

        /// <summary>
        /// Gets the quotes.
        /// </summary>
        public IEnumerable<ShipMethodQuote> Quotes { get; private set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        public string Status { get; set; }
    }
}