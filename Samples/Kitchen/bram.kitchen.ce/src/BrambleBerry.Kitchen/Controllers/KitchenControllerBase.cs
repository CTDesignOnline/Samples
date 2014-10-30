namespace BrambleBerry.Kitchen.Controllers
{
    using System.Web.Mvc;
    using Models;
    using Models.ViewModels;
    using Buzz.Hybrid;
    using Buzz.Hybrid.Controllers;
    using Buzz.Hybrid.Helpers;
    using Umbraco.Web;

    /// <summary>
    /// Represents the base kitchen site controller
    /// </summary>
    /// <remarks>
    /// At the moment this is a placeholder in case we want to add functionality to all of the kitchen site 
    /// controllers
    /// </remarks>
    public abstract class KitchenControllerBase : BaseSurfaceController
    {      
        /// <summary>
        /// Builds the strongly typed model representing the current Umbraco content.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the model
        /// </typeparam>
        /// <returns>
        /// A new render model.
        /// </returns>
        protected T BuildModel<T>() where T : ViewModelBase, new()
        {
            var model = GetModel<T>();

            var linkHelper = new LinkHelper();

            var homePage = model.Content.AncestorOrSelf("HomePage");

            model.Menu = linkHelper.BuildLinkTier(homePage, model.Content, null, 0, 1);

            model.Logo = new SiteLogo()
            {
                SiteName = MvcHtmlString.Create(homePage.GetSafeString("org", homePage.Name)),
                Url = homePage.Url
            };

            return model;
        }

        /// <summary>
        /// The build model.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the model
        /// </typeparam>
        /// <returns>
        /// A new render model.
        /// </returns>
        protected void RebuildFormPostback<T>(T model) where T : ViewModelBase, new()
        {
            var linkHelper = new LinkHelper();

            var homePage = model.Content.AncestorOrSelf("HomePage");

            model.Menu = linkHelper.BuildLinkTier(homePage, model.Content, null, 0, 1);

            model.Logo = new SiteLogo()
            {
                SiteName = MvcHtmlString.Create(homePage.GetSafeString("org", homePage.Name)),
                Url = homePage.Url
            };
        }
    }
}

