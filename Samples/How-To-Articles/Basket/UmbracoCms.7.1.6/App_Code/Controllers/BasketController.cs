namespace Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Models;
    using Merchello.Core;
    using Merchello.Core.Models;
    using Merchello.Web.Models.ContentEditing;
    using Umbraco.Core.Logging;
    using Umbraco.Web.Mvc;
    using System.Globalization;

    /// <summary>
    /// Summary description for BasketController
    /// </summary>
    [PluginController("MerchelloProductListExample")]
    public class BasketController : MerchelloSurfaceContoller
    {

        // TODO These would normally be passed in or looked up so that there is not a 
        // hard coded reference
        private const int BasketContentId = 1089;

        public BasketController()
            : this(MerchelloContext.Current)
        {
        }

        public BasketController(IMerchelloContext merchelloContext)
            : base(merchelloContext)
        {
        }

        //<summary>
        //Renders the BuyButton form which is a partial view 
        //</summary>
        //<param name="product">
        //</param>
        //<returns></returns>
        public ActionResult Display_BuyButton(AddItemModel product)
        {
            return PartialView("BuyButton", product);
        }

        [HttpPost]
        public ActionResult AddToBasket(AddItemModel model)
        {
            // Add to Logical Basket 

            // add Umbraco content id to Merchello Product extended properties
            var extendedData = new ExtendedDataCollection();
            extendedData.SetValue("umbracoContentId", model.ContentId.ToString(CultureInfo.InvariantCulture));

            // get Merchello product 
            var product = Services.ProductService.GetByKey(model.ProductKey);

            // add a single item of the Product to the logical Basket
            Basket.AddItem(product, product.Name, 1, extendedData);

            // Save to Database tables: merchItemCache, merchItemCacheItem
            Basket.Save();

            return RedirectToUmbracoPage(BasketContentId);
        }

        [ChildActionOnly]
        public ActionResult DisplayBasket()
        {
            return PartialView("Basket", GetBasketViewModel());
        }

        /// <summary>
        /// Responsible for updating the quantities of items in the basket.
        /// </summary>
        /// <param name="model"><see cref="BasketViewModel"/></param>
        /// <returns>Redirects to the current Umbraco page (the basket page)</returns>
        [HttpPost]
        public ActionResult UpdateBasket(BasketViewModel model)
        {
            if (ModelState.IsValid)
            {
                // The only thing that can be updated in this basket is the quantity
                foreach (var item in model.Items)
                {
                    Console.WriteLine("model: key=" + item.Key + ", " + "new quantity=" + item.Quantity);

                    if (Basket.Items.First(x => x.Key == item.Key).Quantity != item.Quantity)
                    {
                        Basket.UpdateQuantity(item.Key, item.Quantity);
                    }
                }

                // * Tidbit - Everytime "Save()" is called on the Basket, a new VersionKey (Guid) is generated.
                // *** This is used to validate the SalePreparationBase (BasketCheckoutPrepartion in this case),
                // *** asserting that any previously saved information (rate quotes, shipments ...) correspond to the Basket version.
                // *** If the versions do not match, the  the checkout workflow is essentially reset - meaning 
                // *** you have to start the checkout process all over
                Basket.Save();
            }

            return RedirectToUmbracoPage(BasketContentId);
        }

        /// <summary>
        /// Removes an item from the basket
        /// </summary>
        /// <param name="lineItemKey">
        /// The Merchello Product Key.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpGet]
        public ActionResult RemoveItemFromBasket(Guid lineItemKey)
        {
            if (Basket.Items.FirstOrDefault(x => x.Key == lineItemKey) == null)
            {
                var exception =
                    new InvalidOperationException(
                        "Attempt to delete an item from a basket that does not match the CurrentUser");
                LogHelper.Error<BasketController>("RemoveItemFromBasket failed.", exception);

                throw exception;
            }

            // remove product  
            Basket.RemoveItem(lineItemKey);
            Basket.Save();

            return RedirectToUmbracoPage(BasketContentId);
        }
        private BasketViewModel GetBasketViewModel()
        {
            // ToBasketViewModel is an extension that
            // translates the IBasket to a local view model which
            // can be submitted via a form.
            var basketViewModel = Basket.ToBasketViewModel();

            // grab customer id - for example only
            // regardless if anon or known customer
            // stored in merchAnonymousCustomer table
            // or merchCustomer table
            if (CurrentCustomer.IsAnonymous)
            {
                basketViewModel.Customer = "Anonymous Customer";
            }
            else
            {
                basketViewModel.Customer = "Friend";
            }

            return basketViewModel;
        }
        [HttpGet]
        public ActionResult DisplayCustomerBasket()
        {
            return PartialView("Customer", GetBasketViewModel());
        }
    }
}
