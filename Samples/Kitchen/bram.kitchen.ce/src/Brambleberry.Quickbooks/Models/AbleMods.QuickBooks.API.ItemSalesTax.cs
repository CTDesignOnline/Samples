using System.Collections.Generic;
using System.Xml.Serialization;
using Brambleberry.Quickbooks.Models.Base;


namespace Brambleberry.Quickbooks.Models
{
    public class ItemSalesTaxAddRq : BaseAddRq
    {
        [XmlElement]
        public ItemSalesTaxAdd ItemSalesTaxAdd = new ItemSalesTaxAdd();
    }

    [XmlRoot(ElementName = "ItemSalesTaxAddRs")]
    public partial class ItemSalesTaxAddRs : BaseAddRs
    {
        [XmlElement(ElementName = "ItemSalesTaxRet")]
        public List<ItemSalesTaxRet> ItemSalesTaxRet;
    }

    [XmlRoot(ElementName = "ItemSalesTaxQueryRs")]
    public partial class ItemSalesTaxQueryRs : BaseQryRs
    {
        [XmlElement(ElementName = "ItemSalesTaxRet")]
        public List<ItemSalesTaxRet> ItemSalesTaxRet = new List<ItemSalesTaxRet>();
    }

    [XmlRoot(ElementName = "ItemSalesTaxQueryRq")]
    public partial class ItemSalesTaxQueryRq : BaseQryRq
    {
        [XmlElement(ElementName = "NameRangeFilter")]
        public NameRangeFilter NameRangeFilter;
    }

    public class ItemSalesTaxAdd
    {
        private TaxVendorRef _TaxVendorRef = new TaxVendorRef();
        private string _Name;

        private string _ItemDesc;
        [XmlElementAttribute(Order = 1)]
        public string Name
        {
            get { return _Name; }
            set { _Name = Utility.StripExtendedASCII(value); }
        }

        [XmlElementAttribute(Order = 2)]

        public string IsActive;
        [XmlElementAttribute(Order = 3)]
        public string ItemDesc
        {
            get { return _ItemDesc; }
            set { _ItemDesc = Utility.StripExtendedASCII(value); }
        }

        [XmlElementAttribute(Order = 4)]

        public string TaxRate;
        [XmlElementAttribute(Order = 5)]
        public TaxVendorRef TaxVendorRef
        {
            get { return _TaxVendorRef; }
            set { _TaxVendorRef = value; }
        }

        public bool ShouldSerializeTaxVendorRef()
        {
            return !(_TaxVendorRef.FullName == null & _TaxVendorRef.ListID == null);
        }

    }


    [XmlRoot(ElementName = "ItemSalesTaxRet")]
    public partial class ItemSalesTaxRet : BaseRet
    {
        public string Name;
        public bool IsActive;
        public string ItemDesc;
        public double TaxRate;
        public TaxVendorRef TaxVendorRef;
    }




}
