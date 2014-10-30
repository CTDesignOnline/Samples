namespace Brambleberry.Quickbooks.Settings
{
    public class QBSettings
    {

        public string ClassRef { get; set; }
        public string ARAccount { get; set; }
        public string ShippingItemId { get; set; }
        public string HandlingItemId { get; set; }
        public string GiftWrapItemId { get; set; }
        public string QBPmtARAccount { get; set; }
        public string APAccount { get; set; }
        public string MissingVendor { get; set; }
        public bool GenPO { get; set; }
        public bool GenBill { get; set; }
        public string InvoiceMode { get; set; }
        public int RespectPayShip = 0;
        public int OrderLimit = 0;
        public string ItemAssetAcct { get; set; }
        public string COGSAcctRef { get; set; }
        public string IncomeAcctRef { get; set; }
        public string DebugMode { get; set; }
        public string DataMode { get; set; }
        public bool SetExported { get; set; }
        public bool AddOrderNote { get; set; }
        public bool UpdateInStockFromQB { get; set; }
        public string SalesTaxVendor { get; set; }
        public string CustomerIdMode { get; set; }
        public string InvoiceDiscAcct { get; set; }
        public string ConnectAs { get; set; }
        public string DefCustomerType { get; set; }
        public string DefSalesRep { get; set; }
        public bool UpdateCustomerCCInfo { get; set; }
        public bool UseShipDateAsInvoiceDate { get; set; }
    }
}
