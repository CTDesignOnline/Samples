using System;

namespace AbleMods.Api.Models
{
    public class UWishlistItem
    {
        public int WishlistItemId { get; set; }
        public int WishlistId { get; set; }
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public string LineMessage { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public int Desired { get; set; }
        public int Received { get; set; }
        public int Priority { get; set; }
        public string Comment { get; set; }
    }
}