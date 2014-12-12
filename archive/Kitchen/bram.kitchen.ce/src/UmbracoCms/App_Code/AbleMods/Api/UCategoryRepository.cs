using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Web.Helpers;
using AbleMods.Api.Models;

namespace AbleMods.Api
{
    public static class UCategoryRepository
    {
        private static string _serviceUrl = UApiSettings.Siteurl + "/ucategory/";
        private static WebClient _client;

        static UCategoryRepository()
        {
            _client = new WebClient();
        }

        public static UCategory Load(int categoryId)
        {
            var json = _client.DownloadString(_serviceUrl + categoryId.ToString());
            return Json.Decode(json, typeof(UCategory));
        }

        public static IList<UCategory> LoadAll()
        {
            var json = _client.DownloadString(_serviceUrl);
            return Json.Decode(json, typeof(IList<UCategory>));
        }
    }
}