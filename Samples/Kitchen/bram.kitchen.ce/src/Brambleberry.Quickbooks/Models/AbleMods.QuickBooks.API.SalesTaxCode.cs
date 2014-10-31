using System.Collections.Generic;
using System.Xml.Serialization;
using Brambleberry.Quickbooks.Models.Base;


namespace Brambleberry.Quickbooks.Models
{

    public class SalesTaxCodeAddRq : BaseAddRq
    {
        [XmlElement]
        public SalesTaxCodeAdd SalesTaxCodeAdd = new SalesTaxCodeAdd();
    }

    [XmlRoot(ElementName = "SalesTaxCodeQueryRq")]
    public partial class SalesTaxCodeQueryRq : BaseQryRq
    {
        [XmlElement(ElementName = "NameRangeFilter")]
        public NameRangeFilter NameRangeFilter;
    }

    [XmlRoot(ElementName = "SalesTaxCodeAddRs")]
    public partial class SalesTaxCodeAddRs : BaseAddRs
    {
        [XmlElement(ElementName = "SalesTaxCodeRet")]
        public List<SalesTaxCodeRet> SalesTaxCodeRet;
    }

    [XmlRoot(ElementName = "SalesTaxCodeQueryRs")]
    public partial class SalesTaxCodeQueryRs : BaseQryRs
    {
        [XmlElement(ElementName = "SalesTaxCodeRet")]
        public List<SalesTaxCodeRet> SalesTaxCodeRet = new List<SalesTaxCodeRet>();
    }



    public class SalesTaxCodeAdd
    {

        private string _Name;
        [XmlElementAttribute(Order = 1)]
        public string Name
        {
            get { return _Name; }
            set { _Name = Utility.StripExtendedASCII(value); }
        }

        [XmlElementAttribute(Order = 2)]
        public string IsActive;
        [XmlElementAttribute(Order = 3)]
        public string IsTaxable;
        [XmlElementAttribute(Order = 4)]

        public string Desc;
    }


    [XmlRoot(ElementName = "SalesTaxCodeRet")]
    public partial class SalesTaxCodeRet : BaseRet
    {
        public string Name;
        public bool IsActive;
        public bool IsTaxable;
        public string Desc;
    }

}
