using System.Collections.Generic;
using System.Xml.Serialization;
using Brambleberry.Quickbooks.Models.Base;

namespace Brambleberry.Quickbooks.Models
{

    [XmlRoot(ElementName = "QBXML")]
    public partial class QBXML : QBSerialization 
    {
        [XmlElement(ElementName = "QBXMLMsgsRs")]
        public QBXMLMsgsRs QBXMLMsgsRs;
        public bool ShouldSerializeQBXMLMsgsRs()
        {
            return (QBXMLMsgsRs != null);
        }

        [XmlElement(ElementName = "QBXMLMsgsRq")]
        public List<QBXMLMsgsRq> QBXMLMsgsRq;
        public bool ShouldSerializeQBXMLMsgsRq()
        {
            return (QBXMLMsgsRq.Count > 0);
        }


        //protected void SetRequestIds()
        //{
        //    int _RequestId = 1;
        //    int _Count = this.QBXMLMsgsRq.Count;
        //    for (int x = 0; x < _Count; x++)
        //    {
        //        if (this.QBXMLMsgsRq[x].ShouldSerializeAccountAddRq())
        //        {
        //            this.QBXMLMsgsRq[x].AccountAddRq.requestID = _RequestId;
        //            _RequestId += 1;
        //        }

        //        if (this.QBXMLMsgsRq[x].ShouldSerializeBillAddRq())
        //        {
        //            this.QBXMLMsgsRq[x].BillAddRq.requestID = _RequestId;
        //            _RequestId += 1;
        //        }



        //        for (int y = 0; y < this.QBXMLMsgsRq[x].CustomerAddRq.Count; y++)
        //        {
        //            this.QBXMLMsgsRq[x].CustomerAddRq[y].requestID = _RequestId;
        //            _RequestId += 1;
        //        }

        //        for (int y = 0; y < this.QBXMLMsgsRq[x].InvoiceAddRq.Count; y++)
        //        {
        //            this.QBXMLMsgsRq[x].InvoiceAddRq[y].requestID = _RequestId;
        //            _RequestId += 1;
        //        }

        //        for (int y = 0; y < this.QBXMLMsgsRq[x].ItemDiscountAddRq.Count; y++)
        //        {
        //            this.QBXMLMsgsRq[x].ItemDiscountAddRq[y].requestID = _RequestId;
        //            _RequestId += 1;
        //        }

        //        for (int y = 0; y < this.QBXMLMsgsRq[x].ItemInventoryAddRq.Count; y++)
        //        {
        //            this.QBXMLMsgsRq[x].ItemInventoryAddRq[y].requestID = _RequestId;
        //            _RequestId += 1;
        //        }

        //        for (int y = 0; y < this.QBXMLMsgsRq[x].ItemNonInventoryAddRq.Count; y++)
        //        {
        //            this.QBXMLMsgsRq[x].ItemNonInventoryAddRq[y].requestID = _RequestId;
        //            _RequestId += 1;
        //        }

        //        for (int y = 0; y < this.QBXMLMsgsRq[x].ItemSalesTaxAddRq.Count; y++)
        //        {
        //            this.QBXMLMsgsRq[x].ItemSalesTaxAddRq[y].requestID = _RequestId;
        //            _RequestId += 1;
        //        }

        //        for (int y = 0; y < this.QBXMLMsgsRq[x].PaymentMethodAddRq.Count; y++)
        //        {
        //            this.QBXMLMsgsRq[x].PaymentMethodAddRq[y].requestID = _RequestId;
        //            _RequestId += 1;
        //        }

        //        for (int y = 0; y < this.QBXMLMsgsRq[x].PurchaseOrderAddRq.Count; y++)
        //        {
        //            this.QBXMLMsgsRq[x].PurchaseOrderAddRq[y].requestID = _RequestId;
        //            _RequestId += 1;
        //        }

        //        for (int y = 0; y < this.QBXMLMsgsRq[x].ReceivePaymentAddRq.Count; y++)
        //        {
        //            this.QBXMLMsgsRq[x].ReceivePaymentAddRq[y].requestID = _RequestId;
        //            _RequestId += 1;
        //        }

        //        for (int y = 0; y < this.QBXMLMsgsRq[x].SalesOrderAddRq.Count; y++)
        //        {
        //            this.QBXMLMsgsRq[x].SalesOrderAddRq[y].requestID = _RequestId;
        //            _RequestId += 1;
        //        }

        //        for (int y = 0; y < this.QBXMLMsgsRq[x].SalesReceiptAddRq.Count; y++)
        //        {
        //            this.QBXMLMsgsRq[x].SalesReceiptAddRq[y].requestID = _RequestId;
        //            _RequestId += 1;
        //        }

        //        for (int y = 0; y < this.QBXMLMsgsRq[x].SalesTaxCodeAddRq.Count; y++)
        //        {
        //            this.QBXMLMsgsRq[x].SalesTaxCodeAddRq[y].requestID = _RequestId;
        //            _RequestId += 1;
        //        }

        //        for (int y = 0; y < this.QBXMLMsgsRq[x].ShipMethodAddRq.Count; y++)
        //        {
        //            this.QBXMLMsgsRq[x].ShipMethodAddRq[y].requestID = _RequestId;
        //            _RequestId += 1;
        //        }

        //        for (int y = 0; y < this.QBXMLMsgsRq[x].StandardTermsAddRq.Count; y++)
        //        {
        //            this.QBXMLMsgsRq[x].StandardTermsAddRq[y].requestID = _RequestId;
        //            _RequestId += 1;
        //        }

        //        for (int y = 0; y < this.QBXMLMsgsRq[x].VendorAddRq.Count; y++)
        //        {
        //            this.QBXMLMsgsRq[x].VendorAddRq[y].requestID = _RequestId;
        //            _RequestId += 1;
        //        }

        //        for (int y = 0; y < this.QBXMLMsgsRq[x].ClassAddRq.Count; y++)
        //        {
        //            this.QBXMLMsgsRq[x].ClassAddRq[y].requestID = _RequestId;
        //            _RequestId += 1;
        //        }

        //    }

        //}


    }

}
