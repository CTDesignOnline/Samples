using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using BrambleBerry.Kitchen.Models.Workflow;
using Buzz.Hybrid;

namespace BrambleBerry.Kitchen.Models.ViewModels
{
    public class WishListViewModel : ViewModelBase
    {  /// <summary>
        /// Gets the alias of the content type this view model represents
        /// </summary>
        public static string RepresentsContentType
        {
            get { return "WishList"; }
        }

        /// <summary>
        /// Gets main body text
        /// </summary>
        public IHtmlString BodyText
        {
            get { return this.GetSafeHtmlString("bodyText"); }
        }


        /// <summary>
        /// Gets or sets the customer key.
        /// </summary>
        public Guid CustomerKey { get; set; }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        public WishListModel Wishlist { get; set; }

        /// <summary>
        /// Gets or sets the total basket price.
        /// </summary>
        public decimal TotalBasketPrice { get; set; }

        public bool WishlistNotFound { get; set; }
        public bool UserNotFound { get; set; }
    }
}
