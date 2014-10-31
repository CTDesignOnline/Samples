using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Brambleberry.Quickbooks.Models.Base;


namespace Brambleberry.Quickbooks.Models
{
    public class SalesOrderAddRq : BaseAddRq
    {
        [XmlElement]
        public SalesOrderAdd SalesOrderAdd = new SalesOrderAdd();
    }

    [XmlRoot(ElementName = "SalesOrderQueryRq")]
    public partial class SalesOrderQueryRq : BaseQryRq
    {
        [XmlElement(ElementName = "NameRangeFilter")]
        public NameRangeFilter NameRangeFilter;
    }

    public class SalesOrderAdd
    {

        private ItemSalesTaxRef _ItemSalesTaxRef = new ItemSalesTaxRef();

        private CustomerSalesTaxCodeRef _CustomerSalesTaxCodeRef = new CustomerSalesTaxCodeRef();
        [XmlElementAttribute(Order = 1)]
        public CustomerRef CustomerRef = new CustomerRef();
        [XmlElementAttribute(Order = 2)]
        public ClassRef ClassRef = new ClassRef();
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
        //[XmlArrayItem("SalesOrderLineAdd", typeof(SalesOrderLineAdd))]
        [XmlElementAttribute(Order = 18)]
        public List<SalesOrderLineAdd> SalesOrderLineAdd = new List<SalesOrderLineAdd>();

        public bool ShouldSerializeClassRef()
        {
            return !(ClassRef.FullName == null & ClassRef.ListID == null);
        }
    }

    public class SalesOrderRet : BaseRet
    {
        public int TxnNumber;
        public CustomerRef CustomerRef;
        public ClassRef ClassRef;
        public DateTime TxnDate;
        public string RefNumber;
        public BillAddressBlock BillAddress;
        public ShipAddressBlock ShipAddress;
        public string PONumber;
        public TermsRef TermsRef;
        public DateTime DueDate;
        public SalesRepRef SalesRepRef;
        public string FOB;
        public DateTime ShipDate;
        public ShipMethodRef ShipMethodRef;
        public ItemSalesTaxRef ItemSalesTaxRef;
        public decimal SalesTaxPercentage;
        public decimal SalesTaxTotal;
        public decimal TotalAmount;
        public bool IsManuallyClosed;
        public bool IsFullyInvoiced;
        public CustomerSalesTaxCodeRef CustomerSalesTaxCodeRef;


    }


    public class SalesOrderLineAdd : InvoiceLineAdd
    {
    }



    [XmlRoot(ElementName = "SalesOrderAddRs")]
    public partial class SalesOrderAddRs : BaseAddRs 
    {
        [XmlElement(ElementName = "SalesOrderRet")]
        public SalesOrderRet SalesOrderRet;
    }


}
