using System.Collections.Generic;
using System.Xml.Serialization;
using Brambleberry.Quickbooks.Models.Base;


namespace Brambleberry.Quickbooks.Models
{
    public class ShipMethodAddRq : BaseAddRq
    {
        [XmlElement]
        public ShipMethodAdd ShipMethodAdd = new ShipMethodAdd();
    }

    [XmlRoot(ElementName = "ShipMethodAddRs")]
    public partial class ShipMethodAddRs : BaseAddRs
    {
        [XmlElement(ElementName = "ShipMethodRet")]
        public List<ShipMethodRet> ShipMethodRet;
    }

    [XmlRoot(ElementName = "ShipMethodQueryRs")]
    public partial class ShipMethodQueryRs : BaseQryRs
    {
        [XmlElement(ElementName = "ShipMethodRet")]
        public List<ShipMethodRet> ShipMethodRet = new List<ShipMethodRet>();
    }

    [XmlRoot(ElementName = "ShipMethodQueryRq")]
    public partial class ShipMethodQueryRq : BaseQryRq
    {
        [XmlElement(ElementName = "NameRangeFilter")]
        public NameRangeFilter NameRangeFilter;
    }

    public class ShipMethodAdd
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
    }

    [XmlRoot(ElementName = "ShipMethodRet")]
    public partial class ShipMethodRet : BaseRet
    {
        public string Name;
        public bool IsActive;
    }


}
