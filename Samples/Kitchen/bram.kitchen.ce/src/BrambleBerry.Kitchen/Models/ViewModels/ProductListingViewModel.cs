namespace BrambleBerry.Kitchen.Models.ViewModels
{
    using System.Collections.Generic;

    /// <summary>
    /// The product listing view model.
    /// </summary>
    public class ProductListingViewModel : ProductViewModelBase
    {
        /// <summary>
        /// Gets the alias of the content type this view model represents
        /// </summary>
        public static string RepresentsContentType
        {
            get { return "ProductListing"; }
        }

        /// <summary>
        /// Gets or sets the product model collection
        /// </summary>
        public IEnumerable<ProductListItem> Products { get; set; }
    }
}