namespace BrambleBerry.Kitchen.Controllers
{
    using System.Web.Mvc;    
    using Buzz.Hybrid.Controllers;
    using Merchello.Core;
    using Models.ViewModels;
    using Umbraco.Web.Models;

    /// <summary>
    /// The checkout controller.
    /// </summary>
    [RequireSsl("BrambleBerry:RequireSsl")]
    public class CheckoutController : MerchelloControllerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CheckoutController"/> class.
        /// </summary>
        public CheckoutController()
            : this(MerchelloContext.Current)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckoutController"/> class.
        /// </summary>
        /// <param name="merchelloContext">
        /// The merchello context.
        /// </param>
        public CheckoutController(IMerchelloContext merchelloContext)
            : base(merchelloContext)
        {
        }

        /// <summary>
        /// The index.
        /// </summary>
        /// <param name="model">
        /// The <see cref="RenderModel"/>
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/> to render the checkout page template view
        /// </returns>
        public override ActionResult Index(RenderModel model)
        {
            var checkout = BuildModel<CheckoutViewModel>();
            
            checkout.CustomerKey = CurrentCustomer.Key;

            checkout.Items = Basket.ToBasketFormModel(Umbraco).Items;
            
            checkout.TotalBasketPrice = Basket.TotalBasketPrice;

            return CurrentTemplate(checkout);
        }
    }
}