using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using BrambleBerry.Kitchen.Events;
using umbraco.cms.presentation.create.controls;
using Umbraco.Core;
using umbraco.presentation.webservices;
using Umbraco.Web;

namespace BrambleBerry.Kitchen.HttpHandlers
{
    public class ProtectedMediaHandler : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.BeginRequest += context_BeginRequest;
        }

        void context_BeginRequest(object source, EventArgs e)
        {
            HttpApplication application = (HttpApplication)source;
            var requestedPath = application.Request.Url.PathAndQuery.ToLower();

            //Check if the current request is a media request, if not Bomb out!
            if (IsMediaRequest(requestedPath))
            {
                //Extract the Id from the url, if failing returning null
                var propertyId = ExtractPropertyIdFromUrl(application.Request.Url.PathAndQuery);
                if (propertyId.HasValue)
                {
                    //Verify if the current item isnt a protected media item
                    if (IsPropertyIdProtected(propertyId.Value))
                    {
                        BlockAccessToThisRequest(application);
                    }
                }
            }
        }

        private void BlockAccessToThisRequest(HttpApplication application)
        {
            var response = application.Response;
            // Don't allow this response to be cached by the browser.
            response.Cache.SetCacheability(HttpCacheability.NoCache);
            response.Cache.SetNoStore();
            response.Cache.SetExpires(DateTime.MinValue);
            
            //Set 404 incase someone is fishing
            response.StatusCode = 404;
            
            response.End();
        }

        public static int? ExtractPropertyIdFromUrl(string pathAndQuery)
        {
            var match = Regex.Match(pathAndQuery, "/media/(\\d*)/(.*)");
            if (match.Success)
            {
                return int.Parse(match.Groups[1].Value);
            }
            return null;
        }

        /// <summary>
        /// Checks if the current request is a media item
        /// </summary>
        /// <param name="requestedPath"></param>
        /// <returns></returns>
        public static bool IsMediaRequest(string requestedPath)
        {
            string mediaPrefix = "/media/";

            int mediaPrefixLength = mediaPrefix.Length;

            return requestedPath.StartsWith(mediaPrefix) && requestedPath.Length>mediaPrefixLength;
        }

        /// <summary>
        /// Check if a given propertyid belongs to a protected member type
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private static bool IsPropertyIdProtected(int id)
        {
            //TODO:Replace this is lucene lookup!
            return id == 1384;
        }

        public void Dispose()
        {
        }
        
    }


}
