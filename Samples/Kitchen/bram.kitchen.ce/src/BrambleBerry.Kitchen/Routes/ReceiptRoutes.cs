namespace BrambleBerry.Kitchen.Routes
{
    using System.Web.Configuration;
    using System.Web.Routing;
    using Buzz.Hybrid.Routing;

    /// <summary>
    /// The receipt routes.
    /// </summary>
    public static class ReceiptRoutes
    {
        /// <summary>
        /// Maps the route for rendering a receipt
        /// </summary>
        /// <param name="routes">
        /// The routes.
        /// </param>
        public static void MapRoutes(RouteCollection routes)
        {
            //// TODO this is pretty brittle
            var receiptContentId = int.Parse(WebConfigurationManager.AppSettings["BrambleBerry:ReceiptContentId"]);
            
            routes.MapUmbracoRoute(
                "customerReceipt",
                "checkout/receipt/{key}",
                new
                {
                    controller = "Receipt",
                    action = "Receipt"
                },
                new UmbracoVirtualNodeByIdRouteHandler(receiptContentId));
        }         
    }
}