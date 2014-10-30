namespace BrambleBerry.Kitchen.Controllers
{
    using System.Linq;
    using System.Web.Mvc;

    using BrambleBerry.Kitchen.Models.ViewModels;

    using Buzz.Hybrid;

    using DevTrends.MvcDonutCaching;

    using Merchello.Web;

    using Umbraco.Web;
    using Umbraco.Web.Models;

    /// <summary>
    /// The home page controller.
    /// </summary>
    public class HomePageController : KitchenControllerBase
    {
        /// <summary>
        /// The default controller method
        /// </summary>
        /// <param name="model">
        /// The default <see cref="RenderModel"/>
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/> to render the home page template view
        /// </returns>
        [DonutOutputCache(CacheProfile = "FiveMins")]
        public override ActionResult Index(RenderModel model)
        {
            var homeModel = BuildModel<HomePageViewModel>();


            var merchello = new MerchelloHelper();

            homeModel.Banner = homeModel.Content.GetSafeImage(Umbraco, "banner");

            var mntpProducts = homeModel.Content.GetSafeMntpPublishedContent(Umbraco, "featuredProducts").ToArray();

            // The MNTP supercedes anything but if there are no values we will grab the first 6 products
            homeModel.FeaturedProducts = mntpProducts.Any()
                ? mntpProducts.Select(x => x.ToProductListItem(Umbraco, merchello.Query.Product.GetByKey(x.GetSafeGuid("product"))))
                : homeModel.Content.Descendants("Product").Take(8).ToArray().Select(x => x.ToProductListItem(Umbraco, merchello.Query.Product.GetByKey(x.GetSafeGuid("product"))));  

            return CurrentTemplate(homeModel);
        }
    }
}