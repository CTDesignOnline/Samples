using System;
using System.Collections.Generic;
using BrambleBerry.Kitchen.Models.ViewModels;

namespace BrambleBerry.Kitchen.Models.Account.MyDigitalContent
{
    public class MyDigitalContentRecievedViewModel : ViewModelBase
    {
        public IEnumerable<MyDigitalContentItemRecievedViewModel> DigitalContent { get; set; }
    }

    public class MyDigitalContentItemRecievedViewModel
    {
        public string From { get; set; }
        public Guid OrderId { get; set; }
        public DateTime ClaimedOn { get; set; }
        public List<string> Items { get; set; }
    }
}
