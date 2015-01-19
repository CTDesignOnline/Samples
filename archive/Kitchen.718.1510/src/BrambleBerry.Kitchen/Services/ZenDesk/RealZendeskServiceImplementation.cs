using System;
using System.Collections.Generic;
using System.Linq;
using BrambleBerry.Kitchen.Services.ZenDesk.Models;
using ZendeskApi_v2;

namespace BrambleBerry.Kitchen.Services.ZenDesk
{
    public class RealZendeskServiceImplementation : IZendeskServiceImplementation
    {
        public List<ZendeskUpdateModel> GetUpdatesForOrder(Guid id)
        {

            var updates = new List<ZendeskUpdateModel>();

            var api = new ZendeskApi(ZendeskServiceConfiguration.EndpointUrl, ZendeskServiceConfiguration.Username, ZendeskServiceConfiguration.Password);


            var orders = api.Search.SearchFor(id.ToString());
            var firstCase = orders.Results.FirstOrDefault();
            if (firstCase != null)
            {
                var comments = api.Tickets.GetTicketComments(firstCase.Id);
                var temporaryUserCache = new Dictionary<long, string>();

                updates.AddRange(comments.Comments.Where(x => x.Public).Select(x => new ZendeskUpdateModel()
                {
                    Date = DateTime.Parse(x.CreatedAt),
                    Text = x.Body,
                    UpdatedBy = GetAuthorName(api, x.AuthorId, temporaryUserCache)
                }));
            }

            return updates;
        }


        private string GetAuthorName(ZendeskApi api, long? authorId, Dictionary<long, string> temporaryUserCache)
        {
            var name = "Unknown";
            if (authorId.HasValue)
            {
                var isCached = temporaryUserCache.ContainsKey(authorId.Value);
                if (isCached)
                {
                    name = temporaryUserCache[authorId.Value];
                }
                else
                {
                    //get the name of the user from the api,
                    name = api.Users.GetUser(authorId.Value).User.Name;
                    //and cache the results
                    temporaryUserCache.Add(authorId.Value, name);
                }
            }

            return name;
        }
    }
}