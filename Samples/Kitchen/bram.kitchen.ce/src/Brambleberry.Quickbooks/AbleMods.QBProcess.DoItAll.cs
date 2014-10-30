using System;
using System.Collections.Generic;
using Brambleberry.Quickbooks.Models;
using Brambleberry.Quickbooks.Settings;

namespace Brambleberry.Quickbooks
{

    public partial class QBProcess
    {

        private static QBSettings _configSettings;

        public QBProcess()
        {
            if (_configSettings == null)
            {
                // initialize settings
                _configSettings = SettingsManager.QBSettings;
            }
        }

        public static void DoItAll()
        {
            // set order criteria
            NHibernate.ICriteria criteria = NHibernateHelper.CreateCriteria<Order>();
            criteria.Add(nhc.Restrictions.Eq("Exported", false));

            // check store setting for respecting paid/shipped flags
            switch (_configSettings.RespectPayShip)
            {
                case 0: // when placed
                    break;
                case 1: // when paid
                    criteria.Add(nhc.Restrictions.Eq("PaymentStatusId", (byte)2));
                    break;
                case 2: // when paid and shipped
                    criteria.Add(nhc.Restrictions.Eq("PaymentStatusId", (byte)2));
                    criteria.Add(nhc.Restrictions.Disjunction()
                        .Add(nhc.Restrictions.Eq("ShipmentStatusId", (byte)2))
                        .Add(nhc.Restrictions.Eq("ShipmentStatusId", (byte)3))
                        );
                    break;
            }

            // execute your query
            IList<Order> _Orders = criteria.List<Order>();
            //IList<Order> _Orders = OrderDataSource.LoadForCriteria(_Criteria, ConfigSettings.OrderLimit, 0);

            // if no orders found, bail out since there's nothing to do here
            if (_Orders.Count == 0)
            {
                ErrorMessageDataSource.Insert(new ErrorMessage(MessageSeverity.Info, "AbleModsQB", "No orders found ready to transfer."));
                return;
            }
            else
            {
                ErrorMessageDataSource.Insert(new ErrorMessage(MessageSeverity.Info, "AbleModsQB", String.Format("Total of {0} orders found ready to transfer", _Orders.Count)));
            }

            // loop through each order and build the QB data requests
             int _Count = 0; 
             int _Max = _configSettings.OrderLimit; 
             foreach (Order _Order in _Orders) 
             { 
                // make sure we process up to max count 
                if (_Count > _Max && _Max > 0) 
                    break; 

                // if the order has no line items, do NOT transfer it - QB will just blow up
                // force the exported flag to TRUE so this order is never processed again
                if (_Order.Items.Count <= 0)
                {
                    _Order.Exported = true;
                    _Order.Save(false, false);
                    ErrorMessageDataSource.Insert(new ErrorMessage(MessageSeverity.Error, "AbleModsQB", "Transfer of Order # " + _Order.OrderNumber.ToString() + " failed, no line items exist for this order. The order Exported flag has been set to exclude this order from future transfers."));
                    _Count++;
                    continue;
                }

                // BEGIN MOD:  AbleMods.com
                // 12/19/2011

                // see if order is Net-30.  It's ok to transfer Net30 orders even if they are not paid
                bool _IsNet30 = IsOrderNet30(_Order);
                if (!_IsNet30 && _Order.PaymentStatus != OrderPaymentStatus.Paid)
                    continue;
                // END MOD: AbleMods.com
                
                 // process order based on invoice mode setting
                switch (_configSettings.InvoiceMode)
                {
                    case "Invoices/Payments":
                        BuildInvoiceRq(_Order);
                        BuildPaymentAddRq(_Order);
                        break;

                    case "Sales Receipts":
                        BuildSalesReceiptAddRq(_Order);
                        break;

                    case "Sales Orders":
                        BuildSalesOrderAddRq(_Order);
                        break;
                }

                // Does user want a PO built for this order?
                if (_configSettings.GenPO)
                    BuildPurchaseOrderAddRq(_Order);

                // Does user want a bill built for this order?
                if (_configSettings.GenBill)
                    BuildBillAddRq(_Order);

                // we're done, loop to next order
                _Count++;
            }

            return;

        }

        #region "Build Invoice"

