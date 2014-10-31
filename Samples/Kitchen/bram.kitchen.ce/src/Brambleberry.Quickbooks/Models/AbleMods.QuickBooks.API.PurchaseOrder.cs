using System.Collections.Generic;
using System.Xml.Serialization;
using Brambleberry.Quickbooks.Models.Base;


namespace Brambleberry.Quickbooks.Models
{
    public class PurchaseOrderAddRq : BaseAddRq
    {
        [XmlElement]
        public PurchaseOrderAdd PurchaseOrderAdd = new PurchaseOrderAdd();
    }

    [XmlRoot(ElementName = "PurchaseOrderQueryRq")]
    public partial class PurchaseOrderQueryRq : BaseQryRq
    {
        [XmlElement(ElementName = "NameRangeFilter")]
        public NameRangeFilter NameRangeFilter;
    }

    public class PurchaseOrderAdd
    {

        private TermsRef _termsref = new TermsRef();

        [XmlElementAttribute(Order = 1)]
        public VendorRef VendorRef = new VendorRef();

        [XmlElementAttribute(Order = 2)]
        public ClassRef ClassRef = new ClassRef();

        [XmlElementAttribute(Order = 3)]
        public ShipToEntityRef ShipToEntityRef = new ShipToEntityRef();

        [XmlElementAttribute(Order = 4)]
        public string TxnDate;

        [XmlElementAttribute(Order = 5)]
        public string RefNumber;

        [XmlElementAttribute(Order = 6)]
        public VendorAddressBlock VendorAddress = new VendorAddressBlock();

        [XmlElementAttribute(Order = 7)]
        public ShipAddressBlock ShipAddress = new ShipAddressBlock();

        [XmlElementAttribute(Order = 8)]
        public TermsRef TermsRef
        {
            get { return _termsref; }
            set { _termsref = value; }
        }

        [XmlElementAttribute(Order = 9)]
        public string DueDate;

        [XmlElementAttribute(Order = 10)]
        public string ExpectedDate;

        [XmlElementAttribute(Order = 11)]
        public ShipMethodRef ShipMethodRef = new ShipMethodRef();

        [XmlElementAttribute(Order = 12)]
        public string FOB;

        [XmlElementAttribute(Order = 13)]
        public string Memo;

        [XmlElementAttribute(Order = 14)]
        public string VendorMsg;

        [XmlElementAttribute(Order = 15)]
        public string IsToBePrinted;

        [XmlElementAttribute(Order = 16)]
        public List<PurchaseOrderLineAdd> PurchaseOrderLineAdd = new List<PurchaseOrderLineAdd>();
        
        public bool ShouldSerializeTermsRef()
        {
            return !(TermsRef.FullName == null & TermsRef.ListID == null);
        }

        public bool ShouldSerializeShipMethodRef()
        {
            return !(ShipMethodRef.FullName == null & ShipMethodRef.ListID == null);
        }

        public bool ShouldSerializeClassRef()
        {
            return !(ClassRef.FullName == null & ClassRef.ListID == null);
        }

        public bool ShouldSerializeShipToEntityRef()
        {
            return !(ShipToEntityRef.FullName == null && ShipToEntityRef.ListID == null);
        }

    }




    public class PurchaseOrderLineAdd
    {
        private CustomerRef _CustomerRef = new CustomerRef();

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
        public ClassRef ClassRef = new ClassRef();

        [XmlElementAttribute(Order = 5)]
        public string Amount;

        [XmlElementAttribute(Order = 6)]
        public CustomerRef CustomerRef
        {
            get { return _CustomerRef; }
            set { _CustomerRef = value; }
        }

        [XmlElementAttribute(Order = 7)]
        public string ServiceDate;

        public bool ShouldSerializeCustomerRef()
        {
            return !(_CustomerRef.FullName == null & _CustomerRef.ListID == null);
        }
        public bool ShouldSerializeClassRef()
        {
            return !(ClassRef.FullName == null & ClassRef.ListID == null);
        }

    }

    [XmlRoot(ElementName = "PurchaseOrderAddRs")]
    public partial class PurchaseOrderAddRs : BaseAddRs { }

}
