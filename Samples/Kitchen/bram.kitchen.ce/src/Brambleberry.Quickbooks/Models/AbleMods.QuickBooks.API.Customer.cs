using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Brambleberry.Quickbooks.Models.Base;


namespace Brambleberry.Quickbooks.Models
{
    public class CustomerAddRq : BaseAddRq
    {
        [XmlElement]
        public CustomerAdd CustomerAdd = new CustomerAdd();
    }

    [XmlRoot(ElementName = "CustomerAddRs")]
    public partial class CustomerAddRs : BaseAddRs
    {
        [XmlElement(ElementName = "CustomerRet")]
        public CustomerRet CustomerRet;
    }

    [XmlRoot(ElementName = "CustomerQueryRq")]
    public partial class CustomerQueryRq : BaseQryRq
    {
        [XmlElement(ElementName = "NameRangeFilter")]
        public NameRangeFilter NameRangeFilter = new NameRangeFilter();
        public bool ShouldSerializeNameRangeFilter()
        {
            return (NameRangeFilter.FromName != "" | NameRangeFilter.ToName != "");
        }

    }

    [XmlRoot(ElementName = "CustomerQueryRs")]
    public partial class CustomerQueryRs : BaseQryRs
    {
        [XmlElement(ElementName = "CustomerRet")]
        public List<CustomerRet> CustomerRet = new List<CustomerRet>();
    }

    public class CustomerModRq
    {
        [XmlElement]
        public CustomerMod CustomerMod = new CustomerMod();
    }

    [XmlRoot(ElementName = "CustomerModRs")]
    public partial class CustomerModRs : BaseModRs
    {
        [XmlElement(ElementName = "CustomerRet")]
        public CustomerRet CustomerRet;
    }

    [Serializable()]
    public class CustomerAdd
    {

        private TermsRef _TermsRef = new TermsRef();
        private SalesTaxCodeRef _SalesTaxCodeRef = new SalesTaxCodeRef();
        private ItemSalesTaxRef _ItemSalesTaxRef = new ItemSalesTaxRef();
        private CustomerTypeRef _CustomerTypeRef = new CustomerTypeRef();
        private SalesRepRef _SalesRepRef = new SalesRepRef();
        private CreditCardInfo _CreditCardInfo = new CreditCardInfo();
        private PreferredPaymentMethodRef _PreferredPaymentMethodRef = new PreferredPaymentMethodRef();
        private string _name;
        private string _companyname;
        private string _firstname;
        private string _middlename;
        private string _lastname;
        private string _phone;
        private string _contact;

        [XmlElementAttribute(Order = 1)]
        public string Name
        {
            get { return _name; }
            set { _name = Utility.StripExtendedASCII(value.Substring(0,Math.Min(value.Length,41))); }
        }

        [XmlElementAttribute(Order = 2)]
        public string IsActive;

        [XmlElementAttribute(Order = 3)]
        public string CompanyName
        {
            get { return _companyname; }
            set { _companyname = Utility.StripExtendedASCII(value.Substring(0, Math.Min(value.Length, 41))); }
        }

        [XmlElementAttribute(Order = 4)]
        public string Salutation;

        [XmlElementAttribute(Order = 5)]
        public string FirstName
        {
            get { return _firstname; }
            set { _firstname = Utility.StripExtendedASCII(value.Substring(0, Math.Min(value.Length, 25))); }
        }

        [XmlElementAttribute(Order = 6)]
        public string MiddleName
        {
            get { return _middlename; }
            set { _middlename = Utility.StripExtendedASCII(value.Substring(0, Math.Min(value.Length, 5))); }
        }

        [XmlElementAttribute(Order = 7)]
        public string LastName
        {
            get { return _lastname; }
            set { _lastname = Utility.StripExtendedASCII(value.Substring(0, Math.Min(value.Length, 25))); }
        }

        [XmlElementAttribute(Order = 8)]
        public BillAddressBlock BillAddress = new BillAddressBlock();

        [XmlElementAttribute(Order = 9)]
        public ShipAddressBlock ShipAddress = new ShipAddressBlock();

        [XmlElementAttribute(Order = 10)]
        public string Phone
        {
            get { return _phone; }
            set { _phone = Utility.StripExtendedASCII(value.Substring(0, Math.Min(value.Length, 20))); }
        }

