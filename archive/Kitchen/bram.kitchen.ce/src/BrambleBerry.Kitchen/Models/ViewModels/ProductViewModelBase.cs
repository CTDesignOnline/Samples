namespace BrambleBerry.Kitchen.Models.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using Buzz.Hybrid.Models;
    using Umbraco.Web;

    /// <summary>
    /// The product view model base.
    /// </summary>
    public abstract class ProductViewModelBase : ViewModelBase
    {
        /// <summary>
        /// Gets the product category link collection.
        /// </summary>
        public IEnumerable<ILink> CategoryLinks
        {
            get
            {
                return Content.AncestorOrSelf(2)
                  .Children.ToList().Where(x => x.IsVisible())
                  .Select(
                      x =>
                      new Link()
                      {
                          ContentId = x.Id,
                          Title = x.Name,
                          Url = x.Url,
                          CssClass = Content.Id == x.Id ? "active" : string.Empty,
                          Target = "_self",
                          ElementId = string.Format("category-{0}", x.Id)
                      });
            } 
        }      
    }
}