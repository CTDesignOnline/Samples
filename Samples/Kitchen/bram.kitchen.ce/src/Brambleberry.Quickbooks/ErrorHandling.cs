using Brambleberry.Quickbooks.Models;
using Brambleberry.Quickbooks.Settings;
using Merchello.Core.Models;
using Umbraco.Core.Logging;

namespace Brambleberry.Quickbooks
{

    public class QBErrorHandling
    {
        /// <summary>
        /// Parses for a response from the Intuit Web Connector
        /// </summary>
        /// <param name="msgList">List of messages being sent to QB</param>
        /// <param name="msgCount">Counter value indicating the current message number we're on</param>
        /// <param name="responseXml">Response string of XML returned from QB</param>
        /// <returns>TRUE if response was considered an error, FALSE if no response error</returns>
        public static bool ParseResponse(QBXML msgList, int msgCount, string responseXml)
        {
            // set up return value
            bool retVal = false;

            // pull in original request
            QBXMLMsgsRq qbRequest = QBList.QBXMLMsgsRq[msgCount];

            // find the response error
            QBResponseStatus rs = msgList.QBXMLMsgsRs.ResponseStatus;

            if (rs.StatusCode == "9999")
            {
                // no response was detected, did we miss something?
                LogEvent("Unknown response detected: " + responseXml, true);
                //// WARNING:  Code should never hit here, means we're missing a message response type
                //QBResponseStatus _Err2 = _MsgList.QBXMLMsgsRs.ResponseStatus;
                //QBXML _junk = new QBXML();
                //_junk.QBXMLMsgsRq = new List<QBXMLMsgsRq>();
                //_junk.QBXMLMsgsRq.Add(_QBRequest);
                //XmlDocument _junkdoc = _junk.ToXml();
            }
            // load up the original object name that generated this response
            string respNodeName = rs.ObjectName;

            // parse the error code
            switch (rs.StatusCode)
            {
                case "0": // request responded 'ok'
                    // check if we were adding a new record
                    if (rs.ObjectMode != "Add")
                        break;

                    // ok we were trying to add something
                    // if we're not adding an invoice/sales receipt, no further logging is necessary
                    if (respNodeName != "Invoice" & respNodeName != "SalesReceipt" & respNodeName != "SalesOrder" )
                        break;

                    // we're adding invoice/sales receipt, create additional log details
                    int _OrderNumber = 0;

                    // see if we're adding an invoice or sales receipt
                    switch (respNodeName)
                    {
                        case "SalesOrder":
                            _OrderNumber = AlwaysConvert.ToInt(qbRequest.SalesOrderAddRq.SalesOrderAdd.RefNumber);
                            break;
                        case "Invoice":
                            _OrderNumber = AlwaysConvert.ToInt(qbRequest.InvoiceAddRq.InvoiceAdd.RefNumber);
                            break;
                        case "SalesReceipt":
                            _OrderNumber = AlwaysConvert.ToInt(qbRequest.SalesReceiptAddRq.SalesReceiptAdd.RefNumber);
                            break;
                            
                    }

                    if (_OrderNumber == 0)  // we were adding invoice, sales receipt or sales order but couldn't find original AC7 order number
                    {
                        // we didn't find the original request so just log it and move on
                        LogEvent("Request to add " + respNodeName + " had error " + rs.StatusCode + " with message " + rs.StatusMessage, true);
                        retVal = true;
                        break;
                    }
                    // Load up the order so we can update the Exported flag
                    Order _Order = OrderDataSource.Load(OrderDataSource.LookupOrderId(_OrderNumber));

                    if (_Order == null)
                    {
                        LogEvent("The add request needed to update Order # " + _OrderNumber + " in AC7 but that order could not be found.  Order has not been marked as exported.", true);
                        retVal = true;
                        break;
                    }
                    // Order was found

                    // log a note if set to do so
                    if (ConfigSettings.AddOrderNote)
                    {
                        // make a new order note
                        OrderNote _Note = new OrderNote();
                        _Note.OrderId = _Order.Id;
                        _Note.NoteType = NoteType.SystemPrivate;
                        _Note.CreatedDate = LocaleHelper.LocalNow;
                        _Note.Comment = "Order exported to QuickBooks";
                        _Note.UserId = AbleContext.Current.UserId;
                        _Order.Notes.Add(_Note);
                        _Order.Save(false, false);
                        LogEvent("Order note for Order # " + _Order.OrderNumber.ToString() + " was saved.", false);
                    }

                    // if we're not set to mark-exported, nothing else to do on a status-ok response of '0'
                    if (!ConfigSettings.SetExported)
                    {
                        LogEvent("Response <ok> trying to " + rs.ObjectMode + " a(n) " + respNodeName + " with key value " + GetQbRequestKeyValue(respNodeName, qbRequest), false);
                        break;
                    }

                    _Order.Exported = true;
                    _Order.Save(false, false);
                    LogEvent(respNodeName + ": Order # " + _Order.OrderNumber.ToString() + " Exported flag set.", false);
                    break;

                case "3100":  // ignore "Item name already exists" messages
                    break;

                default: // catch-all for any unusual response errors
                    string keyValue = GetQbRequestKeyValue(respNodeName, qbRequest);
                    LogEvent("Error " + rs.StatusCode + ": While trying to " + rs.ObjectMode + " a(n) " + rs.ObjectName + " with key value of '" + keyValue + "', QB responded " + rs.StatusMessage, true);
                    retVal = true;
                    break;
            }

            // exit and return value
            return retVal;

        }

