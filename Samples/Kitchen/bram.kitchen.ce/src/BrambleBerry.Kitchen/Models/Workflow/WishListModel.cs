using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Merchello.Web;

namespace BrambleBerry.Kitchen.Models.Workflow
{
    public class WishListModel
    {
        public const string DefaultTitle = "Default";
        public Guid ID { get; set; }

        public Guid CustomerID { get; set; }

        public string Title { get; set; }

        public DateTime DateCreated { get; set; }

        public bool IsPublic { get; set; }

        public List<WishListItemModel> Items { get; set; }

        public bool IsDefaultList
        {
            get { return Title == DefaultTitle; }
        }

    }
}
