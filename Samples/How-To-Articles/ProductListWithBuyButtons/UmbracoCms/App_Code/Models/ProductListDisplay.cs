namespace Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Merchello.Web.Models.ContentEditing;

    /// <summary>
    /// Summary description for ProductListDisplay
    /// </summary>
    public class ProductContentDisplay : ProductDisplay
    {
        /// <summary>
        /// Content Id of the ProductDetail page
        /// </summary>
        public int ProductContentId { get; set; }
    }

}