        private static void BuildInvoiceRq(Order _AC7Order)
        {
            // since our original criteria includes Exported = False, we don't need to check
            // if this invoice already exists.

            // Build the component records required for a QB invoice
            BuildStandardTermsAddRq("Web Order");
            BuildCustomerAddRq(_AC7Order);

            // Construct the QB invoice
            InvoiceAdd QBInv = new InvoiceAdd();
            QBInv.ARAccountRef.FullName = _configSettings.ARAccount;
            QBInv.CustomerRef.FullName = MakeQBCustId(_AC7Order);
            QBInv.RefNumber = _AC7Order.OrderNumber.ToString();

            if (!string.IsNullOrEmpty(_configSettings.ClassRef))
            {
                QBInv.ClassRef.FullName = _configSettings.ClassRef;
            }

            if (!string.IsNullOrEmpty(_AC7Order.BillToCompany))
            {
                QBInv.BillAddress.Addr1 = _AC7Order.BillToCompany;
                QBInv.BillAddress.Addr2 = _AC7Order.BillToFirstName + " " + _AC7Order.BillToLastName;
                QBInv.BillAddress.Addr3 = _AC7Order.BillToAddress1;
                QBInv.BillAddress.Addr4 = _AC7Order.BillToAddress2;
            }
            else
            {
                QBInv.BillAddress.Addr1 = _AC7Order.BillToFirstName + " " + _AC7Order.BillToLastName;
                QBInv.BillAddress.Addr2 = _AC7Order.BillToAddress1;
                QBInv.BillAddress.Addr3 = _AC7Order.BillToAddress2;
            }

            QBInv.BillAddress.City = _AC7Order.BillToCity;
            if (_AC7Order.BillToCountry != null)
                QBInv.BillAddress.Country = _AC7Order.BillToCountry.Name;

            QBInv.BillAddress.PostalCode = _AC7Order.BillToPostalCode;
            QBInv.BillAddress.State = _AC7Order.BillToProvince;

            if (_AC7Order.Shipments.Count > 0)
            {
                OrderShipment _Shipment = _AC7Order.Shipments[0];

                if (!string.IsNullOrEmpty(_Shipment.ShipToCompany))
                {
                    QBInv.ShipAddress.Addr1 = _Shipment.ShipToCompany;
                    QBInv.ShipAddress.Addr2 = _Shipment.ShipToFullName;
                    QBInv.ShipAddress.Addr3 = _Shipment.ShipToAddress1;
                    QBInv.ShipAddress.Addr4 = _Shipment.ShipToAddress2;
                }
                else
                {
                    QBInv.ShipAddress.Addr1 = _Shipment.ShipToFullName;
                    QBInv.ShipAddress.Addr2 = _Shipment.ShipToAddress1;
                    QBInv.ShipAddress.Addr3 = _Shipment.ShipToAddress2;
                }

                QBInv.ShipAddress.City = _Shipment.ShipToCity;
                if (_Shipment.ShipToCountry != null)
                    QBInv.ShipAddress.Country = _Shipment.ShipToCountry.Name;

                QBInv.ShipAddress.PostalCode = _Shipment.ShipToPostalCode;
                QBInv.ShipAddress.State = _Shipment.ShipToProvince;

                QBInv.ShipMethodRef.FullName = MakeQBShipMethodName(_Shipment.ShipMethodName);

                // Construct this ship method
                BuildShipMethodAddRq(QBInv.ShipMethodRef.FullName);
            }

            QBInv.TermsRef.FullName = "Web Order";
            if (_configSettings.UseShipDateAsInvoiceDate)
            {
                if (_AC7Order.Shipments.Count > 0)
                    QBInv.TxnDate = MakeQBDate((DateTime)_AC7Order.Shipments[0].ShipDate);
            }
            else
                QBInv.TxnDate = MakeQBDate(_AC7Order.OrderDate);

            // build the line items
            foreach (OrderItem _Item in _AC7Order.Items)
            {
                // Build components required for an invoice line item
                InvoiceLineAdd QBLine = new InvoiceLineAdd();
                OrderItem _DiscountCode = new OrderItem();

                switch (_Item.OrderItemType)
                {
                    case OrderItemType.Product:
                        // Regular item product
                        BuildItemAddRq(_Item);
                        BuildSalesTaxCodeAddRq(_Item.TaxCodeId);
                        QBLine.ItemRef.FullName = GetItemFullName(_Item.Sku);

                        // BEGIN MOD:  AbleMods.com
                        // 4/5/2010
                        // Use the scale factor amt stored in the product custom field
                        // to change the quantity sold.  This is done so that a UOM conversion 
                        // can take place when line items are transferred from AC7 into QB
                        int _Scale = 0;
                        if (_Item.Product != null)
                        {
                            foreach (ProductCustomField _pcf in _Item.Product.CustomFields)
                            {
                                if (_pcf.FieldName == "AM:QBMultiplier")
                                    _Scale = AlwaysConvert.ToInt(_pcf.FieldValue);
                            }

                            if (_Scale > 0) // scale has been populated so we must use it
                            {
                                // multiply quantity ordered by the scale value
                                QBLine.Quantity = _Item.Quantity * _Scale;
                                //  re-factor the line total so that the math still comes out right now that the qty has changed
                                //QBLine.Amount = MakeQBAmt(Math.Round((Decimal)(_Item.ExtendedPrice) / _Scale, 2));
                                QBLine.Amount = MakeQBAmt(Math.Round((Decimal)(_Item.ExtendedPrice), 2));
                            }
                            else
                            {
                                // no scale specified so just do things as normal
                                QBLine.Quantity = _Item.Quantity;
                                QBLine.Amount = MakeQBAmt(Math.Round((Decimal)(_Item.ExtendedPrice), 2));
                            }
                        }
                        // END MOD:  AbleMods.com
                        
                        QBLine.SalesTaxCodeRef.FullName = string.Format("{0:000}", _Item.TaxCodeId);
                        QBLine.Desc = _Item.Name;
                        if (!string.IsNullOrEmpty(_configSettings.ClassRef))
                            QBLine.ClassRef.FullName = _configSettings.ClassRef;

                        QBLine.Amount = MakeQBAmt(_Item.ExtendedPrice);
                        QBInv.InvoiceLineAdd.Add(QBLine);
                        break;

                    case OrderItemType.Shipping:
                        // shipping charge
                        if (_configSettings.ShippingItemId == "")
                        {
                            ErrorMessageDataSource.Insert(new ErrorMessage(MessageSeverity.Warn, "AbleModsQB", "Unable to transfer shipping charge on Order # " + _AC7Order.OrderNumber.ToString() + ", Shipping Charge item not set in configuration"));
                            break;
                        }
                        BuildSalesTaxCodeAddRq(_Item.TaxCodeId);

                        QBLine.ItemRef.FullName = _configSettings.ShippingItemId;
                        if (!string.IsNullOrEmpty(_configSettings.ClassRef))
                            QBLine.ClassRef.FullName = _configSettings.ClassRef;

                        QBLine.Quantity = _Item.Quantity;
                        QBLine.Amount = MakeQBAmt(_Item.ExtendedPrice);
                        QBLine.SalesTaxCodeRef.FullName = string.Format("{0:000}", _Item.TaxCodeId);

                        QBLine.Desc = _Item.Name;
                        QBInv.InvoiceLineAdd.Add(QBLine);
                        break;

                    case OrderItemType.Handling:
                        // handling charge
                        if (_configSettings.HandlingItemId == "")
                        {
                            ErrorMessageDataSource.Insert(new ErrorMessage(MessageSeverity.Warn, "AbleModsQB", "Unable to transfer handling charge on Order # " + _AC7Order.OrderNumber.ToString() + ", Handling Charge item not set in configuration"));
                            break;
                        }

                        BuildSalesTaxCodeAddRq(_Item.TaxCodeId);

                        decimal _HandlingAmt = (decimal)_Item.ExtendedPrice;
                        QBLine.ItemRef.FullName = _configSettings.HandlingItemId;
                        if (!string.IsNullOrEmpty(_configSettings.ClassRef))
                            QBLine.ClassRef.FullName = _configSettings.ClassRef;

                        QBLine.Amount = MakeQBAmt(Math.Round(_HandlingAmt, 2));
                        QBLine.Quantity = _Item.Quantity;
                        QBLine.SalesTaxCodeRef.FullName = string.Format("{0:000}", _Item.TaxCodeId);

                        QBLine.Desc = _Item.Name;
                        QBInv.InvoiceLineAdd.Add(QBLine);

                        break;
                    case OrderItemType.GiftWrap:
                        // GiftWrap charge
                        if (_configSettings.GiftWrapItemId == "")
                        {
                            ErrorMessageDataSource.Insert(new ErrorMessage(MessageSeverity.Warn, "AbleModsQB", "Unable to transfer GiftWrap charge on Order # " + _AC7Order.OrderNumber.ToString() + ", GiftWrap Charge item not set in configuration"));
                            break;
                        }

                        BuildSalesTaxCodeAddRq(_Item.TaxCodeId);

                        decimal _GiftWrapAmt = (decimal)_Item.ExtendedPrice;
                        QBLine.ItemRef.FullName = _configSettings.GiftWrapItemId;
                        if (!string.IsNullOrEmpty(_configSettings.ClassRef))
                        {
                            QBLine.ClassRef.FullName = _configSettings.ClassRef;
                        }

                        QBLine.Amount = MakeQBAmt(Math.Round(_GiftWrapAmt, 2));
                        QBLine.Quantity = _Item.Quantity;
                        QBLine.SalesTaxCodeRef.FullName = string.Format("{0:000}", _Item.TaxCodeId);

                        QBLine.Desc = _Item.Name;
                        QBInv.InvoiceLineAdd.Add(QBLine);

                        break;
                    case OrderItemType.Charge:
                        OrderItem _MiscCharge = new OrderItem();
                        _MiscCharge.Sku = "Misc";
                        _MiscCharge.Name = "Misc Charge";
                        _MiscCharge.OrderItemType = OrderItemType.Charge;

                        BuildItemAddRq(_MiscCharge);

                        QBLine.ItemRef.FullName = "Misc";
                        if (!string.IsNullOrEmpty(_configSettings.ClassRef))
                        {
                            QBLine.ClassRef.FullName = _configSettings.ClassRef;
                        }

                        QBLine.Amount = MakeQBAmt(_Item.ExtendedPrice);
                        QBLine.Quantity = _Item.Quantity;
                        QBLine.SalesTaxCodeRef.FullName = string.Format("{0:000}", _Item.TaxCodeId);

                        QBLine.Desc = _Item.Name;
                        QBInv.InvoiceLineAdd.Add(QBLine);

                        break;
                    case OrderItemType.Tax:

                        OrderItem _TaxCharge = new OrderItem();
                        _TaxCharge.Sku = string.Format("{0:000}", _Item.TaxCodeId);
                        _TaxCharge.Name = _Item.Name;

                        BuildItemSalesTaxAddRq(_TaxCharge);

                        QBLine.ItemRef.FullName = GetItemFullName(_TaxCharge.Sku);
                        if (!string.IsNullOrEmpty(_configSettings.ClassRef))
                        {
                            QBLine.ClassRef.FullName = _configSettings.ClassRef;
                        }

                        QBLine.Amount = MakeQBAmt(_Item.ExtendedPrice);
                        //QBLine.Quantity = AlwaysConvert.ToDouble(_Item.Quantity)
                        //QBLine.SalesTaxCodeRef.FullName = String.Format("{0:000}", _Item.TaxCodeId)
                        QBLine.Desc = _Item.Name;
                        QBInv.InvoiceLineAdd.Add(QBLine);

                        break;
                    case OrderItemType.Discount:
                        _DiscountCode.Sku = "MiscDiscount";
                        _DiscountCode.Name = "Manual discount";

                        BuildItemDiscountAddRq(_DiscountCode);

                        QBLine.ItemRef.FullName = "MiscDiscount";
                        if (!string.IsNullOrEmpty(_configSettings.ClassRef))
                        {
                            QBLine.ClassRef.FullName = _configSettings.ClassRef;
                        }

                        QBLine.Amount = MakeQBAmt(_Item.ExtendedPrice);
                        //QBLine.Quantity = AlwaysConvert.ToDouble(_Item.Quantity)
                        QBLine.SalesTaxCodeRef.FullName = string.Format("{0:000}", _Item.TaxCodeId);

                        QBLine.Desc = _Item.Name;
                        QBInv.InvoiceLineAdd.Add(QBLine);

                        break;
                    case OrderItemType.Coupon:
                        _DiscountCode.Sku = _Item.Sku;
                        _DiscountCode.Name = _Item.Name;
                        BuildItemDiscountAddRq(_DiscountCode);

                        QBLine.ItemRef.FullName = GetItemFullName(_Item.Sku); 
                        if (!string.IsNullOrEmpty(_configSettings.ClassRef))
                        {
                            QBLine.ClassRef.FullName = _configSettings.ClassRef;
                        }

                        QBLine.Amount = MakeQBAmt(_Item.ExtendedPrice);
                        //QBLine.Quantity = AlwaysConvert.ToDouble(_Item.Quantity)
                        QBLine.SalesTaxCodeRef.FullName = string.Format("{0:000}", _Item.TaxCodeId);

                        QBLine.Desc = _Item.Name;
                        QBInv.InvoiceLineAdd.Add(QBLine);

                        break;
                    case OrderItemType.Credit:
                        // skip credits (for now)

                        continue;
                }

            }

            InvoiceAddRq _QBInvoiceAddRq = new InvoiceAddRq();
            _QBInvoiceAddRq.InvoiceAdd = QBInv;

            // build add message
            QBXMLMsgsRq _NewMsgRq = new QBXMLMsgsRq();
            _NewMsgRq.InvoiceAddRq = _QBInvoiceAddRq;
            QBList.QBXMLMsgsRq.Add(_NewMsgRq);

            return;
        }
        #endregion

