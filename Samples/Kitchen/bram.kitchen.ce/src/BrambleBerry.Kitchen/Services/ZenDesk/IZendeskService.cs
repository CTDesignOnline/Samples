using System;
using System.Collections.Generic;
using BrambleBerry.Kitchen.Services.ZenDesk.Models;

namespace BrambleBerry.Kitchen.Services.ZenDesk
{
    public interface IZendeskServiceImplementation
    {
        List<ZendeskUpdateModel> GetUpdatesForOrder(Guid id);
    }
}