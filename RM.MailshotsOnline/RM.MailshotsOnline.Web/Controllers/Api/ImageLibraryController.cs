using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Results;
using System.Web.Mvc;
using Newtonsoft.Json;
using RM.MailshotsOnline.Data.Services;
using RM.MailshotsOnline.Entities.JsonModels;
using RM.MailshotsOnline.PCL.Models;
using RM.MailshotsOnline.PCL.Services;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Editors;

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
            return Request.CreateResponse(HttpStatusCode.OK, results);
        }

        [HttpGet]
        public HttpResponseMessage GetImages(string tag)
        {
            var results = _imageLibrary.GetImages(tag);
            return Request.CreateResponse(HttpStatusCode.OK, results);
        }

        [HttpGet]
        public HttpResponseMessage GetMyImages()
        {
            var authResult = Authenticate();
            if (authResult != null)
            {
                return authResult;
            }

            var results = _imageLibrary.GetImages(_loggedInMember);
            return Request.CreateResponse(HttpStatusCode.OK, results);
        }

        [HttpGet]
        public HttpResponseMessage GetTags()
        {
            var results = _imageLibrary.GetTags();
            return Request.CreateResponse(HttpStatusCode.OK, results);
        }
    }
}