using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using BrambleBerry.Kitchen.Models.Account.MyDigitalContent;
using Merchello.Core.Formatters;
using Merchello.Core.Gateways.Notification;
using Umbraco.Web.Models;

namespace BrambleBerry.Kitchen.Controllers.Account
{
    /// <summary>
    /// The contact page controller.
    /// </summary>
    /// <remarks>
    /// This controller is dependent on Merchello's <see cref="INotificationContext"/> so that we can use the <see cref="IPatternReplaceFormatter"/> and
    /// the back office configured SMTP settings
    /// </remarks>
    public class MyDigitalContentController : BaseAccountController
    {
        

        /// <summary>
        /// The default controller method
        /// </summary>
        /// <param name="model">
        /// The default <see cref="RenderModel"/>
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/> to render the contact page template view
        /// </returns>        
        public override ActionResult Index(RenderModel model)
        {
            var viewmodel = base.BuildModel<MyDigitalContentViewModel>();

            viewmodel.DigitalContent = new List<MyDigitalContentItemViewModel>()
            {
                new MyDigitalContentItemViewModel()
                {
                    HasBeenDownloaded = false,
                    Name = "How to make a candle",
                    OrderId = Guid.Empty,
                    PurchaseDate = DateTime.UtcNow.AddHours(-2),
                    Remaining = 1
                },
                new MyDigitalContentItemViewModel()
                {
                    HasBeenDownloaded = false,
                    Name = "How to make a candle",
                    OrderId = Guid.Empty,
                    PurchaseDate = DateTime.UtcNow.AddDays(-1),
                    Remaining = 0
                },
            };
            

            return View(viewmodel);
        }

        public ActionResult MyGifts()
        {
            var viewmodel = base.BuildModel<MyDigitalContentRecievedViewModel>();

            viewmodel.DigitalContent = new List<MyDigitalContentItemRecievedViewModel>()
            {
                new MyDigitalContentItemRecievedViewModel()
                {
                    From = "Amanda Wallace",
                    ClaimedOn =  new DateTime(2013,10,2),
                    OrderId = Guid.Empty,
                    Items = new List<string>()
                    {
                        "155 Words You Need to Know: Practical Wisdom for Creative Entrepreneurs",
                        " 5-Pointed Star Column Template"
                    }
                },
                 new MyDigitalContentItemRecievedViewModel()
                {
                    From = "Lindsey Douglas",
                    ClaimedOn =  new DateTime(2013,2,2),
                    OrderId = Guid.Empty,
                    Items = new List<string>()
                    {
                        "Advanced Aromatherapy Book"
                    }
                },
                 new MyDigitalContentItemRecievedViewModel()
                {
                    From = "Amanda Wallace",
                    ClaimedOn =  new DateTime(2013,12,12),
                    OrderId = Guid.Empty,
                    Items = new List<string>()
                    {
                        "Essential Oil Book"
                    }
                }
            };


            return View(viewmodel);
        }

        public ActionResult Download(int id)
        {
            //Todo: check current user has access to download it

            //TODO lookup filename from id 
            string filepath = @"Media/1384/namedotcom_api_documentation.pdf";

            //TODO:Log Download somewhere

            string filename = System.IO.Path.GetFileName(filepath);

            //Found a built in way in .net 4.5 to lookup mime type from http://stackoverflow.com/a/14108040
            //although it may not work with popular items apparently, we'll see it it works
            string contentType = MimeMapping.GetMimeMapping(filename);

            string absoluteFilePath = Server.MapPath("~/"+filepath);

            var cd = new System.Net.Mime.ContentDisposition
            {
                // for example foo.bak
                FileName = filename,

                // always prompt the user for downloading, set to true if you want 
                // the browser to try to show the file inline
                Inline = false
            };
            Response.AppendHeader("Content-Disposition", cd.ToString());
            return File(absoluteFilePath, contentType);
        }
    }

  
}