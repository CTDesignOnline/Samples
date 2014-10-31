using System;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace Brambleberry.Quickbooks.Models.Base
{

    public class ListRef
    {

        private string _fullname;
        
        [XmlElement(Order = 1)]
        public string ListID;

        [XmlElement(Order = 2)]
        public string FullName
        {
            get { return _fullname; }
            set { _fullname = Utility.StripExtendedASCII(value); }
        }

    }

    public class SalesRepEntityRef : ListRef
    {
    }

    public class BillAddressBlock
    {
        private string _addr1;
        private string _addr2;
        private string _addr3;
        private string _addr4;
        private string _city;
        private string _state;
        private string _postalcode;

        private string _country;
        [XmlElement(Order = 1)]
        public string Addr1
        {
            get { return _addr1; }
            set { _addr1 = Utility.StripExtendedASCII(value.Substring(0, Math.Min(value.Length, 41))); }
        }

        [XmlElement(Order = 2)]
        public string Addr2
        {
            get { return _addr2; }
            set { _addr2 = Utility.StripExtendedASCII(value.Substring(0,Math.Min(value.Length,41))); }
        }

        [XmlElement(Order = 3)]
        public string Addr3
        {
            get { return _addr3; }
            set { _addr3 = Utility.StripExtendedASCII(value.Substring(0,Math.Min(value.Length,41))); }
        }

        [XmlElement(Order = 4)]
        public string Addr4
        {
            get { return _addr4; }
            set { _addr4 = Utility.StripExtendedASCII(value.Substring(0,Math.Min(value.Length,41))); }
        }

        [XmlElement(Order = 5)]
        public string City
        {
            get { return _city; }
            set { _city = Utility.StripExtendedASCII(value.Substring(0, Math.Min(value.Length, 31))); }
        }

        [XmlElement(Order = 6)]
        public string State
        {
            get { return _state; }
            set { _state = Utility.StripExtendedASCII(value.Substring(0, Math.Min(value.Length, 21))); }
        }

        [XmlElement(Order = 7)]
        public string PostalCode
        {
            get { return _postalcode; }
            set { _postalcode = Utility.StripExtendedASCII(value.Substring(0, Math.Min(value.Length, 13))); }
        }

        [XmlElement(Order = 8)]
        public string Country
        {
            get { return _country; }
            set { _country = Utility.StripExtendedASCII(value.Substring(0, Math.Min(value.Length, 31))); }
        }

    }

    public class ShipAddressBlock
    {
        private string _addr1;
        private string _addr2;
        private string _addr3;
        private string _addr4;
        private string _city;
        private string _state;
        private string _postalcode;

        private string _country;
        [XmlElement(Order = 1)]
        public string Addr1
        {
            get { return _addr1; }
            set { _addr1 = Utility.StripExtendedASCII(value.Substring(0,Math.Min(value.Length,41))); }
        }

        [XmlElement(Order = 2)]
        public string Addr2
        {
            get { return _addr2; }
            set { _addr2 = Utility.StripExtendedASCII(value.Substring(0,Math.Min(value.Length,41))); }
        }

        [XmlElement(Order = 3)]
        public string Addr3
        {
            get { return _addr3; }
            set { _addr3 = Utility.StripExtendedASCII(value.Substring(0,Math.Min(value.Length,41))); }
        }

        [XmlElement(Order = 4)]
        public string Addr4
        {
            get { return _addr4; }
            set { _addr4 = Utility.StripExtendedASCII(value.Substring(0,Math.Min(value.Length,41))); }
        }

        [XmlElement(Order = 5)]
        public string City
        {
            get { return _city; }
            set { _city = Utility.StripExtendedASCII(value.Substring(0, Math.Min(value.Length, 31))); }
        }

        [XmlElement(Order = 6)]
        public string State
        {
            get { return _state; }
            set { _state = Utility.StripExtendedASCII(value.Substring(0, Math.Min(value.Length, 21))); }
        }

        [XmlElement(Order = 7)]
        public string PostalCode
        {
            get { return _postalcode; }
            set { _postalcode = Utility.StripExtendedASCII(value.Substring(0, Math.Min(value.Length, 13))); }
        }

        [XmlElement(Order = 8)]
        public string Country
        {
            get { return _country; }
            set { _country = Utility.StripExtendedASCII(value.Substring(0, Math.Min(value.Length, 31))); }
        }

    }

    public class VendorAddressBlock : BillAddressBlock
    {
    }

    public class SalesTaxCodeRef : ListRef
    {
    }

    public class ItemSalesTaxRef : ListRef
    {
    }

    public class VendorRef : ListRef
    {
    }

    public class APAccountRef : ListRef
    {
    }
    public class TermsRef : ListRef
    {
    }

    public class ItemRef : ListRef
    {
    }

    public class ItemGroupRef : ListRef
    {
    }

    public class CustomerRef : ListRef
    {
    }

    public class ShipToEntityRef : ListRef
    {
    }
    public class ShipMethodRef : ListRef
    {
    }
    public class VendorTypeRef : ListRef
    {
    }
    public class ARAccountRef : ListRef
    {
    }
    public class PaymentMethodRef : ListRef
    {
    }
    public class DepositToAccountRef : ListRef
    {
    }
    public class CustomerSalesTaxCodeRef : ListRef
    {
    }
    public class IncomeAccountRef : ListRef
    {
    }
    public class ExpenseAccountRef : ListRef
    {
    }
    public class COGSAccountRef : ListRef
    {
    }
    public class AssetAccountRef : ListRef
    {
    }
    public class TaxVendorRef : ListRef
    {
    }
    public class AccountRef : ListRef
    {
    }
    public class ClassRef : ListRef
    {
    }
    public class ParentRef : ListRef
    {
    }
    public class CustomerTypeRef : ListRef
    {
    }
    public class SalesRepRef : ListRef
    {
    }
    public class PreferredPaymentMethodRef : ListRef
    {
    }

    public class CreditCardInfo
    {
        public string CreditCardNumber;
        public int ExpirationMonth;
        public int ExpirationYear;
        public string NameOnCard;
        public string CreditCardAddress;
        public string CreditCardPostalCode;
    }

    public class NameRangeFilter
    {
        [XmlElement(Order = 1)]
        public string FromName;

        [XmlElement(Order = 2)]
        public string ToName;
    }


    public class SalesAndPurchase
    {
        private string _salesdesc;
        private string _purchasedesc;

        [XmlElement(Order = 1)]
        public string SalesDesc
        {
            get { return _salesdesc; }
            set { _salesdesc = value.Substring(0, Math.Min(value.Length, 4095)); }
        }

        [XmlElement(Order = 2)]
        public double SalesPrice;

        [XmlElement(Order = 3)]
        public IncomeAccountRef IncomeAccountRef = new IncomeAccountRef();

        [XmlElement(Order = 4)]
        public string PurchaseDesc
        {
            get { return _purchasedesc; }
            set { _purchasedesc = value.Substring(0, Math.Min(value.Length, 4095)); }
        }

        [XmlElement(Order = 5)]
        public double PurchaseCost;

        [XmlElement(Order = 6)]
        public ExpenseAccountRef ExpenseAccountRef = new ExpenseAccountRef();
    }

    public class ItemLineAdd
    {

        private CustomerRef _CustomerRef = new CustomerRef();

        private string _Desc;
        [XmlElement(Order = 1)]
        public ItemRef ItemRef = new ItemRef();

        [XmlElement(Order = 2)]
        public string Desc
        {
            get { return _Desc; }
            set { _Desc = Utility.StripExtendedASCII(value); }
        }

        [XmlElement(Order = 3)]
        public string Quantity;
        [XmlElement(Order = 4)]
        public string Cost;
        [XmlElement(Order = 5)]

        public string Amount;
        [XmlElement(Order = 6)]
        public CustomerRef CustomerRef
        {
            get { return _CustomerRef; }
            set { _CustomerRef = value; }
        }

        [XmlElement(Order = 7)]

        public ClassRef ClassRef = new ClassRef();
        public bool ShouldSerializeCustomerRef()
        {
            return !(_CustomerRef.FullName == null & _CustomerRef.ListID == null);
        }

        public bool ShouldSerializeClassRef()
        {
            return !(ClassRef.FullName == null & ClassRef.ListID == null);
        }
    }

    public class Utility
    {
        // Strip all extended ASCII characters
        // (that is, all characters whose ASCII code is > 127)
        //
        public static string StripExtendedASCII(string source)
        {
            if (source.Length > 0)
            {
                return Regex.Replace(source, "[^\\x20-\\x7E]", "");
            }

            return source;

        }

    }
}
