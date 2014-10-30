
namespace BrambleBerry.Kitchen.Models.Account.MyOrders
{
    using System;

    using Buzz.Hybrid.Models;

    // TODO RSS discuss Line Item Model integration with OffRoadCode
    public class OrderLineItem : ILineItemModel
    {
        public OrderLineItem(Guid itemId) : this( Guid.Empty, itemId )
        {
        }

        public OrderLineItem(Guid orderId, Guid itemId)
        {
            Id = itemId;
            ParentOrderId = orderId;
        }

        public Guid Key
        {
            get
            {
                return Id;
            }

            set
            {
                Id = value;
            }
        }

        public int ContentId { get; set; }

        public IImage Thumbnail { get; set; }

        /// <summary>
        /// The human friendly name of this item at the time it was added to the order, look up on SKU if expecting a change in name
        /// </summary>
        public string Name { get; set; }

        public string VariantName { get; set; }

        /// <summary>
        /// The unique id for this line item, is differs from SKU, many line items can reference the same SKU hence the need for an id
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// The id of the parent order, TODO this might not stay as you can look it up but would be nice to have it wrapped up somewhere
        /// </summary>
        public Guid ParentOrderId { get; private set; }

        /// <summary>
        /// The unique identifier for the product (this should uniquely identify it for size, colour, etc. too)
        /// </summary>
        public string Sku { get; set; }

        public decimal UnitPrice { get; set; }

        /// <summary>
        /// This is the price at the time the order was made
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// The total price for this line item
        /// </summary>
        public decimal TotalPrice { get; set; }

        public Boolean IsShippable { get; set; }


        /// <summary>
        /// How many of the chosen items do they want?
        /// </summary>
        public int Quantity { get; set; }

        public string Url { get; set; }

        public bool HasImage { get; private set; }

        public bool IsVariant { get; set; }

        public DateTime SortDate { get; set; }

        public DateTime DateCreated { get; set; }
    }
}
