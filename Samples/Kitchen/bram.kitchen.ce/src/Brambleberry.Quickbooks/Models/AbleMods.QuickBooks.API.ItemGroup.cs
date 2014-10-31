using System.Collections.Generic;
using System.Xml.Serialization;
using Brambleberry.Quickbooks.Models.Base;

namespace Brambleberry.Quickbooks.Models
{
    [XmlRoot(ElementName = "ItemGroupQueryRs")]
    public partial class ItemGroupQueryRs : BaseQryRs
    {
        [XmlElement(ElementName = "ItemGroupRet")]
        public List<ItemGroupRet> ItemGroupRet = new List<ItemGroupRet>();
    }

    [XmlRoot(ElementName = "ItemGroupAddRq")]
    public partial class ItemGroupAddRq : BaseAddRq
    {
        public ItemGroupAdd ItemGroupAdd = new ItemGroupAdd();

    }

    [XmlRoot(ElementName = "ItemGroupAddRs")]
    public partial class ItemGroupAddRs : BaseAddRs
    {
        [XmlElement(ElementName = "ItemGroupRet")]
        public ItemGroupRet ItemGroupRet;

    }

    [XmlRoot(ElementName = "ItemGroupQueryRq")]
    public partial class ItemGroupQueryRq : BaseQryRq
    {
        [XmlElement(ElementName = "NameRangeFilter")]
        public NameRangeFilter NameRangeFilter;
    }

    [XmlRoot(ElementName = "ItemGroupRet")]
    public partial class ItemGroupRet : BaseRet
    {
        public string Name;
        public string FullName;
        public bool IsActive;
        public string ItemDesc;
        public bool IsPrintItemsInGroup;
        public List<ItemRef> ItemGroupLineList;
    }

    [XmlRoot(ElementName = "ItemGroupAdd")]
    public partial class ItemGroupAdd
    {
        public string Name;
        public string FullName;
        public bool IsActive;
        public string ItemDesc;
        public bool IsPrintItemsInGroup;
        public List<ItemRef> ItemGroupLineList;
    }


}
