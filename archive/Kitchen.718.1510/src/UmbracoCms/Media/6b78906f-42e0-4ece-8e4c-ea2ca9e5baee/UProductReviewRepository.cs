using System.Collections.Generic;
using System.Net;
using System.Web.Helpers;
using AbleMods.Api.Models;

namespace AbleMods.Api
{
    public class UProductReviewRepository
    {
        private static string _serviceUrl = UApiSettings.Siteurl + "/uproductreview/";
        private WebClient _client;

        public UProductReviewRepository()
        {
            _client = new WebClient();
        }

        public List<UProductReview> LoadforProductId(int productId)
        {
            var json = _client.DownloadString(_serviceUrl + productId.ToString());
            return Json.Decode(json, typeof(List<UProductReview>));
        }

        public List<UProductReview> LoadAll()
        {
            var json = _client.DownloadString(_serviceUrl);
            return Json.Decode(json, typeof(List<UProductReview>));
        }
         
    }
}