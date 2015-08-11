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
        public JsonResult<IEnumerable<Image>> GetImages()
        {
            var results = _imageLibrary.GetImages();

            return Json(results);
        }

        [HttpGet]
        public JsonResult<IEnumerable<Image>> GetImages(string tag)
        {
            var results = _imageLibrary.GetImages(tag);

            return Json(results);
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
            
            var results = _imageLibrary.GetImages();
            return Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(results));
        }

        [HttpGet]
        public JsonResult<IEnumerable<string>> GetTags()
        {
            var results = _imageLibrary.GetTags();

            return Json(results);
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
                // todo: log exception
            }

            return serializedResults;
        }
    }
}