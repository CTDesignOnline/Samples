namespace BrambleBerry.Kitchen.Controllers
{
    using System;
    using System.Web.Mvc;
    using System.Web.Security;

    using BrambleBerry.Kitchen.Extensions;
    using BrambleBerry.Kitchen.Localization;
    using BrambleBerry.Kitchen.Models.Authorization;
    using BrambleBerry.Kitchen.Services.AbleCommerce;

    using Umbraco.Core.Logging;

    using MemberExtensions = BrambleBerry.Kitchen.Extensions.MemberExtensions;

    /// <summary>
    /// The home page controller.
    /// </summary>
    public class AuthorizationController : KitchenControllerBase
    {
        public const string LoginErrorMessageKey = "AccountMessage_LoginError";
        public const string RegistrationErrorMessageKey = "AccountMessage_RegistrationError";

        #region Logging In/Out

        /// <summary>
        /// Returns the login form with a new account model, response to GET
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index(string returnUrl)
        {
            var model = BuildModel<AuthorizationLoginModel>();

            model.ReturnUrl = !string.IsNullOrEmpty(returnUrl)? returnUrl : Url.Action("index", "MyAccountIndex" );
            return View(model);
        }

        /// <summary>
        /// Action for handling a login for an account, notice this is the POST version
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(AuthorizationLoginModel model)
        {
            RebuildFormPostback(model);

            Session.Clear();
            FormsAuthentication.SignOut();

            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(model.Username, model.Password))
                {
                    FormsAuthentication.SetAuthCookie(model.Username, model.RememberMe);
                    return GetSuccessUrlActionResult(model);
                }

                // #### Check Able Commerce for an old customer ####
                //In future this whole section will get retired, you can retire CreateCustomerFromLegacyCustomer too
                IAbleCommerceLegacyLogin legacyLoginService = new AbleCommerceLoginService();

                var remoteLoginResponse = legacyLoginService.HandleLogin(model.Username, model.Password);

                var customer = remoteLoginResponse.ConvertToUmbracoMember(Services);

                if (customer != null)
                {
                    FormsAuthentication.SetAuthCookie(model.Username, model.RememberMe);

                    // flag this with Rusty's import Queue to pull this user into the new system
                    AbleCommerceImportQueue.Enqueue(model.Username, model.Password, Convert.ToInt32(customer.GetValue("ableCommerceId")));

                    return GetSuccessUrlActionResult(model);
                }
                
                TempData[LoginErrorMessageKey] = LanguageDictionary.Alerts.InvalidUsernameOrPassword;
            }
            // Username not found either in Umbraco or Legacy system so bounce it back to the user
            //#TODO should this be localised/translated?
            
            return View(model);
            
            
        }

        private ActionResult GetSuccessUrlActionResult(AuthorizationLoginModel model)
        {
            // Redirect user to initially requested URL
            if (!String.IsNullOrEmpty(model.ReturnUrl))
            {
                return Redirect(model.ReturnUrl);
            }

            return Redirect("/");
        }

      

        /// <summary>
        /// Logs a user out and redirects to the Homepage
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Logout()
        {
            Session.Clear();
            FormsAuthentication.SignOut();
            return Redirect("/");
        }

        #endregion

        #region Forgotten Password
        [HttpGet]
        public ActionResult ForgottenPassword()
        {
            var model = BuildModel<AuthorizationForgotPasswordFormModel>();
            return View( model );
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgottenPassword(AuthorizationForgotPasswordFormModel model)
        {
            if (ModelState.IsValid)
            {
                
                base.RebuildFormPostback(model);

                var member = Services.MemberService.GetByEmail(model.EmailAddress);

                if (member != null)
                {
                    var resetToken = Guid.NewGuid();

                    // Get reset password url for member
                    //model.ResetPasswordToken = GetResetPasswordUrl(member);
                    member.SetValue("forgottenPasswordToken", resetToken.ToString());
                    member.SetValue("forgottenPasswordTokenGenerated", DateTime.UtcNow.ToString());
                    Services.MemberService.Save(member);

                    var url = Url.Action("ResetPassword", new { token = resetToken, member = member.Id }).EnsureNonSurfaceUrl();

                    LogHelper.Info<AuthorizationController>("Reset password token generated for member:" + member.Id +
                                                            " token is " + resetToken + " url is " + url);
                }

                TempData.Add("CustomMessage_ForgotPassword",
                    "Thanks, If your email address was correct, we have sent you an email");
            }
            return PartialView(model);
        }

        public ActionResult ResetPassword(Guid token, int memberId)
        {
            var member = Services.MemberService.GetById(memberId);
            var model = BuildModel<AuthorizationResetPasswordFormModel>();

            var validationResults = member.ValidatePasswordResetToken(token);

            model.MemberId = memberId;
            model.ResetToken = token;
            model.IsValid = validationResults;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(AuthorizationResetPasswordFormModel model)
        {
            base.RebuildFormPostback(model);
            if (ModelState.IsValid)
            {
                
                var member = Services.MemberService.GetById(model.MemberId);
                var validationResults = member.ValidatePasswordResetToken(model.ResetToken);
                if (validationResults == MemberExtensions.ValidatePasswordResetTokenResult.Valid)
                {
                    member.SetValue("forgottenPasswordToken", "");
                    member.SetValue("forgottenPasswordTokenGenerated", "");
                    Services.MemberService.Save(member);

                    Services.MemberService.SavePassword(member, model.Password);

                }
                return Redirect("/Authorization");
            }
            else
            {
                return View(model);
            }
        }

        #endregion

        [ChildActionOnly]
        public ActionResult Identity()
        {
            return PartialView("Identity");
        }

        #region Registration goodies
        
        [HttpGet]
        public ActionResult Registration()
        {
            var model = BuildModel<AuthorizationRegistrationModel>();
            return View( "Registration", model );
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration( AuthorizationRegistrationModel model )
        {
             base.RebuildFormPostback(model);
            if (ModelState.IsValid)
            {
                // Check email is unique
                var member = Services.MemberService.CreateMember(model.Email, model.Email, model.Email,
                    SiteConstants.MemberTypes.CustomerAlias);
                member.SetValue("firstName", model.Firstname);
                member.SetValue("lastName", model.Surname); 
                Services.MemberService.Save(member);

                Services.MemberService.SavePassword(member, model.Password);
                FormsAuthentication.SetAuthCookie(model.Email,false);
            
            
                
                return RedirectToAction("Index", "MyAccountIndex");
            }
            return View(model);
        }

        #endregion
    }
}

