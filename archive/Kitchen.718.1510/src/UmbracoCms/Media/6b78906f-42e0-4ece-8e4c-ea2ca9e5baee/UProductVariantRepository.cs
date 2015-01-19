using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Helpers;
using AbleMods.Api.Models;

namespace AbleMods.Api
{
    public static class UProductVariantRepository
    {
        private static string _serviceUrl = UApiSettings.Siteurl + "/uproductvariant/";
        private static WebClient _client;

        static UProductVariantRepository()
        {
            _client = new WebClient();
        }

        public static UProductVariant Load(int productId, string choiceName)
        {
            try
            {
                var json = _client.DownloadString(_serviceUrl + productId.ToString() + "/" + choiceName);
                return Json.Decode(json, typeof(UProductVariant));
            }
            catch (Exception)
            {
                return null;
            }
        }

    }
}