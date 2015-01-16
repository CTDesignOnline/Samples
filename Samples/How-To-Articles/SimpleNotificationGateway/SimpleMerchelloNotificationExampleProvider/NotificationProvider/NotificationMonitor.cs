namespace MerchelloNotificationExampleProvder.NotificationProvider
{

    using System.Collections.Generic;
    using System.Linq;

    using Merchello.Core.Formatters;
    using Merchello.Core.Gateways.Notification;
    using Merchello.Core.Gateways.Notification.Monitors;
    using Merchello.Core.Observation;
    using Merchello.Core.Models;

    [MonitorFor("B829266B-B585-4CA2-BCCB-4EBCEE045114", typeof(ProductAddTrigger), "Product Add Message (Pattern Replace)")]
    public class ProductAddMonitor : NotificationMonitorBase<IProduct>
    {
        public ProductAddMonitor(INotificationContext notificationContext)
            : base(notificationContext)
        {
        }

        /// <summary>
        /// contacts are not used 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="contacts"></param>
        public override void OnNext(IProduct value)
        {
            if (!Messages.Any()) return;

            var patterns = new Dictionary<string, IReplaceablePattern>()
            {
                { "Name", new ReplaceablePattern("Name", "{{Name}}", value.Name.ToString()) },    
                { "TrackInventory", new ReplaceablePattern("TrackInventory", "{{TrackInventory}}", value.TrackInventory.ToString()) },
                { "Barcode", new ReplaceablePattern("Barcode", "{{Barcode}}", value.Barcode) },
                { "Sku", new ReplaceablePattern("Sku", "{{Sku}}", value.Sku) },
                { "Height", new ReplaceablePattern("Height", "{{Height}}", value.Height.ToString()) },
                { "Length", new ReplaceablePattern("Length", "{{Length}}", value.Length.ToString()) },
                { "Width", new ReplaceablePattern("Width", "{{Width}}", value.Width.ToString()) },
                { "Weight", new ReplaceablePattern("Weight", "{{Weight}}", value.Weight.ToString()) }
            };

            var formatter = new PatternReplaceFormatter(patterns);

            foreach (var message in Messages.ToArray())
            {
                // send the message
                NotificationContext.Send(message, formatter);
            }

        }
    }
}
