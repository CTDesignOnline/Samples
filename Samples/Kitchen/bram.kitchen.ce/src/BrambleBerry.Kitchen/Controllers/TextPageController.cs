using BrambleBerry.Kitchen.Models.ViewModels;

namespace BrambleBerry.Kitchen.Controllers
{
    using System.Web.Mvc;
    using DevTrends.MvcDonutCaching;
    using Models;
    using Umbraco.Web.Models;

    /// <summary>
    /// The text page controller.
    /// </summary>
    public class TextPageController : KitchenControllerBase
    {
        /// <summary>
        /// The default controller method
        /// </summary>
        /// <param name="model">
        /// The default <see cref="RenderModel"/>
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/> to render the text page template view
        /// </returns>
        [DonutOutputCache(CacheProfile = "FiveMins")]
        public override ActionResult Index(RenderModel model)
        {
            return CurrentTemplate(BuildModel<TextPageViewModel>());
        }
    }
}