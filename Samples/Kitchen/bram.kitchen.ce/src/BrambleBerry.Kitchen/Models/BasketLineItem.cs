namespace BrambleBerry.Kitchen.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Buzz.Hybrid.Models;

    /// <summary>
    /// The basket line item.
    /// </summary>
    public class BasketLineItem : ILineItemModel
    {
        /// <summary>
        /// Gets or sets the line item key.
        /// </summary>
        public Guid Key { get; set; }

        /// <summary>
        /// Gets or sets the Umbraco content id.
        /// </summary>
        public int ContentId { get; set; }

        /// <summary>
        /// Gets or sets the image.
        /// </summary>
        public IImage Thumbnail { get; set; }

        /// <summary>
        /// Gets or sets name of the item
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the variant name.
        /// </summary>
        public string VariantName { get; set; }

        /// <summary>
        /// Gets or sets the line item SKU
        /// </summary>
        public string Sku { get; set; }

        /// <summary>
        /// Gets or sets the unit price.
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// Gets or sets the total price.
        /// </summary>
        public decimal TotalPrice { get; set; }

        /// <summary>
        /// Gets or sets the quantity.
        /// </summary>
        [Required]
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the url.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets a value indicating whether has an image.
        /// </summary>
        public bool HasImage
        {
            get { return Thumbnail != null; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this item represents a product variant.
        /// </summary>
        public bool IsVariant { get; set; }

        /// <summary>
        /// The datetime the product was entered into the basket items collection
        /// </summary>
        public DateTime SortDate { get; set; }
    }
}