using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using RM.MailshotsOnline.Data.Services;
using RM.MailshotsOnline.Entities.JsonModels;
using RM.MailshotsOnline.PCL.Services;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace RM.MailshotsOnline.Web.Controllers.Api
{
    public class ImageLibraryController : ApiBaseController
    {
        private readonly ImageLibraryService _imageLibrary = new ImageLibraryService();

        // GET: Images
        public ImageLibraryController(IMembershipService membershipService) : base(membershipService)
        {
        }

        [HttpGet]
        public HttpResponseMessage GetImages()
        {
            var results = _imageLibrary.GetImages("animals");

            var concreteResults = results.Select(x => new LibraryImage() {Tags = x.GetPropertyValue("tags").ToString().Split(',')});

            string serializedMedia = null;
            try
            {
                serializedMedia = JsonConvert.SerializeObject(concreteResults, Formatting.Indented,
                    new JsonSerializerSettings
                    {
                        PreserveReferencesHandling = PreserveReferencesHandling.Objects
                    });
            }
            catch
            {
                // error serlializing media items.
            }

            return Request.CreateResponse(HttpStatusCode.Accepted, serializedMedia);
        }

        [HttpGet]
        public HttpResponseMessage Search()
        {
            var results = _imageLibrary.GetImages("animals");

            string serializedMedia = null;
            try
            {
                serializedMedia = JsonConvert.SerializeObject(results);
            }
            catch
            {
                // error serlializing media items.
            }

            return Request.CreateResponse(HttpStatusCode.Accepted, serializedMedia);
        }

        [Authorize]
        [HttpGet]
        public HttpResponseMessage GetImages(string username)
        {
            var results = _imageLibrary.GetImages(_loggedInMember);

            string serializedMedia = null;
            try
            {
                serializedMedia = JsonConvert.SerializeObject(results);
            }
            catch
            {
                // error serlializing media items.
            }

            return Request.CreateResponse(HttpStatusCode.Accepted, serializedMedia);
        }
    }
}