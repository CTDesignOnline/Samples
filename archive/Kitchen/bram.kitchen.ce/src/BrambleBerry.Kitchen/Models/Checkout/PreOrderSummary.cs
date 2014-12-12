namespace BrambleBerry.Kitchen.Models.Checkout
{
    /// <summary>
    /// The pre order summary.
    /// </summary>
    public class PreOrderSummary
    {
        /// <summary>
        /// Gets or sets the item total.
        /// </summary>
        public decimal ItemTotal { get; set; }

        /// <summary>
        /// Gets or sets the shipping total.
        /// </summary>
        public decimal ShippingTotal { get; set; }

        /// <summary>
        /// Gets or sets the tax total.
        /// </summary>
        public decimal TaxTotal { get; set; }

        public decimal TotalDiscounts { get; set; }

        /// <summary>
        /// Gets or sets the invoice total.
        /// </summary>
        public decimal InvoiceTotal { get; set; }
    }
}