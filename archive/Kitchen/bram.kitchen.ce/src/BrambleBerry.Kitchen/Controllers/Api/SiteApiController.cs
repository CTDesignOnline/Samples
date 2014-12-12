namespace BrambleBerry.Kitchen.Controllers.Api
{
    using System;
    using System.Collections.Generic;
    using System.Web.Http;
    using Merchello.Core;
    using Merchello.Web.Models.ContentEditing;

    /// <summary>
    /// The site api controller.
    /// </summary>
    public class SiteApiController : ApiControllerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SiteApiController"/> class.
        /// </summary>
        public SiteApiController()
            : this(MerchelloContext.Current)
        {            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SiteApiController"/> class.
        /// </summary>
        /// <param name="merchelloContext">
        /// The merchello context.
        /// </param>
        public SiteApiController(IMerchelloContext merchelloContext) : base(merchelloContext)
        {
        }

        /// <summary>
        /// Utility method to change pricing on a Product template when a customer changes an option in the drop downs
        /// </summary>
        /// <param name="productKey">The <see cref="ProductDisplay"/> key</param>
        /// <param name="optionChoiceKeys">A collection of option choice keys from the drop down(s)</param>
        /// <returns>The variant price as a formatted <see cref="string"/></returns>
        [HttpGet]
        public string GetProductVariantPrice(Guid productKey, string optionChoiceKeys)
        {
            var optionsArray = optionChoiceKeys.Split(',');

            var guidOptionChoiceKeys = new List<Guid>();

            foreach (var option in optionsArray)
            {
                if (!string.IsNullOrEmpty(option))
                {
                    guidOptionChoiceKeys.Add(new Guid(option));
                }
            }

            var product = Services.ProductService.GetByKey(productKey);
            var variant = Services.ProductVariantService.GetProductVariantWithAttributes(product, guidOptionChoiceKeys.ToArray());

            return variant.Price.ToString("C");
        }
    }
}