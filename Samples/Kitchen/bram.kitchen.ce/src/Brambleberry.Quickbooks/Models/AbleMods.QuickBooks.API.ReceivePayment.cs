using System;
using System.Xml.Serialization;
using Brambleberry.Quickbooks.Models.Base;


namespace Brambleberry.Quickbooks.Models
{
    public class ReceivePaymentAddRq : BaseAddRq
    {
        [XmlElement]
        public ReceivePaymentAdd ReceivePaymentAdd = new ReceivePaymentAdd();
    }

    [XmlRoot(ElementName = "ReceivePaymentAddRs")]
    public partial class ReceivePaymentAddRs : BaseAddRs
    {
        [XmlElement(ElementName = "ReceivePaymentRet")]
        public ReceivePaymentRet ReceivePaymentRet;
    }

    [XmlRoot(ElementName = "ReceivePaymentQueryRq")]
    public partial class ReceivePaymentQueryRq : BaseQryRq
    {
        [XmlElement(ElementName = "NameRangeFilter")]
        public NameRangeFilter NameRangeFilter;
    }

    public class ReceivePaymentAdd
    {

        [XmlElementAttribute(Order = 1)]
        public CustomerRef CustomerRef = new CustomerRef();
        [XmlElementAttribute(Order = 2)]
        public ARAccountRef ARAccountRef = new ARAccountRef();
        [XmlElementAttribute(Order = 3)]
        public string TxnDate;
        [XmlElementAttribute(Order = 4)]
        public string RefNumber;
        [XmlElementAttribute(Order = 5)]
        public string TotalAmount;
        [XmlElementAttribute(Order = 6)]
        public PaymentMethodRef PaymentMethodRef = new PaymentMethodRef();
        [XmlElementAttribute(Order = 7)]
        public string Memo;
        [XmlElementAttribute(Order = 8)]
        public DepositToAccountRef DepositToAccountRef = new DepositToAccountRef();
        [XmlElementAttribute(Order = 9)]

        public string IsAutoApply = "true";
    }

    public class ReceivePaymentRet : BaseRet
    {
        public string TxnNumber;
        public CustomerRef CustomerRef;
        public ARAccountRef ARAccountRef;
        public DateTime TxnDate;
        public string RefNumber;
        public decimal TotalAmount;
        public PaymentMethodRef PaymentMethodRef;
        public DepositToAccountRef DepositToAccountRef;
    }


}
