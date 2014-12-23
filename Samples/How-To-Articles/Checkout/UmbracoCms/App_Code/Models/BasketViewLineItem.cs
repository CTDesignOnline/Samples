using System;
using Merchello.Core.Models;

namespace Models
{
    public class BasketViewLineItem
    {
        // Merchello Product Guid
        public Guid Key { get; set; }

        // Umbraco Content Id - notice in BasketViewModelExtensions that it is 
        // pulled from the extended properties
        public int ContentId { get; set; }

        /// <summary>
        /// Umbraco Content page name or Merchello Product Name 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// This is a the extended properties that travel forward with the basket
        /// </summary>
        public string ExtendedData { get; set; }

        /// <summary>
        /// The sku 
        /// </summary>
        public string Sku { get; set; }

        /// <summary>
        /// The price 
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// The total price of the line item
        /// </summary>
        public decimal TotalPrice { get; set; }

        /// <summary>
        /// The quantity to purchase
        /// </summary>
        public int Quantity { get; set; }
    }
}