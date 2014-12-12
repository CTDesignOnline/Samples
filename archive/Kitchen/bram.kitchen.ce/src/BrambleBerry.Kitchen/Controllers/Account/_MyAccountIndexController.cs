using System.Web.Mvc;
using BrambleBerry.Kitchen.Models.Account;
using BrambleBerry.Kitchen.Models.Account.MyAccountIndex;
using Merchello.Core.Formatters;
using Merchello.Core.Gateways.Notification;
using Umbraco.Web.Models;

namespace BrambleBerry.Kitchen.Controllers.Account
{
    /// <summary>
    /// The contact page controller.
    /// </summary>
    /// <remarks>
    /// This controller is dependent on Merchello's <see cref="INotificationContext"/> so that we can use the <see cref="IPatternReplaceFormatter"/> and
    /// the back office configured SMTP settings
    /// </remarks>
    public class MyAccountIndexController : BaseAccountController
    {
        

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
            var viewModel = BuildModel<MyAccountIndexViewModel>();
            return View(viewModel);
        }

        
    }
}