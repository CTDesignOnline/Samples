namespace BrambleBerry.Kitchen.Controllers.Api
{
    using System;
    using Merchello.Core;
    using Merchello.Core.Gateways.Payment;
    using Merchello.Core.Gateways.Shipping;
    using Merchello.Core.Services;
    using Merchello.Web.WebApi;
    using Umbraco.Web.WebApi;

    /// <summary>
    /// The api controller base.
    /// </summary>
    [JsonCamelCaseFormatter]
    public abstract class ApiControllerBase : UmbracoApiController
    {
        /// <summary>
        /// The Merchello context.
        /// </summary>
        private readonly IMerchelloContext _merchelloContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiControllerBase"/> class.
        /// </summary>
        /// <param name="merchelloContext">
        /// The merchello context.
        /// </param>
        /// <exception cref="NullReferenceException">
        /// Throws if MerchelloContext is null
        /// </exception>
        protected ApiControllerBase(IMerchelloContext merchelloContext)
        {
            if (merchelloContext == null) throw new NullReferenceException("The MerchelloContext is not set");

            _merchelloContext = merchelloContext;
        }

        /// <summary>
        /// Gets the Merchello <see cref="IServiceContext"/>.
        /// </summary>
        protected new IServiceContext Services
        {
            get { return _merchelloContext.Services; }
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

    }
}