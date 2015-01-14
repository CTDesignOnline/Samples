namespace SampleMerchelloShippingExampleProvider.ShippingProvider
{
    using Merchello.Core.Gateways;
    using Merchello.Core.Gateways.Shipping;
    using Merchello.Core.Models;
    using Merchello.Core.Services;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Umbraco.Core.Cache;

    /// <summary>
    /// GatewayProviderActivation Params: key, name, description
    /// GatewayProviderActivation key: generated new guid
    /// GatewayProviderActivation name: name appears in back office
    /// GatewayProviderActivation description: description appears in back office
    /// 
    /// This information is used to add the provider to the backoffice. It will appear in the Merchello section, under gateways.
    /// 
    /// GatewayProviderEditor params: title, editorView
    /// GatewayProviderEditor title: title of editor dialog
    /// GatewayProviderEditor editorView: the html page to display to edit/configure the provider
    /// 
    /// This information is used to give overall/general shipping configuration. This sample doesn't actual
    /// have the providereditor.html file. This setting is meant as an example only. 
    /// </summary>
    //[GatewayProviderEditor("Sample configuration", "~/App_Plugins/Merchello.Docs.SampleShipper/providereditor.html")]
    [GatewayProviderActivation("2e734763-ed20-4ed5-bcca-9a0e622218dd", "Sample Shipping Provider", "Sample Shipping Provider for Merchello Documentation")]
    class SampleShippingGatewayProvider : ShippingGatewayProviderBase
    {

        #region Available Methods

        /// <summary>
        /// The available resources.
        /// </summary>
        /// <remarks>
        /// Gateway resources are used to constrain shipping methods to a shipping provider - so in this case
        /// the SampleShippingProvider will only be able to offer a single shipping method.  In other providers
        /// like the flat rate shipping provider, we have two resources - one for "Vary by Weight" and one for "Very by Price"
        /// as the computations are done differently.
        /// </remarks>
        private static readonly IEnumerable<IGatewayResource> AvailableResources = new List<IGatewayResource>()
            {
                new GatewayResource("SampleShipper", "SampleShipper")
            };

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="SampleShippingGatewayProvider"/> class.
        /// </summary>
        /// <param name="gatewayProviderService">
        /// The <see cref="IGatewayProviderService"/>.
        /// </param>
        /// <param name="gatewayProviderSettings">
        /// The <see cref="IGatewayProviderSettings"/>.
        /// </param>
        /// <param name="runtimeCacheProvider">
        /// Umbraco's <see cref="IRuntimeCacheProvider"/>.
        /// </param>
        public SampleShippingGatewayProvider(IGatewayProviderService gatewayProviderService, IGatewayProviderSettings gatewayProviderSettings, IRuntimeCacheProvider runtimeCacheProvider)
            : base(gatewayProviderService, gatewayProviderSettings, runtimeCacheProvider)
        {
        }

        /// <summary>
        /// The list resources offered.
        /// </summary>
        /// <returns>
        /// The <see cref="IEnumerable{IGatewayResource}"/>.
        /// </returns>
        public override IEnumerable<IGatewayResource> ListResourcesOffered()
        {
            return AvailableResources;
        }

        /// <summary>
        /// Creates a <see cref="ISampleShippingGatewayMethod"/> association with an allowed country.
        /// </summary>
        /// <param name="gatewayResource">
        /// The gateway resource.
        /// </param>
        /// <param name="shipCountry">
        /// The ship country.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// The <see cref="IShippingGatewayMethod"/>.
        /// </returns>
        public override IShippingGatewayMethod CreateShippingGatewayMethod(IGatewayResource gatewayResource, IShipCountry shipCountry, string name)
        {
            //TODO: uncomment these when move to Umbraco 7.2.x
            //Mandate.ParameterNotNull(gatewayResource, "gatewayResource");
            //Mandate.ParameterNotNull(shipCountry, "shipCountry");
            //Mandate.ParameterNotNullOrEmpty(name, "name");

            var attempt = GatewayProviderService.CreateShipMethodWithKey(GatewayProviderSettings.Key, shipCountry, name, gatewayResource.ServiceCode + string.Format("-{0}", Guid.NewGuid()));

            if (!attempt.Success) throw attempt.Exception;

            return new SampleShippingGatewayMethod(gatewayResource, attempt.Result, shipCountry);
        }

        /// <summary>
        /// The save shipping gateway method.
        /// </summary>
        /// <param name="shippingGatewayMethod">
        /// The shipping gateway method.
        /// </param>
        public override void SaveShippingGatewayMethod(IShippingGatewayMethod shippingGatewayMethod)
        {
            GatewayProviderService.Save(shippingGatewayMethod.ShipMethod);
            
            // I will be to moving SaveShippingGatewayMethod to the base class and make it virtual to handle
            // this for people, but it will not be a breaking change so no worries.
            base.ShipMethods = null;
        }

        /// <summary>
        /// The get all shipping gateway methods.
        /// </summary>
        /// <param name="shipCountry">
        /// The ship country.
        /// </param>
        /// <returns>
        /// The <see cref="IEnumerable{IShippingGatewayMethod}"/>.
        /// </returns>
        public override IEnumerable<IShippingGatewayMethod> GetAllShippingGatewayMethods(IShipCountry shipCountry)
        {
            var methods = GatewayProviderService.GetShipMethodsByShipCountryKey(GatewayProviderSettings.Key, shipCountry.Key);
            return methods
                .Select(
                shipMethod => new SampleShippingGatewayMethod(AvailableResources.FirstOrDefault(x => shipMethod.ServiceCode.StartsWith(x.ServiceCode)), shipMethod, shipCountry))
                .OrderBy(x => x.ShipMethod.Name);
        }
    }
}
