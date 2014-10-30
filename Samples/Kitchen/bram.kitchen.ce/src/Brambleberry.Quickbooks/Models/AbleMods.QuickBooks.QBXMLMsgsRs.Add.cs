using System.Xml.Serialization;
using Brambleberry.Quickbooks.Models.Base;

namespace Brambleberry.Quickbooks.Models

{
    public partial class QBXMLMsgsRs : QBSerialization 
    {
        [XmlElement(ElementName = "AccountAddRs")]
        public AccountAddRs AccountAddRs = new AccountAddRs();

        [XmlElement(ElementName = "ItemInventoryAddRs")]
        public ItemInventoryAddRs ItemInventoryAddRs = new ItemInventoryAddRs();

        [XmlElement(ElementName = "ItemNonInventoryAddRs")]
        public ItemNonInventoryAddRs ItemNonInventoryAddRs = new ItemNonInventoryAddRs();

        [XmlElement(ElementName = "ItemGroupAddRs")]
        public ItemGroupAddRs ItemGroupAddRs = new ItemGroupAddRs();

        [XmlElement(ElementName = "ItemServiceAddRs")]
        public ItemServiceAddRs ItemServiceAddRs = new ItemServiceAddRs();

        [XmlElement(ElementName = "ItemOtherChargeAddRs")]
        public ItemOtherChargeAddRs ItemOtherChargeAddRs = new ItemOtherChargeAddRs();

        [XmlElement(ElementName = "ItemDiscountAddRs")]
        public ItemDiscountAddRs ItemDiscountAddRs = new ItemDiscountAddRs();

        [XmlElement(ElementName = "StandardTermsAddRs")]
        public StandardTermsAddRs StandardTermsAddRs = new StandardTermsAddRs();

        [XmlElement(ElementName = "ItemSalesTaxAddRs")]
        public ItemSalesTaxAddRs ItemSalesTaxAddRs = new ItemSalesTaxAddRs();

        [XmlElement(ElementName = "PaymentMethodAddRs")]
        public PaymentMethodAddRs PaymentMethodAddRs = new PaymentMethodAddRs();

        [XmlElement(ElementName = "SalesTaxCodeAddRs")]
        public SalesTaxCodeAddRs SalesTaxCodeAddRs = new SalesTaxCodeAddRs();

        [XmlElement(ElementName = "VendorAddRs")]
        public VendorAddRs VendorAddRs = new VendorAddRs();

        [XmlElement(ElementName = "ShipMethodAddRs")]
        public ShipMethodAddRs ShipMethodAddRs = new ShipMethodAddRs();

        [XmlElement(ElementName = "CustomerAddRs")]
        public CustomerAddRs CustomerAddRs = new CustomerAddRs();

        [XmlElement(ElementName = "ClassAddRs")]
        public ClassAddRs ClassAddRs = new ClassAddRs();

        [XmlElement(ElementName = "ReceivePaymentAddRs")]
        public ReceivePaymentAddRs ReceivePaymentAddRs = new ReceivePaymentAddRs();

        [XmlElement(ElementName = "InvoiceAddRs")]
        public InvoiceAddRs InvoiceAddRs = new InvoiceAddRs();

        [XmlElement(ElementName = "SalesReceiptAddRs")]
        public SalesReceiptAddRs SalesReceiptAddRs = new SalesReceiptAddRs();

        [XmlElement(ElementName = "BillAddRs")]
        public BillAddRs BillAddRs = new BillAddRs();

        [XmlElement(ElementName = "SalesRepAddRs")]
        public SalesRepAddRs SalesRepAddRs = new SalesRepAddRs();

        [XmlElement(ElementName = "CustomerTypeAddRs")]
        public CustomerTypeAddRs CustomerTypeAddRs = new CustomerTypeAddRs();

        [XmlElement(ElementName = "SalesOrderAddRs")]
        public SalesOrderAddRs SalesOrderAddRs = new SalesOrderAddRs();

        [XmlElement(ElementName = "PurchaseOrderAddRs")]
        public PurchaseOrderAddRs PurchasOrderAddRs = new PurchaseOrderAddRs();

        [XmlElement(ElementName = "CustomerModRs")]
        public CustomerModRs CustomerModRs = new CustomerModRs();

