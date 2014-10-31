using System;
using System.Collections.Generic;
using Brambleberry.Quickbooks.Models;

namespace Brambleberry.Quickbooks
{
    public partial class QBProcess
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(QBProcess));
        private static readonly bool _DemoMode = false;

        public static void BuildCustomerAddRq(Order _AC7Order)
        {
            // If the customer add request already exists, don't construct another one
            if (QBList.CustomerAddRqExists(MakeQBCustId(_AC7Order)))
                return;

            // check if in the response list from QB
            if (InResponseList("Customer", MakeQBCustId(_AC7Order)))
                return;

            // Construct subordinate required records
            BuildStandardTermsAddRq("Web Order");

            // build the main customer record
            CustomerAdd _CustomerAdd = new CustomerAdd();

            _CustomerAdd.CompanyName = _AC7Order.BillToCompany;
            _CustomerAdd.Contact = _AC7Order.BillToFirstName + " " + _AC7Order.BillToLastName;
            _CustomerAdd.Email = _AC7Order.BillToEmail;
            _CustomerAdd.Fax = _AC7Order.BillToFax;
            _CustomerAdd.FirstName = _AC7Order.BillToFirstName;
            _CustomerAdd.IsActive = "true";
            _CustomerAdd.LastName = _AC7Order.BillToLastName;
            _CustomerAdd.Name = MakeQBCustId(_AC7Order);
            _CustomerAdd.Phone = _AC7Order.BillToPhone;
            _CustomerAdd.TermsRef.FullName = "Web Order";

            // if the setting is that all orders go under the same customer ID, then push 
            // the address lines down one and store the customer name on address line 1.
            if (_CustomerAdd.Name == "Web Store")
            {
                _CustomerAdd.BillAddress.Addr1 = _CustomerAdd.FirstName + " " + _CustomerAdd.LastName;
                _CustomerAdd.BillAddress.Addr2 = _AC7Order.BillToAddress1;
                _CustomerAdd.BillAddress.Addr3 = _AC7Order.BillToAddress2;
            }
            else
            {
                _CustomerAdd.BillAddress.Addr1 = _AC7Order.BillToAddress1;
                _CustomerAdd.BillAddress.Addr2 = _AC7Order.BillToAddress2;
            }

            _CustomerAdd.BillAddress.City = _AC7Order.BillToCity;
            _CustomerAdd.BillAddress.State = _AC7Order.BillToProvince;
            _CustomerAdd.BillAddress.PostalCode = _AC7Order.BillToPostalCode;

            if (_AC7Order.BillToCountry != null)
            {
                _CustomerAdd.BillAddress.Country = _AC7Order.BillToCountry.Name;
            }

            if (_configSettings.DefCustomerType != "")
            {
                _CustomerAdd.CustomerTypeRef.FullName = _configSettings.DefCustomerType;
            }

            if (_configSettings.DefSalesRep != "")
            {
                _CustomerAdd.SalesRepRef.FullName = _configSettings.DefSalesRep;
            }

            // build add request and exit
            CustomerAddRq _QBCustomerAddRq = new CustomerAddRq();
            _QBCustomerAddRq.CustomerAdd = _CustomerAdd;

            // build add message
            QBXMLMsgsRq _NewMsgRq = new QBXMLMsgsRq();
            _NewMsgRq.CustomerAddRq = _QBCustomerAddRq;
            QBList.QBXMLMsgsRq.Add(_NewMsgRq);

            return;
        }

        public static void BuildItemAddRq(OrderItem _OrderItem)
        {
            // some basic error checking
            if (_OrderItem == null)
            {
                ErrorMessageDataSource.Insert(new ErrorMessage(MessageSeverity.Warn, "AbleModsQB", "Null line item passed to BuildItemAddRq, line item skipped."));
                return;
            }
            //if (_OrderItem.Product == null && _OrderItem.OrderItemType == OrderItemType.Product)
            //{
            //    ErrorMessageDataSource.Insert(new ErrorMessage(MessageSeverity.Warn, "AbleModsQB", "Order # " + _OrderItem.Order.OrderNumber.ToString() + " references a product that no longer exists in the catalog.  Order transfer most likely will fail."));
            //    return;
            //}

            Product _Product = _OrderItem.Product;
            if (_Product != null)
            {
                if (_Product.InventoryMode == InventoryMode.None)
                    BuildNonInventoryItemAddRq(_OrderItem);
                else
                    BuildInventoryItemAddRq(_OrderItem);
            }
            else
            {
                BuildInventoryItemAddRq(_OrderItem);
            }


            return;
        }

        private static void BuildInventoryItemAddRq(OrderItem _OrderItem)
        {
            // Make sure we have a SKU value
            if (string.IsNullOrEmpty(_OrderItem.Sku))
            {
                ErrorMessageDataSource.Insert(new ErrorMessage(MessageSeverity.Warn, "AbleModsQB", "OrderItemId " + _OrderItem.Id.ToString() + " has no SKU.  Using default QuickBooks Item Name of 'Unknown Item'"));
                _OrderItem.Sku = "Unknown Item";
            }

            // If the item add request already exists, don't construct another one
            if (QBList.ItemInventoryAddRqExists(_OrderItem.Sku))
                return;

            // check if in the response list from QB
            if (InResponseList("Item", _OrderItem.Sku))
                return;

            // create the item as an inventoryitem in Quickbooks
            ItemInventoryAdd QBItem = new ItemInventoryAdd();
            QBItem.IsActive = "true";
            QBItem.Name = _OrderItem.Sku;
            string _ItemName = _OrderItem.Name;
            if (!string.IsNullOrEmpty(_OrderItem.VariantName))
            {  
                // its a variant product
                _ItemName += "(" + _OrderItem.VariantName + ")";

                // try to pull in stock quantity, but only if product still exists
                QBItem.QuantityOnHand = 0;
                if (_OrderItem.Product != null)
                {
                    if (_OrderItem.ProductVariant != null)
                        QBItem.QuantityOnHand = _OrderItem.ProductVariant.InStock;
                }
            }
            else
            { // regular product
                if (_OrderItem.Product != null)
                    QBItem.QuantityOnHand = _OrderItem.Product.InStock;
                else
                    QBItem.QuantityOnHand = 0;
            }

            QBItem.PurchaseDesc = _OrderItem.Name;
            QBItem.SalesDesc = _OrderItem.Name;
            QBItem.PurchaseCost = (Double)_OrderItem.CostOfGoods;
            QBItem.SalesPrice = (Double)_OrderItem.Price;
            QBItem.IncomeAccountRef.FullName = _configSettings.IncomeAcctRef;
            QBItem.COGSAccountRef.FullName = _configSettings.COGSAcctRef;
            QBItem.AssetAccountRef.FullName = _configSettings.ItemAssetAcct;

            // create the request and return
            ItemInventoryAddRq _QBItemAddRq = new ItemInventoryAddRq();
            _QBItemAddRq.ItemInventoryAdd = QBItem;

            // build add message
            QBXMLMsgsRq _NewMsgRq = new QBXMLMsgsRq();
            _NewMsgRq.ItemInventoryAddRq = _QBItemAddRq;
            QBList.QBXMLMsgsRq.Add(_NewMsgRq);

            return;
        }

        private static void BuildNonInventoryItemAddRq(OrderItem _OrderItem)
        {
            // Make sure we have a SKU value
            if (string.IsNullOrEmpty(_OrderItem.Sku))
                _OrderItem.Sku = "Unknown Item";

            // If the customer add request already exists, don't construct another one
            if (QBList.ItemNonInventoryAddRqExists(_OrderItem.Sku))
                return;

            // check if in the response list from QB
            if (InResponseList("Item", _OrderItem.Sku))
                return;

            // create the item as a non-inventory item in QuickBooks
            ItemNonInventoryAdd QBItem = new ItemNonInventoryAdd();
            QBItem.Name = _OrderItem.Sku;
            QBItem.IsActive = "true";

            QBItem.SalesAndPurchase.IncomeAccountRef.FullName = _configSettings.IncomeAcctRef;
            QBItem.SalesAndPurchase.ExpenseAccountRef.FullName = _configSettings.COGSAcctRef;

            QBItem.SalesAndPurchase.SalesPrice = (Double)_OrderItem.Price;
            QBItem.SalesAndPurchase.SalesDesc = _OrderItem.Name;
            QBItem.SalesAndPurchase.PurchaseDesc = _OrderItem.Name;
            QBItem.SalesAndPurchase.PurchaseCost = (Double)_OrderItem.CostOfGoods;

            ItemNonInventoryAddRq _QBItemNonAddRq = new ItemNonInventoryAddRq();
            _QBItemNonAddRq.ItemNonInventoryAdd = QBItem;

            // build add message
            QBXMLMsgsRq _NewMsgRq = new QBXMLMsgsRq();
            _NewMsgRq.ItemNonInventoryAddRq = _QBItemNonAddRq;
            QBList.QBXMLMsgsRq.Add(_NewMsgRq);

            return;
        }


        public static void BuildItemDiscountAddRq(OrderItem _OrderItem)
        {
            if (_OrderItem.OrderItemType != OrderItemType.Product)
                return;

            // If the item already exists, don't construct it
            if (QBList.ItemDiscountAddRqExists(_OrderItem.Sku))
                return;

            // check if in the response list from QB
            if (InResponseList("ItemDiscount", _OrderItem.Sku))
                return;

            ItemDiscountAdd QBItem = new ItemDiscountAdd();
            QBItem.Name = _OrderItem.Sku;
            QBItem.IsActive = "true";
            QBItem.ItemDesc = "AC7 Discount";
            //QBItem.SalesTaxCodeRef.FullName = "000"
            QBItem.AccountRef.FullName = AbleContext.Current.Store.Settings.GetValueByKey("QB:InvDiscAcct");
            QBItem.DiscountRate = 0;

            ItemDiscountAddRq _QBItemDiscountAddRq = new ItemDiscountAddRq();
            _QBItemDiscountAddRq.ItemDiscountAdd = QBItem;

            // build add message
            QBXMLMsgsRq _NewMsgRq = new QBXMLMsgsRq();
            _NewMsgRq.ItemDiscountAddRq = _QBItemDiscountAddRq;
            QBList.QBXMLMsgsRq.Add(_NewMsgRq);

            return;
        }


        public static void BuildItemSalesTaxAddRq(OrderItem _OrderItem)
        {
            if (_OrderItem.OrderItemType != OrderItemType.Product)
                return;

            // If the item already exists, don't construct it
            if (QBList.ItemSalesTaxAddRqExists(_OrderItem.Sku))
                return;

            // check if in the response list from QB
            if (InResponseList("ItemSalesTax", _OrderItem.Sku))
                return;

            TaxRule _AC7TaxRule = TaxRuleDataSource.Load(AlwaysConvert.ToInt(_OrderItem.Sku));

            ItemSalesTaxAdd QBItem = new ItemSalesTaxAdd();
            QBItem.Name = string.Format("{0:000}", AlwaysConvert.ToInt(_OrderItem.Sku));
            QBItem.ItemDesc = _OrderItem.Name;
            QBItem.IsActive = "true";
            QBItem.TaxVendorRef.FullName = AbleContext.Current.Store.Settings.GetValueByKey("QB:SalesTaxVendor");

            if (_AC7TaxRule != null)
            {
                QBItem.TaxRate = _AC7TaxRule.TaxRate.ToString();
            }
            else
            {
                QBItem.TaxRate = "5";
                // default to 5% tax rate
            }

            ItemSalesTaxAddRq _QBItemSalesTaxAddRq = new ItemSalesTaxAddRq();
            _QBItemSalesTaxAddRq.ItemSalesTaxAdd = QBItem;

            // build add message
            QBXMLMsgsRq _NewMsgRq = new QBXMLMsgsRq();
            _NewMsgRq.ItemSalesTaxAddRq = _QBItemSalesTaxAddRq;
            QBList.QBXMLMsgsRq.Add(_NewMsgRq);

            return;
        }

        public static void BuildStandardTermsAddRq(string _Name)
        {
            // If the add request already exists, don't construct another one
            if (QBList.StandardTermsAddRqExists(_Name))
                return;

            // check if in the response list from QB
            if (InResponseList("StandardTerms", _Name))
                return;

            StandardTermsAdd _QBStandardTerms = new StandardTermsAdd();
            _QBStandardTerms.IsActive = "true";
            _QBStandardTerms.Name = _Name;
            StandardTermsAddRq _QBStandardTermsAddRq = new StandardTermsAddRq();
            _QBStandardTermsAddRq.StandardTermsAdd = _QBStandardTerms;

            // build add message
            QBXMLMsgsRq _NewMsgRq = new QBXMLMsgsRq();
            _NewMsgRq.StandardTermsAddRq = _QBStandardTermsAddRq;
            QBList.QBXMLMsgsRq.Add(_NewMsgRq);

            return;
        }

        public static void BuildPaymentMethodAddRq(string _Name)
        {
            // If the add request already exists, don't construct another one
            if (QBList.PaymentMethodAddRqExists(_Name))
                return;

            // check if in the response list from QB
            if (InResponseList("PaymentMethod", _Name))
                return;

            PaymentMethodAdd QBMethod = new PaymentMethodAdd();
            QBMethod.IsActive = "true";
            QBMethod.Name = _Name;
            PaymentMethodAddRq _QBPaymentMethodAddRq = new PaymentMethodAddRq();
            _QBPaymentMethodAddRq.PaymentMethodAdd = QBMethod;

            // build add message.
            // BEGIN MOD: AbleMods.com
            // 5/2/2011
            // Payment Method values HAVE to be processed first in the list before any other objects
            // END MOD: AbleMods.com

            QBXMLMsgsRq _NewMsgRq = new QBXMLMsgsRq();
            _NewMsgRq.PaymentMethodAddRq = _QBPaymentMethodAddRq;
            QBList.QBXMLMsgsRq.Insert(0, _NewMsgRq);

            return;
        }

        public static void BuildAccountAddRq(string _Name, string _AcctType)
        {
            // If the add request already exists, don't construct another one
            if (QBList.AccountAddRqExists(_Name))
                return;

            // check if in the response list from QB
            if (InResponseList("Account", _Name))
                return;

            AccountAdd _QBCashAccount = new AccountAdd();
            _QBCashAccount.AccountType = _AcctType;
            _QBCashAccount.Name = _Name;
            _QBCashAccount.Desc = "AC7 Payment Method";

            AccountAddRq _QBCashAccountAddRq = new AccountAddRq();
            _QBCashAccountAddRq.AccountAdd = _QBCashAccount;

            // build add message
            QBXMLMsgsRq _NewMsgRq = new QBXMLMsgsRq();
            _NewMsgRq.AccountAddRq = _QBCashAccountAddRq;
            QBList.QBXMLMsgsRq.Add(_NewMsgRq);

            return;
        }

        //' Tax rules in AC7 become ItemSalesTaxCodes in Quickbooks
        //Public Shared Sub BuildTaxRuleXML(ByRef _Req As ArrayList)

        //    ' load in ac7 tax rules
        //    Dim AC7TaxRules As TaxRuleCollection = TaxRuleDataSource.LoadForStore()

        //    For Each _TaxRule As TaxRule In AC7TaxRules

        //        Dim QBItemSalesTaxAdd As New ItemSalesTaxAdd
        //        QBItemSalesTaxAdd.Name = _TaxRule.Name
        //        QBItemSalesTaxAdd.TaxRate = _TaxRule.TaxRate
        //        QBItemSalesTaxAdd.TaxVendorRef.FullName = AbleContext.Current.Store.Settings.GetValueByKey("QB:SalesTaxVendor")
        //        Dim QBItemSalesTaxAddRq As New ItemSalesTaxAddRq
        //        QBItemSalesTaxAddRq.ItemSalesTaxAdd = QBItemSalesTaxAdd
        //        _Req.Add(QBItemSalesTaxAddRq.ToXml)
        //    Next

        //End Sub

        // Tax codes in AC7 becomes SalesTaxCodes in QuickBooks
        // Load in all AC7 tax codes

        public static void BuildSalesTaxCodeAddRq(int _AC7TaxCodeId)
        {
            string _TaxCode = string.Format("{0:000}", _AC7TaxCodeId);

            // If the add request already exists, don't construct another one
            if (QBList.SalesTaxCodeAddRqExists(_TaxCode))
                return;

            // check if in the response list from QB
            if (InResponseList("SalesTaxCode", _TaxCode))
                return;

            SalesTaxCodeAdd QBSalesTaxCode = new SalesTaxCodeAdd();

            TaxCode _AC7TaxCode = TaxCodeDataSource.Load(_AC7TaxCodeId);
            if (_AC7TaxCode != null)
            {
                QBSalesTaxCode.Name = string.Format("{0:000}", _AC7TaxCode.Id);
                QBSalesTaxCode.Desc = _AC7TaxCode.Name;
            }
            else
            {
                QBSalesTaxCode.Name = "000";
                QBSalesTaxCode.Desc = "Misc Code";
            }

            QBSalesTaxCode.IsTaxable = "true";
            SalesTaxCodeAddRq _QBTaxCodeAddRq = new SalesTaxCodeAddRq();
            _QBTaxCodeAddRq.SalesTaxCodeAdd = QBSalesTaxCode;

            // build add message
            QBXMLMsgsRq _NewMsgRq = new QBXMLMsgsRq();
            _NewMsgRq.SalesTaxCodeAddRq = _QBTaxCodeAddRq;
            QBList.QBXMLMsgsRq.Add(_NewMsgRq);

            return;
        }

        public static void BuildVendorAddRq(string _VendorName)
        {
            // If the add request already exists, don't construct another one
            if (QBList.VendorAddRqExists(_VendorName))
                return;

            // check if in the response list from QB
            if (InResponseList("Vendor", _VendorName))
                return;

            ICriteria _Criteria = NHibernateHelper.CreateCriteria<Vendor>();
            _Criteria.Add(nhc.Restrictions.Eq("Store", AbleContext.Current.Store));
            _Criteria.Add(nhc.Restrictions.Eq("Name", _VendorName));

            IList<Vendor> _Vendors = VendorDataSource.LoadForCriteria(_Criteria);
            Vendor _Vendor = new Vendor();
            if (_Vendors.Count > 0)
                _Vendor = _Vendors[0];

            VendorAdd QBVendor = new VendorAdd();
            QBVendor.CompanyName = _Vendor.Name;
            QBVendor.Email = _Vendor.Email;
            QBVendor.Name = _Vendor.Name;
            QBVendor.Notes = "AC7 Vendor ID: " + _Vendor.Id.ToString();

            VendorAddRq _QBVendorAddRq = new VendorAddRq();
            _QBVendorAddRq.VendorAdd = QBVendor;

            // build add message
            QBXMLMsgsRq _NewMsgRq = new QBXMLMsgsRq();
            _NewMsgRq.VendorAddRq = _QBVendorAddRq;
            QBList.QBXMLMsgsRq.Add(_NewMsgRq);

            return;
        }


        public static void BuildShipMethodAddRq(string _Name)
        {
            // If the add request already exists, don't construct another one
            if (QBList.ShipMethodAddRqExists(_Name))
                return;

            // check if in the response list from QB
            if (InResponseList("ShipMethod", _Name))
                return;

            ShipMethodAdd QBShipMethod = new ShipMethodAdd();
            QBShipMethod.IsActive = "true";
            QBShipMethod.Name = MakeQBShipMethodName(_Name);

            ShipMethodAddRq _QBShipMethodAddRq = new ShipMethodAddRq();
            _QBShipMethodAddRq.ShipMethodAdd = QBShipMethod;

            // build add message
            QBXMLMsgsRq _NewMsgRq = new QBXMLMsgsRq();
            _NewMsgRq.ShipMethodAddRq = _QBShipMethodAddRq;
            QBList.QBXMLMsgsRq.Add(_NewMsgRq);

            return;
        }


        #region "Utility Functions"

        // Create a QB-compatible shipping method name
        public static string MakeQBShipMethodName(string _MethodName)
        {
            string _RetVal = "";
            _RetVal = _MethodName.Replace("®", "");
            _RetVal = _RetVal.Replace(":", " ").Substring(0, Math.Min(_MethodName.Length, 15));
            //_RetVal = Strings.Left(_RetVal.Replace(":", " "), 15);
            _RetVal = _RetVal.Trim();

            if (string.IsNullOrEmpty(_RetVal))
                _RetVal = "None";

            return _RetVal;
        }

        public static string MakeQBCustId(Order _Order)
        {
            // BEGIN MOD:  AbleMods.com
            // 3/1/2012
            // if a user custom field specified for QBCompany Name, use it before anything else applies
            if (_Order.User != null)
            {
                string _QBCustomerName = _Order.User.Settings.GetValueByKey("QBCustomerName");
                if (_QBCustomerName != string.Empty)
                    return _QBCustomerName;
            }
            // END MOD: AbleMods.com

            string RetVal = "";

            switch (_configSettings.CustomerIdMode)
            {
                case "COMPANY":
                    if (_Order.BillToCompany.Trim() != string.Empty)
                        RetVal = _Order.BillToCompany.Trim();
                    else
                        RetVal = _Order.BillToLastName + " " + _Order.BillToFirstName;
                    break;
                case "LNFN":
                    RetVal = _Order.BillToLastName + ", " + _Order.BillToFirstName;
                    break;
                case "FNLN":
                    RetVal = _Order.BillToFirstName + " " + _Order.BillToLastName;
                    break;
                case "UserId":
                    RetVal = _Order.UserId.ToString();
                    break;
                case "BilltoEmail":
                    if (string.IsNullOrEmpty(_Order.BillToEmail))
                    {
                        if (_Order.User != null)
                        {
                            RetVal = _Order.User.Email;
                        }
                        else
                        {
                            RetVal = "UserNotFound@AbleCommerce.xyz";
                        }

                    }
                    else
                    {
                        RetVal = _Order.BillToEmail;
                    }
                    break;
                case "BilltoPhone":
                    RetVal = _Order.BillToPhone;
                    break;
                case "SingleName":
                    RetVal = "Web Store";
                    break;
                default:
                    RetVal = _Order.BillToLastName + ", " + _Order.BillToFirstName;
                    break;
            }

            // BEGIN MOD: AbleMods.com
            // DATE:  05/15/2014

            // 3/15/2010
            // modify routine to use a customized logic for determination of customer name
            // Rule 1:  Order is taxed, then use "WEB - ID SUPPLY (Arizona)"
            // Rule 2:  Order is not taxed, then use "WEB - ID SUPPLY (Out of State)"
            // Rule 3:  Order is paid with "Net 30" terms, use the company name of the AC7 user

            // is order taxed?
            if (OrderIsTaxed(_Order))
                RetVal = "WEB - ID SUPPLY (Arizona)";
            else
                RetVal = "WEB - ID SUPPLY (Out of State)";

            // is order a net30 terms order?  if so, use the company name as the QB Customer ID
            if (IsOrderNet30(_Order))
                RetVal = _Order.BillToCompany;

            // if the selected customer ID value results in blank, default to the userid value
            if (string.IsNullOrEmpty(RetVal))
            {
                RetVal = _Order.BillToLastName + ", " + _Order.BillToFirstName;
            }

            // do some cleanup to get rid of anomolies that QB doesn't like
            RetVal = RetVal.Trim();
            RetVal = RetVal.Replace("'", "");
            RetVal = AbleMods.QuickBooks.Utility.StripExtendedASCII(RetVal);

            // strip out colon, not permitted per the QB API
            RetVal = RetVal.Replace(":", "-");

            return RetVal;
        }

        public static bool OrderIsTaxed(Order order)
        {
            // set up return value
            bool retVal = false;

            // loop through order items
            foreach (OrderItem item in order.Items)
            {
                if (item.OrderItemType == OrderItemType.Tax && item.Price > 0)
                {
                    retVal = true;
                    break;
                }
            }

            return retVal;
        }

        public static bool IsOrderNet30(Order order)
        {
            // set up return value
            bool retVal = false;

            // find payment method
            foreach (Payment payment in order.Payments)
            {
                if (payment.PaymentMethodName.Contains("Bill My Account"))
                {
                    retVal = true;
                    break;
                }
            }

            // exit and return value
            return retVal;
        }
        // END MOD: AbleMods.com

        // Return only the handling amt in a freight+handling type line item
        public static decimal GetAC7Handling(OrderItem _LineItem)
        {

            decimal RetVal = 0;
            if (_LineItem.OrderShipment != null)
            {
                if (_LineItem.OrderShipment.ShipMethod != null)
                {
                    decimal _Handling = _LineItem.OrderShipment.ShipMethod.Surcharge;
                    bool _IsPercent = (bool)_LineItem.OrderShipment.ShipMethod.SurchargeIsPercent;
                    if (_IsPercent)
                    {
                        RetVal = (decimal)(_LineItem.Price * (_Handling / 100));
                    }
                    else
                    {
                        RetVal = (decimal)_Handling;
                    }
                }
            }

            return RetVal;
        }

        // Return only the freight amt in a freight+handling type line item
        public static decimal GetAC7Freight(OrderItem _LineItem)
        {

            decimal RetVal = 0;
            if (_LineItem.OrderShipment != null)
            {
                if (_LineItem.OrderShipment.ShipMethod != null)
                {
                    decimal _Handling = _LineItem.OrderShipment.ShipMethod.Surcharge;
                    bool _IsPercent = (bool)_LineItem.OrderShipment.ShipMethod.SurchargeIsPercent;
                    if (_IsPercent)
                    {
                        RetVal = (decimal)(_LineItem.Price - (_LineItem.Price * (_Handling / 100)));
                    }
                    else
                    {
                        RetVal = (decimal)(_LineItem.Price - _Handling);
                    }
                }
            }

            return RetVal;
        }
        // Create a QB-compatible date field
        public static string MakeQBDate(System.DateTime _Date)
        {
            return _Date.Year.ToString() + "-" + _Date.Month.ToString().PadLeft(2, '0') + "-" + _Date.Day.ToString().PadLeft(2, '0');
        }


        // Creates a QB-compatible dollar amount
        public static string MakeQBAmt(decimal _Amt)
        {
            if (_DemoMode) return "0.01";
            return string.Format("{0:f2}", Math.Round(AlwaysConvert.ToDecimal(_Amt), 2));
        }

        private static bool InResponseList(string _ListName, string _Value)
        {// set up return value
            switch (_ListName)
            {
                case "Customer":
                    {
                        { foreach (CustomerRet _Ret in QBList.QBXMLMsgsRs.CustomerQueryRs.CustomerRet)if (_Ret.Name == _Value)return true; }
                        break;
                    }

                case "Account":
                    {
                        { foreach (AccountRet _Ret in QBList.QBXMLMsgsRs.AccountQueryRs.AccountRet)if (_Ret.Name == _Value)return true; }
                        break;
                    }
                case "SalesTaxCode":
                    {
                        { foreach (SalesTaxCodeRet _Ret in QBList.QBXMLMsgsRs.SalesTaxCodeQueryRs.SalesTaxCodeRet)if (_Ret.Name == _Value)return true; }
                        break;
                    }
                case "StandardTerms":
                    {
                        { foreach (StandardTermsRet _Ret in QBList.QBXMLMsgsRs.StandardTermsQueryRs.StandardTermsRet)if (_Ret.Name == _Value)return true; }
                        break;
                    }
                case "Item":
                    {
                        // check iteminventory
                        { foreach (ItemInventoryRet _Ret in QBList.QBXMLMsgsRs.ItemInventoryQueryRs.ItemInventoryRet)if (_Ret.Name == _Value)return true; }
                        // check itemnoninventory
                        { foreach (ItemNonInventoryRet _Ret in QBList.QBXMLMsgsRs.ItemNonInventoryQueryRs.ItemNonInventoryRet)if (_Ret.Name == _Value)return true; }
                        // check itemgroup
                        { foreach (ItemGroupRet _Ret in QBList.QBXMLMsgsRs.ItemGroupQueryRs.ItemGroupRet)if (_Ret.Name == _Value)return true; }
                        // check itemnoninventory
                        { foreach (ItemServiceRet _Ret in QBList.QBXMLMsgsRs.ItemServiceQueryRs.ItemServiceRet)if (_Ret.Name == _Value)return true; }
                        break;
                    }
                case "ItemDiscount":
                    {
                        { foreach (ItemDiscountRet _Ret in QBList.QBXMLMsgsRs.ItemDiscountQueryRs.ItemDiscountRet)if (_Ret.Name == _Value)return true; }
                        break;
                    }
                case "ItemSalesTax":
                    {
                        { foreach (ItemSalesTaxRet _Ret in QBList.QBXMLMsgsRs.ItemSalesTaxQueryRs.ItemSalesTaxRet)if (_Ret.Name == _Value)return true; }
                        break;
                    }
                case "PaymentMethod":
                    {
                        { foreach (PaymentMethodRet _Ret in QBList.QBXMLMsgsRs.PaymentMethodQueryRs.PaymentMethodRet)if (_Ret.Name == _Value)return true; }
                        break;
                    }
                case "Vendor":
                    {
                        { foreach (VendorRet _Ret in QBList.QBXMLMsgsRs.VendorQueryRs.VendorRet)if (_Ret.Name == _Value)return true; }
                        break;
                    }
                case "ShipMethod":
                    {
                        { foreach (ShipMethodRet _Ret in QBList.QBXMLMsgsRs.ShipMethodQueryRs.ShipMethodRet)if (_Ret.Name == _Value)return true; }
                        break;
                    }
            }
            return false;
        }
        #endregion
    }
}