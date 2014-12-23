namespace Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    /// <summary>
    /// Summary description for AddItemModel
    /// </summary>
    public class AddItemModel
    {
        /// <summary>
        /// Content Id of the ProductDetail page
        /// </summary>
        public int ContentId { get; set; }

        /// <summary>
        /// The Product Key (pk) of the product to be added to the cart.
        /// </summary>
        /// <remarks>
        /// 
        /// NOTE : This is not the ProductVariantKey. The variant will be figured out
        /// by the product key and the array of Guids (OptionChoices).  These define the ProductVariant 
        /// 
        /// </remarks>
        public Guid ProductKey { get; set; }
    }
}