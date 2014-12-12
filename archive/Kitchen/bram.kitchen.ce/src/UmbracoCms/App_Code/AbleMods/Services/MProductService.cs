using System;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Linq;
using System.Web.Services.Description;
using AbleMods.Api;
using AbleMods.Api.Models;
using Merchello.Core;
using Merchello.Core.Models;
using Merchello.Core.Services;

namespace AbleMods.Services
{
    public static class MProductService
    {

        private static ProductService _service;

        static MProductService()
        {
            _service = new ProductService();
        }

        public static Guid MakeMerchelloProduct(UProduct product)
        {
            // set up return value
            Guid retVal;

            // make sure we have a unique sku
            string sku = MakeUniqueSku(product.Sku);

            if (product.Name.Contains("Droppers With Suction Bulb"))
            {
                string dummy = string.Empty;
            }


            IProduct mProduct = _service.CreateProduct(product.Name, sku, product.Price);
            
            mProduct.CostOfGoods = product.COGS;
            mProduct.Shippable = true;
            mProduct.Taxable = true;

            if (product.HasOptions)
            {
                foreach (UOption uOption in product.Options)
                {
                    // make new merchello option
                    ProductOption mOption = new ProductOption(uOption.Name.Trim());

                    // load able option choices into merchello option
                    IList<string> skuValues = new List<string>();
                    foreach (UOptionChoice uChoice in uOption.Choices)
                    {

                        // make sure we have a unique sku
                        string tempSku = string.IsNullOrEmpty(uChoice.SkuModifier) ? uChoice.OptionChoiceId.ToString() : uChoice.SkuModifier;
                        int skuCounter = 1;
                        while (skuValues.Contains(tempSku) || _service.SkuExists(tempSku))
                        {
                            tempSku += "-" + skuCounter.ToString();
                            skuCounter++;
                        }

                        string newSku = tempSku; 

                        // add attribute as new option choice to this option
                        mOption.Choices.Add(new ProductAttribute(uChoice.Name.Trim(), newSku));
                        skuValues.Add(newSku);
                    }

                    // add new option to merchello product
                    mProduct.ProductOptions.Add(mOption);
                }
            }

            // save the merchello product
            _service.Save(mProduct);

            IEnumerable<IProductVariant> newVariants = MerchelloContext.Current.Services.ProductVariantService.GetProductVariantsThatCanBeCreated(mProduct);

            MerchelloContext.Current.Services.ProductVariantService.Save(newVariants);


            // set prices on variants now that they are constructed
            foreach (IProductVariant pVariant in newVariants)
            {
                // find the able variant for this choice and get the price
                string originalOptionName = pVariant.Name.Replace(product.Name + " - ", string.Empty);
                originalOptionName = originalOptionName.Trim();
                UProductVariant uVariant = UProductVariantRepository.Load(product.ProductId, originalOptionName);
                if (uVariant != null)
                {
                    // set variant price
                    pVariant.Price = uVariant.Price;
                    pVariant.Weight = uVariant.Weight;
                    pVariant.Length = product.Length;
                    pVariant.Height = product.Height;
                    pVariant.Width = product.Width;
                    

                    MerchelloContext.Current.Services.ProductVariantService.Save(pVariant);

                    // make mapping entry
                    string optionList = string.Format("{0},{1},{2}", uVariant.Option1, uVariant.Option2, uVariant.Option3);
                    UProductMappingService.Save(product.ProductId, uVariant.ProductVariantId, optionList, mProduct.Key, pVariant.Key, uVariant.AcUrl, uVariant.AcSku);
                }

            }

            

            // get key and return Guid
            retVal = mProduct.Key;
            return retVal;
        }

        private static string MakeUniqueSku(string sku)
        {
            // establish default increment value
            string retVal = sku;
            int counter = 1;

            // loop until unique value is identified
            while (_service.SkuExists(retVal))
            {
                retVal = sku + "-" + counter.ToString();
                counter++;
            }

            // exit and return value
            return retVal;
        }

        public static void DeleteProducts()
        {

            var products = ((ProductService)MerchelloContext.Current.Services.ProductService).GetAll();
            MerchelloContext.Current.Services.ProductService.Delete(products);
        }

    }

}