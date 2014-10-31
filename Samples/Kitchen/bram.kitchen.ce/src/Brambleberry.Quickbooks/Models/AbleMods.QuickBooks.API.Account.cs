using System.Collections.Generic;
using System.Xml.Serialization;
using Brambleberry.Quickbooks.Models.Base;

namespace Brambleberry.Quickbooks.Models
{
    public class AccountAddRq : BaseAddRq
    {
        [XmlElement]
        public AccountAdd AccountAdd = new AccountAdd();
    }

    public partial class AccountAddRs : BaseAddRs
    {
        [XmlElement(ElementName = "AccountRet")]
        public AccountRet AccountRet;
    }

    public class AccountQueryRq : BaseQryRq { }

    public partial class AccountQueryRs : BaseQryRs
    {
        [XmlElement(ElementName = "AccountRet")]
        public List<AccountRet> AccountRet = new List<AccountRet>();

    }


    public class AccountAdd
    {

        private string _Name;

        [XmlElement(Order = 1)]
        public string Name
        {
            get { return _Name; }
            set { _Name = Utility.StripExtendedASCII(value); }
        }

        [XmlElement(Order = 2)]
        public string IsActive;

        //<XmlElementAttribute(Order:=3)> Public ParentRef As New ParentRef
        [XmlElement(Order = 3)]
        public string AccountType;

        [XmlElement(Order = 4)]
        public string AccountNumber;

        [XmlElement(Order = 5)]
        public string BankNumber;

        [XmlElement(Order = 6)]
        public string Desc;
    }

    [XmlRoot(ElementName = "AccountRet")]
    public partial class AccountRet : BaseRet
    {
        public string Name;
        public string FullName;
        public bool IsActive;
        public int Sublevel;
        public string AccountType;
        public decimal Balance;
        public decimal TotalBalance;
        public string CashFlowClassification;
    }


}
