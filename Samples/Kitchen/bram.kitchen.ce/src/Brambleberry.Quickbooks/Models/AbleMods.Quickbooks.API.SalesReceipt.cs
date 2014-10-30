using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Brambleberry.Quickbooks.Models.Base;


namespace Brambleberry.Quickbooks.Models
{
    public class SalesReceiptAddRq : BaseAddRq
    {
        [XmlElement]
        public SalesReceiptAdd SalesReceiptAdd = new SalesReceiptAdd();
    }

    [XmlRoot(ElementName = "SalesReceiptQueryRq")]
    public partial class SalesReceiptQueryRq : BaseQryRq
    {
        [XmlElement(ElementName = "NameRangeFilter")]
        public NameRangeFilter NameRangeFilter;
    }

    [XmlRoot(ElementName = "SalesReceiptAddRs")]
    public partial class SalesReceiptAddRs : BaseAddRs 
    {
        public SalesReceiptRet SalesReceiptRet;
    }


    public class SalesReceiptAdd
    {
        private ItemSalesTaxRef _ItemSalesTaxRef = new ItemSalesTaxRef();

        private CustomerSalesTaxCodeRef _CustomerSalesTaxCodeRef = new CustomerSalesTaxCodeRef();
        [XmlElementAttribute(Order = 1)]
        public CustomerRef CustomerRef = new CustomerRef();
        [XmlElementAttribute(Order = 2)]
        public ClassRef ClassRef = new ClassRef();
        [XmlElementAttribute(Order = 3)]
        public string TxnDate;
        [XmlElementAttribute(Order = 4)]
        public string RefNumber;
        [XmlElementAttribute(Order = 5)]
        public BillAddressBlock BillAddress = new BillAddressBlock();
        [XmlElementAttribute(Order = 6)]
        public ShipAddressBlock ShipAddress = new ShipAddressBlock();
        [XmlElementAttribute(Order = 7)]
        public string CheckNumber;
        [XmlElementAttribute(Order = 8)]
        public PaymentMethodRef PaymentMethodRef = new PaymentMethodRef();
        [XmlElementAttribute(Order = 9)]
        public string DueDate;
        [XmlElementAttribute(Order = 10)]
        public ShipMethodRef ShipMethodRef = new ShipMethodRef();
        public bool ShouldSerializeShipMethodRef()
        {
            return !(ShipMethodRef.FullName == null & ShipMethodRef.ListID == null);
        }

        [XmlElementAttribute(Order = 11)]
        public ItemSalesTaxRef ItemSalesTaxRef
        {
            get { return _ItemSalesTaxRef; }
            set { _ItemSalesTaxRef = value; }
        }

        public bool ShouldSerializeItemSalesTaxRef()
        {
            return !(_ItemSalesTaxRef.FullName == null & _ItemSalesTaxRef.ListID == null);
        }

        [XmlElementAttribute(Order = 12)]

        public string Memo;
        [XmlElementAttribute(Order = 13)]
        public CustomerSalesTaxCodeRef CustomerSalesTaxCodeRef
        {
            get { return _CustomerSalesTaxCodeRef; }
            set { _CustomerSalesTaxCodeRef = value; }
        }

        public bool ShouldSerializeCustomerSalesTaxCodeRef()
        {
            return !(_CustomerSalesTaxCodeRef.FullName == null & _CustomerSalesTaxCodeRef.ListID == null);
        }

        [XmlElementAttribute(Order = 14)]
        public DepositToAccountRef DepositToAccountRef = new DepositToAccountRef();

        [XmlElementAttribute(Order = 15)]
        public List<SalesReceiptLineAdd> SalesReceiptLineAdd = new List<SalesReceiptLineAdd>();

        public bool ShouldSerializeClassRef()
        {
            return !(ClassRef.FullName == null & ClassRef.ListID == null);
        }

    }

    public class SalesReceiptLineAdd : InvoiceLineAdd
    {
    }

    [XmlRoot(ElementName = "SalesReceiptRet")]
    public partial class SalesReceiptRet : BaseRet
    {
        public int TxnNumber;
        public CustomerRef CustomerRef;
        public ClassRef ClassRef;
        public DateTime TxnDate;
        public string RefNumber;
        public BillAddressBlock BillAddressBlock;
        public ShipAddressBlock ShipAddressBlock;
        public bool IsPending;
        public string CheckNumber;
        public PaymentMethodRef PaymentMethodRef;
        public DateTime DueDate;
        public SalesRepRef SalesRepRef;
        public DateTime ShipDate;
        public ShipMethodRef ShipMethodRef;
        public string FOB;
        public double Subtotal;
        public ItemSalesTaxRef ItemSalesTaxRef;
        public double SalesTaxPercentage;
        public double SalesTaxTotal;
        public double TotalAmount;
        public string Memo;
        public CustomerSalesTaxCodeRef CustomerSalesTaxCodeRef;
        public DepositToAccountRef DepositToAccountRef;
        public List<SalesReceiptLineRet> SalesReceiptLineRet;
    }

    public class SalesReceiptLineRet
    {
        public string TxnLineId;
        public ItemRef ItemREf;
        public string Desc;
        public double Quantity;
        public string UnitOfMeasure;
        public ClassRef ClassRef;
        public double Amount;
        public DateTime ServiceDate;
        public SalesTaxCodeRef SalesTaxCodeRef;
        public string Other1;
        public string Other2;
    }


}
