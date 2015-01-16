namespace MerchelloNotificationExampleProvder.NotificationProvider
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Merchello.Core.Gateways.Notification.Triggering;
    using Merchello.Core.Observation;
    using Merchello.Core.Models;

    [TriggerFor("ProductAdd", Topic.Notifications)]
    public class ProductAddTrigger : NotificationTriggerBase<Merchello.Core.Models.IProduct, Merchello.Core.Models.IProduct>
    {
        protected override void Notify(Merchello.Core.Models.IProduct product, IEnumerable<string> contacts)
        {
            this.NotifyMonitors(product);
        }
    }
}
