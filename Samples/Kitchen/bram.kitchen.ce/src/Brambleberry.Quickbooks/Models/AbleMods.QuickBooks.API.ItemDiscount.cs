using System.Collections.Generic;
using System.Xml.Serialization;
using Brambleberry.Quickbooks.Models.Base;


namespace Brambleberry.Quickbooks.Models
{
    public class ItemDiscountAddRq : BaseAddRq
    {
        [XmlElement]
        public ItemDiscountAdd ItemDiscountAdd = new ItemDiscountAdd();
    }

    [XmlRoot(ElementName = "ItemDiscountQueryRq")]
    public partial class ItemDiscountQueryRq : BaseQryRq
    {
        [XmlElement(ElementName = "NameRangeFilter")]
        public NameRangeFilter NameRangeFilter;
    }

    [XmlRoot(ElementName = "ItemDiscountAddRs")]
    public partial class ItemDiscountAddRs : BaseAddRs
    {
        [XmlElement(ElementName = "ItemDiscountRet")]
        public ItemDiscountRet ItemDiscountRet;
    }

    [XmlRoot(ElementName = "ItemDiscountQueryRs")]
    public partial class ItemDiscountQueryRs : BaseQryRs
    {
        [XmlElement(ElementName = "ItemDiscountRet")]
        public List<ItemDiscountRet> ItemDiscountRet = new List<ItemDiscountRet>();
    }

    public class ItemDiscountAdd
    {

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
        public double DiscountRate;
        [XmlElementAttribute(Order = 5)]

        public AccountRef AccountRef = new AccountRef();
    }


    [XmlRoot(ElementName = "ItemDiscountRet")]
    public partial class ItemDiscountRet : BaseRet
    {
        public string Name;
        public string FullName;
        public bool IsActive;
        public ParentRef ParentRef;
        public int Sublevel;
        public string ItemDesc;
        public SalesTaxCodeRef SalesTaxCodeRef;
    }

}
