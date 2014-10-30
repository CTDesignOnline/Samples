using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrambleBerry.Kitchen.Models.Workflow;
using Merchello.Core.Models;

namespace BrambleBerry.Kitchen.Helpers
{
    static class WishlistHelpers
    {
        internal static IEnumerable<WishListModel> GetUsersWishLists(Guid customerKey)
        {
            var wishlistFromCache = Merchello.Web.Workflow.WishList.GetWishList(customerKey);
            if (wishlistFromCache == null)
            {
                return null;
            }
            var wishListsVisitor = new WishlistDifferentiatorVisitor();
            wishlistFromCache.Items.Accept(wishListsVisitor);
            return wishListsVisitor.Wishlists;
        }
        private class WishlistDifferentiatorVisitor : ILineItemVisitor
        {
            private readonly List<WishListModel> wishlists = new List<WishListModel>();


            public void Visit(ILineItem lineItem)
            {
                var name = lineItem.ExtendedData["WishlistTitle"];
                var wishlist = wishlists.FirstOrDefault(x => x.Title == name);
                if (wishlist == null)
                {
                    wishlist = new WishListModel()
                    {
                        CustomerID = lineItem.ContainerKey,
                        IsPublic = bool.Parse(lineItem.ExtendedData["IsPublic"])
                    };
                    wishlists.Add(wishlist);
                }
                wishlist.Items.Add(new WishListItemModel()
                {
                    QuantityPurchased = int.Parse(lineItem.ExtendedData["QuantityPurchased"]),
                    QuantityWanted = lineItem.Quantity,
                    SKU = lineItem.Sku,
                });
            }
            public IEnumerable<WishListModel> Wishlists
            {
                get { return wishlists; }
            }
        }
    }
    
}
