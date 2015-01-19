using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Web.Helpers;
using AbleMods.Api.Models;

namespace AbleMods.Api
{
    public static class UCategoryNodesRepository
    {
        private static string _serviceUrl = UApiSettings.Siteurl + "/ucategorynodes/";
        private static WebClient _client;

        static UCategoryNodesRepository()
        {
            _client = new WebClient();
        }

        public static IList<UCategory> Load(int parentId)
        {
            var json = _client.DownloadString(_serviceUrl + parentId.ToString());
            return Json.Decode(json, typeof(IList<UCategory>));
        }
    }
}