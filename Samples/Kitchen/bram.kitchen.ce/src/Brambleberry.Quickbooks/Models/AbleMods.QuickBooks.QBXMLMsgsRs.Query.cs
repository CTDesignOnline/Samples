using System.Xml.Serialization;
using Brambleberry.Quickbooks.Models.Base;

namespace Brambleberry.Quickbooks.Models
{
    [XmlRoot(ElementName = "QBXMLMsgsRs")]
    public partial class QBXMLMsgsRs : QBSerialization 
    {
        // QUERY response methods
        [XmlElement(ElementName = "AccountQueryRs")]
        public AccountQueryRs AccountQueryRs = new AccountQueryRs();

        [XmlElement(ElementName = "ItemInventoryQueryRs")]
        public ItemInventoryQueryRs ItemInventoryQueryRs = new ItemInventoryQueryRs();

        [XmlElement(ElementName = "ItemNonInventoryQueryRs")]
        public ItemNonInventoryQueryRs ItemNonInventoryQueryRs = new ItemNonInventoryQueryRs();

        [XmlElement(ElementName = "ItemGroupQueryRs")]
        public ItemGroupQueryRs ItemGroupQueryRs = new ItemGroupQueryRs();

        [XmlElement(ElementName = "ItemOtherChargeQueryRs")]
        public ItemOtherChargeQueryRs ItemOtherChargeQueryRs = new ItemOtherChargeQueryRs();

        [XmlElement(ElementName = "ItemServiceQueryRs")]
        public ItemServiceQueryRs ItemServiceQueryRs = new ItemServiceQueryRs();

        [XmlElement(ElementName = "ItemDiscountQueryRs")]
        public ItemDiscountQueryRs ItemDiscountQueryRs = new ItemDiscountQueryRs();

        [XmlElement(ElementName = "StandardTermsQueryRs")]
        public StandardTermsQueryRs StandardTermsQueryRs = new StandardTermsQueryRs();

        [XmlElement(ElementName = "ItemSalesTaxQueryRs")]
        public ItemSalesTaxQueryRs ItemSalesTaxQueryRs = new ItemSalesTaxQueryRs();

        [XmlElement(ElementName = "PaymentMethodQueryRs")]
        public PaymentMethodQueryRs PaymentMethodQueryRs = new PaymentMethodQueryRs();

        [XmlElement(ElementName = "SalesTaxCodeQueryRs")]
        public SalesTaxCodeQueryRs SalesTaxCodeQueryRs = new SalesTaxCodeQueryRs();

        [XmlElement(ElementName = "VendorQueryRs")]
        public VendorQueryRs VendorQueryRs = new VendorQueryRs();

        [XmlElement(ElementName = "ShipMethodQueryRs")]
        public ShipMethodQueryRs ShipMethodQueryRs = new ShipMethodQueryRs();

        [XmlElement(ElementName = "CustomerQueryRs")]
        public CustomerQueryRs CustomerQueryRs = new CustomerQueryRs();

        [XmlElement(ElementName = "ClassQueryRs")]
        public ClassQueryRs ClassQueryRs = new ClassQueryRs();

        [XmlElement(ElementName = "SalesRepQueryRs")]
        public SalesRepQueryRs SalesRepQueryRs = new SalesRepQueryRs();

        [XmlElement(ElementName = "CustomerTypeQueryRs")]
        public CustomerTypeQueryRs CustomerTypeQueryRs = new CustomerTypeQueryRs();
    }
}