        [XmlIgnore]
        public QBResponseStatus ResponseStatus
        {
            get
            {
                // check QUERY responses
                if (this.AccountQueryRs.HasResponse) return this.AccountQueryRs.StatusMsg;
                if (this.ClassQueryRs.HasResponse) return this.ClassQueryRs.StatusMsg;
                if (this.CustomerQueryRs.HasResponse) return this.CustomerQueryRs.StatusMsg;
                if (this.ItemDiscountQueryRs.HasResponse) return this.ItemDiscountQueryRs.StatusMsg;
                if (this.ItemGroupQueryRs.HasResponse) return this.ItemGroupQueryRs.StatusMsg;
                if (this.ItemInventoryQueryRs.HasResponse) return this.ItemInventoryQueryRs.StatusMsg;
                if (this.ItemNonInventoryQueryRs.HasResponse) return this.ItemNonInventoryQueryRs.StatusMsg;
                if (this.ItemOtherChargeQueryRs.HasResponse) return this.ItemOtherChargeQueryRs.StatusMsg;
                if (this.ItemSalesTaxQueryRs.HasResponse) return this.ItemSalesTaxQueryRs.StatusMsg;
                if (this.ItemServiceQueryRs.HasResponse) return this.ItemServiceQueryRs.StatusMsg;
                if (this.PaymentMethodQueryRs.HasResponse) return this.PaymentMethodQueryRs.StatusMsg;
                if (this.SalesTaxCodeQueryRs.HasResponse) return this.SalesTaxCodeQueryRs.StatusMsg;
                if (this.ShipMethodQueryRs.HasResponse) return this.ShipMethodQueryRs.StatusMsg;
                if (this.StandardTermsQueryRs.HasResponse) return this.StandardTermsQueryRs.StatusMsg;
                if (this.VendorQueryRs.HasResponse) return this.VendorQueryRs.StatusMsg;
                if (this.SalesRepQueryRs.HasResponse) return this.SalesRepQueryRs.StatusMsg;
                if (this.CustomerTypeQueryRs.HasResponse) return this.CustomerTypeQueryRs.StatusMsg;

                // check ADD responses
                if (this.AccountAddRs.HasResponse) return this.AccountAddRs.StatusMsg;
                if (this.ClassAddRs.HasResponse) return this.ClassAddRs.StatusMsg;
                if (this.CustomerAddRs.HasResponse) return this.CustomerAddRs.StatusMsg;
                if (this.ItemDiscountAddRs.HasResponse) return this.ItemDiscountAddRs.StatusMsg;
                if (this.ItemGroupAddRs.HasResponse) return this.ItemGroupAddRs.StatusMsg;
                if (this.ItemInventoryAddRs.HasResponse) return this.ItemInventoryAddRs.StatusMsg;
                if (this.ItemNonInventoryAddRs.HasResponse) return this.ItemNonInventoryAddRs.StatusMsg;
                if (this.ItemOtherChargeAddRs.HasResponse) return this.ItemOtherChargeAddRs.StatusMsg;
                if (this.ItemSalesTaxAddRs.HasResponse) return this.ItemSalesTaxAddRs.StatusMsg;
                if (this.ItemServiceAddRs.HasResponse) return this.ItemServiceAddRs.StatusMsg;
                if (this.PaymentMethodAddRs.HasResponse) return this.PaymentMethodAddRs.StatusMsg;
                if (this.ReceivePaymentAddRs.HasResponse) return this.ReceivePaymentAddRs.StatusMsg;
                if (this.SalesTaxCodeAddRs.HasResponse) return this.SalesTaxCodeAddRs.StatusMsg;
                if (this.ShipMethodAddRs.HasResponse) return this.ShipMethodAddRs.StatusMsg;
                if (this.StandardTermsAddRs.HasResponse) return this.StandardTermsAddRs.StatusMsg;
                if (this.VendorAddRs.HasResponse) return this.VendorAddRs.StatusMsg;
                if (this.InvoiceAddRs.HasResponse) return this.InvoiceAddRs.StatusMsg;
                if (this.SalesReceiptAddRs.HasResponse) return this.SalesReceiptAddRs.StatusMsg;
                if (this.BillAddRs.HasResponse) return this.BillAddRs.StatusMsg;
                if (this.SalesRepAddRs.HasResponse) return this.SalesRepAddRs.StatusMsg;
                if (this.CustomerTypeAddRs.HasResponse) return this.CustomerTypeAddRs.StatusMsg;
                if (this.SalesOrderAddRs.HasResponse) return this.SalesOrderAddRs.StatusMsg;
                if (this.PurchasOrderAddRs.HasResponse) return this.PurchasOrderAddRs.StatusMsg;

                // check MOD responses
                if (this.CustomerModRs.HasResponse) return this.CustomerModRs.StatusMsg;

                // all responses checked, something was missed.  Make a fake response so it can 
                // be parsed in the calling routine
                QBResponseStatus _RetVal = new QBResponseStatus();
                _RetVal.StatusCode = "9999";
                return _RetVal;
            }

        }

    }
}
