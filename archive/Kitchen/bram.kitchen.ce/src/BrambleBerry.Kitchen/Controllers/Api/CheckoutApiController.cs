namespace BrambleBerry.Kitchen.Controllers.Api
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    using Merchello.Core;
    using Merchello.Core.Gateways;
    using Merchello.Core.Models;
    using Merchello.Web;
    using Merchello.Web.Models.ContentEditing;
    using Merchello.Web.Workflow;
	using Models.Checkout;
    using Models.Workflow;
    using Umbraco.Core;

    /// <summary>
    /// The checkout API controller
    /// </summary>
    /// <remarks>
    /// Single page checkout
    /// </remarks>
    public class CheckoutApiController : ApiControllerBase
    {
        /// <summary>
        /// A collection of allowable shipping countries
        /// </summary>
        private Lazy<IEnumerable<ICountry>> _allowableCountries;

        /// <summary>
        /// A collection of all countries
        /// </summary>
        private Lazy<IEnumerable<ICountry>> _allCountries;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckoutApiController"/> class.
        /// </summary>
        public CheckoutApiController() : this(MerchelloContext.Current)
        {            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckoutApiController"/> class.
        /// </summary>
        /// <param name="merchelloContext">
        /// The merchello context.
        /// </param>
        public CheckoutApiController(IMerchelloContext merchelloContext) : base(merchelloContext)
        {
            Initialize();
        }

        #endregion

        /// <summary>
        /// Gets the allowable ship counties.
        /// </summary>
        private IEnumerable<ICountry> AllowableShipCounties
        {
            get { return _allowableCountries.Value.OrderBy(x => x.Name); }
        }


        /// <summary>
        /// Gets the all countries.
        /// </summary>
        private IEnumerable<ICountry> AllCountries
        {
            get
            {
                return _allCountries.Value.OrderBy(x => x.Name);
            }
        }

        /// <summary>
        /// Returns a list of all countries Merchello can ship to - for the drop down in the checkout view
        /// </summary>
        /// <returns>
        /// A collection of <see cref="CountryDisplay"/>
        /// </returns>
        [HttpGet]
        public IEnumerable<CountryDisplay> GetCountries()
        {
            return AllowableShipCounties
                .OrderBy(x => x.Name).Select(GetCountryDisplay);
        }

        /// <summary>
        /// Returns a list of all countries
        /// </summary>
        /// <returns>A collection of <see cref="CountryDisplay"/></returns>
        [HttpGet]
        public IEnumerable<CountryDisplay> GetAllCountries()
        {
            return AllCountries.Select(GetCountryDisplay);
        }

        /// <summary>
        /// Gets a collection of all provinces for the shipping drop down in the checkout view
        /// </summary>
        /// <returns>
        /// The collection of <see cref="ProvinceDisplay"/>
        /// </returns>
        [HttpGet]
        public IEnumerable<ProvinceDisplay> GetProvinces()
        {
            return BuildProvinceCollection(AllowableShipCounties.Where(x => x.Provinces.Any()));
        }

        /// <summary>
        /// Gets a collection of all provinces for the payment drop down
        /// </summary>
        /// <returns>
        /// The collection of <see cref="ProvinceDisplay"/>
        /// </returns>
        [HttpGet]
        public IEnumerable<ProvinceDisplay> GetAllProvinces()
        {
            return BuildProvinceCollection(AllCountries.Where(x => x.Provinces.Any()));
        }


        /// <summary>
        /// Gets shipment rate quotes for the drop down in the checkout view
        /// </summary>
        /// <param name="address">The <see cref="AddressModel"/></param>
        /// <returns>
        /// The <see cref="ShippingRateQuotes"/>
        /// </returns>
        [HttpPost, HttpGet]
        public ShippingRateQuotes GetShippingMethodQuotes(CheckoutAddress address)
        {
            var customer = Services.CustomerService.GetAnyByKey(address.CustomerKey);

            if (customer == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));

            var basket = Basket.GetBasket(customer);

            // for this version there is only ever a single shipment
            var destination = address.ToAddress();

            // validate we can estimate shipping
            if (string.IsNullOrEmpty(destination.CountryCode) || AllowableShipCounties.FirstOrDefault(x => x.CountryCode == destination.CountryCode) == null)
                throw new InvalidOperationException("The shipping destination's country code is either null or contains a country code that is not associated with any shipping provider.");

            var shipment = basket.PackageBasket(address.ToAddress()).FirstOrDefault();

            if (shipment == null) return new ShippingRateQuotes() { Status = ShipQuoteStatus.NoShippableItems.ToString() };

            var providerQuotes = shipment.ShipmentRateQuotes();

            return new ShippingRateQuotes(providerQuotes.ToShipMethodQuotes())
            {
                Status = ShipQuoteStatus.Ok.ToString()
            };
        }

        /// <summary>
        /// Gets the payment methods available
        /// </summary>
        /// <returns>
        /// A collection of <see cref="object"/> representing payment methods
        /// </returns>
        [HttpGet]
        public IEnumerable<object> GetPaymentMethods()
        {
            return Payment.GetPaymentGatewayMethods().Select(x => new
            {
                x.PaymentMethod.Key,
                x.PaymentMethod.Name,
                x.PaymentMethod.Description,
                Alias = GetDesignerAlias(x.GetType())
            });
        }

        /// <summary>
        /// Saves the shipping address to the sale preparation item cache.
        /// </summary>
        /// <param name="model">
        /// The <see cref="SalesPreparationModel"/>
        /// </param>
        /// <returns>
        /// The <see cref="PreOrderSummary"/>.
        /// </returns>
        [HttpPost]
        public PreOrderSummary SaveShippingAddress(SalesPreparationModel model)
        {
            var salesPreparation = GetSalePreparation(model.ShippingAddress.CustomerKey);

            salesPreparation.SaveShipToAddress(model.ShippingAddress.ToAddress());

            return GetPreOrderSummary(salesPreparation);
        }

        /// <summary>
        /// The save shipping rate quote.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <returns>
        /// The <see cref="PreOrderSummary"/>.
        /// </returns>
        /// <exception cref="HttpResponseException">
        /// Throws an not found exception if the customer record could not be found.
        /// </exception>
        [HttpPost]
        public PreOrderSummary SaveShippingRateQuote(SalesPreparationModel model)
        {
            // This is sort of weird to have the customer key in the ShippingAddress ... but we repurposed an object 
            // to simplify the JS
            var customer = Services.CustomerService.GetAnyByKey(model.ShippingAddress.CustomerKey);

            if (customer == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));

            var salesPreparation = customer.Basket().SalePreparation();

            if (salesPreparation.ItemCache.Items.Any(x => x.LineItemType == LineItemType.Shipping))
            {
                foreach (var shipQuote in salesPreparation.ItemCache.Items.Where(x => x.LineItemType == LineItemType.Shipping))
                {
                    salesPreparation.ItemCache.Items.RemoveItem(shipQuote.Sku);
                }
            }

            // save the ship rate quote selected
            var shipments = customer.Basket().PackageBasket(model.ShippingAddress.ToAddress()).ToArray();

            if (shipments.Any() && model.ShipMethodKey != Guid.Empty)
            {
                // there will only be one shipment in version 1.  This quote is cached in 
                // the runtime cache so there should not be another trip to the provider (if it were carrier based)
                var approvedQuote = shipments.First().ShipmentRateQuoteByShipMethod(model.ShipMethodKey);

                salesPreparation.SaveShipmentRateQuote(approvedQuote);
            }

            return GetPreOrderSummary(salesPreparation);
        }

        /// <summary>
        /// The saves the billing address.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <returns>
        /// The <see cref="PreOrderSummary"/>.
        /// </returns>
        [HttpPost]
        public PreOrderSummary SaveBillingAddress(SalesPreparationModel model)
        {
            var salesPreparation = GetSalePreparation(model.BillingAddress.CustomerKey);

            salesPreparation.SaveBillToAddress(model.BillingAddress.ToAddress());

            return GetPreOrderSummary(salesPreparation);
        }

        /// <summary>
        /// The place order.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        [HttpPost]
        public object PlaceOrder(SalesPreparationModel model)
        {
            var customer = Services.CustomerService.GetAnyByKey(model.ShippingAddress.CustomerKey);

            if (customer == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));

            var salesPreparation = customer.Basket().SalePreparation();
            salesPreparation.SaveBillToAddress(model.BillingAddress.ToAddress());

            if (!salesPreparation.IsReadyToInvoice()) return new
            {
                Success = false,
                Message = "Not ready to invoice"
            };

            var processorArgs = model.PaymentArgs.ToProcessorArgumentCollection();

            //// TODO - refactor after Merchello issue #M-297 has been resolved.

            var method = MerchelloContext.Current.Gateways.Payment.GetPaymentGatewayMethodByKey(model.PaymentMethodKey);

            var attempt = method.PaymentMethod.ProviderKey.Equals(Merchello.Core.Constants.ProviderKeys.Payment.CashPaymentProviderKey) ? 
                salesPreparation.AuthorizePayment(model.PaymentMethodKey, processorArgs) : 
                salesPreparation.AuthorizeCapturePayment(model.PaymentMethodKey, processorArgs);

            if (!attempt.Payment.Success)
                return new
                {
                    Success = attempt.Payment.Success.ToString(), 
                    attempt.Payment.Exception.Message
                };

            // Example of http://issues.merchello.com/youtrack/issue/M-290
            if (customer.IsAnonymous)
            {
                if (customer.ExtendedData.ContainsKey(SiteConstants.ExtendedDataKeys.AnonymousCustomersInvoices))
                {
                    var existing = customer.ExtendedData.GetValue(SiteConstants.ExtendedDataKeys.AnonymousCustomersInvoices);
                    customer.ExtendedData.SetValue(SiteConstants.ExtendedDataKeys.AnonymousCustomersInvoices, string.Format("{0},{1}", existing, attempt.Invoice.Key));
                }
                else
                {
                    customer.ExtendedData.SetValue(SiteConstants.ExtendedDataKeys.AnonymousCustomersInvoices, attempt.Invoice.Key.ToString());
                }
            }

            Notification.Trigger("OrderConfirmation", attempt, new[] { model.ContactInfo.Email });

            return new { Redirect = string.Format("/checkout/receipt/{0}", attempt.Invoice.Key.ToString()) };
        }

        /// <summary>
        /// The build province collection.
        /// </summary>
        /// <param name="countries">
        /// The countries.
        /// </param>
        /// <returns>
        /// The collection of <see cref="ProvinceDisplay"/>.
        /// </returns>
        private static IEnumerable<ProvinceDisplay> BuildProvinceCollection(IEnumerable<ICountry> countries)
        {
            var models = new List<ProvinceDisplay>();
            
            foreach (var country in countries)
            {
                models.AddRange(country.Provinces.Select(p => new ProvinceDisplay() { Code = p.Code, Name = p.Name }));
            }

            return models;
        }
        
        /// <summary>
        /// Builds a <see cref="CountryDisplay"/> given an <see cref="ICountry"/>
        /// </summary>
        /// <param name="country">
        /// The country.
        /// </param>
        /// <returns>
        /// The <see cref="CountryDisplay"/>.
        /// </returns>
        private static CountryDisplay GetCountryDisplay(ICountry country)
        {
            return new CountryDisplay()
            {
                CountryCode = country.CountryCode,
                Key = country.Key,
                Name = country.Name,
                ProvinceLabel = country.ProvinceLabel,
                Provinces = country.Provinces.Select(y => new ProvinceDisplay()
                {
                    Code = y.Code,
                    Name = y.Name
                })
            };
        }

        /// <summary>
        /// The get designer alias.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private static string GetDesignerAlias(Type type)
        {
            var att = type.GetCustomAttribute<GatewayMethodUiAttribute>(false);

            return att == null ? string.Empty : att.Alias;
        }

        /// <summary>
        /// The get pre order summary.
        /// </summary>
        /// <param name="salesPreparation">
        /// The sales preparation.
        /// </param>
        /// <returns>
        /// The <see cref="PreOrderSummary"/>.
        /// </returns>
        private static PreOrderSummary GetPreOrderSummary(BasketSalePreparation salesPreparation)
        {
            var summary = new PreOrderSummary();

            if (!salesPreparation.IsReadyToInvoice()) return summary;

            var invoice = salesPreparation.PrepareInvoice();

            // item total
            summary.ItemTotal = invoice.TotalItemPrice();

            // shipping total
            summary.ShippingTotal = invoice.TotalShipping();

            // tax total
            summary.TaxTotal = invoice.TotalTax();

            // invoice total
            summary.InvoiceTotal = invoice.Total;

            return summary;
        }

        /// <summary>
        /// The get basket sale preparation.
        /// </summary>
        /// <param name="customerKey">
        /// The customer key.
        /// </param>
        /// <returns>
        /// The <see cref="BasketSalePreparation"/>.
        /// </returns>
        /// <exception cref="NullReferenceException">
        /// Throws if customer record cannot be found with key passed as argument
        /// </exception>
        private BasketSalePreparation GetSalePreparation(Guid customerKey)
        {
            // This is sort of weird to have the customer key in the ShippingAddress ... but we repurposed an object 
            // to simplify the JS
            var customer = Services.CustomerService.GetAnyByKey(customerKey);

            if (customer == null) throw new NullReferenceException(string.Format("The customer with key {0} was not found.", customerKey));

            return customer.Basket().SalePreparation();
        }

        /// <summary>
        /// The initialize.
        /// </summary>
        private void Initialize()
        {
            _allowableCountries = new Lazy<IEnumerable<ICountry>>(() => Shipping.GetAllowedShipmentDestinationCountries());
            _allCountries = new Lazy<IEnumerable<ICountry>>(() => Services.StoreSettingService.GetAllCountries());
        }
    }
}