namespace BrambleBerry.Kitchen.Controllers
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;
    using Models;
    using Models.ViewModels;
    using Buzz.Hybrid;
    using DevTrends.MvcDonutCaching;
    using Merchello.Core;
    using Merchello.Core.Models;
    using Umbraco.Web;
    using Umbraco.Web.Models;
    using Umbraco.Web.Routing;

    /// <summary>
    /// The basket controller.
    /// </summary>
    public class BasketController : MerchelloControllerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BasketController"/> class.
        /// </summary>
        public BasketController()
            : this(MerchelloContext.Current)
        {           
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BasketController"/> class.
        /// </summary>
        /// <param name="merchelloContext">
        /// The merchello context.
        /// </param>
        public BasketController(IMerchelloContext merchelloContext) : base(merchelloContext)
        {
        }

        /// <summary>
        /// The index.
        /// </summary>
        /// <param name="model">
        /// The <see cref="RenderModel"/>
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/> to render the basket page template view
        /// </returns>
        [DonutOutputCache(CacheProfile = "FiveMins")]
        public override ActionResult Index(RenderModel model)
        {
            return CurrentTemplate(BuildModel<ViewModelBase>());
        }

        /// <summary>
        /// The render basket menu.
        /// </summary>
        /// <returns>
        /// The <see cref="ActionResult"/> BasketMenu partial view
        /// </returns>
        /// <remarks>
        /// Not cached
        /// </remarks>
        [ChildActionOnly]
        public ActionResult RenderBasketMenu()
        {
            var model = new BasketMenuModel()
            {
                BasketPageUrl = BasketPageUrl,
                ItemCount = Basket.Items.Count,
                TotalPrice = Basket.TotalBasketPrice
            };

            return PartialView("BasketMenu", model);
        }

        /// <summary>
        /// Renders the basket.
        /// </summary>
        /// <returns>
        /// The <see cref="ActionResult"/> Basket partial view.
        /// </returns>
        /// <remarks>
        /// Not cached
        /// </remarks>
        [ChildActionOnly]
        public ActionResult RenderBasket()
        {
            var emptyBasketText = CurrentPage.GetSafeString("emptyBasketText", "Your basket is empty");

            var model = Basket.ToBasketFormModel(Umbraco, emptyBasketText);
            model.BasketPageContentId = CurrentPage.Id;
            model.ContinueShoppingUrl = HomePage.Descendant(ProductListingViewModel.RepresentsContentType).Url;
            model.CheckoutUrl = Umbraco.TypedContent(HomePage.GetSafeInteger("checkoutPage")).Url;

            return PartialView("BasketForm", model);
        }        

        /// <summary>
        /// The add item.
        /// </summary>
        /// <param name="model">
        /// The <see cref="AddItemFormModel"/>
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpPost]
        public ActionResult AddItem(AddItemFormModel model)
        {
            var extendedData = new ExtendedDataCollection();

            extendedData.SetValue("umbracoContentId", model.ContentId.ToString(CultureInfo.InvariantCulture));

            var product = Services.ProductService.GetByKey(model.ProductKey);

            if (model.OptionChoices != null && model.OptionChoices.Any())
            {
                var variant = Services.ProductVariantService.GetProductVariantWithAttributes(product, model.OptionChoices);

                extendedData.SetValue("isVariant", "true");

                Basket.AddItem(variant, variant.Name, 1, extendedData);                
            }
            else
            {
                Basket.AddItem(product, product.Name, 1, extendedData);
            }

            Basket.Save();
            
            return RedirectToCurrentUmbracoPage();
        }

        /// <summary>
        /// Updates the basket.
        /// </summary>
        /// <param name="model">
        /// The <see cref="BasketFormModel"/>
        /// </param>
        /// <returns>
        /// Redirects to the basket page
        /// </returns>
        [HttpPost]
        public ActionResult UpdateBasket(BasketFormModel model)
        {
            if (ModelState.IsValid)
            {
                foreach (var item in model.Items.Where(item => Basket.Items.First(x => x.Key == item.Key).Quantity != item.Quantity))
                {
                    Basket.UpdateQuantity(item.Key, item.Quantity);
                }

                Basket.Save();
            }

            return RedirectToCurrentUmbracoPage();
        }

        /// <summary>
        /// The remove item.
        /// </summary>
        /// <param name="lineItemKey">
        /// The line item key.
        /// </param>
        /// <param name="basketPageId">The Umbraco content Id for the basket page.
        /// We need this because the <see cref="PublishedContentRequest"/> will be null on the <see cref="UmbracoContext"/>
        /// when this method is called by an ActionLink
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpGet]
        public ActionResult RemoveItem(Guid lineItemKey, int basketPageId)
        {
            Basket.RemoveItem(lineItemKey);

            Basket.Save();

            return RedirectToUmbracoPage(basketPageId);
        }
    }
}