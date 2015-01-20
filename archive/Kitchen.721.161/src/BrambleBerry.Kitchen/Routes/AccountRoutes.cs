using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BrambleBerry.Kitchen.Routes
{
    using System.Web.Configuration;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Buzz.Hybrid.Routing;
    using Umbraco.Web.PublishedCache;

    public static class AccountRoutes
    {
        public static void MapRoutes(RouteCollection routes)
        {
            int nodeid = int.Parse(WebConfigurationManager.AppSettings["BrambleBerry:AccountContentId"]);

            routes.MapUmbracoRoute(
                "accountRoutes",
                "account/{controller}/{action}/{id}",
                new
                {
                    controller = "MyAccountIndex",
                    action = "index",
                    id = UrlParameter.Optional
                },
                new UmbracoVirtualNodeByIdRouteHandler(nodeid),
                new { controller = new AccountSectionConstraint() },
                new[] { "BrambleBerry.Kitchen.Controllers.Account" }
            );
        }
    }

    public class AccountSectionConstraint : IRouteConstraint
    {
        private static List<string> _controllers = new List<string>() { "MyAccountIndex", 
            "my-orders", 
            "MyOrders",
            
            "my-settings", 
            "MySettings",
            
            "my-address-book", 
            "MyAddressBook",
            
            "my-payment-settings", 
            "MyPaymentSettings",
            
            "my-digital-content", 
            "MyDigitalContent",
            
            "my-wish-lists", 
            "MyWishLists",
            
            "my-reviews",
            "MyReviews" 
        };

        public AccountSectionConstraint()
        {

        }

        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            var controllerName = values["controller"].ToString();
            var anyMatching = _controllers.Any(x => x.Equals(controllerName, StringComparison.OrdinalIgnoreCase)); // case insensitive
            return anyMatching;

        }
    }
}
