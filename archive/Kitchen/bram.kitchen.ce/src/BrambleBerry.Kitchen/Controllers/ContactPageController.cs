using BrambleBerry.Kitchen.Models.ViewModels;

namespace BrambleBerry.Kitchen.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;    
    using Buzz.Hybrid;
    using Merchello.Core;
    using Merchello.Core.Formatters;
    using Merchello.Core.Gateways.Notification;
    using Merchello.Core.Models;
    using Models;
    using Umbraco.Web;
    using Umbraco.Web.Models;

    /// <summary>
    /// The contact page controller.
    /// </summary>
    /// <remarks>
    /// This controller is dependent on Merchello's <see cref="INotificationContext"/> so that we can use the <see cref="IPatternReplaceFormatter"/> and
    /// the back office configured SMTP settings
    /// </remarks>
    public class ContactPageController : MerchelloControllerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContactPageController"/> class.
        /// </summary>
        public ContactPageController() : this(MerchelloContext.Current)
        {            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactPageController"/> class.
        /// </summary>
        /// <param name="merchelloContext">
        /// The merchello context.
        /// </param>
        public ContactPageController(IMerchelloContext merchelloContext) : base(merchelloContext)
        {
        }

        /// <summary>
        /// The default controller method
        /// </summary>
        /// <param name="model">
        /// The default <see cref="RenderModel"/>
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/> to render the contact page template view
        /// </returns>        
        public override ActionResult Index(RenderModel model)
        {
            return CurrentTemplate(BuildModel<ContactPageViewModel>());
        }

        /// <summary>
        /// The render contact form.
        /// </summary>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        public ActionResult RenderContactForm()
        {
            var model = new ContactFormModel()
            {
                ContentId = UmbracoContext.PageId == null ? 0 : UmbracoContext.PageId.Value,
                ViewMode = ViewBag.Mode ?? ContactFormModel.DisplayMode.RenderForm,
                ErrorMessage = ViewBag.ErrorMessage ?? MvcHtmlString.Empty
            };
            return PartialView("ContactForm", model);
        }

        /// <summary>
        /// The send message using Merchello <see cref="INotificationContext"/>
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        /// <remarks>
        /// In this method we create a <see cref="INotificationMessage"/>, use the <see cref="IPatternReplaceFormatter"/> and send the message
        /// using the SMTP provider method.  If a method does not exist, the message should not send.
        /// </remarks>
        [HttpPost]
        public ActionResult SendMessage(ContactFormModel model)
        {
            if (!ModelState.IsValid) return CurrentUmbracoPage();

            var umbraco = new UmbracoHelper(UmbracoContext);

            var content = umbraco.TypedContent(model.ContentId);

            //// Get the SMTP notification method from Merchello.  If it has not been configured, return
            if (SmtpProvider != null)
            {
                var method = SmtpProvider.GetAllNotificationGatewayMethods().FirstOrDefault();

                if (method == null)
                {                 
                    ViewBag.Mode = ContactFormModel.DisplayMode.Error;
                    ViewBag.ErrorMessage = MvcHtmlString.Create("The SMTP provider does not have a method configured.");

                    return CurrentUmbracoPage();
                }

                var patterns = new Dictionary<string, IReplaceablePattern>()
                    {
                        { "name", new ReplaceablePattern("name", "{{name}}", model.FromName) },
                        { "message", new ReplaceablePattern("message", "{{message}}", model.Message) },
                        { "email", new ReplaceablePattern("email", "{{email}}", model.FromEmail) },
                        { "phone", new ReplaceablePattern("phone", "{{phone}}", model.Phone) }
                    };

                var formatter = new PatternReplaceFormatter(patterns);

                var message = new NotificationMessage(method.NotificationMethod.Key, content.GetSafeString("subject"), model.FromEmail)
                {
                    BodyText = content.GetSafeHtmlString("emailTemplate").ToHtmlString(),
                    Recipients = content.GetSafeString("to")
                };

                MerchelloContext.Current.Gateways.Notification.Send(message, formatter);

                ViewBag.ConfirmationMessage = content.GetSafeHtmlString("confirmationMessage").ToHtmlString();
                ViewBag.ViewMode = ContactFormModel.DisplayMode.ConfirmationMessage;                
            }
            else
            {
                ViewBag.ViewMode = ContactFormModel.DisplayMode.Error;
                ViewBag.ErrorMessage = MvcHtmlString.Create("The SMTP provider has not been configured.");
            }
            
            return CurrentUmbracoPage();
        }
    }
}