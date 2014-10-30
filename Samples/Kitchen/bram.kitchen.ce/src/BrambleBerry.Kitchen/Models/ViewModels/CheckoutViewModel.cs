namespace BrambleBerry.Kitchen.Models.ViewModels
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The checkout view model.
    /// </summary>
    public class CheckoutViewModel : ViewModelBase
    {
        /// <summary>
        /// Gets or sets the customer key.
        /// </summary>
        public Guid CustomerKey { get; set; }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        public IEnumerable<BasketLineItem> Items { get; set; }

        /// <summary>
        /// Gets or sets the total basket price.
        /// </summary>
        public decimal TotalBasketPrice { get; set; }

        /// <summary>
        /// Gets or sets the receipt url.
        /// </summary>
        public string ReceiptUrl { get; set; }
    }
}