using System.Collections.Generic;
using System.Xml.Serialization;
using Brambleberry.Quickbooks.Models.Base;

namespace Brambleberry.Quickbooks.Models
{
    [XmlRoot(ElementName = "ItemOtherChargeQueryRs")]
    public partial class ItemOtherChargeQueryRs : BaseQryRs
    {
        [XmlElement(ElementName = "ItemOtherChargeRet")]
        public List<ItemOtherChargeRet> ItemOtherChargeRet = new List<ItemOtherChargeRet>();
    }

    [XmlRoot(ElementName = "ItemOtherChargeAddRq")]
    public partial class ItemOtherChargeAddRq : BaseAddRq
    {
        [XmlElement(ElementName = "ItemOtherChargeAdd")]
        public ItemOtherChargeAdd ItemOtherChargeAdd = new ItemOtherChargeAdd();
    }


    [XmlRoot(ElementName = "ItemOtherChargeAddRs")]
    public partial class ItemOtherChargeAddRs : BaseAddRs
    {
        [XmlElement(ElementName = "ItemOtherChargeRet")]
        public List<ItemOtherChargeRet> ItemOtherChargeRet = new List<ItemOtherChargeRet>();
    }

    [XmlRoot(ElementName = "ItemOtherChargeQueryRq")]
    public partial class ItemOtherChargeQueryRq : BaseQryRq
    {
        [XmlElement(ElementName = "NameRangeFilter")]
        public NameRangeFilter NameRangeFilter;
    }

    [XmlRoot(ElementName = "ItemOtherChargeRet")]
    public partial class ItemOtherChargeRet : BaseRet
    {
        public string Name;
        public string FullName;
        public bool IsActive;
        public ParentRef ParentRef;
        public int Sublevel;
        public SalesTaxCodeRef SalesTaxCodeRef;
        public SalesAndPurchase SalesAndPurchase;
    }

    [XmlRoot(ElementName = "ItemOtherChargeAdd")]
    public partial class ItemOtherChargeAdd
    {
        public string Name;
        public string FullName;
        public bool IsActive;
        public int Sublevel;
        public SalesTaxCodeRef SalesTaxCodeRef;
        public SalesAndPurchase SalesAndPurchase;
    }


}
