namespace SampleMerchelloShippingExampleProvider.ShippingProvider
{
    using Merchello.Core.Gateways;
    using Merchello.Core.Gateways.Shipping;
    using Merchello.Core.Models;
    using System.Web.Configuration;
    using Umbraco.Core;

    /// <summary>
    /// Represents a Preferred Customer shipping gateway method.
    /// </summary>
    /// <remarks>
    /// If you want to make add some configurations for your ship method, you can uncomment out the attribute below and write an angular view / controller
    /// to save information to the IShipMethod's extendedData collection.  (this adds the 'pencil' on the settings->shipping page in the back office).
    /// Example, you might want to make your £30 configurable so if the rate changes it is not hard coded.  Alternatively you could just 
    /// add it as an AppSetting to keep it simple - which I did here.  AppSetting is "BoxRoger:IsForHireShippingRate"
    /// </remarks>
    [GatewayMethodEditor("Sample ship method editor", "Sample ship method editor", "~/App_Plugins/Merchello.Docs.SampleShipper/methodeditor.html")]
    public class SampleShippingGatewayMethod : ShippingGatewayMethodBase
    {
        /// <summary>
        /// The rental shipping rate.
        /// </summary>
        private readonly decimal _isHeavyShippingRate;

        /// <summary>
        /// Initializes a new instance of the <see cref="BoxRogerShippingGatewayMethod"/> class.
        /// </summary>
        /// <param name="gatewayResource">
        /// The gateway resource.
        /// </param>
        /// <param name="shipMethod">
        /// The <see cref="IShipMethod"/>.
        /// </param>
        /// <param name="shipCountry">
        /// The <see cref="IShipCountry"/>.
        /// </param>
        /// <remarks>
        /// IShipMethod should really be named IShipMethodSettings as it is the configuration saved to the
        /// database of for the ShippingGatewayMethod.
        /// </remarks>
        public SampleShippingGatewayMethod(IGatewayResource gatewayResource, IShipMethod shipMethod, IShipCountry shipCountry)
            : base(gatewayResource, shipMethod, shipCountry)
        {
            // this is pretty brittle as it requires the AppSetting be defined
            try
            {
                _isHeavyShippingRate = decimal.Parse(WebConfigurationManager.AppSettings["SampleShipProvider:HeavyShippingCharge"]);
            }
            catch
            {
                _isHeavyShippingRate = 30M;
            }

        }

        /// <summary>
        /// Performs the actual work of performing the shipment rate quote for the shipment.
        /// </summary>
        /// <param name="shipment">
        /// The shipment.
        /// </param>
        /// <returns>
        /// An Umbraco <see cref="Attempt"/>.
        /// </returns>
        public override Attempt<IShipmentRateQuote> QuoteShipment(IShipment shipment)
        {
            //// I usually use a visitor to analize line items (checkout PluralSight design patterns library for videos on this).
            //// http://en.wikipedia.org/wiki/Visitor_pattern
            //var visitor = new MyLineItemVisitor();

            //shipment.Items.Accept(visitor);

            //return Attempt<IShipmentRateQuote>.Succeed(new ShipmentRateQuote(shipment, this.ShipMethod)
            //{
            //    Rate = visitor.RentalLineItemCount * _isPreferredCustomerShippingRate
            //});

            var itemHeavyCount = 0;

            //Example NOT using visitor pattern
            foreach (ILineItem item in shipment.Items)
            {
                var isHeavyItem = false;
                bool.TryParse(item.ExtendedData.GetValue("IsOverweight"), out isHeavyItem);

                if (isHeavyItem)
                {
                    itemHeavyCount++;
                }
            }

            var shippingRate = itemHeavyCount * _isHeavyShippingRate;

            return Attempt<IShipmentRateQuote>.Succeed(new ShipmentRateQuote(shipment, this.ShipMethod){Rate = shippingRate});
        }
    }
}
