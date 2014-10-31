using System.Collections.Generic;
using System.Xml.Serialization;
using Brambleberry.Quickbooks.Models.Base;


namespace Brambleberry.Quickbooks.Models
{
    public class ClassAddRq : BaseAddRq
    {
        [XmlElement]
        public ClassAdd ClassAdd = new ClassAdd();
    }

    [XmlRoot(ElementName = "ClassQueryRq")]
    public partial class ClassQueryRq : BaseQryRq
    {
        [XmlElement(ElementName = "NameRangeFilter")]
        public NameRangeFilter NameRangeFilter;
    }

    [XmlRoot(ElementName = "ClassAddRs")]
    public partial class ClassAddRs : BaseAddRs
    {
        [XmlElement(ElementName = "ClassRet")]
        public ClassRet ClassRet;
    }

    [XmlRoot(ElementName = "ClassQueryRs")]
    public partial class ClassQueryRs : BaseQryRs
    {
        [XmlElement(ElementName = "ClassRet")]
        public List<ClassRet> ClassRet = new List<ClassRet>();
    }

    public class ClassAdd
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
        public ParentRef ParentRef;
    }

    [XmlRoot(ElementName = "ClassRet")]
    public partial class ClassRet : BaseRet
    {
        public string Name;
        public string FullName;
        public bool IsActive;
        public ParentRef ParentRef;
        public int Sublevel;
    }




}
