namespace BrambleBerry.Kitchen.Routes.Logic
{
    using System.Web.Routing;
    using Umbraco.Core.Models;
    using Umbraco.Web;

    /// <summary>
    /// For MVC Routes, taken from https://github.com/Shandem/Articulate/blob/master/Articulate/UmbracoVirtualNodeByIdRouteHandler.cs
    /// </summary>
    internal class UmbracoVirtualNodeByIdRouteHandler : UmbracoVirtualNodeRouteHandler
    {
        private readonly int _realNodeId;

        internal UmbracoVirtualNodeByIdRouteHandler(int realNodeId)
        {
            _realNodeId = realNodeId;
        }

        protected sealed override IPublishedContent FindContent(RequestContext requestContext, UmbracoContext umbracoContext)
        {
            var byId = umbracoContext.ContentCache.GetById(_realNodeId);
            if (byId == null) return null;

            return FindContent(requestContext, umbracoContext, byId);
        }

        protected virtual IPublishedContent FindContent(RequestContext requestContext, UmbracoContext umbracoContext, IPublishedContent baseContent)
        {
            return baseContent;
        }
    }
}
