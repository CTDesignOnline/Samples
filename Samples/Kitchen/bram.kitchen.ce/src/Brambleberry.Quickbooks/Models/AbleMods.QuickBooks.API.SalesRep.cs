using System.Collections.Generic;
using System.Xml.Serialization;
using Brambleberry.Quickbooks.Models.Base;

namespace Brambleberry.Quickbooks.Models
{
    public class SalesRepAddRq : BaseAddRq
    {
        [XmlElement]
        public SalesRepAdd SalesRepAdd = new SalesRepAdd();
    }

    public partial class SalesRepAddRs : BaseAddRs
    {
        [XmlElement(ElementName = "SalesRepRet")]
        public SalesRepRet SalesRepRet;
    }

    public class SalesRepQueryRq : BaseQryRq { }

    public partial class SalesRepQueryRs : BaseQryRs
    {
        [XmlElement(ElementName = "SalesRepRet")]
        public List<SalesRepRet> SalesRepRet = new List<SalesRepRet>();

    }


    public class SalesRepAdd
    {

        [XmlElementAttribute(Order = 1)]
        public string Initial;

        [XmlElementAttribute(Order = 2)]
        public bool IsActive;

        [XmlElementAttribute(Order = 3)]
        public SalesRepEntityRef SalesRepEntityRef = new SalesRepEntityRef();

    }

    [XmlRoot(ElementName = "SalesRepRet")]
    public partial class SalesRepRet : BaseRet
    {
        public string Initial;
        public bool IsActive;
        public SalesRepEntityRef SalesRepEntityRef;
    }


}
