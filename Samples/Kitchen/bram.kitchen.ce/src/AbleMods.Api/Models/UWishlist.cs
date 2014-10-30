using System.Collections.Generic;

namespace AbleMods.Api.Models
{
    public class UWishlist
    {
        public int WishlistId { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string ViewPassword { get; set; }
        public bool IsPublic { get; set; }
        public List<UWishlistItem> WishlistItems { get; set; }
    }
}