using System;
using System.Linq;

namespace BrambleBerry.Kitchen.Controllers.Account
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Models.Account.MyOrders;
    using Merchello.Core.Formatters;
    using Merchello.Core.Gateways.Notification;
    using Umbraco.Web.Models;

    /// <summary>
    /// The contact page controller.
    /// </summary>
    /// <remarks>
    /// This controller is dependent on Merchello's <see cref="INotificationContext"/> so that we can use the <see cref="IPatternReplaceFormatter"/> and
    /// the back office configured SMTP settings
    /// </remarks>
    public class MyOrdersController : BaseAccountController
    {


        /// <summary>
        /// The default controller method
        /// </summary>
        /// <param name="model">
        /// The default <see cref="RenderModel"/>
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/> to render the contact page template view
        /// </returns>        
        public override ActionResult Index(RenderModel model)
        {
            var viewModel = BuildModel<BrambleBerry.Kitchen.Models.Account.MyOrders.IndexViewModel>();

            viewModel.Orders = GetUsersOrders().Where(x => x.State != OrderModel.OrderState.Cancelled);


            return View(viewModel);
        }

        public ActionResult Open()
        {
            var viewModel = BuildModel<BrambleBerry.Kitchen.Models.Account.MyOrders.IndexViewModel>();
            
            viewModel.Orders = GetUsersOrders().Where(x => x.State == OrderModel.OrderState.NotFulfilled || x.State == OrderModel.OrderState.BackOrder);

            return View(viewModel);
        }

        public ActionResult Cancelled()
        {
            var viewModel = BuildModel<BrambleBerry.Kitchen.Models.Account.MyOrders.IndexViewModel>();
            
            viewModel.Orders = GetUsersOrders().Where(x => x.State == OrderModel.OrderState.Cancelled);

            return View(viewModel);
        }
        public ActionResult View(Guid id)
        {
            var viewModel = BuildModel<OrderViewModel>();

            //get current users order, this will ensure the current user has access.
            viewModel.Order = GetUsersOrders().FirstOrDefault(x => x.Id == id);
            
            return View(viewModel);
        }


        
        private IEnumerable<OrderModel> GetUsersOrders()
        {
            var invoices = Services.InvoiceService.GetInvoicesByCustomerKey(Customer.Key);

            var orders = new List<OrderModel>();
            foreach (var invoice in invoices)
            {
                foreach (var order in invoice.Orders)
                {
                    orders.Add(order.ToOrderViewModel(invoice));
                }
            }
            return orders.OrderByDescending(x => x.CreatedOn);
        }

    }


}