using System.Collections.Generic;
using Brambleberry.Quickbooks.Models;

namespace Brambleberry.Quickbooks
{

    public partial class QBProcess
    {
        public static void PullLists()
        {
            // reset the master request list (just in case)
            QBList.QBXMLMsgsRq = new List<QBXMLMsgsRq>();

            QBXMLMsgsRq _Rq1 = new QBXMLMsgsRq();
            QBXMLMsgsRq _Rq2a = new QBXMLMsgsRq();
            QBXMLMsgsRq _Rq2b = new QBXMLMsgsRq();
            QBXMLMsgsRq _Rq2c = new QBXMLMsgsRq();
            QBXMLMsgsRq _Rq2d = new QBXMLMsgsRq();
            QBXMLMsgsRq _Rq2e = new QBXMLMsgsRq();
            QBXMLMsgsRq _Rq2f = new QBXMLMsgsRq();
            QBXMLMsgsRq _Rq2g = new QBXMLMsgsRq();
            QBXMLMsgsRq _Rq3 = new QBXMLMsgsRq();
            QBXMLMsgsRq _Rq4 = new QBXMLMsgsRq();
            QBXMLMsgsRq _Rq5 = new QBXMLMsgsRq();
            QBXMLMsgsRq _Rq6 = new QBXMLMsgsRq();
            QBXMLMsgsRq _Rq7 = new QBXMLMsgsRq();
            QBXMLMsgsRq _Rq8 = new QBXMLMsgsRq();
            QBXMLMsgsRq _Rq9 = new QBXMLMsgsRq();
            QBXMLMsgsRq _Rq10 = new QBXMLMsgsRq();
            QBXMLMsgsRq _Rq11 = new QBXMLMsgsRq();
            QBXMLMsgsRq _Rq12 = new QBXMLMsgsRq();
            QBXMLMsgsRq _Rq13 = new QBXMLMsgsRq();
            QBXMLMsgsRq _Rq14 = new QBXMLMsgsRq();
            QBXMLMsgsRq _Rq15 = new QBXMLMsgsRq();
            QBXMLMsgsRq _Rq16 = new QBXMLMsgsRq();
            QBXMLMsgsRq _Rq17 = new QBXMLMsgsRq();
            QBXMLMsgsRq _Rq18 = new QBXMLMsgsRq();
            QBXMLMsgsRq _Rq19 = new QBXMLMsgsRq();
            QBXMLMsgsRq _Rq20 = new QBXMLMsgsRq();
            QBXMLMsgsRq _Rq21 = new QBXMLMsgsRq();
            QBXMLMsgsRq _Rq22 = new QBXMLMsgsRq();
            QBXMLMsgsRq _Rq23 = new QBXMLMsgsRq();
            QBXMLMsgsRq _Rq24 = new QBXMLMsgsRq();
            QBXMLMsgsRq _Rq25 = new QBXMLMsgsRq();
            QBXMLMsgsRq _Rq26 = new QBXMLMsgsRq();
            QBXMLMsgsRq _Rq27 = new QBXMLMsgsRq();
            QBXMLMsgsRq _Rq28 = new QBXMLMsgsRq();
            QBXMLMsgsRq _Rq29 = new QBXMLMsgsRq();

            // set up queries
            _Rq1.AccountQueryRq.Add(new AccountQueryRq());

            // break up iteminventory requests to reduce possibility of timeout errors
            ItemInventoryQueryRq _iq1 = new ItemInventoryQueryRq();
            _iq1.NameRangeFilter.ToName = "c";
            _Rq2a.ItemInventoryQueryRq.Add(_iq1);

            ItemInventoryQueryRq _iq2 = new ItemInventoryQueryRq();
            _iq2.NameRangeFilter.FromName = "c";
            _iq2.NameRangeFilter.ToName = "g";
            _Rq2b.ItemInventoryQueryRq.Add(_iq2);

            ItemInventoryQueryRq _iq3 = new ItemInventoryQueryRq();
            _iq3.NameRangeFilter.FromName = "g";
            _iq3.NameRangeFilter.ToName = "j";
            _Rq2c.ItemInventoryQueryRq.Add(_iq3);
            
            ItemInventoryQueryRq _iq4 = new ItemInventoryQueryRq();
            _iq4.NameRangeFilter.FromName = "j";
            _iq4.NameRangeFilter.ToName = "m";
            _Rq2d.ItemInventoryQueryRq.Add(_iq4);
            
            ItemInventoryQueryRq _iq5 = new ItemInventoryQueryRq();
            _iq5.NameRangeFilter.FromName = "m";
            _iq5.NameRangeFilter.ToName = "p";
            _Rq2e.ItemInventoryQueryRq.Add(_iq5);

            ItemInventoryQueryRq _iq6 = new ItemInventoryQueryRq();
            _iq6.NameRangeFilter.FromName = "p";
            _iq6.NameRangeFilter.ToName = "s";
            _Rq2f.ItemInventoryQueryRq.Add(_iq6);

            ItemInventoryQueryRq _iq7 = new ItemInventoryQueryRq();
            _iq7.NameRangeFilter.FromName = "s";
            _Rq2g.ItemInventoryQueryRq.Add(_iq7);

            //_Rq2.ItemInventoryQueryRq.Add(new ItemInventoryQueryRq());

            // pull remaining lists
            _Rq3.ItemNonInventoryQueryRq.Add(new ItemNonInventoryQueryRq());
            _Rq4.ItemOtherChargeQueryRq.Add(new ItemOtherChargeQueryRq());
            _Rq5.ItemDiscountQueryRq.Add(new ItemDiscountQueryRq());
            _Rq6.ItemServiceQueryRq.Add(new ItemServiceQueryRq());
            _Rq7.ItemSalesTaxQueryRq.Add(new ItemSalesTaxQueryRq());
            _Rq8.ItemGroupQueryRq.Add(new ItemGroupQueryRq());
            _Rq9.VendorQueryRq.Add(new VendorQueryRq());

            // break customer list response into multiple queries so response doesn't timeout
            CustomerQueryRq _q1  = new CustomerQueryRq();
            CustomerQueryRq _q2  = new CustomerQueryRq();
            CustomerQueryRq _q3  = new CustomerQueryRq();
            CustomerQueryRq _q4  = new CustomerQueryRq();
            CustomerQueryRq _q5  = new CustomerQueryRq();
            CustomerQueryRq _q6  = new CustomerQueryRq();
            CustomerQueryRq _q7  = new CustomerQueryRq();
            CustomerQueryRq _q8  = new CustomerQueryRq();
            CustomerQueryRq _q9  = new CustomerQueryRq();
            CustomerQueryRq _q10 = new CustomerQueryRq();
            CustomerQueryRq _q11 = new CustomerQueryRq();
            CustomerQueryRq _q12 = new CustomerQueryRq();
            CustomerQueryRq _q13 = new CustomerQueryRq();

            _q1.NameRangeFilter.ToName = "b";

            _q2.NameRangeFilter.FromName = "b";
            _q2.NameRangeFilter.ToName = "d";

            _q3.NameRangeFilter.FromName = "d";
            _q3.NameRangeFilter.ToName = "f";

            _q4.NameRangeFilter.FromName = "f";
            _q4.NameRangeFilter.ToName = "h";
            
            _q5.NameRangeFilter.FromName = "h";
            _q5.NameRangeFilter.ToName = "j";
            
            _q6.NameRangeFilter.FromName = "j";
            _q6.NameRangeFilter.ToName = "l";
            
            _q7.NameRangeFilter.FromName = "l";
            _q7.NameRangeFilter.ToName = "n";
            
            _q8.NameRangeFilter.FromName = "n";
            _q8.NameRangeFilter.ToName = "p";

            _q9.NameRangeFilter.FromName = "p";
            _q9.NameRangeFilter.ToName = "r";

            _q10.NameRangeFilter.FromName = "r";
            _q10.NameRangeFilter.ToName = "t";
            
            _q11.NameRangeFilter.FromName = "t";
            _q11.NameRangeFilter.ToName = "v";
            
            _q12.NameRangeFilter.FromName = "v";
            _q12.NameRangeFilter.ToName = "x";

            _q13.NameRangeFilter.FromName = "x";

            _Rq10.CustomerQueryRq.Add(_q1);
            _Rq11.CustomerQueryRq.Add(_q2);
            _Rq12.CustomerQueryRq.Add(_q3);
            _Rq13.CustomerQueryRq.Add(_q4);
            _Rq14.CustomerQueryRq.Add(_q5);
            _Rq15.CustomerQueryRq.Add(_q6);
            _Rq16.CustomerQueryRq.Add(_q7);
            _Rq17.CustomerQueryRq.Add(_q8);
            _Rq18.CustomerQueryRq.Add(_q9);
            _Rq19.CustomerQueryRq.Add(_q10);
            _Rq20.CustomerQueryRq.Add(_q11);
            _Rq21.CustomerQueryRq.Add(_q12);
            _Rq22.CustomerQueryRq.Add(_q13);

            _Rq23.PaymentMethodQueryRq.Add(new PaymentMethodQueryRq());
            _Rq24.SalesTaxCodeQueryRq.Add(new SalesTaxCodeQueryRq());
            _Rq25.ShipMethodQueryRq.Add(new ShipMethodQueryRq());
            _Rq26.StandardTermsQueryRq.Add(new StandardTermsQueryRq());
            _Rq27.ClassQueryRq.Add(new ClassQueryRq());
            _Rq28.SalesRepQueryRq.Add(new SalesRepQueryRq());
            _Rq29.CustomerTypeQueryRq.Add(new CustomerTypeQueryRq());

            QBList.QBXMLMsgsRq.Add(_Rq1);

            QBList.QBXMLMsgsRq.Add(_Rq2a);
            QBList.QBXMLMsgsRq.Add(_Rq2b);
            QBList.QBXMLMsgsRq.Add(_Rq2c);
            QBList.QBXMLMsgsRq.Add(_Rq2d);
            QBList.QBXMLMsgsRq.Add(_Rq2e);
            QBList.QBXMLMsgsRq.Add(_Rq2f);
            QBList.QBXMLMsgsRq.Add(_Rq2g);

            QBList.QBXMLMsgsRq.Add(_Rq3);
            QBList.QBXMLMsgsRq.Add(_Rq4);
            QBList.QBXMLMsgsRq.Add(_Rq5);
            QBList.QBXMLMsgsRq.Add(_Rq6);
            QBList.QBXMLMsgsRq.Add(_Rq7);
            QBList.QBXMLMsgsRq.Add(_Rq8);
            QBList.QBXMLMsgsRq.Add(_Rq9);
            QBList.QBXMLMsgsRq.Add(_Rq10);
            QBList.QBXMLMsgsRq.Add(_Rq11);
            QBList.QBXMLMsgsRq.Add(_Rq12);
            QBList.QBXMLMsgsRq.Add(_Rq13);
            QBList.QBXMLMsgsRq.Add(_Rq14);
            QBList.QBXMLMsgsRq.Add(_Rq15);
            QBList.QBXMLMsgsRq.Add(_Rq16);
            QBList.QBXMLMsgsRq.Add(_Rq17);
            QBList.QBXMLMsgsRq.Add(_Rq18);
            QBList.QBXMLMsgsRq.Add(_Rq19);
            QBList.QBXMLMsgsRq.Add(_Rq20);
            QBList.QBXMLMsgsRq.Add(_Rq21);
            QBList.QBXMLMsgsRq.Add(_Rq22);
            QBList.QBXMLMsgsRq.Add(_Rq23);
            QBList.QBXMLMsgsRq.Add(_Rq24);
            QBList.QBXMLMsgsRq.Add(_Rq25);
            QBList.QBXMLMsgsRq.Add(_Rq26);
            QBList.QBXMLMsgsRq.Add(_Rq27);
            QBList.QBXMLMsgsRq.Add(_Rq28);
            QBList.QBXMLMsgsRq.Add(_Rq29);

            // master QBList of query requests is populated, exit and return
            return;
        }


    }
}