        #region "Build Sales Order"

        private static void BuildSalesOrderAddRq(Order _AC7Order)
        {
            // since our original criteria includes Exported = False, we don't need to check
            // if this invoice already exists.

            // Build the component records required for a QB invoice
            BuildStandardTermsAddRq("Web Order");
            BuildCustomerAddRq(_AC7Order);

            // Construct the QB invoice
            SalesOrderAdd QBSO = new SalesOrderAdd();
            QBSO.CustomerRef.FullName = MakeQBCustId(_AC7Order);
            QBSO.RefNumber = _AC7Order.OrderNumber.ToString();

            if (!string.IsNullOrEmpty(_configSettings.ClassRef))
            {
                QBSO.ClassRef.FullName = _configSettings.ClassRef;
            }

            QBSO.BillAddress.Addr1 = _AC7Order.BillToFirstName + " " + _AC7Order.BillToLastName;
            QBSO.BillAddress.Addr2 = _AC7Order.BillToAddress1;
            QBSO.BillAddress.Addr3 = _AC7Order.BillToAddress2;
            QBSO.BillAddress.City = _AC7Order.BillToCity;
            if (_AC7Order.BillToCountry != null)
            {
                QBSO.BillAddress.Country = _AC7Order.BillToCountry.Name;
            }

            QBSO.BillAddress.PostalCode = _AC7Order.BillToPostalCode;
            QBSO.BillAddress.State = _AC7Order.BillToProvince;


            if (_AC7Order.Shipments.Count > 0)
            {
                OrderShipment _Shipment = _AC7Order.Shipments[0];
                QBSO.ShipAddress.Addr1 = _Shipment.ShipToFullName;
                QBSO.ShipAddress.Addr2 = _Shipment.ShipToAddress1;
                QBSO.ShipAddress.Addr3 = _Shipment.ShipToAddress2;
                QBSO.ShipAddress.City = _Shipment.ShipToCity;
                if (_Shipment.ShipToCountry != null)
                {
                    QBSO.ShipAddress.Country = _Shipment.ShipToCountry.Name;
                }
                QBSO.ShipAddress.PostalCode = _Shipment.ShipToPostalCode;
                QBSO.ShipAddress.State = _Shipment.ShipToProvince;

                QBSO.ShipMethodRef.FullName = MakeQBShipMethodName(_Shipment.ShipMethodName);

                // Construct this ship method
                BuildShipMethodAddRq(QBSO.ShipMethodRef.FullName);
            }

            //If _AC7Order.Payments.Count > 0 Then
            //    BuildPayMethodXML(_AC7Order.Payments(0).PaymentMethodName, _QBLists, _Req)
            //    QBSO.TermsRef.FullName = _AC7Order.Payments(0).PaymentMethodName
            //End If

            QBSO.TermsRef.FullName = "Web Order";

            if (_configSettings.UseShipDateAsInvoiceDate)
            {
                if (_AC7Order.Shipments.Count > 0)
                {
                    QBSO.TxnDate = MakeQBDate((DateTime)_AC7Order.Shipments[0].ShipDate);
                }
            }
            else
                QBSO.TxnDate = MakeQBDate(_AC7Order.OrderDate);

            OrderItem _MiscCharge = new OrderItem();

            // build the line items

            foreach (OrderItem _Item in _AC7Order.Items)
            {
                // check for returned items
                if (_Item.Quantity < 0)
                {
                    ErrorMessageDataSource.Insert(new ErrorMessage(MessageSeverity.Warn, "AbleModsQB", "Order # " + _AC7Order.OrderNumber.ToString() + " has a negative-quantity line item.  This line item is unsupported in QuickBooks and will not be transferred."));
                    continue;
                }
                // Build components required for an invoice line item
                SalesOrderLineAdd QBLine = new SalesOrderLineAdd();
                OrderItem _TaxCharge = new OrderItem();
                OrderItem _DiscountCode = new OrderItem();

                switch (_Item.OrderItemType)
                {
                    case OrderItemType.Product:
                        // Regular item product
                        BuildItemAddRq(_Item);
                        BuildSalesTaxCodeAddRq(_Item.TaxCodeId);

                        QBLine.ItemRef.FullName = GetItemFullName(_Item.Sku);
                        QBLine.Quantity = _Item.Quantity;
                        QBLine.SalesTaxCodeRef.FullName = string.Format("{0:000}", _Item.TaxCodeId);
                        QBLine.Desc = _Item.Name;
                        if (!string.IsNullOrEmpty(_configSettings.ClassRef))
                        {
                            QBLine.ClassRef.FullName = _configSettings.ClassRef;
                        }

                        QBLine.Amount = MakeQBAmt(_Item.ExtendedPrice);
                        QBSO.SalesOrderLineAdd.Add(QBLine);

                        break;
                    case OrderItemType.Shipping:
                        // shipping charge
                        if (_configSettings.ShippingItemId == "")
                        {
                            ErrorMessageDataSource.Insert(new ErrorMessage(MessageSeverity.Warn, "AbleModsQB", "Unable to transfer shipping charge on Order # " + _AC7Order.OrderNumber.ToString() + ", Shipping Charge item not set in configuration"));
                            break;
                        }
                        BuildSalesTaxCodeAddRq(_Item.TaxCodeId);

                        QBLine.ItemRef.FullName = _configSettings.ShippingItemId;
                        if (!string.IsNullOrEmpty(_configSettings.ClassRef))
                        {
                            QBLine.ClassRef.FullName = _configSettings.ClassRef;
                        }

                        QBLine.Quantity = _Item.Quantity;
                        QBLine.Amount = MakeQBAmt(_Item.ExtendedPrice);
                        QBLine.SalesTaxCodeRef.FullName = string.Format("{0:000}", _Item.TaxCodeId);

                        QBLine.Desc = _Item.Name;
                        QBSO.SalesOrderLineAdd.Add(QBLine);

                        break;
                    case OrderItemType.Handling:
                        // handling charge
                        if (_configSettings.HandlingItemId == "")
                        {
                            ErrorMessageDataSource.Insert(new ErrorMessage(MessageSeverity.Warn, "AbleModsQB", "Unable to transfer Handling charge on Order # " + _AC7Order.OrderNumber.ToString() + ", Handling Charge item not set in configuration"));
                            break;
                        }
                        BuildSalesTaxCodeAddRq(_Item.TaxCodeId);

                        decimal _HandlingAmt = (decimal)_Item.ExtendedPrice;
                        QBLine.ItemRef.FullName = _configSettings.HandlingItemId;
                        if (!string.IsNullOrEmpty(_configSettings.ClassRef))
                        {
                            QBLine.ClassRef.FullName = _configSettings.ClassRef;
                        }

                        QBLine.Amount = MakeQBAmt(_HandlingAmt);
                        QBLine.Quantity = _Item.Quantity;
                        QBLine.SalesTaxCodeRef.FullName = string.Format("{0:000}", _Item.TaxCodeId);

                        QBLine.Desc = _Item.Name;
                        QBSO.SalesOrderLineAdd.Add(QBLine);

                        break;
                    case OrderItemType.GiftWrap:
                        // GiftWrap charge
                        if (_configSettings.GiftWrapItemId == "")
                        {
                            ErrorMessageDataSource.Insert(new ErrorMessage(MessageSeverity.Warn, "AbleModsQB", "Unable to transfer GiftWrap charge on Order # " + _AC7Order.OrderNumber.ToString() + ", GiftWrap Charge item not set in configuration"));
                            break;
                        }
                        BuildSalesTaxCodeAddRq(_Item.TaxCodeId);

                        decimal _GiftWrapAmt = (decimal)_Item.ExtendedPrice;
                        QBLine.ItemRef.FullName = _configSettings.GiftWrapItemId;
                        if (!string.IsNullOrEmpty(_configSettings.ClassRef))
                        {
                            QBLine.ClassRef.FullName = _configSettings.ClassRef;
                        }

                        QBLine.Amount = MakeQBAmt(_GiftWrapAmt);
                        QBLine.Quantity = _Item.Quantity;
                        QBLine.SalesTaxCodeRef.FullName = string.Format("{0:000}", _Item.TaxCodeId);

                        QBLine.Desc = _Item.Name;
                        QBSO.SalesOrderLineAdd.Add(QBLine);

                        break;
                    case OrderItemType.Charge:
                        _MiscCharge.Sku = "Misc";
                        _MiscCharge.Name = "Misc Charge";
                        _MiscCharge.OrderItemType = OrderItemType.Charge;

                        BuildItemAddRq(_MiscCharge);

                        QBLine.ItemRef.FullName = "Misc";
                        if (!string.IsNullOrEmpty(_configSettings.ClassRef))
                        {
                            QBLine.ClassRef.FullName = _configSettings.ClassRef;
                        }

                        QBLine.Amount = MakeQBAmt(_Item.ExtendedPrice);
                        QBLine.Quantity = _Item.Quantity;
                        QBLine.SalesTaxCodeRef.FullName = string.Format("{0:000}", _Item.TaxCodeId);

                        QBLine.Desc = _Item.Name;
                        QBSO.SalesOrderLineAdd.Add(QBLine);

                        break;
                    case OrderItemType.Tax:

                        _TaxCharge.Sku = string.Format("{0:000}", _Item.TaxCodeId);
                        _TaxCharge.Name = _Item.Name;
                        BuildItemSalesTaxAddRq(_TaxCharge);

                        QBLine.ItemRef.FullName = GetItemFullName(_TaxCharge.Sku);
                        if (!string.IsNullOrEmpty(_configSettings.ClassRef))
                        {
                            QBLine.ClassRef.FullName = _configSettings.ClassRef;
                        }

                        QBLine.Amount = MakeQBAmt(_Item.ExtendedPrice);
                        //QBLine.Quantity = AlwaysConvert.ToDouble(_Item.Quantity)
                        //QBLine.SalesTaxCodeRef.FullName = String.Format("{0:000}", _Item.TaxCodeId)
                        QBLine.Desc = _Item.Name;
                        QBSO.SalesOrderLineAdd.Add(QBLine);

                        break;
                    case OrderItemType.Discount:
                        _DiscountCode.Sku = "MiscDiscount";
                        _DiscountCode.Name = "Manual discount";
                        BuildItemDiscountAddRq(_DiscountCode);

                        QBLine.ItemRef.FullName = "MiscDiscount";
                        if (!string.IsNullOrEmpty(_configSettings.ClassRef))
                        {
                            QBLine.ClassRef.FullName = _configSettings.ClassRef;
                        }

                        QBLine.Amount = MakeQBAmt(_Item.ExtendedPrice);
                        //QBLine.Quantity = AlwaysConvert.ToDouble(_Item.Quantity)
                        QBLine.SalesTaxCodeRef.FullName = string.Format("{0:000}", _Item.TaxCodeId);

                        QBLine.Desc = _Item.Name;
                        QBSO.SalesOrderLineAdd.Add(QBLine);

                        break;
                    case OrderItemType.Coupon:
                        _DiscountCode.Sku = _Item.Sku;
                        _DiscountCode.Name = _Item.Name;
                        BuildItemDiscountAddRq(_DiscountCode);

                        QBLine.ItemRef.FullName = GetItemFullName(_Item.Sku);
                        if (!string.IsNullOrEmpty(_configSettings.ClassRef))
                        {
                            QBLine.ClassRef.FullName = _configSettings.ClassRef;
                        }

                        QBLine.Amount = MakeQBAmt(_Item.ExtendedPrice);
                        //QBLine.Quantity = AlwaysConvert.ToDouble(_Item.Quantity)
                        QBLine.SalesTaxCodeRef.FullName = string.Format("{0:000}", _Item.TaxCodeId);

                        QBLine.Desc = _Item.Name;
                        QBSO.SalesOrderLineAdd.Add(QBLine);

                        break;
                    case OrderItemType.Credit:
                        // skip credits (for now)

                        continue;
                }

            }

            SalesOrderAddRq _QBSalesOrderAddRq = new SalesOrderAddRq();
            _QBSalesOrderAddRq.SalesOrderAdd = QBSO;

            // build add message
            QBXMLMsgsRq _NewMsgRq = new QBXMLMsgsRq();
            _NewMsgRq.SalesOrderAddRq = _QBSalesOrderAddRq;
            QBList.QBXMLMsgsRq.Add(_NewMsgRq);

            // now see if we should apply credit-card payment info to the customer record
            if (!_configSettings.UpdateCustomerCCInfo)
                return;

            // make sure we have at least 1 payment
            if (_AC7Order.Payments.Count <= 0)
                return;

            // pull in the first payment record
            Payment _Payment = _AC7Order.Payments[0];

            // if payment has no account data, nothing we can do
            if (!_Payment.HasAccountData)
            {
                ErrorMessageDataSource.Insert(new ErrorMessage(MessageSeverity.Warn, "AbleModsQB", "Sales Order # " + _AC7Order.OrderNumber.ToString() + " has a payment but payment account data no longer exists, unable to update Customer credit card information for this sales order."));
                return;
            }

            // make sure it's paid via CC
            string _MethodNames = "Visa|MasterCard|Discover|American Express";
            if (!_MethodNames.Contains(_Payment.PaymentMethodName))
                return;
            
            // set up variables we'll need
            // BEGIN MOD:  AbleMods.com
            // 8/5/2011
            // BUG FIX:  Only use address line 1 and limit length to 40 characters
            string _CreditCardAddress = "";
            if (_AC7Order.BillToAddress1.Length > 40)
                _CreditCardAddress = _AC7Order.BillToAddress1.Substring(0, 40);
            else
                _CreditCardAddress = _AC7Order.BillToAddress1;
            // END MOD: AbleMods.com

            
            string _CreditCardPostalCode = _AC7Order.BillToPostalCode;
            string _NameOnCard = "";
            string _CreditCardNumber = "";
            int _ExpirationMonth = 0;
            int _ExpirationYear = 0;

            // pull in card info and populate the object
            AccountDataDictionary _AcctData = new AccountDataDictionary(_Payment.AccountData);
            foreach (KeyValuePair<string,string> _KeyData in _AcctData)
            {
                if (_KeyData.Key == "AccountName")
                    _NameOnCard = _KeyData.Value;

                if (_KeyData.Key == "AccountNumber")
                    _CreditCardNumber = _KeyData.Value;

                if (_KeyData.Key == "ExpirationMonth")
                    _ExpirationMonth = AlwaysConvert.ToInt(_KeyData.Value);

                if (_KeyData.Key == "ExpirationYear")
                    _ExpirationYear = AlwaysConvert.ToInt(_KeyData.Value);
            }


            // find the original customer ListId and EditSequence values
            string _ListId = "";
            string _EditSequence = "";
            string _CustName = MakeQBCustId(_AC7Order);
            foreach (CustomerRet _CustomerRet in QBList.QBXMLMsgsRs.CustomerQueryRs.CustomerRet)
            {
                if (_CustomerRet.Name == _CustName)
                {
                    _ListId = _CustomerRet.ListID;
                    _EditSequence = _CustomerRet.EditSequence;
                    break;
                }
            }

            // if customer was found in existing customers list, create MOD request
            if (_ListId != "")
            {
                // Make sure the payment method is built
                BuildPaymentMethodAddRq(_Payment.PaymentMethodName);

                CustomerModRq _ModRq = new CustomerModRq();
                _ModRq.CustomerMod.ListID = _ListId;
                _ModRq.CustomerMod.EditSequence = _EditSequence;
                // BEGIN MOD:  AbleMods.com
                // 8/5/2011
                // BUG FIX:  only use address line 1 and limit length to 40 characters
                if (_AC7Order.BillToAddress1.Trim().Length > 40)
                    _ModRq.CustomerMod.CreditCardInfo.CreditCardAddress = _AC7Order.BillToAddress1.Trim().Substring(0, 40);
                else
                    _ModRq.CustomerMod.CreditCardInfo.CreditCardAddress = _AC7Order.BillToAddress1.Trim();
                // END MOD: AbleMods.com

                
                _ModRq.CustomerMod.CreditCardInfo.CreditCardPostalCode = _CreditCardPostalCode;
                _ModRq.CustomerMod.CreditCardInfo.CreditCardNumber = _CreditCardNumber;
                _ModRq.CustomerMod.CreditCardInfo.ExpirationMonth = _ExpirationMonth;
                _ModRq.CustomerMod.CreditCardInfo.ExpirationYear = _ExpirationYear;
                _ModRq.CustomerMod.CreditCardInfo.NameOnCard = _NameOnCard;

                _ModRq.CustomerMod.PreferredPaymentMethodRef.FullName = _Payment.PaymentMethodName;

                // build MOD message
                QBXMLMsgsRq _NewMsgModRq = new QBXMLMsgsRq();
                _NewMsgModRq.CustomerModRq.Add(_ModRq);
                QBList.QBXMLMsgsRq.Add(_NewMsgModRq);

                // exit out, we're done here
                return;
            }

            // customer was not found in existing customer list
            // we must check add-requests to see if it's currently queued to be added
            // if so, we update the add-request to include the CC information

            // loop through requests and see if we have an add-request for this customer
            int _Index = -1;
            for (int x=0;x<QBList.QBXMLMsgsRq.Count;x++)
            {
                if (QBList.QBXMLMsgsRq[x].CustomerAddRq.CustomerAdd.Name == _CustName)
                {
                    _Index = x;
                    break;
                }
            }

            // if add request was found, update it with the CC info data
            if (_Index > -1)
            {
                // Build our component records required for a payment record to be built
                BuildPaymentMethodAddRq(_Payment.PaymentMethodName);

                QBList.QBXMLMsgsRq[_Index].CustomerAddRq.CustomerAdd.CreditCardInfo.CreditCardAddress = _CreditCardAddress;
                QBList.QBXMLMsgsRq[_Index].CustomerAddRq.CustomerAdd.CreditCardInfo.CreditCardNumber = _CreditCardNumber;
                QBList.QBXMLMsgsRq[_Index].CustomerAddRq.CustomerAdd.CreditCardInfo.CreditCardPostalCode = _CreditCardPostalCode;
                QBList.QBXMLMsgsRq[_Index].CustomerAddRq.CustomerAdd.CreditCardInfo.ExpirationMonth = _ExpirationMonth;
                QBList.QBXMLMsgsRq[_Index].CustomerAddRq.CustomerAdd.CreditCardInfo.ExpirationYear = _ExpirationYear;
                QBList.QBXMLMsgsRq[_Index].CustomerAddRq.CustomerAdd.CreditCardInfo.NameOnCard = _NameOnCard;
                QBList.QBXMLMsgsRq[_Index].CustomerAddRq.CustomerAdd.PreferredPaymentMethodRef.FullName = _Payment.PaymentMethodName;

                // add-request has been updated, nothing more to do.
                return;
            }

            // we shouldn't ever reach this point in the code.  If we do, log it for debugging purposes.
            // no existing customer and no add-request for this customer, cannot update CC info
            ErrorMessageDataSource.Insert(new ErrorMessage(MessageSeverity.Error, "AbleModsQB", "Sales Order # " + _AC7Order.OrderNumber.ToString() + " has payment account data but could not find customer '" + _CustName + "', unable to update Customer credit card information for the customer on this sales order."));
            return;

        }
        #endregion

