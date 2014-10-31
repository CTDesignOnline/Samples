

using BrambleBerry.Kitchen.Models.ViewModels;

namespace BrambleBerry.Kitchen.Models.Account.MyOrders
{
    using System;
    using System.Collections.Generic;
    using Merchello.Core.Models;

    public class OrderViewModel : ViewModelBase
    {
        public OrderModel Order { get; set; }

    }
}
