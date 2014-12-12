using System.Linq;
using System.Web.Mvc;
using Merchello.Core.Formatters;
using Merchello.Core.Gateways.Notification;
using Umbraco.Web.Models;

namespace BrambleBerry.Kitchen.Controllers.Account
{
    /// <summary>
    /// The contact page controller.
    /// </summary>
    /// <remarks>
    /// This controller is dependent on Merchello's <see cref="INotificationContext"/> so that we can use the <see cref="IPatternReplaceFormatter"/> and
    /// the back office configured SMTP settings
    /// </remarks>
    public class MyWishListsController : BaseAccountController
    {
        

        /// <summary>
        /// The default controller method
        /// </summary>
        /// <param name="model">
        /// The default <see cref="RenderModel"/>
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/> to render the contact page template view
        /// </returns>        
        public override ActionResult Index(RenderModel model)
        {
            var usersWishlists = Helpers.WishlistHelpers.GetUsersWishLists(Customer.Key).ToList();

            var viewModel = BuildModel<Models.Account.MyWishlists.MyWishlistsViewModel>();

            viewModel.Wishlists = usersWishlists;
            viewModel.CurrentWishList = usersWishlists.FirstOrDefault(x => x.IsDefaultList);

            return View(viewModel);
        }

        /// <summary>
        /// The default controller method
        /// </summary>
        /// <param name="model">
        /// The default <see cref="RenderModel"/>
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/> to render the contact page template view
        /// </returns>        
        public ActionResult View(string id)
        {
            var usersWishlists = Helpers.WishlistHelpers.GetUsersWishLists(Customer.Key).ToList();

            var viewModel = BuildModel<Models.Account.MyWishlists.MyWishlistsViewModel>();

            viewModel.Wishlists = usersWishlists;
            viewModel.CurrentWishList = usersWishlists.FirstOrDefault(x => x.Title.ToLower() == id.ToLower());

            return View(viewModel);
        }
       
    }

  
}