        #region "Build Payment"

        private static void BuildPaymentAddRq(Order _AC7Order)
        {
            // Loop through all payments and refunds applied to this order

            foreach (Payment _Payment in _AC7Order.Payments)
            {
                // Skip this payment is it's a refund
                if (_Payment.Amount < 0)
                {
                    continue;
                }

                // skip the payment if it's not captured
                if (_Payment.PaymentStatus != PaymentStatus.Captured & _Payment.PaymentStatus != PaymentStatus.Completed)
                {
                    continue;
                }

                // Build our component records required for a payment record to be built
                BuildPaymentMethodAddRq(_Payment.PaymentMethodName);

                // pull in cash account assigned to this payments payment-method
                string _CashAccount = AbleContext.Current.Store.Settings.GetValueByKey("QB:PayMethod" + _Payment.PaymentMethodId.ToString());
                //BuildAccountAddRq(_CashAccount, "Bank");

                // build the actual payment record
                ReceivePaymentAdd QBPmt = new ReceivePaymentAdd();

                QBPmt.CustomerRef.FullName = MakeQBCustId(_Payment.Order);
                QBPmt.ARAccountRef.FullName = _configSettings.QBPmtARAccount;
                QBPmt.TxnDate = MakeQBDate(_Payment.PaymentDate);
                QBPmt.RefNumber = _Payment.Id.ToString();
                QBPmt.TotalAmount = MakeQBAmt(_Payment.Amount);
                QBPmt.PaymentMethodRef.FullName = _Payment.PaymentMethodName;
                QBPmt.Memo = "Full Pmt Ref #:" + _Payment.ReferenceNumber;

                QBPmt.DepositToAccountRef.FullName = _CashAccount;
                QBPmt.IsAutoApply = "true";

                // Assign this payment to the cash account specified by the user
                ReceivePaymentAddRq _QBReceivePaymentAddRq = new ReceivePaymentAddRq();
                _QBReceivePaymentAddRq.ReceivePaymentAdd = QBPmt;

                // build add message
                QBXMLMsgsRq _NewMsgRq = new QBXMLMsgsRq();
                _NewMsgRq.ReceivePaymentAddRq = _QBReceivePaymentAddRq;
                QBList.QBXMLMsgsRq.Add(_NewMsgRq);
            }

        }