        [XmlElementAttribute(Order = 11)]
        public string Fax;

        [XmlElementAttribute(Order = 12)]
        public string Email;

        [XmlElementAttribute(Order = 13)]
        public string Contact
        {
            get { return _contact; }
            set { _contact = Utility.StripExtendedASCII(value.Substring(0, Math.Min(value.Length, 40))); }
        }

        [XmlElementAttribute(Order = 14)]
        public CustomerTypeRef CustomerTypeRef
        {
            get { return _CustomerTypeRef; }
            set { _CustomerTypeRef = value; }
        }

        [XmlElementAttribute(Order = 15)]
        public TermsRef TermsRef
        {
            get { return _TermsRef; }
            set { _TermsRef = value; }
        }

        [XmlElementAttribute(Order = 16)]
        public SalesRepRef SalesRepRef
        {
            get { return _SalesRepRef; }
            set { _SalesRepRef = value; }
        }

        [XmlElementAttribute(Order = 17)]
        public SalesTaxCodeRef SalesTaxCodeRef
        {
            get { return _SalesTaxCodeRef; }
            set { _SalesTaxCodeRef = value; }
        }

        [XmlElementAttribute(Order = 18)]
        public ItemSalesTaxRef ItemSalesTaxRef
        {
            get { return _ItemSalesTaxRef; }
            set { _ItemSalesTaxRef = value; }
        }

        [XmlElementAttribute(Order = 19)]
        public string AccountNumber;

        [XmlElementAttribute(Order = 20)]
        public PreferredPaymentMethodRef PreferredPaymentMethodRef
        {
            get { return _PreferredPaymentMethodRef; }
            set { _PreferredPaymentMethodRef = value; }
        }


        [XmlElementAttribute(Order = 21)]
        public CreditCardInfo CreditCardInfo
        {
            get { return _CreditCardInfo; }
            set { _CreditCardInfo = value; }
        }

        public bool ShouldSerializeTermsRef()
        {
            return !(_TermsRef.FullName == null & _TermsRef.ListID == null);
        }

        public bool ShouldSerializeSalesTaxCodeRef()
        {
            return !(_SalesTaxCodeRef.FullName == null & _SalesTaxCodeRef.ListID == null);
        }

        public bool ShouldSerializeItemSalesTaxRef()
        {
            return !(_ItemSalesTaxRef.FullName == null & _ItemSalesTaxRef.ListID == null);
        }

        public bool ShouldSerializeCustomerTypeRef()
        {
            return !(_CustomerTypeRef.FullName == null & _CustomerTypeRef.ListID == null);
        }

        public bool ShouldSerializeSalesRepRef()
        {
            return !(_SalesRepRef.FullName == null & _SalesRepRef.ListID == null);
        }

        public bool ShouldSerializeCreditCardInfo()
        {
            return !(_CreditCardInfo.CreditCardNumber == null);
        }

        public bool ShouldSerializePreferredPaymentMethodRef()
        {
            return !(_PreferredPaymentMethodRef.FullName == null & _PreferredPaymentMethodRef.ListID == null);
        }
    }


    [XmlRoot(ElementName = "CustomerRet")]
    public partial class CustomerRet : BaseRet
    {
        public string Name;
        public string FullName;
        public bool IsActive;
        public ParentRef ParentRef;
        public int Sublevel;
        public string CompanyName;
        public string Salutation;
        public string FirstName;
        public string MiddleName;
        public string LastName;
        public BillAddressBlock BillAddress;
        public ShipAddressBlock ShipAddress;
        public string Phone;
        public string Email;
        public string Contact;
        public CustomerTypeRef CustomerTypeRef;
        public TermsRef TermsRef;
        public SalesRepRef SalesRepRef;
    }

    [Serializable()]
    public class CustomerMod
    {
        [XmlElementAttribute(Order = 1)]
        public string ListID;

        [XmlElementAttribute(Order = 2)]
        public string EditSequence;

        [XmlElementAttribute(Order = 3)]
        public PreferredPaymentMethodRef PreferredPaymentMethodRef = new PreferredPaymentMethodRef();

        [XmlElementAttribute(Order = 4)]
        public CreditCardInfo CreditCardInfo = new CreditCardInfo();

    }


}
