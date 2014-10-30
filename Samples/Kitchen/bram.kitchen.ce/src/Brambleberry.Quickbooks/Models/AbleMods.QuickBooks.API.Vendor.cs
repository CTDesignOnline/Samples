using System.Collections.Generic;
using System.Xml.Serialization;
using Brambleberry.Quickbooks.Models.Base;


namespace Brambleberry.Quickbooks.Models
{
    public class VendorAddRq : BaseAddRq
    {
        [XmlElement]
        public VendorAdd VendorAdd = new VendorAdd();
    }

    [XmlRoot(ElementName = "VendorQueryRq")]

    public partial class VendorQueryRq : BaseQryRq
    {
        [XmlElement(ElementName = "NameRangeFilter")]
        public NameRangeFilter NameRangeFilter;
    }
    [XmlRoot(ElementName = "VendorAddRs")]
    public partial class VendorAddRs : BaseAddRs
    {
        [XmlElement(ElementName = "VendorRet")]
        public List<VendorRet> VendorRet;
    }


    [XmlRoot(ElementName = "VendorQueryRs")]
    public partial class VendorQueryRs : BaseQryRs
    {
        [XmlElement(ElementName = "VendorRet")]
        public List<VendorRet> VendorRet = new List<VendorRet>();
    }

    public class VendorAdd
    {

        private VendorTypeRef _VendorTypeRef = new VendorTypeRef();
        private TermsRef _TermsRef = new TermsRef();
        private string _Name;

        private string _CompanyName;
        [XmlElementAttribute(Order = 1)]
        public string Name
        {
            get { return _Name; }
            set { _Name = Utility.StripExtendedASCII(value); }
        }

        [XmlElementAttribute(Order = 2)]

        public string IsActive;
        [XmlElementAttribute(Order = 3)]
        public string CompanyName
        {
            get { return _CompanyName; }
            set { _CompanyName = Utility.StripExtendedASCII(value); }
        }

        [XmlElementAttribute(Order = 4)]
        public string Salutation;

        [XmlElementAttribute(Order = 5)]
        public string FirstName;

        [XmlElementAttribute(Order = 6)]
        public string MiddleName;

        [XmlElementAttribute(Order = 7)]
        public string LastName;

        [XmlElementAttribute(Order = 8)]
        public VendorAddressBlock VendorAddress;

        [XmlElementAttribute(Order = 9)]
        public string Phone;

        [XmlElementAttribute(Order = 10)]
        public string Fax;

        [XmlElementAttribute(Order = 11)]
        public string Email;

        [XmlElementAttribute(Order = 12)]
        public string Contact;

        [XmlElementAttribute(Order = 13)]
        public string AccountNumber;

        [XmlElementAttribute(Order = 14)]
        public string Notes;

        [XmlElementAttribute(Order = 15)]
        public VendorTypeRef VendorTypeRef
        {
            get { return _VendorTypeRef; }
            set { _VendorTypeRef = value; }
        }

        [XmlElementAttribute(Order = 16)]
        public TermsRef TermsRef
        {
            get { return _TermsRef; }
            set { _TermsRef = value; }
        }

        public bool ShouldSerializeVendorTypeRef()
        {
            return !(_VendorTypeRef.FullName == null & _VendorTypeRef.ListID == null);
        }

        public bool ShouldSerializeTermsRef()
        {
            return !(_TermsRef.FullName == null & _TermsRef.ListID == null);
        }

    }

    [XmlRoot(ElementName = "VendorRet")]
    public partial class VendorRet : BaseRet
    {
        public string Name;
        public bool IsActive;
        public string CompanyName;
        public string Salutation;
        public string FirstName;
        public string MiddleName;
        public string LastName;
        public VendorAddressBlock VendorAddress;
        public string Phone;
        public string AltPhone;
        public string Fax;
        public string Email;
        public string Contact;
        public string AccountNumber;
        public VendorTypeRef VendorTypeRef;
        public TermsRef TermsRef;
    }

}
