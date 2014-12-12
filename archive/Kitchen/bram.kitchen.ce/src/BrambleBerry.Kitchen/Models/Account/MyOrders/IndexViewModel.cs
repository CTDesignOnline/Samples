namespace BrambleBerry.Kitchen.Models.Account.MyOrders
{
    using ViewModels;
    using System.Collections.Generic;

    public class IndexViewModel : ViewModelBase
    {
        public IEnumerable<OrderModel> Orders { get; set; }
    }
}
