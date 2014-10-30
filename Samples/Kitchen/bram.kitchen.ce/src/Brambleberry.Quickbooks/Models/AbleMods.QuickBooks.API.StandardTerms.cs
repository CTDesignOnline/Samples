using System.Collections.Generic;
using System.Xml.Serialization;
using Brambleberry.Quickbooks.Models.Base;

namespace Brambleberry.Quickbooks.Models
{

    public class StandardTermsAddRq : BaseAddRq
    {
        [XmlElement]
        public StandardTermsAdd StandardTermsAdd = new StandardTermsAdd();
    }

    [XmlRoot(ElementName = "StandardTermsQueryRq")]
    public partial class StandardTermsQueryRq : BaseQryRq
    {
        [XmlElement(ElementName = "NameRangeFilter")]
        public NameRangeFilter NameRangeFilter;
    }

    [XmlRoot(ElementName = "StandardTermsAddRs")]
    public partial class StandardTermsAddRs : BaseAddRs
    {
        [XmlElement(ElementName = "StandardTermsRet")]
        public List<StandardTermsRet> StandardTermsRet;
    }


    [XmlRoot(ElementName = "StandardTermsQueryRs")]
    public partial class StandardTermsQueryRs : BaseQryRs
    {
        [XmlElement(ElementName = "StandardTermsRet")]
        public List<StandardTermsRet> StandardTermsRet = new List<StandardTermsRet>();
    }

    public class StandardTermsAdd
    {
        [XmlElementAttribute(Order = 1)]
        public string Name;

        [XmlElementAttribute(Order = 2)]
        public string IsActive;

        [XmlElementAttribute(Order = 3)]
        public string StdDueDays;

        [XmlElementAttribute(Order = 4)]
        public string StdDiscountDays;

        [XmlElementAttribute(Order = 5)]
        public string DiscountPct;
    }


    [XmlRoot(ElementName = "StandardTermsRet")]
    public partial class StandardTermsRet : BaseRet
    {
        public string Name;
        public bool IsActive;
        public int StdDueDays;
        public int StdDiscountDays;
        public decimal DiscountPct;
    }


}
