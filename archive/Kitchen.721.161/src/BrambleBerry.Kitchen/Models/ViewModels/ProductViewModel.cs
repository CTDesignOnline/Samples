
namespace BrambleBerry.Kitchen.Models.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using Buzz.Hybrid;
    using Buzz.Hybrid.Models;
    using Merchello.Web.Models.ContentEditing;
    using System.Linq;

    /// <summary>
    /// The product view model.
    /// </summary>
    public class ProductViewModel : ProductViewModelBase, IProductModel
    {
        /// <summary>
        /// Gets the alias of the content type this view model represents
        /// </summary>
        public static string RepresentsContentType
        {
            get { return "Product"; }
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name
        {
            get { return Content.GetSafeString("headline", Content.Name); }
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        public IHtmlString Description
        {
            get { return this.GetSafeHtmlString("bodyText"); }
        }

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
        /// Gets the url to the product
        /// </summary>
        public string Url 
        {
            get { return Content.Url; }
        }

        /// <summary>
        /// Gets the Umbraco content id.
        /// </summary>
        public int ContentId
        {
            get { return Content.Id; }
        }

        /// <summary>
        /// Gets a value indicating whether has image.
        /// </summary>
        public bool HasImage
        {
            get
            {
                return Images.Any();
            }
        }

        /// <summary>
        /// Gets or sets the add item form model.
        /// </summary>
        public AddItemFormModel AddItemFormModel { get; set; }
    }
}