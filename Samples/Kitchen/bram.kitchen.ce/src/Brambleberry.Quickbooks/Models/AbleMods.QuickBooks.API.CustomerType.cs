using System.Collections.Generic;
using System.Xml.Serialization;
using Brambleberry.Quickbooks.Models.Base;

namespace Brambleberry.Quickbooks.Models
{
    public class CustomerTypeAddRq : BaseAddRq
    {
        [XmlElement]
        public CustomerTypeAdd CustomerTypeAdd = new CustomerTypeAdd();
    }

    public partial class CustomerTypeAddRs : BaseAddRs
    {
        [XmlElement(ElementName = "CustomerTypeRet")]
        public CustomerTypeRet CustomerTypeRet;
    }

    public class CustomerTypeQueryRq : BaseQryRq { }

    public partial class CustomerTypeQueryRs : BaseQryRs
    {
        [XmlElement(ElementName = "CustomerTypeRet")]
        public List<CustomerTypeRet> CustomerTypeRet = new List<CustomerTypeRet>();

    }


    public class CustomerTypeAdd
    {

        [XmlElementAttribute(Order = 1)]
        public string Name;

        [XmlElementAttribute(Order = 2)]
        public bool IsActive;

        [XmlElementAttribute(Order = 3)]
        public ParentRef ParentRef;

    }

    [XmlRoot(ElementName = "CustomerTypeRet")]
    public partial class CustomerTypeRet : BaseRet
    {
        public string Name;
        public string FullName;
        public bool IsActive;
        public ParentRef ParentRef;
        public int Sublevel;
    }


}
