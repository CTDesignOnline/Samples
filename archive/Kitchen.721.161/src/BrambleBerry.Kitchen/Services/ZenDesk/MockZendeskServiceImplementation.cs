using System;
using System.Collections.Generic;
using BrambleBerry.Kitchen.Services.ZenDesk.Models;

namespace BrambleBerry.Kitchen.Services.ZenDesk
{
    public class MockZendeskServiceImplementation : IZendeskServiceImplementation
    {
        public List<ZendeskUpdateModel> GetUpdatesForOrder(Guid id)
        {
            return new List<ZendeskUpdateModel>()
            {
                new ZendeskUpdateModel()
                {
                    Date = new DateTime(2014,5,5,17,25,25),
                    Text = "Your order has been posted",
                    UpdatedBy = "Packer 1"
                },
                new ZendeskUpdateModel()
                {
                    Date = new DateTime(2014,5,6,10,12,7),
                    Text = "I was missing an item from my package",
                    UpdatedBy = "Customer"
                },
                new ZendeskUpdateModel()
                {
                    Date = new DateTime(2014,5,6,12,12,7),
                    Text = "Sorry, your missing item is in transit",
                    UpdatedBy = "Customer services"
                }
            };
        }
    }
}