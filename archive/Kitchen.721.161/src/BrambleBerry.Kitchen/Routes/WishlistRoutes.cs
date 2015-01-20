namespace BrambleBerry.Kitchen.Routes
{
    using System.Web.Configuration;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Buzz.Hybrid.Routing;
    using Umbraco.Web.PublishedCache;

    public static class WishlistRoutes
    {
        /// <summary>
        /// maps all the routes for the Public Wishlist Pages
        /// </summary>
        /// <param name="routes"></param>
        /// <param name="umbracoCache"></param>
        public static void MapRoutes(RouteCollection routes)
        {
            int nodeid = int.Parse(WebConfigurationManager.AppSettings["BrambleBerry:WishListContentId"]);

            routes.MapUmbracoRoute(
                "wishlistSharedPage",
                "wishlist/{customerId}/{wishlistName}",
                new
                {
                    controller = "Wishlist",
                    action = "View",
                    wishlistName = UrlParameter.Optional
                },
                new UmbracoVirtualNodeByIdRouteHandler(nodeid));
        }

    }
}
