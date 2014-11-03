namespace Controllers
{

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Merchello.Core;
    using Merchello.Core.Gateways.Payment;
    using Merchello.Core.Models;
    using Merchello.Core.Models.MonitorModels;
    using Merchello.Web;
    using Models;
    using Umbraco.Core;
    using Umbraco.Web.Mvc;

    /// <summary>
    /// Summary description for CheckoutController
    /// </summary>
    [PluginController("MerchelloExample")]
    public class CheckoutController : MerchelloSurfaceContoller
    {

        private const int BasketPageId = 1062;
        private const int ShipRateQuoteId = 1075; // Shipment Rate Quotes
        private const int PaymentInfoId = 1111; // Selecting payment information
        private const int ConfirmationId = 1112; // confirmation
        private const int ReceiptId = 1117; // final page - receipt (if paid)

        public CheckoutController()
            : this(MerchelloContext.Current)
        { }

        public CheckoutController(IMerchelloContext merchelloContext)
            : base(merchelloContext)
        { }

        [HttpPost]
        public ActionResult SaveAddress(AddressModel model)
        {
            var address = model.ToAddress();

            // address saved to extended data on table merchAnonymousCustomer
            Basket.SalePreparation().SaveBillToAddress(address);
            Basket.SalePreparation().SaveShipToAddress(address);

            // go to payment page - only the cash payment is installed
            return RedirectToUmbracoPage(1116);
        }

        // get here from button on payment page
        [HttpGet]
        public ActionResult SavePayment(PaymentInformationModel model)
        {
            IPaymentResult attempt = null;
            var paymentMethod = Payment.GetPaymentGatewayMethodByKey(model.PaymentMethodKey).PaymentMethod;

            // Save the payment method selection
            Basket.SalePreparation().SavePaymentMethod(paymentMethod);


            // make sure there is an address row  
            if (!Basket.SalePreparation().IsReadyToInvoice()) return RedirectToUmbracoPage(BasketPageId);


            var preparation = Basket.SalePreparation();

            if (Merchello.Core.Constants.ProviderKeys.Payment.CashPaymentProviderKey == paymentMethod.ProviderKey)
            {
                // AuthorizePayment will save the invoice with an Invoice Number.
                //
                attempt = preparation.AuthorizePayment(paymentMethod.Key);

            }

            //return RenderConfirmationThankyou(attempt, preparation.GetBillToAddress().Email);

            if (!attempt.Payment.Success)
            {
                // TODO Notification trigger for bad payment
                // Notific
            }
            else
            {
                Basket.Empty();
                Basket.Save();

                // trigger the order notification confirmation
                //Notification.Trigger("OrderConfirmation", attempt, new[] { customerEmail });
            }
            Basket.SalePreparation().PrepareInvoice();

            //return RedirectToUmbracoPage(ReceiptId);
            var receipt = Umbraco.TypedContent(ReceiptId);

            var invoice = Basket.SalePreparation().PrepareInvoice();

            //return View("CheckoutInvoice",invoice);

            return
                Redirect(string.Format("{0}?inv={1}", receipt.Url,
                                       attempt.Invoice.Key.ToString().EncryptWithMachineKey()));


        }

        private ActionResult RenderInvoice(IInvoice invoice)
        {
            return PartialView("Invoice", invoice);
        }
        [ChildActionOnly]
        public ActionResult RenderReceipt(string invoiceKey)
        {
            Guid key;
            if (Guid.TryParse(invoiceKey, out key))
            {
                var invoice = Services.InvoiceService.GetByKey(key);
                return PartialView("Invoice", invoice);
            }
            throw new InvalidOperationException();
        }
    }
}