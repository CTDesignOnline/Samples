using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Brambleberry.Quickbooks.Models.Base;


namespace Brambleberry.Quickbooks.Models
{
    public class ItemInventoryAddRq : BaseAddRq
    {
        [XmlElement]
        public ItemInventoryAdd ItemInventoryAdd = new ItemInventoryAdd();
    }

    [XmlRoot(ElementName = "ItemInventoryAddRs")]
    public partial class ItemInventoryAddRs : BaseAddRs
    {
        [XmlElement(ElementName = "ItemInventoryRet")]
        public ItemInventoryRet ItemInventoryRet;
    }

    [XmlRoot(ElementName = "ItemInventoryQueryRq")]
    public partial class ItemInventoryQueryRq : BaseQryRq
    {
        [XmlElement(ElementName = "NameRangeFilter")]
        public NameRangeFilter NameRangeFilter = new NameRangeFilter();
        public bool ShouldSerializeNameRangeFilter()
        {
            return (NameRangeFilter.FromName != "" | NameRangeFilter.ToName != "");
        }

    }

    [XmlRoot(ElementName = "ItemInventoryQueryRs")]
    public partial class ItemInventoryQueryRs : BaseQryRs
    {
        [XmlElement(ElementName = "ItemInventoryRet")]
        public List<ItemInventoryRet> ItemInventoryRet = new List<ItemInventoryRet>();
    }

    public class ItemInventoryAdd
    {

        private SalesTaxCodeRef _SalesTaxCodeRef = new SalesTaxCodeRef();
        private string _Name;
        private string _SalesDesc;
        private string _PurchaseDesc;
        private ParentRef _ParentRef;

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
        public ParentRef ParentRef
        {
            get { return _ParentRef; }
            set { _ParentRef = value; }
        }

        [XmlElementAttribute(Order = 4)]
        public SalesTaxCodeRef SalesTaxCodeRef
        {
            get { return _SalesTaxCodeRef; }
            set { _SalesTaxCodeRef = value; }
        }

        [XmlElementAttribute(Order = 5)]
        public string SalesDesc
        {
            get { return _SalesDesc; }
            set { _SalesDesc = Utility.StripExtendedASCII(value); }
        }

        [XmlElementAttribute(Order = 6)]
        public double SalesPrice;
        
        [XmlElementAttribute(Order = 7)]
        public IncomeAccountRef IncomeAccountRef = new IncomeAccountRef();

        [XmlElementAttribute(Order = 8)]
        public string PurchaseDesc
        {
            get { return _PurchaseDesc; }
            set { _PurchaseDesc = Utility.StripExtendedASCII(value); }
        }

        [XmlElementAttribute(Order = 9)]
        public double PurchaseCost;
        
        [XmlElementAttribute(Order = 10)]
        public COGSAccountRef COGSAccountRef = new COGSAccountRef();
        
        [XmlElementAttribute(Order = 11)]
        public AssetAccountRef AssetAccountRef = new AssetAccountRef();
        
        [XmlElementAttribute(Order = 12)]
        public decimal ReorderPoint;
        
        [XmlElementAttribute(Order = 13)]
        public int QuantityOnHand;
        
        [XmlElementAttribute(Order = 14)]
        public string InventoryDate;
        
        public bool ShouldSerializeSalesTaxCodeRef()
        {
            return !(_SalesTaxCodeRef.FullName == null & _SalesTaxCodeRef.ListID == null);
        }
    }

    [XmlRoot(ElementName = "ItemInventoryRet")]
    public partial class ItemInventoryRet : BaseRet
    {
        public string Name;
        public string FullName;
        public bool IsActive;
        public ParentRef ParentRef;
        public int Sublevel;
        public SalesTaxCodeRef SalesTaxCodeRef;
        public string SalesDesc;
        public decimal SalesPrice;
        public IncomeAccountRef IncomeAccountRef;
        public string PurchaseDesc;
        public decimal PurchaseCost;
        public COGSAccountRef COGSAccountRef;
        public AssetAccountRef AssetAccountRef;
        public decimal ReorderPoint;
        public decimal QuantityOnHand;
        public decimal AverageCost;
        public decimal QuantityOnOrder;
        public decimal QuantityOnSalesOrder;
    }



}
