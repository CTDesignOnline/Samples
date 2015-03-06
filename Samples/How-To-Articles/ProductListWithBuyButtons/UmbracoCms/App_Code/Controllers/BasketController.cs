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

    /// <summary>
    /// Summary description for BasketController
    /// </summary>
    [PluginController("MerchelloExample")]
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
            return RedirectToUmbracoPage(BasketContentId);
        }
    }
}
