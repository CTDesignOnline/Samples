using BrambleBerry.Kitchen.Models.ViewModels;

namespace BrambleBerry.Kitchen.Controllers
{
    using System.Web.Mvc;    
    using DevTrends.MvcDonutCaching;
    using Models;
    using Umbraco.Web.Models;

    /// <summary>
    /// The default controller.
    /// </summary>
    /// <remarks>
    /// If route hijacking doesn't find a controller it will use this controller.
    /// </remarks>
    public class DefaultController : KitchenControllerBase
    {
        /// <summary>
        /// The default controller method
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/> to render the basic view model
        /// </returns>
        [DonutOutputCache(CacheProfile = "FiveMins")]
        public override ActionResult Index(RenderModel model)
        {
            return CurrentTemplate(BuildModel<ViewModelBase>());
        }
    }
}