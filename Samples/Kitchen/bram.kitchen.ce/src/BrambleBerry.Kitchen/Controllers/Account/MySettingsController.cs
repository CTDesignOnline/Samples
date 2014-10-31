using System.Web.Mvc;
using BrambleBerry.Kitchen.Extensions;
using BrambleBerry.Kitchen.Models.Account.MySettings;
using Merchello.Core.Formatters;
using Merchello.Core.Gateways.Notification;
using Merchello.Core.Models;
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
    public class MySettingsController : BaseAccountController
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
            var viewmodel = base.BuildModel<MyAccountSettingsViewModel>();

            if (string.IsNullOrEmpty(Customer.Email))
            {
                //Merchello doesnt have the users email address, so i'll use it from there
                var umbracoMember = Customer.GetUmbracoMember(UmbracoServices.MemberService);
                Customer.Email = umbracoMember.Email;
                Services.CustomerService.Save(Customer);
            }
            
            viewmodel.Firstname = Customer.FirstName;
            viewmodel.Lastname = Customer.LastName;
            viewmodel.EmailAddress = Customer.Email;

            return View(viewmodel);
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(MyAccountSettingsViewModel model)
        {
            base.RebuildFormPostback(model);

            if (ModelState.IsValid)
            {
                var memberService = UmbracoServices.MemberService;
                var umbracoMember = Customer.GetUmbracoMember(memberService);
                
                if (!string.IsNullOrEmpty(model.Password))
                {
                    memberService.SavePassword(umbracoMember,model.Password);
                }

                Customer.FirstName = model.Firstname;
                Customer.LastName = model.Lastname;
                Customer.Email = model.EmailAddress;
                Services.CustomerService.Save(Customer);

                umbracoMember.SetValue("firstname",model.Firstname);
                umbracoMember.SetValue("surname", model.Lastname);
                umbracoMember.Email = model.EmailAddress;
                umbracoMember.Username = model.EmailAddress;

                UmbracoServices.MemberService.Save(umbracoMember);

            }

            return View(model);
        }
    }

  
}