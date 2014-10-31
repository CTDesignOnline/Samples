using System.Collections.Generic;
using System.Xml.Serialization;
using Brambleberry.Quickbooks.Models.Base;

namespace Brambleberry.Quickbooks.Models
{

    public class BillAddRq : BaseAddRq
    {
        [XmlElement]
        public BillAdd BillAdd = new BillAdd();
    }

    [XmlRoot(ElementName = "BillQueryRq")]
    public partial class BillQueryRq : BaseQryRq
    {
        [XmlElement(ElementName = "NameRangeFilter")]
        public NameRangeFilter NameRangeFilter;
    }

    [XmlRoot(ElementName = "BillAddRs")]
    public partial class BillAddRs : BaseAddRs
    {
        [XmlElement(ElementName = "BillRet")]
        public BillRet BillRet;
    }

    public class BillAdd
    {
        private TermsRef _TermsRef = new TermsRef();

        [XmlElementAttribute(Order = 1)]
        public VendorRef VendorRef = new VendorRef();

        [XmlElementAttribute(Order = 2)]
        public APAccountRef APAccountRef = new APAccountRef();

        [XmlElementAttribute(Order = 3)]
        public string TxnDate;

        [XmlElementAttribute(Order = 4)]
        public string DueDate;

        [XmlElementAttribute(Order = 5)]
        public string RefNumber;

        [XmlElementAttribute(Order = 6)]
        public TermsRef TermsRef
        {
            get { return _TermsRef; }
            set { _TermsRef = value; }
        }

        [XmlElementAttribute(Order = 7)]
        public List<ItemLineAdd> ItemLineAdd = new List<ItemLineAdd>();

        public bool ShouldSerializeTermsRef()
        {
            return !(_TermsRef.FullName == null & _TermsRef.ListID == null);
        }


    }

    public class BillRet : BaseRet
    {
        public VendorRef VendorRef = new VendorRef();
        public APAccountRef APAccountRef = new APAccountRef();
        public string TxnDate;
        public string DueDate;
        public string RefNumber;
        public TermsRef TermsRef;
    }


}