        #region "UtilityMethods"

        private static string GetQbRequestKeyValue(string node, QBXMLMsgsRq resp)
        {
            string retVal = "";
            switch (node)
                {
                case "Account":
                    retVal = resp.AccountAddRq.AccountAdd.Name;
                    break;
                case "Class":
                    retVal = resp.ClassAddRq.ClassAdd.Name;
                    break;
                case "Customer":
                    retVal = resp.CustomerAddRq.CustomerAdd.Name;
                    break;
                case "Invoice":
                    retVal = resp.InvoiceAddRq.InvoiceAdd.RefNumber;
                    break;
                case "ItemDiscount":
                    retVal = resp.ItemDiscountAddRq.ItemDiscountAdd.Name;
                    break;
                case "ItemGroup":
                    retVal = resp.ItemGroupAddRq.ItemGroupAdd.Name;
                    break;
                case "ItemInventory":
                    retVal = resp.ItemInventoryAddRq.ItemInventoryAdd.Name;
                    break;
                case "ItemNonInventory":
                    retVal = resp.ItemNonInventoryAddRq.ItemNonInventoryAdd.Name;
                    break;
                case "ItemOtherCharge":
                    retVal = resp.ItemOtherChargeAddRq.ItemOtherChargeAdd.Name;
                    break;
                case "ItemSalesTax":
                    retVal = resp.ItemSalesTaxAddRq.ItemSalesTaxAdd.Name;
                    break;
                case "ItemService":
                    retVal = resp.ItemServiceAddRq.ItemServiceAdd.Name;
                    break;
                case "PaymentMethod":
                    retVal = resp.PaymentMethodAddRq.PaymentMethodAdd.Name;
                    break;
                case "PurchaseOrder":
                    retVal = resp.PurchaseOrderAddRq.PurchaseOrderAdd.RefNumber;
                    break;
                case "ReceivePayment":
                    retVal = resp.ReceivePaymentAddRq.ReceivePaymentAdd.RefNumber;
                    break;
                case "SalesOrder":
                    retVal = resp.SalesOrderAddRq.SalesOrderAdd.RefNumber;
                    break;
                case "SalesReceipt":
                    retVal = resp.SalesReceiptAddRq.SalesReceiptAdd.RefNumber;
                    break;
                case "SalesTaxCode":
                    retVal = resp.SalesTaxCodeAddRq.SalesTaxCodeAdd.Name;
                    break;
                case "ShipMethod":
                    retVal = resp.ShipMethodAddRq.ShipMethodAdd.Name;
                    break;
                case "StandardTerms":
                    retVal = resp.StandardTermsAddRq.StandardTermsAdd.Name;
                    break;
                case "Vendor":
                    retVal = resp.VendorAddRq.VendorAdd.Name;
                    break;
                case "SalesRep":
                    retVal = resp.SalesRepAddRq.SalesRepAdd.SalesRepEntityRef.FullName;
                    break;
                case "CustomerType":
                    retVal = resp.CustomerTypeAddRq.CustomerTypeAdd.Name;
                    break;
                }

            return retVal;
        }

        public static void LogEvent(string logMsg, bool required)
        {
            // debug mode is on, log everything to the ~/App_Data/logs/App.log file
            if (ConfigSettings.DebugMode == "ON" | required)
                LogHelper.Info(typeof(QBSettings), logMsg);
        }
        #endregion

    }


}
