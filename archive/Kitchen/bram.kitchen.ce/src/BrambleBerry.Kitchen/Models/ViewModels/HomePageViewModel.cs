namespace BrambleBerry.Kitchen.Models.ViewModels
{
    using System.Collections.Generic;
    using System.Web;
    using Buzz.Hybrid;
    using Buzz.Hybrid.Models;

    /// <summary>
    /// The home page view model
    /// </summary>
    public class HomePageViewModel : ViewModelBase
    {
        /// <summary>
        /// Gets the alias of the content type this view model represents
        /// </summary>
        public static string RepresentsContentType
        {
            get { return "HomePage"; }
        }

        /// <summary>
        /// Gets the tag line
        /// </summary>
        public string TagLine 
        {
            get { return this.GetSafeString("tagLine"); }
        }

        /// <summary>
        /// Gets the brief text
        /// </summary>
        public string Brief
        {
            get { return this.GetSafeString("brief"); }
        }

        /// <summary>
        /// Gets main body text
        /// </summary>
        public IHtmlString BodyText
        {
            get { return this.GetSafeHtmlString("bodyText"); }
        }

        /// <summary>
        /// Gets or sets the banner image.
        /// </summary>
        public IImage Banner { get; set; }

        /// <summary>
        /// Gets or sets the featured products.
        /// </summary>
        public IEnumerable<ProductListItem> FeaturedProducts { get; set; }
    }
}