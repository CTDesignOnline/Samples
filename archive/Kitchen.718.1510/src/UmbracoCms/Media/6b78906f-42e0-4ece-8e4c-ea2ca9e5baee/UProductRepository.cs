using System.Collections.Generic;
using System.Net;
using System.Web.Helpers;
using AbleMods.Api.Models;

namespace AbleMods.Api
{
    public static class UProductRepository
    {
        private static string _serviceUrl = UApiSettings.Siteurl + "/uproduct/";
        private static WebClient _client;

        static UProductRepository()
        {
            _client = new WebClient();
        }

        public static UProduct Load(int productId)
        {
            var json = _client.DownloadString(_serviceUrl + productId.ToString());
            return Json.Decode(json, typeof(UProduct));
        }

        public static IList<UProduct> LoadAll()
        {
            var json = _client.DownloadString(_serviceUrl);
            return Json.Decode(json, typeof(IList<UProduct>));
        }
 
    }
}