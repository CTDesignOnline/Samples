using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Web.Helpers;
using AbleMods.Api.Models;

namespace AbleMods.Api
{
    public static class UCategoryProductRepository
    {

        private static string _serviceUrl = UApiSettings.Siteurl + "/ucategoryproduct/";
        private static WebClient _client;

        static UCategoryProductRepository()
        {
            _client = new WebClient();
        }

        public static IList<UProduct> LoadAll(int categoryId)
        {
            var json = _client.DownloadString(_serviceUrl + categoryId.ToString());
            return Json.Decode(json, typeof(IList<UProduct>));
        }
    }
}