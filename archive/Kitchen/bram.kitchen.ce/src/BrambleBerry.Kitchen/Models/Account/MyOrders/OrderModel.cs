

using BrambleBerry.Kitchen.Services.ZenDesk.Models;

namespace BrambleBerry.Kitchen.Models.Account.MyOrders
{
    using System;
    using System.Collections.Generic;
    using Merchello.Core.Models;

    public class OrderModel
    {
        public OrderModel(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; private set; }

        public IEnumerable<OrderLineItem> Items { get; set; }

        public DateTime CreatedOn { get; set; }

        public IAddress BillingAddress { get; set; }

        public OrderState State { get; set; }

        private List<ZendeskUpdateModel> _supportUpdates;
        public List<ZendeskUpdateModel> SupportUpdates
        {
            get
            {
                if (_supportUpdates == null)
                {
                    _supportUpdates = Services.ZenDesk.ZendeskService.Instance.GetUpdatesForOrder(Id);
                }

                return _supportUpdates;
            }
        }

        public enum OrderState
        {
            NotFulfilled,
            BackOrder,
            Cancelled,
            Fulfilled
        }
    }
}
