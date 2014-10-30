namespace BrambleBerry.Kitchen
{
    using System.Web;
    using System.Web.Mvc;
    using Buzz.Hybrid.Models;

    /// <summary>
    /// The site markup extensions.
    /// </summary>
    public static class SiteMarkupExtensions
    {
        /// <summary>
        /// Utility method to create the class="{CSS class}" attribute if defined on the <see cref="ILink"/>
        /// </summary>
        /// <param name="link">
        /// The link.
        /// </param>
        /// <returns>
        /// The <see cref="IHtmlString"/>.
        /// </returns>
        public static IHtmlString ActiveLinkClass(this ILink link)
        {
            return string.IsNullOrEmpty(link.CssClass)
                ? MvcHtmlString.Create(string.Empty)
                : MvcHtmlString.Create(string.Format(" class=\"{0}\"", link.CssClass));
        }
    }
}