        #endregion

        #region "Build Bill"

        private static void BuildBillAddRq(Order _AC7Order)
        {

            foreach (OrderShipment _Shipment in _AC7Order.Shipments)
            {
                // Skip the shipment if it's a Solunar/AbleMods email product, no need for a PO/Bill to be generated
                if (_Shipment.Warehouse != null)
                {
                    if (_Shipment.Warehouse.Name.Contains("AbleMods") | _Shipment.ShipMethodName.Contains("Email"))
                        continue;
                }

                BillAdd QBBill = new BillAdd();

                // make sure we have items on this shipment
                if (_Shipment.OrderItems.Count == 0)
                {
                    ErrorMessageDataSource.Insert(new ErrorMessage(MessageSeverity.Warn, "AbleModsQB", "Order # " + _AC7Order.OrderNumber.ToString() + " has a shipment with no items.  Unable to generate QuickBooks Bill for this order shipment."));
                    continue;
                }

                // Pull in the vendor ID from the first line item.  Not elegant, but all
                // we can do with how QB versus AC7 is designed.
                OrderItem _ShipmentItem = _Shipment.OrderItems[0];
                QBBill.VendorRef.FullName = _configSettings.MissingVendor;

                if (_ShipmentItem.Product != null)
                {
                    if (_ShipmentItem.Product.Vendor != null)
                    {
                        if (_ShipmentItem.Product.Vendor.Name != "")
                            QBBill.VendorRef.FullName = _ShipmentItem.Product.Vendor.Name;
                    }
                }

                // Build a vendor record if needed
                BuildVendorAddRq(QBBill.VendorRef.FullName);

                // establish bill reference number based on number of shipments
                QBBill.RefNumber = _Shipment.Order.OrderNumber.ToString();
                if (_AC7Order.Shipments.Count > 1)
                    QBBill.RefNumber += "-" + _Shipment.ShipmentNumber.ToString();
                
                // Continue constructing the bill
                QBBill.APAccountRef.FullName = _configSettings.APAccount;
                QBBill.TxnDate = MakeQBDate(_Shipment.Order.OrderDate);


                // Now loop through the line items for this shipment and add to the QB request
                foreach (OrderItem _OrderItem in _Shipment.OrderItems)
                {
                    ItemLineAdd QBLine = new ItemLineAdd();

                    QBLine.Desc = _OrderItem.Name;
                    QBLine.Quantity = _OrderItem.Quantity.ToString();

                    switch (_OrderItem.OrderItemType)
                    {
                        case OrderItemType.Product:
                            // Regular item product
                            QBLine.ItemRef.FullName = GetItemFullName(_OrderItem.Sku);
                            QBLine.Amount = MakeQBAmt(_OrderItem.Quantity * _OrderItem.CostOfGoods);
                            break;

                        case OrderItemType.Shipping:
                            // shipping charge
                            QBLine.ItemRef.FullName = _configSettings.ShippingItemId;
                            QBLine.Amount = MakeQBAmt(_OrderItem.Price);
                            break;

                        case OrderItemType.Handling:
                            // handling charge as a separate line item in AC7
                            QBLine.ItemRef.FullName = _configSettings.HandlingItemId;
                            QBLine.Amount = MakeQBAmt(_OrderItem.Price);
                            break;

                        default:
                            continue;   // if item is not a product, shipping or handling then skip it

                    }

                    if (!string.IsNullOrEmpty(_configSettings.ClassRef))
                        QBLine.ClassRef.FullName = _configSettings.ClassRef;

                    QBBill.ItemLineAdd.Add(QBLine);

                }

                BillAddRq _QBBillAddRq = new BillAddRq();
                _QBBillAddRq.BillAdd = QBBill;

                // build add message
                QBXMLMsgsRq _NewMsgRq = new QBXMLMsgsRq();
                _NewMsgRq.BillAddRq = _QBBillAddRq;
                QBList.QBXMLMsgsRq.Add(_NewMsgRq);

                return;
            }

        }
        #endregion

