namespace BrambleBerry.Kitchen.Routes
{
    using System.Web.Configuration;
    using System.Web.Routing;

    using Buzz.Hybrid.Routing;

    /// <summary>
    /// The authorization routes.
    /// </summary>
    public static class AuthorizationRoutes
    {
        /// <summary>
        /// The map routes.
        /// </summary>
        /// <param name="routes">
        /// The routes.
        /// </param>
        public static void MapRoutes(RouteCollection routes)
        {
            int nodeid = int.Parse(WebConfigurationManager.AppSettings["BrambleBerry:AuthorizationContentId"]);
            string firstUrlSegment = "authorization";


            routes.MapUmbracoRoute(
                    "authResetPassword",
                    firstUrlSegment + "/reset-password/{token}/{memberId}",
                    new
                    {
                        controller = "Authorization",
                        action = "ResetPassword",
                    },
                    new UmbracoVirtualNodeByIdRouteHandler(nodeid));


            routes.MapUmbracoRoute(
               "auth",
               firstUrlSegment + "/{action}",
               new
               {
                   controller = "Authorization",
                   action = "Index",
               },
               new UmbracoVirtualNodeByIdRouteHandler(nodeid));
        }
    }
}
