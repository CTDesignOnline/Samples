using BrambleBerry.Kitchen.Models.ViewModels;

namespace BrambleBerry.Kitchen.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using Models;
    using Buzz.Hybrid;
    using Merchello.Web;
    using Umbraco.Web;
    using Umbraco.Web.Models;

    /// <summary>
    /// The product listing controller.
    /// </summary>
    public class ProductListingController : KitchenControllerBase
    {
        /// <summary>
        /// The default controller method
        /// </summary>
        /// <param name="model">
        /// The default <see cref="RenderModel"/>
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/> to render the product listing page template view
        /// </returns>
        public override ActionResult Index(RenderModel model)
        {
            var productListing = BuildModel<ProductListingViewModel>();

            var merchello = new MerchelloHelper();

            productListing.Products = productListing.Content.Descendants("Product").ToList().Where(x => x.IsVisible())
                .Select(x => x.ToProductListItem(Umbraco, merchello.Query.Product.GetByKey(x.GetSafeGuid("product"))));

            return CurrentTemplate(productListing);
        }
    }
}