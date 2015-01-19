using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using AbleMods.Api.Models;
using Merchello.Core.Models;
using Merchello.Core.Services;
using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web;

namespace AbleMods.Services
{
    public static class UProductService
    {
        private static IContentService _cs;
        private static IMediaService _ms;

        static UProductService()
        {
            _cs = UmbracoContext.Current.Application.Services.ContentService;
            _ms = UmbracoContext.Current.Application.Services.MediaService;
        }

        public static void MakeProduct(UProduct prod, int contentNodeParentId, Guid merchelloGuid)
        {
            // create new content
            var content = _cs.CreateContent(prod.Name.Trim(), contentNodeParentId, "Product");

            // set content properties based on data from Able
            content.SetValue("headline", prod.Name);
            content.SetValue("metaDescription", prod.MetaDescription);
            content.SetValue("metaKeywords", prod.MetaKeywords);
            content.SetValue("pageTitle", prod.Name);
            content.SetValue("bodyText", prod.Description);
            content.SetValue("brief", prod.Summary);
            
            // Establish link to merchello product
            content.SetValue("product", merchelloGuid.ToString());


            // publish content so we can get nodeId
            _cs.Publish(content);

            // pull in images if we have some
            if (prod.Images.Count > 0)
            {
                // make media objects for each product image
                List<string> mediaIds = new List<string>();
                foreach (string imageUrl in prod.Images)
                {
                    // make media and get ID
                    if (!string.IsNullOrEmpty(imageUrl))
                    {
                        string mediaNodeId = MakeMediaNode(content, imageUrl, prod.Name);
                        if (!string.IsNullOrEmpty(mediaNodeId))
                        {
                            mediaIds.Add(mediaNodeId);
                        }
                    }

                }

                // save all images to the media picker property value
                string idList = string.Join(",", mediaIds.ToArray());
                content.SetValue("images", idList);
                _cs.Save(content);

            }
        }


        private static string MakeMediaNode(IContent content, string imageUrl, string name)
        {
            // pull down remote image
            string localFile = GetRemoteImage(imageUrl, content.Name.Trim());

            localFile = localFile.Replace(HttpContext.Current.Server.MapPath("~/App_Data/TEMP/Import/"), "~/App_Data/TEMP/Import/");

            // bail out now if a valid local file name could not be achieved
            if (string.IsNullOrEmpty(localFile))
            {
                return string.Empty;
            }

            // make media folder
            IMedia folder = null;
            IEnumerable<IMedia> rootMedia = _ms.GetRootMedia();
            foreach (IMedia media in rootMedia)
            {
                if (media.Name.Contains("Product Images"))
                {
                    folder = media;
                    break;
                }
            }

            if (folder == null)
            {
                folder = _ms.CreateMedia("Product Images", -1, "Folder");
                _ms.Save(folder);
            }

            int folderId = folder.Id;

            LogHelper.Info(typeof(UProductService), "Local file: " + localFile);

            // store media folder id with the content item
            //content.SetValue("mediaFolder", folderId);

            // create a media item in the new folder that references the image
            //var mediaImage = _ms.CreateMedia(name, folderId, Constants.Conventions.MediaTypes.File);
            //mediaImage.SetValue("umbracoFile", localFile);
            //_ms.Save(mediaImage);
            var mediaImportService = new MediaImportService();

            var mediaImage = mediaImportService.ImportMedia(localFile, content.Name, folderId);


            return mediaImage.Id.ToString();
        }

        private static string GetRemoteImage(string imageUrl, string subFolder)
        {
            // set up return value
            string retVal = string.Empty;

            // create path
            string mediaPath = HttpContext.Current.Server.MapPath("~/App_Data/TEMP/Import/");
            string desiredPath = mediaPath + MakeValidPath(subFolder) + "/";

            // make sure path exists
            if (!Directory.Exists(desiredPath))
            {
                Directory.CreateDirectory(desiredPath);
            }

            if (imageUrl.Contains("3002-2"))
            {
                string dummy = string.Empty;
            }

            // strip any relative link
            if (imageUrl.Contains("~"))
            {
                imageUrl = imageUrl.Replace("~", "http://www.brambleberry.com");
            }

            if (imageUrl.StartsWith("/assets/productimages/", StringComparison.InvariantCultureIgnoreCase))
            {
                imageUrl = imageUrl.Replace("/assets/productimages/", "http://www.brambleberry.com/assets/productimages/", StringComparison.InvariantCultureIgnoreCase);
            }

            // make final path/filename
            retVal = desiredPath + Path.GetFileName(imageUrl);
            //LogHelper.Info(typeof(UProductService),localFileName + " ... " + imageUrl);

            try
            {
                // pull in web image
                WebClient webClient = new WebClient();
                webClient.DownloadFile(imageUrl, retVal);
            }
            catch (Exception)
            {
                retVal = string.Empty;
            }

            // exit and return value
            return retVal;
        }

        private static string MakeValidPath(string path)
        {
            // pull in .Net invalid character list
            string invalid = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());

            // add our own that we know umbraco doesn't like
            invalid += ",";

            // loop and parse out bad characters
            foreach (char c in invalid)
            {
                path = path.Replace(c.ToString(), "");
            }

            // Builds a string out of valid chars
            return path;
        }

        private static string MakeValidFileName(string fname)
        {
            char[] invalidFileNameChars = Path.GetInvalidFileNameChars();

            // Builds a string out of valid chars
            return new string(fname.Where(ch => !invalidFileNameChars.Contains(ch)).ToArray());
        }

         
    }
}