        #region "Build-PO"

        private static void BuildPurchaseOrderAddRq(Order _AC7Order)
        {
            // Retrieve a list of the shipments created for this order
            // AC7 will automatically make a seperate shipment record for each different
            // warehouse involved in the order.  Since each warehouse is essentially a different vendor,
            // we leverage this design.

            foreach (OrderShipment _Shipment in _AC7Order.Shipments)
            {
                // Skip the shipment if it's a Solunar/AbleMods email product, no need for a PO/Bill to be generated
                if (_Shipment.Warehouse != null)
                {
                    if (_Shipment.Warehouse.Name.Contains("AbleMods") | _Shipment.ShipMethodName.Contains("Email"))
                        continue;
                }

                PurchaseOrderAdd QBPO = new PurchaseOrderAdd();

                // set up purchase order number
                QBPO.RefNumber = _AC7Order.OrderNumber.ToString();
                if (_AC7Order.Shipments.Count > 1)
                    QBPO.RefNumber += "-" + _Shipment.ShipmentNumber.ToString();

                if (!string.IsNullOrEmpty(_configSettings.ClassRef))
                {
                    QBPO.ClassRef.FullName = _configSettings.ClassRef;
                }


                // snag the ship-to from the shipment of the first lineitem
                QBPO.ShipAddress.Addr1 = _Shipment.ShipToFullName;
                QBPO.ShipAddress.Addr2 = _Shipment.ShipToCompany;
                QBPO.ShipAddress.Addr3 = _Shipment.ShipToAddress1;
                QBPO.ShipAddress.Addr4 = _Shipment.ShipToAddress2;
                QBPO.ShipAddress.City = _Shipment.ShipToCity;
                QBPO.ShipAddress.Country = _Shipment.ShipToCountry.Name;
                QBPO.ShipAddress.PostalCode = _Shipment.ShipToPostalCode;
                QBPO.ShipAddress.State = _Shipment.ShipToProvince;
                QBPO.ShipMethodRef.FullName = MakeQBShipMethodName(_Shipment.ShipMethodName);

                // Construct this ship method
                BuildShipMethodAddRq(QBPO.ShipMethodRef.FullName);

                // Set the shipment msg
                QBPO.VendorMsg = _Shipment.ShipMessage;

                // Set the vendor address based on the warehouse for this shipment
                QBPO.VendorAddress.Addr1 = _Shipment.Warehouse.Address1;
                QBPO.VendorAddress.Addr2 = _Shipment.Warehouse.Address2;
                QBPO.VendorAddress.City = _Shipment.Warehouse.City;
                QBPO.VendorAddress.Country = _Shipment.Warehouse.Country.Name;
                QBPO.VendorAddress.PostalCode = _Shipment.Warehouse.PostalCode;
                QBPO.VendorAddress.State = _Shipment.Warehouse.Province;


                if (_Shipment.OrderItems.Count == 0)
                    continue;  // can't proceed, no line items for this shipment so skip it

                // Pull in the vendor ID from the first line item.  Not elegant, but all
                // we can do with how QB versus AC7 is designed.
                OrderItem _ShipmentItem = _Shipment.OrderItems[0];
                QBPO.VendorRef.FullName = _configSettings.MissingVendor;
                if (_ShipmentItem.Product != null)
                {
                    if (_ShipmentItem.Product.Vendor != null)
                    {
                        if (_ShipmentItem.Product.Vendor.Name != null)
                            QBPO.VendorRef.FullName = _ShipmentItem.Product.Vendor.Name;
                    }
                }

                // Build a vendor record if needed
                BuildVendorAddRq(QBPO.VendorRef.FullName);

                // Set the transaction date to match the order date.
                QBPO.TxnDate = MakeQBDate(_AC7Order.OrderDate);

                // Now loop through the line items for this shipment and add to the QB request
                foreach (OrderItem _OrderItem in _Shipment.OrderItems)
                {
                    PurchaseOrderLineAdd QBLine = new PurchaseOrderLineAdd();

                    QBLine.Desc = _OrderItem.Name;
                    QBLine.Quantity = _OrderItem.Quantity;
                    if (!string.IsNullOrEmpty(_configSettings.ClassRef))
                        QBLine.ClassRef.FullName = _configSettings.ClassRef;

                    switch (_OrderItem.OrderItemType)
                    {
                        case OrderItemType.Product:
                            // Regular item product
                            QBLine.ItemRef.FullName = GetItemFullName(_OrderItem.Sku);
                            QBLine.Amount = MakeQBAmt(_OrderItem.ExtendedPrice);
                            break;

                        case OrderItemType.Shipping:
                            // shipping charge

                            //Dim _FreightAmt As Decimal = GetAC7Freight(_OrderItem)
                            QBLine.ItemRef.FullName = _configSettings.ShippingItemId;
                            QBLine.Amount = MakeQBAmt(_OrderItem.ExtendedPrice);
                            break;

                        case OrderItemType.Handling:
                            // handling charge as a separate line item in AC7

                            //Dim _HandlingAmt As Decimal = _OrderItem.Price
                            QBLine.ItemRef.FullName = _configSettings.HandlingItemId;
                            QBLine.Amount = MakeQBAmt(_OrderItem.Price);
                            break;

                        default:
                            continue;  // if item is not a product, shipping or handling then skip it
                    }

                    // add the line item to the PO
                    QBPO.PurchaseOrderLineAdd.Add(QBLine);

                }

                PurchaseOrderAddRq _QBPurchaseOrderAddRq = new PurchaseOrderAddRq();
                _QBPurchaseOrderAddRq.PurchaseOrderAdd = QBPO;

                // build add message
                QBXMLMsgsRq _NewMsgRq = new QBXMLMsgsRq();
                _NewMsgRq.PurchaseOrderAddRq = _QBPurchaseOrderAddRq;
                QBList.QBXMLMsgsRq.Add(_NewMsgRq);
            }

            return;

        }
        #endregion

        #region "Build Sales Receipt"

