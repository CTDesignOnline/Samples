using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
using AbleMods.Api;
using AbleMods.Api.Models;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web;

namespace AbleMods.Services
{
    public static class UCategoryService
    {
        private static IContentService _cs;

        static UCategoryService()
        {
            _cs = UmbracoContext.Current.Application.Services.ContentService;
        }

        public static void ProcessCategory(int contentNodeParentId, int categoryParentId, bool deepImport, bool includeProducts, bool test3Only)
        {
            // first make a node for the starting category
            UCategory startingCat = UCategoryRepository.Load(categoryParentId);
            int nodeId = CreateCategoryNode(startingCat, contentNodeParentId);

            if (deepImport)
            {
                // now get child nodes for this category
                IList<UCategory> subCats = UCategoryNodesRepository.Load(categoryParentId);

                // loop through each child node and process them as well
                foreach (UCategory cat in subCats)
                {
                    ProcessCategory(nodeId, cat.CategoryId, deepImport, includeProducts, test3Only);
                }
            }

            // no more categories to process for this level.  
            // Start processing products in this level
            if (includeProducts)
            {
                IList<UProduct> products = UCategoryProductRepository.LoadAll(startingCat.CategoryId);

                int badSkuCounter = 1;
                int prodCounter = 0;
                // process each product for this category
                foreach (UProduct product in products)
                {
                    if (string.IsNullOrEmpty(product.Sku))
                    {
                        product.Sku = string.Format("{0}-{1}", product.ProductId, badSkuCounter);
                        badSkuCounter++;
                    }

                    // make merchello product
                    Guid merchelloGuid = MProductService.MakeMerchelloProduct(product);

                    // make umbraco product content node
                    UProductService.MakeProduct(product, nodeId, merchelloGuid);

                    // update counter and limit to 3 if flag is set
                    if (test3Only)
                    {
                        prodCounter++;
                        if (prodCounter > 3)
                        {
                            break;
                        }
                    }
                }
            }
        }

        private static int CreateCategoryNode(UCategory uCategory, int contentNodeParentId)
        {
            // create new content
            var content = _cs.CreateContent(uCategory.Name, contentNodeParentId, "ProductListing");

            // set content properties based on data from Able
            content.SetValue("headline", uCategory.Title);
            content.SetValue("metaDescription", uCategory.MetaDescription);
            content.SetValue("metaKeywords", uCategory.MetaKeywords);
            content.SetValue("pageTitle", uCategory.Title);
            content.SetValue("bodyText", uCategory.Description);
            content.SetValue("summary", uCategory.Summary);

            // publish content so we can get nodeId
            _cs.Publish(content);

            return content.Id;
        }
 


        /// <summary>
        /// Prepares a SelectItem list of the root Able categories
        /// </summary>
        /// <returns>SelectListItem collection</returns>
        public static IEnumerable<SelectListItem> GetCategoryListItems()
        {
            IEnumerable<UCategory> cats = UCategoryNodesRepository.Load(0);
            return new SelectList(cats, "CategoryId", "Name");
        }

    }
}