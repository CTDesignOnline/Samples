namespace BrambleBerry.Kitchen.Models.ViewModels
{
    using System.Web;
    using Buzz.Hybrid;

    /// <summary>
    /// The text page view model.
    /// </summary>
    public class TextPageViewModel : ViewModelBase
    {
        /// <summary>
        /// Gets the alias of the content type this view model represents
        /// </summary>
        public static string RepresentsContentType
        {
            get { return "TextPage"; }
        }

        /// <summary>
        /// Gets main body text
        /// </summary>
        public IHtmlString BodyText 
        {
            get { return this.GetSafeHtmlString("bodyText"); }
        }
    }
}