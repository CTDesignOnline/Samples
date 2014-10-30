using BrambleBerry.Kitchen.Models.Authorization;
using Umbraco.Web;

namespace BrambleBerry.Kitchen.Events
{
    using System;
    using System.Web.Mvc;
    using System.Web.Routing;

    using BrambleBerry.Kitchen.Controllers;
    using BrambleBerry.Kitchen.Routes;

    using DevTrends.MvcDonutCaching;

    using Merchello.Core.Models;
    using Merchello.Core.Services;

    using Umbraco.Core;
    using Umbraco.Core.Events;
    using Umbraco.Core.Logging;
    using Umbraco.Core.Models;
    using Umbraco.Core.Publishing;
    using Umbraco.Core.Services;
    using Umbraco.Web.Mvc;

    /// <summary>
    /// Registers site specific Umbraco application event handlers
    /// </summary>
    public class UmbracoEvents : ApplicationEventHandler
    {
        /// <summary>
        /// Overrides for Umbraco application starting.
        /// </summary>
        /// <param name="umbracoApplication">
        /// The Umbraco application.
        /// </param>
        /// <param name="applicationContext">
        /// The application context.
        /// </param>
        protected override void ApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            LogHelper.Info<UmbracoEvents>("Binding MvcDonutCache refreshers");

            PublishingStrategy.Published += PublishingStrategyOnPublished;
            ContentService.Deleted += ContentServiceOnDeleted;
            ProductService.Saved += ProductServiceOnSaved;
            ProductService.Deleted += ProductServiceOnDeleted;
            ProductVariantService.Saved += ProductVariantServiceOnSaved;
            ProductVariantService.Deleted += ProductVariantServiceOnDeleted;
            


            LogHelper.Info<UmbracoEvents>("Reassigning default render mvc controller");

            try
            {
                //// By registering this here we can make sure that if route hijacking doesn't find a controller it will use this controller.
                //// That way each page will always be routed through one of our controllers.
                DefaultRenderMvcControllerResolver.Current.SetDefaultControllerType(typeof(DefaultController));
                base.ApplicationStarting(umbracoApplication, applicationContext);
                
                LogHelper.Info<UmbracoEvents>("Default render mvc controller successfully reassigned");
            }
            catch (Exception ex)
            {
                LogHelper.Error<UmbracoEvents>("Failed to reassign default render mvc controller", ex);
            }
            
            //// Add custom routes
            LogHelper.Info<UmbracoEvents>("Adding Custom Routes");
            RouteTable.Routes.LowercaseUrls = true;

            WishlistRoutes.MapRoutes(RouteTable.Routes);
            AuthorizationRoutes.MapRoutes(RouteTable.Routes);
            
            AccountRoutes.MapRoutes(RouteTable.Routes);
            ReceiptRoutes.MapRoutes(RouteTable.Routes);

            var razorEngine = new RazorViewEngine();
            razorEngine.ViewLocationFormats = new string[]{"~/Views/Account/{1}/{0}.cshtml"};
            razorEngine.PartialViewLocationFormats = new string[] { "~/Views/Account/{1}/Partials/{0}.cshtml" };
            ViewEngines.Engines.Add(razorEngine);          
            
        }

        /// <summary>
        /// We stash this here for the HttpHandlers to have access to an umbraco cache
        /// </summary>
        public static UmbracoContext UmbContext { get; private set; }
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            UmbContext = UmbracoContext.Current;
        }


        /// <summary>
        /// The product variant service on deleted.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="deleteEventArgs">
        /// The delete event args.
        /// </param>
        protected void ProductVariantServiceOnDeleted(IProductVariantService sender, DeleteEventArgs<IProductVariant> deleteEventArgs)
        {
            ClearCache();
        }

        /// <summary>
        /// The product variant service on saved.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="saveEventArgs">
        /// The save event args.
        /// </param>
        protected void ProductVariantServiceOnSaved(IProductVariantService sender, SaveEventArgs<IProductVariant> saveEventArgs)
        {
            ClearCache();
        }

        /// <summary>
        /// The product service on deleted.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="deleteEventArgs">
        /// The delete event args.
        /// </param>
        protected void ProductServiceOnDeleted(IProductService sender, DeleteEventArgs<IProduct> deleteEventArgs)
        {
            ClearCache();
        }

        /// <summary>
        /// The product service on saved.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="saveEventArgs">
        /// The save event args.
        /// </param>
        protected void ProductServiceOnSaved(IProductService sender, SaveEventArgs<IProduct> saveEventArgs)
        {
            ClearCache();
        }

        /// <summary>
        /// The content service on deleted.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="deleteEventArgs">
        /// The delete event args.
        /// </param>
        protected void ContentServiceOnDeleted(IContentService sender, DeleteEventArgs<IContent> deleteEventArgs)
        {
            ClearCache();
        }

        /// <summary>
        /// The publishing strategy on published.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="publishEventArgs">
        /// The publish event args.
        /// </param>
        protected void PublishingStrategyOnPublished(IPublishingStrategy sender, PublishEventArgs<IContent> publishEventArgs)
        {
            ClearCache();
        }

        /// <summary>
        /// Clears the MvcDonutCache
        /// </summary>
        private static void ClearCache()
        {
            try
            {
                var cacheManager = new OutputCacheManager();
                cacheManager.RemoveItems();
            }
            catch (Exception ex)
            {
                LogHelper.Error<UmbracoEvents>("Failed to clear MvcDonutCache", ex);
                throw;
            } 
        }
    }
}