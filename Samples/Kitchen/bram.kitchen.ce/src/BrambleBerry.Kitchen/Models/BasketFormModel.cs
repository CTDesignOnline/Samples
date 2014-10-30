namespace BrambleBerry.Kitchen.Models
{
    /// <summary>
    /// The basket form.
    /// </summary>
    public class BasketFormModel
    {
        /// <summary>
        /// Gets or sets the basket page content id.
        /// </summary>
        public int BasketPageContentId { get; set; }

        /// <summary>
        /// Gets or sets the empty basket text.
        /// </summary>
        public string EmptyBasketText { get; set; }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        public BasketLineItem[] Items { get; set; }

        /// <summary>
        /// Gets or sets the total price.
        /// </summary>
        public decimal TotalPrice { get; set; }

        /// <summary>
        /// Gets or sets the continue shopping url.
        /// </summary>
        public string ContinueShoppingUrl { get; set; }

        /// <summary>
        /// Gets or sets the checkout url.
        /// </summary>
        public string CheckoutUrl { get; set; }
    }
}