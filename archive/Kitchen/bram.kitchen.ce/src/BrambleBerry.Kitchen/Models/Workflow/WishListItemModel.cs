using System;

namespace BrambleBerry.Kitchen.Models.Workflow
{
    public class WishListItemModel
    {
        public Guid ID { get; set; }
        public Guid WishListID { get; set; }

        public string SKU { get; set; }

        /// <summary>
        /// The amount the user would like
        /// </summary>
        public int QuantityWanted { get; set; }

        /// <summary>
        /// The amount they already have
        /// </summary>
        public int QuantityPurchased { get; set; }

        public int SortOrder { get; set; }
    }
}