using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrambleBerry.Kitchen.Services.AbleCommerce
{
    /// <summary>
    /// A work item for the import job to use
    /// </summary>
    public class AbleCommerceImportItem
    {
        public int CustomerId { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }

        public AbleCommerceImportItem(string username, string password, int customerId)
        {
            Username = username;
            Password = password;
            CustomerId = customerId;
        }
    }

    /// <summary>
    /// Stub for the import queue, Rusty to flesh this out I imagine? #TODO
    /// </summary>
    public static class AbleCommerceImportQueue
    {
        private static Queue<AbleCommerceImportItem> queue = new Queue<AbleCommerceImportItem>();

        public static void Enqueue(string username, string password, int customerId)
        {
            Enqueue( new AbleCommerceImportItem( username, password, customerId) );
        }

        public static void Enqueue(AbleCommerceImportItem item)
        {
            queue.Enqueue( item );
        }

        public static bool HasItems()
        {
            return queue.Count > 0;
        }

        public static void Clear()
        {
            queue.Clear();
        }
    }
}
