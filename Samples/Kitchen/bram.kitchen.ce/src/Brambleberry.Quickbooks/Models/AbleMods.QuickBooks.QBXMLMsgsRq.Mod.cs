using System.Collections.Generic;
using System.Xml.Serialization;
using Brambleberry.Quickbooks.Models.Base;

namespace Brambleberry.Quickbooks.Models
{
    public partial class QBXMLMsgsRq : BaseXMLMsgs
    {

        // MOD requests
        [XmlElement(ElementName = "CustomerModRq")]
        public List<CustomerModRq> CustomerModRq = new List<CustomerModRq>();
        public bool ShouldSerializeCustomerModRq() { return (this.CustomerModRq.Count > 0); }
    }
}