        private static void BuildSalesReceiptAddRq(Order _AC7Order)
        {
            // Build the component records required for a QB invoice
            BuildStandardTermsAddRq("Web Order");
            BuildCustomerAddRq(_AC7Order);

            SalesReceiptAdd QBSalesReceipt = new SalesReceiptAdd();

            if (!string.IsNullOrEmpty(_configSettings.ClassRef))
            {
                QBSalesReceipt.ClassRef.FullName = _configSettings.ClassRef;
            }

            QBSalesReceipt.CustomerRef.FullName = MakeQBCustId(_AC7Order);
            QBSalesReceipt.RefNumber = _AC7Order.OrderNumber.ToString();

            if (!string.IsNullOrEmpty(_AC7Order.BillToCompany))
            {
                QBSalesReceipt.BillAddress.Addr1 = _AC7Order.BillToCompany;
                QBSalesReceipt.BillAddress.Addr2 = _AC7Order.BillToFirstName + " " + _AC7Order.BillToLastName;
                QBSalesReceipt.BillAddress.Addr3 = _AC7Order.BillToAddress1;
                QBSalesReceipt.BillAddress.Addr4 = _AC7Order.BillToAddress2;
            }
            else
            {
                QBSalesReceipt.BillAddress.Addr1 = _AC7Order.BillToFirstName + " " + _AC7Order.BillToLastName;
                QBSalesReceipt.BillAddress.Addr2 = _AC7Order.BillToAddress1;
                QBSalesReceipt.BillAddress.Addr3 = _AC7Order.BillToAddress2;
            }

            QBSalesReceipt.BillAddress.City = _AC7Order.BillToCity;
            if (_AC7Order.BillToCountry != null)
            {
                QBSalesReceipt.BillAddress.Country = _AC7Order.BillToCountry.Name;
            }

            QBSalesReceipt.BillAddress.PostalCode = _AC7Order.BillToPostalCode;
            QBSalesReceipt.BillAddress.State = _AC7Order.BillToProvince;

            if (_AC7Order.Shipments.Count > 0)
            {
                OrderShipment _OrderShipment = _AC7Order.Shipments[0];

                if (!string.IsNullOrEmpty(_OrderShipment.ShipToCompany))
                {
                    QBSalesReceipt.ShipAddress.Addr1 = _OrderShipment.ShipToCompany;
                    QBSalesReceipt.ShipAddress.Addr2 = _OrderShipment.ShipToFullName;
                    QBSalesReceipt.ShipAddress.Addr3 = _OrderShipment.ShipToAddress1;
                    QBSalesReceipt.ShipAddress.Addr4 = _OrderShipment.ShipToAddress2;
                }
                else
                {
                    QBSalesReceipt.ShipAddress.Addr1 = _OrderShipment.ShipToFullName;
                    QBSalesReceipt.ShipAddress.Addr2 = _OrderShipment.ShipToAddress1;
                    QBSalesReceipt.ShipAddress.Addr3 = _OrderShipment.ShipToAddress2;
                }

                QBSalesReceipt.ShipAddress.City = _OrderShipment.ShipToCity;

                if (_OrderShipment.ShipToCountry != null)
                    QBSalesReceipt.ShipAddress.Country = _OrderShipment.ShipToCountry.Name;

                QBSalesReceipt.ShipAddress.PostalCode = _OrderShipment.ShipToPostalCode;
                QBSalesReceipt.ShipAddress.State = _OrderShipment.ShipToProvince;
                if (_OrderShipment.ShipMethod != null)
                {
                    QBSalesReceipt.ShipMethodRef.FullName = MakeQBShipMethodName(_OrderShipment.ShipMethod.Name);
                    // Construct this ship method
                    BuildShipMethodAddRq(QBSalesReceipt.ShipMethodRef.FullName);
                }
            }

            if (_configSettings.UseShipDateAsInvoiceDate)
            {
                if (_AC7Order.Shipments.Count > 0)
                {
                    QBSalesReceipt.TxnDate = MakeQBDate((DateTime)_AC7Order.Shipments[0].ShipDate);
                }
            }
            else
                QBSalesReceipt.TxnDate = MakeQBDate(_AC7Order.OrderDate);

            if (_AC7Order.Payments.Count > 0)
            {
                // Build our component records required for a payment method record to be built
                BuildPaymentMethodAddRq(_AC7Order.Payments[0].PaymentMethodName);
                string _CashAccount = AbleContext.Current.Store.Settings.GetValueByKey("QB:PayMethod" + _AC7Order.Payments[0].PaymentMethodId.ToString());
                BuildAccountAddRq(_CashAccount, "Bank");
                QBSalesReceipt.PaymentMethodRef.FullName = _AC7Order.Payments[0].PaymentMethodName;
                QBSalesReceipt.DepositToAccountRef.FullName = _CashAccount;
            }
            else
            {
                // 1-20-2011
                // QB will not allow a sales receipt incoming if the payment method and deposit-to accounts 
                // are not specified.  Even if the order is zero-charge.
                // so if no payments exist for the current order, fake out QB by telling it to use a deposit
                // account from the first payment method found in the store.
                IList<PaymentMethod> _PayMethods = PaymentMethodDataSource.LoadAll();
                if (_PayMethods.Count > 0)
                {
                    BuildPaymentMethodAddRq(_PayMethods[0].Name);
                    string _CashAccount = AbleContext.Current.Store.Settings.GetValueByKey("QB:PayMethod" + _PayMethods[0].Id.ToString());
                    BuildAccountAddRq(_CashAccount, "Bank");
                    QBSalesReceipt.PaymentMethodRef.FullName = _PayMethods[0].Name;
                    QBSalesReceipt.DepositToAccountRef.FullName = _CashAccount;
                }
                else
                {
                    // we are not able to transfer this to QB without some sort of payment method info
                    // log the error and bail out
                    ErrorMessageDataSource.Insert(new ErrorMessage(MessageSeverity.Error, "AbleModsQB", "Transfer of Order # " + _AC7Order.OrderNumber.ToString() + " failed, no payments exist for this order and no store payment methods were found."));
                    return;
                }


            }

            // Determine if order was taxed.
            bool _OrderTaxed = false;
            foreach (OrderItem _OrderItem in _AC7Order.Items)
            {
                if (_OrderItem.OrderItemType == OrderItemType.Tax)
                {
                    _OrderTaxed = true;
                    break; 
                }
            }

            // build the line items

            foreach (OrderItem _Item in _AC7Order.Items)
            {
                // Build components required for an invoice line item
                SalesReceiptLineAdd QBLine = new SalesReceiptLineAdd();
                OrderItem _DiscountCode = new OrderItem();
                OrderItem _TaxCharge = new OrderItem();
                switch (_Item.OrderItemType)
                {

                    case OrderItemType.Product:
                        // Regular item product
                        BuildItemAddRq(_Item);
                        BuildSalesTaxCodeAddRq(_Item.TaxCodeId);

                        QBLine.ItemRef.FullName = GetItemFullName(_Item.Sku);
                        QBLine.Quantity = _Item.Quantity;

                        // if product sold is taxable, mark it as such on the qb line item
                        QBLine.SalesTaxCodeRef.FullName = SetLineTaxCode(_OrderTaxed, _Item);

                        QBLine.Desc = _Item.Name;
                        if (!string.IsNullOrEmpty(_configSettings.ClassRef))
                        {
                            QBLine.ClassRef.FullName = _configSettings.ClassRef;
                        }

                        QBLine.Amount = MakeQBAmt(_Item.ExtendedPrice);

                        QBSalesReceipt.SalesReceiptLineAdd.Add(QBLine);

                        break;
                    case OrderItemType.Shipping:
                        // shipping charge
                        if (_configSettings.ShippingItemId == "")
                        {
                            ErrorMessageDataSource.Insert(new ErrorMessage(MessageSeverity.Warn, "AbleModsQB", "Unable to transfer shipping charge on Order # " + _AC7Order.OrderNumber.ToString() + ", Shipping Charge item not set in configuration"));
                            break;
                        }
                        BuildSalesTaxCodeAddRq(_Item.TaxCodeId);

                        QBLine.ItemRef.FullName = _configSettings.ShippingItemId;
                        if (!string.IsNullOrEmpty(_configSettings.ClassRef))
                        {
                            QBLine.ClassRef.FullName = _configSettings.ClassRef;
                        }

                        QBLine.Quantity = _Item.Quantity;
                        QBLine.Amount = MakeQBAmt(_Item.ExtendedPrice);

                        // if product sold is taxable, mark it as such on the qb line item
                        QBLine.SalesTaxCodeRef.FullName = SetLineTaxCode(_OrderTaxed, _Item);

                        QBLine.Desc = _Item.Name;
                        QBSalesReceipt.SalesReceiptLineAdd.Add(QBLine);

                        break;
                    case OrderItemType.Handling:
                        // handling charge
                        if (_configSettings.HandlingItemId == "")
                        {
                            ErrorMessageDataSource.Insert(new ErrorMessage(MessageSeverity.Warn, "AbleModsQB", "Unable to transfer Handling charge on Order # " + _AC7Order.OrderNumber.ToString() + ", Handling charge item not set in configuration"));
                            break;
                        }
                        BuildSalesTaxCodeAddRq(_Item.TaxCodeId);

                        decimal _HandlingAmt = (decimal)_Item.ExtendedPrice;
                        QBLine.ItemRef.FullName = _configSettings.HandlingItemId;
                        if (!string.IsNullOrEmpty(_configSettings.ClassRef))
                        {
                            QBLine.ClassRef.FullName = _configSettings.ClassRef;
                        }

                        QBLine.Amount = MakeQBAmt(_HandlingAmt);
                        QBLine.Quantity = _Item.Quantity;
                    
                        // if product sold is taxable, mark it as such on the qb line item
                        QBLine.SalesTaxCodeRef.FullName = SetLineTaxCode(_OrderTaxed, _Item);

                        QBLine.Desc = _Item.Name;
                        QBSalesReceipt.SalesReceiptLineAdd.Add(QBLine);

                        break;
                    case OrderItemType.GiftWrap:
                        // GiftWrap charge
                        if (_configSettings.GiftWrapItemId == "")
                        {
                            ErrorMessageDataSource.Insert(new ErrorMessage(MessageSeverity.Warn, "AbleModsQB", "Unable to transfer GiftWrap charge on Order # " + _AC7Order.OrderNumber.ToString() + ", GiftWrap charge item not set in configuration"));
                            break;
                        }
                        BuildSalesTaxCodeAddRq(_Item.TaxCodeId);

                        decimal _GiftWrapAmt = (decimal)_Item.ExtendedPrice;
                        QBLine.ItemRef.FullName = _configSettings.GiftWrapItemId;
                        if (!string.IsNullOrEmpty(_configSettings.ClassRef))
                        {
                            QBLine.ClassRef.FullName = _configSettings.ClassRef;
                        }

                        QBLine.Amount = MakeQBAmt(_GiftWrapAmt);
                        QBLine.Quantity = _Item.Quantity;
                        QBLine.SalesTaxCodeRef.FullName = string.Format("{0:000}", _Item.TaxCodeId);

                        QBLine.Desc = _Item.Name;
                        QBSalesReceipt.SalesReceiptLineAdd.Add(QBLine);

                        break;
                    case OrderItemType.Charge:
                        OrderItem _MiscCharge = new OrderItem();
                        _MiscCharge.Sku = "Misc";
                        _MiscCharge.Name = "Misc Charge";
                        _MiscCharge.OrderItemType = OrderItemType.Charge;

                        BuildItemAddRq(_MiscCharge);

                        QBLine.ItemRef.FullName = "Misc";
                        if (!string.IsNullOrEmpty(_configSettings.ClassRef))
                        {
                            QBLine.ClassRef.FullName = _configSettings.ClassRef;
                        }

                        QBLine.Amount = MakeQBAmt(_Item.ExtendedPrice);
                        QBLine.Quantity = _Item.Quantity;

                        // if product sold is taxable, mark it as such on the qb line item
                        QBLine.SalesTaxCodeRef.FullName = SetLineTaxCode(_OrderTaxed, _Item);

                        QBLine.Desc = _Item.Name;
                        QBSalesReceipt.SalesReceiptLineAdd.Add(QBLine);

                        break;
                    case OrderItemType.Tax:

                        _TaxCharge.Sku = string.Format("{0:000}", _Item.TaxCodeId);
                        _TaxCharge.Name = _Item.Name;
                        BuildItemSalesTaxAddRq(_TaxCharge);

                        QBLine.ItemRef.FullName = GetItemFullName(_TaxCharge.Sku); 
                        if (!string.IsNullOrEmpty(_configSettings.ClassRef))
                        {
                            QBLine.ClassRef.FullName = _configSettings.ClassRef;
                        }

                        QBLine.Amount = MakeQBAmt(_Item.ExtendedPrice);
                        //QBLine.Quantity = AlwaysConvert.ToDouble(_Item.Quantity)
                        //QBLine.SalesTaxCodeRef.FullName = String.Format("{0:000}", _Item.TaxCodeId)
                        QBLine.Desc = _Item.Name;
                        QBSalesReceipt.SalesReceiptLineAdd.Add(QBLine);

                        break;
                    case OrderItemType.Discount:
                        _DiscountCode.Sku = "MiscDiscount";
                        _DiscountCode.Name = "Manual discount";
                        BuildItemDiscountAddRq(_DiscountCode);

                        QBLine.ItemRef.FullName = "MiscDiscount";
                        if (!string.IsNullOrEmpty(_configSettings.ClassRef))
                        {
                            QBLine.ClassRef.FullName = _configSettings.ClassRef;
                        }

                        QBLine.Amount = MakeQBAmt(_Item.ExtendedPrice);
                        //QBLine.Quantity = AlwaysConvert.ToDouble(_Item.Quantity)
                        QBLine.SalesTaxCodeRef.FullName = string.Format("{0:000}", _Item.TaxCodeId);

                        QBLine.Desc = _Item.Name;
                        QBSalesReceipt.SalesReceiptLineAdd.Add(QBLine);

                        break;
                    case OrderItemType.Coupon:
                        _DiscountCode.Sku = _Item.Sku;
                        _DiscountCode.Name = _Item.Name;
                        BuildItemDiscountAddRq(_DiscountCode);

                        QBLine.ItemRef.FullName = GetItemFullName(_Item.Sku); 
                        if (!string.IsNullOrEmpty(_configSettings.ClassRef))
                        {
                            QBLine.ClassRef.FullName = _configSettings.ClassRef;
                        }

                        QBLine.Amount = MakeQBAmt(_Item.ExtendedPrice);
                        //QBLine.Quantity = AlwaysConvert.ToDouble(_Item.Quantity)
                        QBLine.SalesTaxCodeRef.FullName = string.Format("{0:000}", _Item.TaxCodeId);

                        QBLine.Desc = _Item.Name;
                        QBSalesReceipt.SalesReceiptLineAdd.Add(QBLine);

                        break;
                    case OrderItemType.Credit:
                        // skip credits (for now)

                        continue;
                }

            }

            SalesReceiptAddRq _QBSalesReceiptAddRq = new SalesReceiptAddRq();
            _QBSalesReceiptAddRq.SalesReceiptAdd = QBSalesReceipt;

            // build add message
            QBXMLMsgsRq _NewMsgRq = new QBXMLMsgsRq();
            _NewMsgRq.SalesReceiptAddRq = _QBSalesReceiptAddRq;
            QBList.QBXMLMsgsRq.Add(_NewMsgRq);

            return;
        }
        #endregion

