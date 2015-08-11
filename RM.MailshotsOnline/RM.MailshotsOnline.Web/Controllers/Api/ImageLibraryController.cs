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
            var results = _imageLibrary.GetImages();
            var serializedResults = Serialize(results);

            return Request.CreateResponse(HttpStatusCode.Accepted, serializedResults);
        }

        [HttpGet]
        public HttpResponseMessage GetSearchResults(string tag)
        {
            var results = _imageLibrary.GetImages(tag);
            var serializedResults = Serialize(results);

            return Request.CreateResponse(HttpStatusCode.Accepted, serializedResults);
        }

        [Authorize]
        [HttpGet]
        public HttpResponseMessage GetMyImages()
        {
            var authResult = Authenticate();
            if (authResult != null)
            {
                return authResult;
            }

            var results = _imageLibrary.GetImages(_loggedInMember);
            var serializedResults = Serialize(results);

            return Request.CreateResponse(HttpStatusCode.Accepted, serializedResults);

        }

        private static string Serialize(IEnumerable<Image> results)
        {
            string serializedResults = null;
            try
            {
                serializedResults = JsonConvert.SerializeObject(results);
            }
            catch
            {
                // error serlializing media items.
            }

            return serializedResults;
        }
    }
}