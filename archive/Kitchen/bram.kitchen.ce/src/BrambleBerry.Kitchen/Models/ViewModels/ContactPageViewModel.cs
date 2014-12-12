namespace BrambleBerry.Kitchen.Models.ViewModels
{
    using System.Web;
    using Buzz.Hybrid;

    /// <summary>
    /// The contact page view model.
    /// </summary>
    public class ContactPageViewModel : ViewModelBase
    {
        /// <summary>
        /// Gets the alias of the content type this view model represents
        /// </summary>
        public static string RepresentsContentType
        {
            get { return "ContactPage"; }
        }

        /// <summary>
        /// Gets the headline.
        /// </summary>
        public string Headline
        {
            get { return Content.GetSafeString("headline"); }
        }

        /// <summary>
        /// Gets the body text.
        /// </summary>
        public IHtmlString BodyText
        {
            get { return Content.GetSafeHtmlString("bodyText"); }
        }
       
        /// <summary>
        /// Gets the to (or recipient's) email.
        /// </summary>
        /// <remarks>
        /// Set in the back office
        /// </remarks>
        public string ToEmail 
        {
            get { return Content.GetSafeString("to"); }
        }

        /// <summary>
        /// Gets the to (or recipient's) name.
        /// </summary>
        public string ToName
        {
            get { return Content.GetSafeString("toName"); }
        }

        /// <summary>
        /// Gets the subject.
        /// </summary>
        public string Subject
        {
            get { return Content.GetSafeString("subject"); }
        }        

        /// <summary>
        /// Gets the email template.
        /// </summary>
        public IHtmlString EmailTemplate
        {
            get { return Content.GetSafeHtmlString("emailTemplate"); }             
        }        
    }
}