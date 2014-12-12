namespace BrambleBerry.Kitchen.Models.ViewModels
{
    using System.Web;
    using Buzz.Hybrid;
    using Merchello.Core.Models;

    /// <summary>
    /// The receipt view model.
    /// </summary>
    public class ReceiptViewModel : ViewModelBase
    {
        /// <summary>
        /// Gets the represents content type.
        /// </summary>
        public static string RepresentsContentType
        {
            get { return "Receipt"; }
        }

        /// <summary>
        /// Gets or sets the invoice.
        /// </summary>
        public IInvoice Invoice { get; set; }

        /// <summary>
        /// Gets or sets the shipping information
        /// </summary>
        public IAddress ShippingAddress { get; set; }

        /// <summary>
        /// Gets or sets the ship method name.
        /// </summary>
        public string ShipMethodName { get; set; }

        /// <summary>
        /// Gets the body text.
        /// </summary>
        public IHtmlString BodyText
        {
            get { return Content.GetSafeHtmlString("bodyText"); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the order is a shippable order.
        /// </summary>
        public bool IsShippable { get; set; }
    }
}