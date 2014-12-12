namespace BrambleBerry.Kitchen.Models
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using Buzz.Hybrid.Models;
    using Merchello.Web.Models.ContentEditing;

    /// <summary>
    /// Defines a product model
    /// </summary>
    public interface IProductModel
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the description.
        /// </summary>
        IHtmlString Description { get; }

        /// <summary>
        /// Gets or sets the Merchello product key.
        /// </summary>
        Guid ProductKey { get; set; }

        /// <summary>
        /// Gets or sets the price from Merchello.
        /// </summary>
        decimal Price { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the Merchello product defines variants.
        /// </summary>
        bool HasVariants { get; set; }

        /// <summary>
        /// Gets or sets the product options.
        /// </summary>
        IEnumerable<ProductOptionDisplay> Options { get; set; }

        /// <summary>
        /// Gets the thumbnail image
        /// </summary>
        IImage Thumbnail { get; }

        /// <summary>
        /// Gets the collection of images from the multi media picker.
        /// </summary>
        IEnumerable<IImage> Images { get; }

        /// <summary>
        /// Gets the url.
        /// </summary>
        string Url { get; }

        /// <summary>
        /// Gets the Umbraco content id.
        /// </summary>
        int ContentId { get; }

        /// <summary>
        /// Gets a value indicating whether has an image.
        /// </summary>
        bool HasImage { get; }

        /// <summary>
        /// Gets or sets the add item form model.
        /// </summary>
        AddItemFormModel AddItemFormModel { get; set; }
    }
}