namespace MerchelloNotificationExampleProvder.Events
{

    using System;
    using System.Linq;

    using Merchello.Core;
    using Merchello.Core.Events;
    using Merchello.Core.Models;
    using Merchello.Core.Services;

    using Umbraco.Core;
    using Umbraco.Core.Events;
    using Umbraco.Core.Logging;

    public class MerchelloEvents : ApplicationEventHandler
    {

        protected override void ApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            //ProductService.Created += ProductService_Created;
            ProductService.Saving += ProductService_Saving;
        }

        void ProductService_Saving(IProductService sender, SaveEventArgs<IProduct> e)
        {
            foreach (var product in e.SavedEntities)
            {
                if (!product.HasIdentity)
                {
                    Notification.Trigger("ProductAdd", product);
                }
                
            }

        }

    }
}
