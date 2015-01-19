using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrambleBerry.Kitchen.Models.Workflow;

namespace BrambleBerry.Kitchen.Models.Account.MyWishlists
{
    public class MyWishlistsViewModel : BaseAccountViewModel
    {
        /// <summary>
        /// Users Wishlists
        /// </summary>
        public IEnumerable<WishListModel> Wishlists { get; set; }

        public WishListModel CurrentWishList { get; set; }
    }
}
