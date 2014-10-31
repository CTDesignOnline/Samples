using System.Collections.Generic;
using System.Xml.Serialization;
using Brambleberry.Quickbooks.Models.Base;

namespace Brambleberry.Quickbooks.Models
{
    public partial class QBXMLMsgsRq : BaseXMLMsgs
    {

        // QUERY requests
        [XmlElement(ElementName = "AccountQueryRq")]
        public List<AccountQueryRq> AccountQueryRq = new List<AccountQueryRq>();
        public bool ShouldSerializeAccountQueryRq() { return (this.AccountQueryRq.Count > 0); }

        [XmlElement(ElementName = "BillQueryRq")]
        public List<BillQueryRq> BillQueryRq = new List<BillQueryRq>();
        public bool ShouldSerializeBillQueryRq() { return (this.BillQueryRq.Count > 0); }

        [XmlElement(ElementName = "CustomerQueryRq")]
        public List<CustomerQueryRq> CustomerQueryRq = new List<CustomerQueryRq>();
        public bool ShouldSerializeCustomerQueryRq() { return (this.CustomerQueryRq.Count > 0); }

        [XmlElement(ElementName = "InvoiceQueryRq")]
        public List<InvoiceQueryRq> InvoiceQueryRq = new List<InvoiceQueryRq>();
        public bool ShouldSerializeInvoiceQueryRq() { return (this.InvoiceQueryRq.Count > 0); }

        [XmlElement(ElementName = "ItemDiscountQueryRq")]
        public List<ItemDiscountQueryRq> ItemDiscountQueryRq = new List<ItemDiscountQueryRq>();
        public bool ShouldSerializeItemDiscountQueryRq() { return (this.ItemDiscountQueryRq.Count > 0); }

        [XmlElement(ElementName = "ItemInventoryQueryRq")]
        public List<ItemInventoryQueryRq> ItemInventoryQueryRq = new List<ItemInventoryQueryRq>();
        public bool ShouldSerializeItemInventoryQueryRq() { return (this.ItemInventoryQueryRq.Count > 0); }

        [XmlElement(ElementName = "ItemNonInventoryQueryRq")]
        public List<ItemNonInventoryQueryRq> ItemNonInventoryQueryRq = new List<ItemNonInventoryQueryRq>();
        public bool ShouldSerializeItemNonInventoryQueryRq() { return (this.ItemNonInventoryQueryRq.Count > 0); }

        [XmlElement(ElementName = "ItemOtherChargeQueryRq")]
        public List<ItemOtherChargeQueryRq> ItemOtherChargeQueryRq = new List<ItemOtherChargeQueryRq>();
        public bool ShouldSerializeItemOtherChargeQueryRq() { return (this.ItemOtherChargeQueryRq.Count > 0); }

        [XmlElement(ElementName = "ItemGroupQueryRq")]
        public List<ItemGroupQueryRq> ItemGroupQueryRq = new List<ItemGroupQueryRq>();
        public bool ShouldSerializeItemGroupQueryRq() { return (this.ItemGroupQueryRq.Count > 0); }

        [XmlElement(ElementName = "ItemServiceQueryRq")]
        public List<ItemServiceQueryRq> ItemServiceQueryRq = new List<ItemServiceQueryRq>();
        public bool ShouldSerializeItemServiceQueryRq() { return (this.ItemServiceQueryRq.Count > 0); }

        [XmlElement(ElementName = "ItemSalesTaxQueryRq")]
        public List<ItemSalesTaxQueryRq> ItemSalesTaxQueryRq = new List<ItemSalesTaxQueryRq>();
        public bool ShouldSerializeItemSalesTaxQueryRq() { return (this.ItemSalesTaxQueryRq.Count > 0); }

        [XmlElement(ElementName = "PurchaseOrderQueryRq")]
        public List<PurchaseOrderQueryRq> PurchaseOrderQueryRq = new List<PurchaseOrderQueryRq>();
        public bool ShouldSerializePurchaseOrderQueryRq() { return (this.PurchaseOrderQueryRq.Count > 0); }

        [XmlElement(ElementName = "ReceivePaymentQueryRq")]
        public List<ReceivePaymentQueryRq> ReceivePaymentQueryRq = new List<ReceivePaymentQueryRq>();
        public bool ShouldSerializeReceivePaymentQueryRq() { return (this.ReceivePaymentQueryRq.Count > 0); }

        [XmlElement(ElementName = "SalesOrderQueryRq")]
        public List<SalesOrderQueryRq> SalesOrderQueryRq = new List<SalesOrderQueryRq>();
        public bool ShouldSerializeSalesOrderQueryRq() { return (this.SalesOrderQueryRq.Count > 0); }

        [XmlElement(ElementName = "SalesReceiptQueryRq")]
        public List<SalesReceiptQueryRq> SalesReceiptQueryRq = new List<SalesReceiptQueryRq>();
        public bool ShouldSerializeSalesReceiptQueryRq() { return (this.SalesReceiptQueryRq.Count > 0); }

        [XmlElement(ElementName = "SalesTaxCodeQueryRq")]
        public List<SalesTaxCodeQueryRq> SalesTaxCodeQueryRq = new List<SalesTaxCodeQueryRq>();
        public bool ShouldSerializeSalesTaxCodeQueryRq() { return (this.SalesTaxCodeQueryRq.Count > 0); }

        [XmlElement(ElementName = "ShipMethodQueryRq")]
        public List<ShipMethodQueryRq> ShipMethodQueryRq = new List<ShipMethodQueryRq>();
        public bool ShouldSerializeShipMethodQueryRq() { return (this.ShipMethodQueryRq.Count > 0); }

        [XmlElement(ElementName = "StandardTermsQueryRq")]
        public List<StandardTermsQueryRq> StandardTermsQueryRq = new List<StandardTermsQueryRq>();
        public bool ShouldSerializeStandardTermsQueryRq() { return (this.StandardTermsQueryRq.Count > 0); }

        [XmlElement(ElementName = "VendorQueryRq")]
        public List<VendorQueryRq> VendorQueryRq = new List<VendorQueryRq>();
        public bool ShouldSerializeVendorQueryRq() { return (this.VendorQueryRq.Count > 0); }

        [XmlElement(ElementName = "PaymentMethodQueryRq")]
        public List<PaymentMethodQueryRq> PaymentMethodQueryRq = new List<PaymentMethodQueryRq>();
        public bool ShouldSerializePaymentMethodQueryRq() { return (this.PaymentMethodQueryRq.Count > 0); }

        [XmlElement(ElementName = "ClassQueryRq")]
        public List<ClassQueryRq> ClassQueryRq = new List<ClassQueryRq>();
        public bool ShouldSerializeClassQueryRq() { return (this.ClassQueryRq.Count > 0); }

        [XmlElement(ElementName = "SalesRepQueryRq")]
        public List<SalesRepQueryRq> SalesRepQueryRq = new List<SalesRepQueryRq>();
        public bool ShouldSerializeSalesRepQueryRq() { return (this.SalesRepQueryRq.Count > 0); }

        [XmlElement(ElementName = "CustomerTypeQueryRq")]
        public List<CustomerTypeQueryRq> CustomerTypeQueryRq = new List<CustomerTypeQueryRq>();
        public bool ShouldSerializeCustomerTypeQueryRq() { return (this.CustomerTypeQueryRq.Count > 0); }

    }
}