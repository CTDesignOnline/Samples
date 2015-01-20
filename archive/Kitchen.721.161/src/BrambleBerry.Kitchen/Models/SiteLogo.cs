namespace BrambleBerry.Kitchen.Models
{
    using System.Web;

    /// <summary>
    /// The site's logo
    /// </summary>    
    public class SiteLogo
    {
        /// <summary>
        /// Gets or sets the site name.
        /// </summary>
        public IHtmlString SiteName { get; set; }

        /// <summary>
        /// Gets or sets the url.
        /// </summary>
        public string Url { get; set; }
    }
}