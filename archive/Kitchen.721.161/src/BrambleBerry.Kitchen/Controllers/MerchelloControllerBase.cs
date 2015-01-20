namespace BrambleBerry.Kitchen.Controllers
{
    using System;
    using Buzz.Hybrid;
    using Merchello.Core;
    using Merchello.Core.Gateways.Notification.Smtp;
    using Merchello.Core.Gateways.Payment;
    using Merchello.Core.Gateways.Shipping;
    using Merchello.Core.Models;
    using Merchello.Core.Services;
    using Merchello.Web;
    using Merchello.Web.Workflow;
    using Umbraco.Core.Logging;
    using Umbraco.Core.Models;
    using Umbraco.Web;

    /// <summary>
    /// The merchello controller base.
    /// </summary>
    public abstract class MerchelloControllerBase : KitchenControllerBase
    {
        #region Private Fields

        /// <summary>
        /// The Merchello context
        /// </summary>
        private readonly IMerchelloContext _merchelloContext;

        /// <summary>
        /// The customer basket
        /// </summary>
        private IBasket _basket;

        /// <summary>
        /// The current customer.
        /// </summary>
        private ICustomerBase _currentCustomer;

        /// <summary>
        /// The reference to the homepage content
        /// </summary>
        private Lazy<IPublishedContent> _home; 

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="MerchelloControllerBase"/> class.
        /// </summary>
        /// <param name="merchelloContext">
        /// The merchello context.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Asserts the merchello context is not null
        /// </exception>
        protected MerchelloControllerBase(IMerchelloContext merchelloContext)
        {
            if (merchelloContext == null)
            {
                var ex = new ArgumentNullException("merchelloContext");

                LogHelper.Error<MerchelloControllerBase>("The MerchelloContext was null upon instantiating the CartController.", ex);

                throw ex;
            }

            _merchelloContext = merchelloContext;

            Initialize();
        }

        /// <summary>
        /// Gets the basket.
        /// </summary>
        protected IBasket Basket
        {
            get { return _basket; }
        }

        /// <summary>
        /// Gets the home page <see cref="IPublishedContent"/>
        /// </summary>
        protected IPublishedContent HomePage
        {
            get { return _home.Value; }
        }

        /// <summary>
        /// Gets the basket content id.
        /// </summary>
        protected int BasketContentId
        {
            get { return _home.Value != null ? _home.Value.GetSafeInteger("cartPage") : 0; }
        }

        /// <summary>
        /// Gets the basket page url.
        /// </summary>
        protected string BasketPageUrl
        {
            get
            {
                return BasketContentId > 0 ? Umbraco.TypedContent(BasketContentId).Url : "#";                 
            }
        }

        /// <summary>
        /// Gets the checkout page url.
        /// </summary>
        protected string CheckoutPageUrl
        {
            get
            {
                return _home.Value != null
                    ? Umbraco.TypedContent(_home.Value.GetSafeInteger("checkout")).Url
                    : "#";
            }
        }

        /// <summary>
        /// Gets the current customer.
        /// </summary>
        protected ICustomerBase CurrentCustomer
        {
            get { return _currentCustomer; }
        }

        /// <summary>
        /// Gets the Merchello <see cref="IServiceContext"/>.
        /// </summary>
        protected new IServiceContext Services
        {
            get { return _merchelloContext.Services; }
        }
        /// <summary>
        /// Gets the Umbraco Services <see cref="ServiceContext"/>.
        /// </summary>
        protected Umbraco.Core.Services.ServiceContext UmbracoServices
        {
            get { return base.Services; }
        }

        /// <summary>
        /// Gets the Merchello <see cref="IPaymentContext"/>
        /// </summary>
        protected IPaymentContext Payment
        {
            get { return _merchelloContext.Gateways.Payment; }
        }

        /// <summary>
        /// Gets the Merchello <see cref="IShippingContext"/>
        /// </summary>
        protected IShippingContext Shipping
        {
            get { return _merchelloContext.Gateways.Shipping; }
        }

        /// <summary>
        /// Gets the smtp provider.
        /// </summary>
        protected ISmtpNotificationGatewayProvider SmtpProvider
        {
            get
            {
                return
                    (SmtpNotificationGatewayProvider)
                        _merchelloContext.Gateways.Notification.GetProviderByKey(Constants.ProviderKeys.Notification.SmtpNotificationProviderKey);
            }
        }

        /// <summary>
        /// Initializes the <see cref="MerchelloControllerBase"/>
        /// </summary>
        private void Initialize()
        {
            var customerContext = new CustomerContext(UmbracoContext);

            _currentCustomer = customerContext.CurrentCustomer;

            _home = new Lazy<IPublishedContent>(GetHomePage);
                        
            _basket = _currentCustomer.Basket();
        }

        /// <summary>
        /// Utitlity method to safely get the homepage.
        /// </summary>
        /// <returns>
        /// The <see cref="IPublishedContent"/>.
        /// </returns>
        private IPublishedContent GetHomePage()
        {
            return UmbracoContext.PublishedContentRequest == null ? null : UmbracoContext.PublishedContentRequest.PublishedContent.AncestorOrSelf("HomePage");
        }
    }
}