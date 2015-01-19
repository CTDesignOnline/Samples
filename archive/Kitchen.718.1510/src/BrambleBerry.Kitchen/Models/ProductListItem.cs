using System.Linq;

namespace BrambleBerry.Kitchen.Models
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using Buzz.Hybrid.Models;
    using Merchello.Web.Models.ContentEditing;

    using Umbraco.Core;

    /// <summary>
    /// Represents a product list item.
    /// </summary>
    /// <remarks>
    /// We don't want to use a view model here because of extraneous properties
    /// such as the menu(s)
    /// </remarks>
    public class ProductListItem : IProductModel
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name { get; internal set; }

        /// <summary>
        /// Gets the description.
        /// </summary>
        public IHtmlString Description { get; internal set; }

        /// <summary>
        /// Gets or sets the Merchello product key.
        /// </summary>
        public Guid ProductKey { get; set; }

        /// <summary>
        /// Gets or sets the price from Merchello
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the Merchello product defines variants.
        /// </summary>
        public bool HasVariants { get; set; }

        /// <summary>
        /// Gets or sets the options.
        /// </summary>
        public IEnumerable<ProductOptionDisplay> Options { get; set; }

        /// <summary>
        /// Gets the thumbnail image
        /// </summary>
        public IImage Thumbnail { get; internal set; }

        /// <summary>
        /// Gets the collection of images from the multi media picker.
        /// </summary>
        public IEnumerable<IImage> Images { get; internal set; }

        /// <summary>
        /// Gets the url.
        /// </summary>
        public string Url { get; internal set; }

        /// <summary>
        /// Gets the Umbraco content id.
        /// </summary>
        public int ContentId { get; internal set; }

        /// <summary>
        /// Gets a value indicating whether has image.
        /// </summary>
        public bool HasImage 
        {
            get
            {
                return Images.Any() && Thumbnail != null;
            }
        }

        /// <summary>
        /// Gets or sets the add item form model.
        /// </summary>
        public AddItemFormModel AddItemFormModel { get; set; }
    }
}