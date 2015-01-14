namespace SampleMerchelloShippingExampleProvider.ShippingProvider
{
    using Merchello.Core.Gateways;
    using Merchello.Core.Gateways.Shipping;
    using Merchello.Core.Models;
    using System.Web.Configuration;
    using Umbraco.Core;

    /// <summary>
    /// Represents a Sample shipping gateway method.
    /// </summary>
    /// <remarks>
    /// If you want to add some configurations for your ship method, you can write an angular view / controller
    /// to save information to the IShipMethod's extendedData collection.  (this adds the 'pencil' on the settings->shipping page in the back office).
    /// Example, you might want to make your shipping charge configurable so if the rate changes it is not hard coded.  Alternatively you could just 
    /// add it as an AppSetting to keep it simple - which I did here.  AppSetting is "SampleShipProvider:HeavyShippingCharge"
    /// </remarks>
    //[GatewayMethodEditor("Sample ship method editor", "Sample ship method editor", "~/App_Plugins/Merchello.Docs.SampleShipper/methodeditor.html")]
    public class SampleShippingGatewayMethod : ShippingGatewayMethodBase
    {
        /// <summary>
        /// The overweight shipping rate.
        /// </summary>
        private readonly decimal _isHeavyShippingRate;

        /// <summary>
        /// Initializes a new instance of the <see cref="SampleShippingGatewayMethod"/> class.
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
        /// Performs the actual work of performing the shipment rate quote for the shipment based on what items are in the shipment.
        /// </summary>
        /// <param name="shipment">
        /// The shipment.
        /// </param>
        /// <returns>
        /// An Umbraco <see cref="Attempt"/>.
        /// </returns>
        public override Attempt<IShipmentRateQuote> QuoteShipment(IShipment shipment)
        {
            var itemHeavyCount = 0;

            foreach (ILineItem item in shipment.Items)
            {
                var isHeavyItem = false;
                bool.TryParse(item.ExtendedData.GetValue("IsOverweight"), out isHeavyItem);

                if (isHeavyItem)
                {
                    itemHeavyCount++;
                }
            }

            // each item gets an added charge, instead of one charge regardless of how many items there are
            var shippingRate = itemHeavyCount * _isHeavyShippingRate;

            return Attempt<IShipmentRateQuote>.Succeed(new ShipmentRateQuote(shipment, this.ShipMethod){Rate = shippingRate});
        }
    }
}
