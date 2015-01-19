using BrambleBerry.Kitchen.Models.ViewModels;

namespace BrambleBerry.Kitchen.Controllers
{
    using System.Linq;
    using System.Web.Mvc;    
    using Buzz.Hybrid;
    using Merchello.Web;
    using Models;
    using Umbraco.Web.Models;

    /// <summary>
    /// The product controller.
    /// </summary>
    public class ProductController : KitchenControllerBase
    {
        /// <summary>
        /// The default controller method
        /// </summary>
        /// <param name="model">
        /// The default <see cref="RenderModel"/>
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/> to render the product page template view
        /// </returns>
        public override ActionResult Index(RenderModel model)
        {
            var productModel = BuildModel<ProductViewModel>();

            productModel.Thumbnail = productModel.Content.GetSafeImage(Umbraco, "images");
            productModel.Images = productModel.Content.GetSafeImages(Umbraco, "images", null);

            var merchello = new MerchelloHelper();

            var product = merchello.Query.Product.GetByKey(productModel.Content.GetSafeGuid("product"));

            productModel.ProductKey = product.Key;
            productModel.Price = product.Price;
            productModel.HasVariants = product.ProductVariants.Any();
            productModel.Options = product.ProductOptions;

            productModel.AddItemFormModel = new AddItemFormModel()
            {
                ContentId = productModel.Content.Id,
                ProductKey = product.Key,
                Product = product,
                Quantity = 1
            };

            return CurrentTemplate(productModel);
        } 
    }
}