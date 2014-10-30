using System;
using System.Collections.Generic;
using Brambleberry.Quickbooks.Models;

namespace Brambleberry.Quickbooks
{

    public partial class QBProcess
    {

        public static void StockUpdate()
        {
            // check our stock update mode setting
            if (_configSettings.UpdateInStockFromQB)
                QBToAC7Update();
        }

        private static void QBToAC7Update()
        {
            // Update AC7 with stock values from QuickBooks items
            // first grab the list of QB items from the response message list
            List<ItemInventoryRet> _QBItems = QBList.QBXMLMsgsRs.ItemInventoryQueryRs.ItemInventoryRet;

            // loop through each item and determine 
            foreach (ItemInventoryRet _QBItem in _QBItems)
            {
                // Find AC7 product(s) for the SKU
                IList<Product> _Products = ProductDataSource.FindProducts(String.Empty, _QBItem.Name.Trim(), 0, 0, 0);

                // loop through each product found for this SKU
                AbleContext.Current.Database.BeginTransaction();

                foreach (Product _Product in _Products)
                {
                    // if stock level is different, update it and save AC7 product
                    if (_Product.InStock != _QBItem.QuantityOnHand)
                    {
                        _Product.InStock = AlwaysConvert.ToInt(_QBItem.QuantityOnHand);
                        _Product.Save();
                    }
                }
                AbleContext.Current.Database.CommitTransaction();

                // find AC7 variants for the SKU
                NHibernate.ICriteria _Criteria = NHibernateHelper.CreateCriteria<ProductVariant>();
                _Criteria.Add(NHibernate.Criterion.Restrictions.Eq("Sku", _QBItem.Name.Trim()));
                
                // execute your query
                IList<ProductVariant> _Variants = ProductVariantDataSource.LoadForCriteria(_Criteria);

                // loop through each variant found for this SKU
                AbleContext.Current.Database.BeginTransaction();

                foreach (ProductVariant _Variant in _Variants)
                {
                    // if stock level is different, update it and save AC7 variant
                    if (_Variant.InStock != _QBItem.QuantityOnHand)
                    {
                        _Variant.InStock = AlwaysConvert.ToInt(_QBItem.QuantityOnHand);
                        _Variant.Save();
                    }
                }
                AbleContext.Current.Database.CommitTransaction();

            }
        }
    }
}
