namespace BrambleBerry.Kitchen.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using Models.ViewModels;
    using Merchello.Core;
    using Merchello.Core.Models;
	using Umbraco.Core;
    using Umbraco.Web.Models;

    /// <summary>
    /// The receipt controller.
    /// </summary>
    public class ReceiptController : MerchelloControllerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReceiptController"/> class.
        /// </summary>
        public ReceiptController()
            : this(MerchelloContext.Current)
        {            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReceiptController"/> class.
        /// </summary>
        /// <param name="merchelloContext">
        /// The merchello context.
        /// </param>
        public ReceiptController(IMerchelloContext merchelloContext) 
            : base(merchelloContext)
        {
        }

        /// <summary>
        /// The default controller method
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        public override ActionResult Index(RenderModel model)
        {
            var receiptModel = BuildModel<ReceiptViewModel>();          

            return CurrentTemplate(receiptModel);
        }

        /// <summary>
        /// The receipt.
        /// </summary>
        /// <param name="key">
        /// The invoice key.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        public ActionResult Receipt(Guid key)
        {
            var invoice = Services.InvoiceService.GetByKey(key);

            var shipment = invoice.Items.Where(x => x.LineItemType == LineItemType.Shipping)
                .Select(x => x.ExtendedData.GetShipment<InvoiceLineItem>()).FirstOrDefault();

            var receiptModel = BuildModel<ReceiptViewModel>();

            receiptModel.Invoice = invoice;

            if (shipment == null)
            {
                receiptModel.IsShippable = false;                
            }
            else
            {
                receiptModel.IsShippable = true;
                receiptModel.ShippingAddress = shipment.GetDestinationAddress();
                receiptModel.ShipMethodName = shipment.Carrier;
            }


            return View(receiptModel);
        }
    }
}