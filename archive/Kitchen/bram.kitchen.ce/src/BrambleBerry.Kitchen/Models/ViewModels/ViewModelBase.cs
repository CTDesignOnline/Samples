namespace BrambleBerry.Kitchen.Models.ViewModels
{
    using System.Web;

    using Buzz.Hybrid.Models;

    /// <summary>
    /// The kitchen site base view model
    /// </summary>
    public class ViewModelBase : BaseModel
    {       
        /// <summary>
        /// Gets the logo.
        /// </summary>
        public SiteLogo Logo { get; internal set; }
         
        /// <summary>
        /// Gets the menu
        /// </summary>
        public ILinkTier Menu { get; internal set; }


        private IHtmlString _title { get; set; }
        public IHtmlString Title
        {
            get
            {
                if (_title == null)
                {
                    return PageTitle;
                }
                else
                {
                    return _title;
                }
            }
        }

        public void SetPageTitle(string title)
        {
            _title = new HtmlString(title);
        }

    }
}