using System.Collections.Generic;
using System.Xml.Serialization;
using Brambleberry.Quickbooks.Models.Base;


namespace Brambleberry.Quickbooks.Models
{
    public class PaymentMethodAddRq : BaseAddRq
    {
        [XmlElement]
        public PaymentMethodAdd PaymentMethodAdd = new PaymentMethodAdd();
    }

    [XmlRoot(ElementName = "PaymentMethodQueryRq")]
    public partial class PaymentMethodQueryRq : BaseQryRq
    {
        [XmlElement(ElementName = "NameRangeFilter")]
        public NameRangeFilter NameRangeFilter;
    }

    [XmlRoot(ElementName = "PaymentMethodAddRs")]
    public partial class PaymentMethodAddRs : BaseAddRs
    {
        [XmlElement(ElementName = "PaymentMethodRet")]
        public List<PaymentMethodRet> PaymentMethodRet;
    }

    [XmlRoot(ElementName = "PaymentMethodQueryRs")]
    public partial class PaymentMethodQueryRs : BaseQryRs
    {
        [XmlElement(ElementName = "PaymentMethodRet")]
        public List<PaymentMethodRet> PaymentMethodRet = new List<PaymentMethodRet>();
    }

    
    public class PaymentMethodAdd
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
        public string PaymentMethodType;
    }


    [XmlRoot(ElementName = "PaymentMethodRet")]
    public partial class PaymentMethodRet : BaseRet
    {
        public string Name;
        public bool IsActive;
        public string PaymentMethodType;
    }

}
