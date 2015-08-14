using HC.RM.Common.PCL.Helpers;
using RM.MailshotsOnline.PCL.Services;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;

namespace RM.MailshotsOnline.Web.Controllers.Api
{
    public class ImageLibraryController : ApiBaseController
    {
        private readonly IImageLibraryService _imageLibrary;

        // GET: Images
        public ImageLibraryController(IMembershipService membershipService, IImageLibraryService imageLibraryService, ILogger logger) 
            : base(membershipService, logger)
        {
            _imageLibrary = imageLibraryService;
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

        [HttpPut]
        public HttpResponseMessage UploadImage(byte[] bytes, string name)
        {
            // todo: check size of bytes[]

            var authResult = Authenticate();
            if (authResult != null)
            {
                return authResult;
            }

            var media = _imageLibrary.AddImage(bytes, name, _loggedInMember);

            if (media == null)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }

            return Request.CreateResponse(HttpStatusCode.Created, media);
        }

        [HttpPost]
        public HttpResponseMessage DeleteImage(string id)
        {
            var authResult = Authenticate();
            if (authResult != null)
            {
                return authResult;
            }

            // delete from blob store.

            // delete from umbraco

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpPost]
        public HttpResponseMessage RenameImage(string id)
        {
            var authResult = Authenticate();
            if (authResult != null)
            {
                return authResult;
            }

            // rename in blob store.

            // rename in umbraco

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}