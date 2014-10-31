using System.Collections.Generic;
using System.Xml.Serialization;
using Brambleberry.Quickbooks.Models.Base;

namespace Brambleberry.Quickbooks.Models
{
    [XmlRoot(ElementName = "ItemServiceQueryRs")]
    public partial class ItemServiceQueryRs : BaseQryRs
    {
        [XmlElement(ElementName = "ItemServiceRet")]
        public List<ItemServiceRet> ItemServiceRet = new List<ItemServiceRet>();
    }

    [XmlRoot(ElementName = "ItemServiceAddRq")]
    public partial class ItemServiceAddRq : BaseAddRq
    {
        [XmlElement(ElementName = "ItemServiceAdd")]
        public ItemServiceAdd ItemServiceAdd = new ItemServiceAdd();
    }

    [XmlRoot(ElementName = "ItemServiceAddRs")]
    public partial class ItemServiceAddRs : BaseAddRs
    {
        [XmlElement(ElementName = "ItemServiceRet")]
        public List<ItemServiceRet> ItemServiceRet = new List<ItemServiceRet>();
    }

    [XmlRoot(ElementName = "ItemServiceQueryRq")]
    public partial class ItemServiceQueryRq : BaseQryRq
    {
        [XmlElement(ElementName = "NameRangeFilter")]
        public NameRangeFilter NameRangeFilter;
    }

    [XmlRoot(ElementName = "ItemServiceRet")]
    public partial class ItemServiceRet : BaseRet
    {
        public string Name;
        public string FullName;
        public bool IsActive;
        public ParentRef ParentRef;
        public SalesTaxCodeRef SalesTaxCodeRef;
        public SalesAndPurchase SalesAndPurchase;
    }

    [XmlRoot(ElementName = "ItemServiceAdd")]
    public partial class ItemServiceAdd
    {
        public string Name;
        public string FullName;
        public bool IsActive;
        public ParentRef ParentRef;
        public SalesTaxCodeRef SalesTaxCodeRef;
        public SalesAndPurchase SalesAndPurchase;
    }

}
