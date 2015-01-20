using System;
using System.Collections.Generic;
using BrambleBerry.Kitchen.Models.ViewModels;

namespace BrambleBerry.Kitchen.Models.Account.MyDigitalContent
{
    public class MyDigitalContentViewModel : ViewModelBase
    {
        public IEnumerable<MyDigitalContentItemViewModel> DigitalContent { get; set; }
    }

    public class MyDigitalContentItemViewModel
    {
        public string Name { get; set; }
        public Guid OrderId { get; set; }
        public int Remaining { get; set; }
        public DateTime PurchaseDate { get; set; }
        public bool HasBeenDownloaded { get; set; }
    }
}
