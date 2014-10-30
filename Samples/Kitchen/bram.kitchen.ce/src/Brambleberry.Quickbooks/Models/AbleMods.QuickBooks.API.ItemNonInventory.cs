using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Brambleberry.Quickbooks.Models.Base;


namespace Brambleberry.Quickbooks.Models
{
    public class ItemNonInventoryAddRq : BaseAddRq
    {
        [XmlElement]
        public ItemNonInventoryAdd ItemNonInventoryAdd = new ItemNonInventoryAdd();
    }

    [XmlRoot(ElementName = "ItemNonInventoryQueryRq")]
    public partial class ItemNonInventoryQueryRq : BaseQryRq
    {
        [XmlElement(ElementName = "NameRangeFilter")]
        public NameRangeFilter NameRangeFilter;
    }

    [XmlRoot(ElementName = "ItemNonInventoryAddRs")]
    public partial class ItemNonInventoryAddRs : BaseAddRs
    {
        [XmlElement(ElementName = "ItemNonInventoryRet")]
        public List<ItemNonInventoryRet> ItemNonInventoryRet;
    }

    [XmlRoot(ElementName = "ItemNonInventoryQueryRs")]
    public partial class ItemNonInventoryQueryRs : BaseQryRs
    {
        [XmlElement(ElementName = "ItemNonInventoryRet")]
        public List<ItemNonInventoryRet> ItemNonInventoryRet = new List<ItemNonInventoryRet>();
    }

    public class ItemNonInventoryAdd
    {

        private SalesTaxCodeRef _SalesTaxCodeRef = new SalesTaxCodeRef();
        private string _Name;
        //Private _ManufacturerPartNumber As String

        [XmlElementAttribute(Order = 1)]
        public string Name
        {
            get { return _Name; }
            // 31-char limit for item name
            set { _Name = Utility.StripExtendedASCII(value.Substring(0, Math.Min(value.Length,31))); }
        }

        [XmlElementAttribute(Order = 2)]
        public string IsActive;

        [XmlElementAttribute(Order = 3)]
        public SalesTaxCodeRef SalesTaxCodeRef
        {
            get { return _SalesTaxCodeRef; }
            set { _SalesTaxCodeRef = value; }
        }

        [XmlElementAttribute(Order = 4)]

        public SalesAndPurchase SalesAndPurchase = new SalesAndPurchase();
        public bool ShouldSerializeSalesTaxCodeRef()
        {
            return !(_SalesTaxCodeRef.FullName == null & _SalesTaxCodeRef.ListID == null);
        }

    }



    [XmlRoot(ElementName = "ItemNonInventoryRet")]
    public partial class ItemNonInventoryRet : BaseRet
    {
        public string Name;
        public string FullName;
        public bool IsActive;
        public ParentRef ParentRef;
        public int Sublevel;
        public SalesTaxCodeRef SalesTaxCodeRef;
        public SalesAndPurchase SalesAndPurchase;
    }



}
