using System;

namespace BrambleBerry.Kitchen.Services.ZenDesk.Models
{
    public class ZendeskUpdateModel
    {
        public DateTime Date { get; set; }
        public string Text { get; set; }
        public string UpdatedBy { get; set; }
    }
}