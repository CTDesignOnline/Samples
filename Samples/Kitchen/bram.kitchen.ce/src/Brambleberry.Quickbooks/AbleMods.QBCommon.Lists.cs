using System.Collections.Generic;
using Brambleberry.Quickbooks.Models;

namespace Brambleberry.Quickbooks
{

    public static class QBList
    {
        public static List<QBXMLMsgsRq> QBXMLMsgsRq;
        public static QBXMLMsgsRs QBXMLMsgsRs;
        public static void Initialize()
        {
            QBList.QBXMLMsgsRq = new List<QBXMLMsgsRq>();
            QBList.QBXMLMsgsRs = new QBXMLMsgsRs();
        }

        public static bool CustomerAddRqExists(string _Name)
        {
            foreach (QBXMLMsgsRq _Rq in QBList.QBXMLMsgsRq)
                if (_Rq.CustomerAddRq.CustomerAdd.Name == _Name) return true;
            return false;
        }

        public static bool AccountAddRqExists(string _Name)
        {
            foreach (QBXMLMsgsRq _Rq in QBList.QBXMLMsgsRq)
                if (_Rq.AccountAddRq.AccountAdd.Name == _Name) return true;
            return false;
        }

        public static bool ItemDiscountAddRqExists(string _Name)
        {
            foreach (QBXMLMsgsRq _Rq in QBList.QBXMLMsgsRq)
                if (_Rq.ItemDiscountAddRq.ItemDiscountAdd.Name == _Name) return true;
            return false;
        }

        public static bool ItemInventoryAddRqExists(string _Name)
        {
            foreach (QBXMLMsgsRq _Rq in QBList.QBXMLMsgsRq)
                if (_Rq.ItemInventoryAddRq.ItemInventoryAdd.Name == _Name) return true;
            return false;

        }

        public static bool ItemNonInventoryAddRqExists(string _Name)
        {
            foreach (QBXMLMsgsRq _Rq in QBList.QBXMLMsgsRq)
                if (_Rq.ItemNonInventoryAddRq.ItemNonInventoryAdd.Name == _Name) return true;
            return false;

        }
        public static bool ItemSalesTaxAddRqExists(string _Name)
        {
            foreach (QBXMLMsgsRq _Rq in QBList.QBXMLMsgsRq)
                if (_Rq.ItemSalesTaxAddRq.ItemSalesTaxAdd.Name == _Name) return true;
            return false;

        }

        public static bool SalesTaxCodeAddRqExists(string _Name)
        {
            foreach (QBXMLMsgsRq _Rq in QBList.QBXMLMsgsRq)
                if (_Rq.SalesTaxCodeAddRq.SalesTaxCodeAdd.Name == _Name) return true;
            return false;

        }

        public static bool ShipMethodAddRqExists(string _Name)
        {
            foreach (QBXMLMsgsRq _Rq in QBList.QBXMLMsgsRq)
                if (_Rq.ShipMethodAddRq.ShipMethodAdd.Name == _Name) return true;
            return false;

        }

        public static bool StandardTermsAddRqExists(string _Name)
        {
            foreach (QBXMLMsgsRq _Rq in QBList.QBXMLMsgsRq)
                if (_Rq.StandardTermsAddRq.StandardTermsAdd.Name == _Name) return true;
            return false;
        }

        public static bool VendorAddRqExists(string _Name)
        {
            foreach (QBXMLMsgsRq _Rq in QBList.QBXMLMsgsRq)
                if (_Rq.VendorAddRq.VendorAdd.Name == _Name) return true;
            return false;
        }

        public static bool PaymentMethodAddRqExists(string _Name)
        {
            foreach (QBXMLMsgsRq _Rq in QBList.QBXMLMsgsRq)
                if (_Rq.PaymentMethodAddRq.PaymentMethodAdd.Name == _Name) return true;
            return false;
        }
    }
}
