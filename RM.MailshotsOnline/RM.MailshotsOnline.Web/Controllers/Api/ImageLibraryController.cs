using HC.RM.Common.PCL.Helpers;
using Newtonsoft.Json;
using RM.MailshotsOnline.Data.Services;
using RM.MailshotsOnline.Entities.JsonModels;
using RM.MailshotsOnline.PCL.Models;
using RM.MailshotsOnline.PCL.Services;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Results;
using System.Web.Mvc;
using HC.RM.Common.Images;
using RM.MailshotsOnline.Data.Constants;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Editors;
using Image = System.Drawing.Image;

namespace RM.MailshotsOnline.Web.Controllers.Api
{
    public class ImageLibraryController : ApiBaseController
    {
        private readonly IImageLibraryService _imageLibrary;
        private readonly ICmsImageService _cmsImageService;

        // GET: Images
        public ImageLibraryController(IMembershipService membershipService, IImageLibraryService imageLibraryService, ICmsImageService cmsImageService, ILogger logger) 
            : base(membershipService, logger)
        {
            _imageLibrary = imageLibraryService;
            _cmsImageService = cmsImageService;
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

            Image original;
            using (var stream = new MemoryStream())
            {
                original = Image.FromStream(stream);
            }

            var resizer = new ImageResizer();
            var resizedSmall = resizer.GetResizedImageBytes(original, ContentConstants.Settings.ImageThumbnailSizeSmall);
            var resizedLarge = resizer.GetResizedImageBytes(original, ContentConstants.Settings.ImageThumbnailSizeLarge);

            ImageFormat format;
            using (var stream = new MemoryStream())
            {
                format = Image.FromStream(stream).RawFormat;
            }

            var s1 = _imageLibrary.AddImage(bytes, name, original.RawFormat.ToString(), _loggedInMember);
            var s2 = _imageLibrary.AddImage(resizedSmall, name, format.ToString(), _loggedInMember);
            var s3 = _imageLibrary.AddImage(resizedLarge, name, format.ToString(), _loggedInMember);

            if (!(s1 || s2 || s3))
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }

            return Request.CreateResponse(HttpStatusCode.Created);
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

        [HttpGet]
        public HttpResponseMessage GetUsedImages()
        {
            var authResult = Authenticate();
            if (authResult != null)
            {
                return authResult;
            }

            var usedImages = new List<PCL.Models.IMedia>();

            // Search the CMS Image service for all images used by this user
            var usedCmsImages = _cmsImageService.FindImagesUsedByUser(_loggedInMember.Id);
            foreach (var cmsImage in usedCmsImages)
            {
                usedImages.Add(_imageLibrary.GetImage(cmsImage.UmbracoMediaId, string.IsNullOrEmpty(cmsImage.UserName)));
            }

            return Request.CreateResponse(HttpStatusCode.OK, usedImages);
        }
    }
}