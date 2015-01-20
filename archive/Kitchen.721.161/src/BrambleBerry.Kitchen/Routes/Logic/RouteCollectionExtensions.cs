namespace BrambleBerry.Kitchen.Routes.Logic
{
    using System.Web.Mvc;
    using System.Web.Routing;
    /// <summary>
    /// Helpers for MVC routes taken from https://github.com/Shandem/Articulate/blob/master/Articulate/RouteCollectionExtensions.cs
    /// </summary>
    internal static class RouteCollectionExtensions
    {

        internal static Route MapUmbracoRoute(this RouteCollection routes,
            string name, string url, object defaults, UmbracoVirtualNodeRouteHandler virtualNodeHandler,
            object constraints = null, string[] namespaces = null)
        {
            var route = RouteTable.Routes.MapRoute(name, url, defaults, constraints, namespaces);
            route.RouteHandler = virtualNodeHandler;
            return route;
        }

    }
}
