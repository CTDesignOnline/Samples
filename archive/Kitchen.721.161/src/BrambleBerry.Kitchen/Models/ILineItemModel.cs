namespace BrambleBerry.Kitchen.Models
{
    using System;

    using Buzz.Hybrid.Models;

    /// <summary>
    /// Defines a Line Item Model
    /// </summary>
    public interface ILineItemModel
    {
        /// <summary>
        /// Gets or sets the line item key.
        /// </summary>
        Guid Key { get; set; }

        /// <summary>
        /// Gets or sets the Umbraco content id.
        /// </summary>
        int ContentId { get; set; }

        /// <summary>
        /// Gets or sets the image.
        /// </summary>
        IImage Thumbnail { get; set; }

        /// <summary>
        /// Gets or sets name of the item
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the variant name.
        /// </summary>
        string VariantName { get; set; }

        /// <summary>
        /// Gets or sets the line item SKU
        /// </summary>
        string Sku { get; set; }

        /// <summary>
        /// Gets or sets the unit price.
        /// </summary>
        decimal UnitPrice { get; set; }

        /// <summary>
        /// Gets or sets the total price.
        /// </summary>
        decimal TotalPrice { get; set; }

        /// <summary>
        /// Gets or sets the quantity.
        /// </summary>        
        int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the url.
        /// </summary>
        string Url { get; set; }

        /// <summary>
        /// Gets a value indicating whether has an image.
        /// </summary>
        bool HasImage { get; }

        /// <summary>
        /// Gets or sets a value indicating whether this item represents a product variant.
        /// </summary>
        bool IsVariant { get; set; }

        /// <summary>
        /// Gets or sets the sort date.
        /// </summary>
        DateTime SortDate { get; set; }
    }
}