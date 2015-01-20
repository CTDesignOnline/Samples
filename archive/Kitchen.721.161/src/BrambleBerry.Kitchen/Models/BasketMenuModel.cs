namespace BrambleBerry.Kitchen.Models
{
    /// <summary>
    /// The basket menu model.
    /// </summary>
    public class BasketMenuModel
    {
        /// <summary>
        /// Gets or sets the basket page url.
        /// </summary>
        public string BasketPageUrl { get; set; }

        /// <summary>
        /// Gets or sets the item count.
        /// </summary>
        public int ItemCount { get; set; }

        /// <summary>
        /// Gets or sets the total price.
        /// </summary>
        public decimal TotalPrice { get; set; }
    }
}