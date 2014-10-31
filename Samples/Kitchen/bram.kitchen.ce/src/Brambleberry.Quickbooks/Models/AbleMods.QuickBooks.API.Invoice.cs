using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Brambleberry.Quickbooks.Models.Base;


namespace Brambleberry.Quickbooks.Models
{
    public class InvoiceAddRq : BaseAddRq
    {
        [XmlElement]
        public InvoiceAdd InvoiceAdd = new InvoiceAdd();
    }

    [XmlRoot(ElementName = "InvoiceAddRs")]
    public partial class InvoiceAddRs : BaseAddRs 
    {
        [XmlElement(ElementName = "InvoiceRet")]
        public InvoiceRet InvoiceRet;
    }


    [XmlRoot(ElementName = "InvoiceQueryRq")]
    public partial class InvoiceQueryRq : BaseQryRq { }


    public class InvoiceAdd
    {

        private ItemSalesTaxRef _ItemSalesTaxRef = new ItemSalesTaxRef();

        private CustomerSalesTaxCodeRef _CustomerSalesTaxCodeRef = new CustomerSalesTaxCodeRef();
        [XmlElementAttribute(Order = 1)]
        public CustomerRef CustomerRef = new CustomerRef();

        [XmlElementAttribute(Order = 2)]
        public ClassRef ClassRef = new ClassRef();

        [XmlElementAttribute(Order = 3)]
        public ARAccountRef ARAccountRef = new ARAccountRef();

        [XmlElementAttribute(Order = 4)]
        public string TxnDate;

        [XmlElementAttribute(Order = 5)]
        public string RefNumber;

        [XmlElementAttribute(Order = 6)]
        public BillAddressBlock BillAddress = new BillAddressBlock();

        [XmlElementAttribute(Order = 7)]
        public ShipAddressBlock ShipAddress = new ShipAddressBlock();

        [XmlElementAttribute(Order = 8)]
        public TermsRef TermsRef = new TermsRef();

        [XmlElementAttribute(Order = 9)]
        public string DueDate;

        [XmlElementAttribute(Order = 10)]
        public string FOB;

        [XmlElementAttribute(Order = 11)]
        public string ShipDate;

        [XmlElementAttribute(Order = 12)]
        public ShipMethodRef ShipMethodRef = new ShipMethodRef();
        public bool ShouldSerializeShipMethodRef()
        {
            return !(ShipMethodRef.FullName == null & ShipMethodRef.ListID == null);
        }

        [XmlElementAttribute(Order = 13)]
        public ItemSalesTaxRef ItemSalesTaxRef
        {
            get { return _ItemSalesTaxRef; }
            set { _ItemSalesTaxRef = value; }
        }
        public bool ShouldSerializeItemSalesTaxRef()
        {
            return !(_ItemSalesTaxRef.FullName == null & _ItemSalesTaxRef.ListID == null);
        }
        [XmlElementAttribute(Order = 14)]
        public string Memo;

        [XmlElementAttribute(Order = 15)]
        public string IsToBePrinted;

        [XmlElementAttribute(Order = 16)]
        public string IsToBeEmailed;

        [XmlElementAttribute(Order = 17)]
        public CustomerSalesTaxCodeRef CustomerSalesTaxCodeRef
        {
            get { return _CustomerSalesTaxCodeRef; }
            set { _CustomerSalesTaxCodeRef = value; }
        }
        public bool ShouldSerializeCustomerSalesTaxCodeRef()
        {
            return !(_CustomerSalesTaxCodeRef.FullName == null & _CustomerSalesTaxCodeRef.ListID == null);
        }

        //[XmlArray(Order = 18)]
        //[XmlArrayItem("InvoiceLineAdd", typeof(InvoiceLineAdd))]
        [XmlElementAttribute(Order = 18)]
        public List<InvoiceLineAdd> InvoiceLineAdd = new List<InvoiceLineAdd>();

        public bool ShouldSerializeClassRef()
        {
            return !(ClassRef.FullName == null & ClassRef.ListID == null);
        }
    }

    public class InvoiceLineAdd
    {

        private double _Rate;
        private SalesTaxCodeRef _SalesTaxCodeRef = new SalesTaxCodeRef();

        private string _Desc;
        [XmlElementAttribute(Order = 1)]

        public ItemRef ItemRef = new ItemRef();
        [XmlElementAttribute(Order = 2)]
        public string Desc
        {
            get { return _Desc; }
            set { _Desc = Utility.StripExtendedASCII(value); }
        }

        [XmlElementAttribute(Order = 3)]
        public int Quantity;

        [XmlElementAttribute(Order = 4)]
        public double Rate
        {
            get { return _Rate; }
            set { _Rate = value; }
        }

        [XmlElementAttribute(Order = 5)]
        public ClassRef ClassRef = new ClassRef();

        [XmlElementAttribute(Order = 6)]
        public string Amount;

        [XmlElementAttribute(Order = 7)]
        public string ServiceDate;

        [XmlElementAttribute(Order = 8)]
        public SalesTaxCodeRef SalesTaxCodeRef
        {
            get { return _SalesTaxCodeRef; }
            set { _SalesTaxCodeRef = value; }
        }

        [XmlElementAttribute(Order = 9)]
        public string Other1;

        [XmlElementAttribute(Order = 10)]
        public string Other2;

        public bool ShouldSerializeRate()
        {
            return _Rate > 0;
        }
        public bool ShouldSerializeSalesTaxCodeRef()
        {
            return !(_SalesTaxCodeRef.FullName == null & _SalesTaxCodeRef.ListID == null);
        }
        public bool ShouldSerializeClassRef()
        {
            return !(ClassRef.FullName == null & ClassRef.ListID == null);
        }
        public bool ShouldSerializeQuantity()
        {
            return (Quantity != 0);
        }

    }

    public class InvoiceLineGroupAdd
    {
        [XmlElementAttribute(Order = 1)]
        public ItemGroupRef ItemGroupRef = new ItemGroupRef();

        [XmlElementAttribute(Order = 2)]
        public int Quantity;
    }

    [XmlRoot(ElementName = "InvoiceRet")]
    public partial class InvoiceRet : BaseRet
    {
        public CustomerRef CustomerRef;
        public ClassRef ClassRef;
        public ARAccountRef ARAccountRef;
        public DateTime TxnDate;
        public string RefNumber;
        public BillAddressBlock BillAddressBlock;
        public ShipAddressBlock ShipAddressBlock;
        public bool IsPending;
        public bool IsFinanceCharge;
        public string PONumber;
        public TermsRef TermsRef;
        public SalesRepRef SalesRepRef;
        public string FOB;
        public DateTime ShipDate;
        public ShipMethodRef ShipMethodRef;
        public ItemSalesTaxRef ItemSalesTaxRef;
        public double SalesTaxPercentage;
        public double SalesTaxTotal;
        public double AppliedAmount;
        public double BalanceRemaining;
        public bool IsPaid;
        public CustomerSalesTaxCodeRef CustomerSalesTaxCodeRef;
        public List<InvoiceLineRet> InvoiceLineRet;
    }

    public class InvoiceLineRet
    {
        public string TxnLineId;
        public ItemRef ItemRef;
        public string Desc;
        public double Quantity;
        public string UnitOfMeasure;
        public ClassRef ClassRef;
        public double Amount;
        public SalesTaxCodeRef SalesTaxCodeRef;
        public string Other1;
        public string Other2;
    }

}