        #region "Utilities"

        public static bool UserAuthenticated(string _UserName, string _Password)
        {
            // if either value is invalid, return false
            if (_UserName == string.Empty || _Password == string.Empty)
                return false;

            // Check the credentials against the AC7 user admin list
            IList<User> _Users = UserDataSource.LoadForGroup(1);

            // Only super users can authenticate
            foreach (User _User in _Users)
            {
                if (_UserName == _User.UserName & _User.CheckPassword(_Password))
                    return true;
            }
            return false;
        }

        private static string SetLineTaxCode(bool _Taxed, OrderItem _Item)
        {
            string _RetVal = "";

            // if order was taxed, line items must be marked as taxable?
            if (_Taxed)
            {
                if (_Item.TaxCodeId > 0)
                    _RetVal = "Tax";
                else
                    _RetVal = "Non";

                // make it non-taxable
            }
            else
                _RetVal = "Non";

            return _RetVal;
        }

        /// <summary>
        /// Returns the QB FullName reference for the given SKU.  If none found, returns the original SKU provided.
        /// </summary>
        /// <param name="_Sku">Product SKU to locate.  NOTE only first value matching the SKU is checked.</param>
        /// <returns>FullName value to be used as the QB item FullName or the original SKU provided</returns>
        private static string GetItemFullName(string _Sku)
        {
            // set up return value as the SKU provided
            string _RetVal = _Sku;

            foreach (ItemInventoryRet _Ret in QBList.QBXMLMsgsRs.ItemInventoryQueryRs.ItemInventoryRet)
                if (_Ret.Name == _Sku)
                    _RetVal = _Ret.FullName;

            foreach (ItemNonInventoryRet _Ret in QBList.QBXMLMsgsRs.ItemNonInventoryQueryRs.ItemNonInventoryRet)
                if (_Ret.Name == _Sku)
                    _RetVal = _Ret.FullName;

            foreach (ItemServiceRet _Ret in QBList.QBXMLMsgsRs.ItemServiceQueryRs.ItemServiceRet)
                if (_Ret.Name == _Sku)
                    _RetVal = _Ret.FullName;

            foreach (ItemDiscountRet _Ret in QBList.QBXMLMsgsRs.ItemDiscountQueryRs.ItemDiscountRet)
                if (_Ret.Name == _Sku)
                    _RetVal = _Ret.FullName;

            foreach (ItemOtherChargeRet _Ret in QBList.QBXMLMsgsRs.ItemOtherChargeQueryRs.ItemOtherChargeRet)
                if (_Ret.Name == _Sku)
                    _RetVal = _Ret.FullName;

            foreach (ItemSalesTaxRet _Ret in QBList.QBXMLMsgsRs.ItemSalesTaxQueryRs.ItemSalesTaxRet)
                if (_Ret.Name == _Sku)
                    _RetVal = _Ret.Name;

            // exit and return value
            return _RetVal;
        }


        #endregion

    }




}