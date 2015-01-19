namespace BrambleBerry.Kitchen.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Buzz.Hybrid;
    using Merchello.Core;
    using Merchello.Core.Formatters;
    using Merchello.Core.Gateways.Notification;
    using Merchello.Core.Models;
    using Models;
    using Umbraco.Web;
    using Umbraco.Web.Models;
    using System;
    using BrambleBerry.Kitchen.Events;
    using BrambleBerry.Kitchen.Models.ViewModels;
    using BrambleBerry.Kitchen.Models.Workflow;

    /// <summary>
    /// The contact page controller.
    /// </summary>
    /// <remarks>
    /// This controller is dependent on Merchello's <see cref="INotificationContext"/> so that we can use the <see cref="IPatternReplaceFormatter"/> and
    /// the back office configured SMTP settings
    /// </remarks>
    public class WishListController : MerchelloControllerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WishListPageController"/> class.
        /// </summary>
        public WishListController()
            : this(MerchelloContext.Current)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WishListPageController"/> class.
        /// </summary>
        /// <param name="merchelloContext">
        /// The merchello context.
        /// </param>
        public WishListController(IMerchelloContext merchelloContext)
            : base(merchelloContext)
        {
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
        public override ActionResult Index(RenderModel model)
        {
            var viewModel = BuildModel<WishListViewModel>();
            return View("WishListIndex", viewModel);
        }

        public ActionResult View(Guid customerId, string wishlistName)
        {
            var viewModel = BuildModel<WishListViewModel>();

            //extract the user
            viewModel.CustomerKey = customerId;
            //Get all wishlists for a user
            var usersWishlists = Helpers.WishlistHelpers.GetUsersWishLists(customerId);
            //null is user wasnt found
            if (usersWishlists != null)
            {
                if (!string.IsNullOrEmpty(wishlistName))
                {
                    //request specified which wishlist to view, so we'll try to extract that one to render

                    viewModel.Wishlist = usersWishlists.FirstOrDefault(x => x.IsPublic && x.Title == wishlistName);
                }
                else
                {
                    //no list was specified so lets find the default list
                    viewModel.Wishlist = usersWishlists.FirstOrDefault(x => x.IsPublic && x.IsDefaultList);
                }

                //Did we find a list?
                if (viewModel.Wishlist != null)
                {
                    viewModel.CustomerKey = CurrentCustomer.Key;
                    viewModel.TotalBasketPrice = 5;
                    //viewModel.CustomerKey = customerId;
                    return CurrentTemplate(viewModel);
                }
                else
                {
                    viewModel.WishlistNotFound = true;
                }
            }
            else
            {
                viewModel.UserNotFound = true;
            }

            return View("WishListIndex", viewModel);
        }

      
    }

   
}