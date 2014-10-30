using System.Xml.Serialization;
using Brambleberry.Quickbooks.Models.Base;

namespace Brambleberry.Quickbooks.Models
{
    public partial class QBXMLMsgsRq : BaseXMLMsgs 
    {
        // ADD requests
        [XmlElement(ElementName = "AccountAddRq")]
        public AccountAddRq AccountAddRq = new AccountAddRq();
        public bool ShouldSerializeAccountAddRq(){return (AccountAddRq.AccountAdd.Name != null);}

        [XmlElement(ElementName = "BillAddRq")]
        public BillAddRq BillAddRq = new BillAddRq();
        public bool ShouldSerializeBillAddRq(){return (BillAddRq.BillAdd.RefNumber != null);}

        [XmlElement(ElementName = "CustomerAddRq")]
        public CustomerAddRq CustomerAddRq = new CustomerAddRq();
        public bool ShouldSerializeCustomerAddRq(){return (CustomerAddRq.CustomerAdd.Name != null);}

        [XmlElement(ElementName = "InvoiceAddRq")]
        public InvoiceAddRq InvoiceAddRq = new InvoiceAddRq();
        public bool ShouldSerializeInvoiceAddRq(){return (InvoiceAddRq.InvoiceAdd.RefNumber != null);}

        [XmlElement(ElementName = "ItemDiscountAddRq")]
        public ItemDiscountAddRq ItemDiscountAddRq = new ItemDiscountAddRq();
        public bool ShouldSerializeItemDiscountAddRq(){return (ItemDiscountAddRq.ItemDiscountAdd.Name != null);}

        [XmlElement(ElementName = "ItemServiceAddRq")]
        public ItemServiceAddRq ItemServiceAddRq = new ItemServiceAddRq();
        public bool ShouldSerializeItemServiceAddRq() { return (ItemServiceAddRq.ItemServiceAdd.Name != null); }

        [XmlElement(ElementName = "ItemGroupAddRq")]
        public ItemGroupAddRq ItemGroupAddRq = new ItemGroupAddRq();
        public bool ShouldSerializeItemGroupAddRq() { return (ItemGroupAddRq.ItemGroupAdd.Name != null); }

        [XmlElement(ElementName = "ItemOtherChargeAddRq")]
        public ItemOtherChargeAddRq ItemOtherChargeAddRq = new ItemOtherChargeAddRq();
        public bool ShouldSerializeItemOtherChargeAddRq() { return (ItemOtherChargeAddRq.ItemOtherChargeAdd.Name != null); }

        [XmlElement(ElementName = "ItemInventoryAddRq")]
        public ItemInventoryAddRq ItemInventoryAddRq = new ItemInventoryAddRq();
        public bool ShouldSerializeItemInventoryAddRq(){return (ItemInventoryAddRq.ItemInventoryAdd.Name != null);}

        [XmlElement(ElementName = "ItemNonInventoryAddRq")]
        public ItemNonInventoryAddRq ItemNonInventoryAddRq = new ItemNonInventoryAddRq();
        public bool ShouldSerializeItemNonInventoryAddRq(){return (ItemNonInventoryAddRq.ItemNonInventoryAdd.Name != null);}

        [XmlElement(ElementName = "ItemSalesTaxAddRq")]
        public ItemSalesTaxAddRq ItemSalesTaxAddRq = new ItemSalesTaxAddRq();
        public bool ShouldSerializeItemSalesTaxAddRq(){return (ItemSalesTaxAddRq.ItemSalesTaxAdd.Name != null);}

        [XmlElement(ElementName = "PurchaseOrderAddRq")]
        public PurchaseOrderAddRq PurchaseOrderAddRq = new PurchaseOrderAddRq();
        public bool ShouldSerializePurchaseOrderAddRq(){return (PurchaseOrderAddRq.PurchaseOrderAdd.RefNumber != null);}

        [XmlElement(ElementName = "ReceivePaymentAddRq")]
        public ReceivePaymentAddRq ReceivePaymentAddRq = new ReceivePaymentAddRq();
        public bool ShouldSerializeReceivePaymentAddRq(){return (ReceivePaymentAddRq.ReceivePaymentAdd.RefNumber != null);}

        [XmlElement(ElementName = "SalesOrderAddRq")]
        public SalesOrderAddRq SalesOrderAddRq = new SalesOrderAddRq();
        public bool ShouldSerializeSalesOrderAddRq(){return (SalesOrderAddRq.SalesOrderAdd.RefNumber != null);}

        [XmlElement(ElementName = "SalesReceiptAddRq")]
        public SalesReceiptAddRq SalesReceiptAddRq = new SalesReceiptAddRq();
        public bool ShouldSerializeSalesReceiptAddRq(){return (SalesReceiptAddRq.SalesReceiptAdd.RefNumber != null);}

        [XmlElement(ElementName = "SalesTaxCodeAddRq")]
        public SalesTaxCodeAddRq SalesTaxCodeAddRq = new SalesTaxCodeAddRq();
        public bool ShouldSerializeSalesTaxCodeAddRq(){return (SalesTaxCodeAddRq.SalesTaxCodeAdd.Name != null);}

        [XmlElement(ElementName = "ShipMethodAddRq")]
        public ShipMethodAddRq ShipMethodAddRq = new ShipMethodAddRq();
        public bool ShouldSerializeShipMethodAddRq(){return (ShipMethodAddRq.ShipMethodAdd.Name != null);}

        [XmlElement(ElementName = "StandardTermsAddRq")]
        public StandardTermsAddRq StandardTermsAddRq = new StandardTermsAddRq();
        public bool ShouldSerializeStandardTermsAddRq(){return (StandardTermsAddRq.StandardTermsAdd.Name != null);}

        [XmlElement(ElementName = "VendorAddRq")]
        public VendorAddRq VendorAddRq = new VendorAddRq();
        public bool ShouldSerializeVendorAddRq(){return (VendorAddRq.VendorAdd.Name != null);}

        [XmlElement(ElementName = "PaymentMethodAddRq")]
        public PaymentMethodAddRq PaymentMethodAddRq = new PaymentMethodAddRq();
        public bool ShouldSerializePaymentMethodAddRq(){return (PaymentMethodAddRq.PaymentMethodAdd.Name != null);}

        [XmlElement(ElementName = "ClassAddRq")]
        public ClassAddRq ClassAddRq = new ClassAddRq();
        public bool ShouldSerializeClassAddRq() { return (ClassAddRq.ClassAdd.Name != null); }

        [XmlElement(ElementName = "SalesRepAddRq")]
        public SalesRepAddRq SalesRepAddRq = new SalesRepAddRq();
        public bool ShouldSerializeSalesRepAddRq() { return (SalesRepAddRq.SalesRepAdd.SalesRepEntityRef.FullName != null); }

        [XmlElement(ElementName = "CustomerTypeAddRq")]
        public CustomerTypeAddRq CustomerTypeAddRq = new CustomerTypeAddRq();
        public bool ShouldSerializeCustomerTypeAddRq() { return (CustomerTypeAddRq.CustomerTypeAdd.Name != null); }

    }
}