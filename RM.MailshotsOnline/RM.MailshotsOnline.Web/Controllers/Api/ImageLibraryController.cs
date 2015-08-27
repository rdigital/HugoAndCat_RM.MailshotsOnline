using System.Collections.Generic;
using HC.RM.Common.PCL.Helpers;
using RM.MailshotsOnline.PCL.Services;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;
using System;
using RM.MailshotsOnline.Entities.ViewModels;
using Examine;
using RM.MailshotsOnline.Entities.JsonModels;
using RM.MailshotsOnline.Data.Helpers;
using System.Web;
using Umbraco.Core.Security;

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
        public HttpResponseMessage GetImageUsageCount(int id)
        {
            bool umbracoAccess = false;
            // Check to see if the user is logged into Umbraco
            var umbracoUser = UmbracoContext.Security.CurrentUser;

            if (umbracoUser == null)
            {
                var authTicket = new HttpContextWrapper(HttpContext.Current).GetUmbracoAuthTicket();
                if (authTicket != null)
                {
                    var userName = authTicket.Name;
                    umbracoUser = UmbracoContext.Application.Services.UserService.GetByUsername(userName);
                }
            }

            if (umbracoUser != null)
            {
                // TODO: Double check this is the way to do it
                if (umbracoUser.UserType.Alias.ToLowerInvariant() == "admin")
                {
                    umbracoAccess = true;
                }
            }

            if (!umbracoAccess)
            {
                ErrorMessage(HttpStatusCode.Forbidden, "Only administrators can view usage count via this method.");
            }

            // Get item
            var media = _imageLibrary.GetMedia(id);
            if (media == null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, 0);
            }

            return Request.CreateResponse(HttpStatusCode.OK, media.MailshotUses);
        }

        [HttpGet]
        public HttpResponseMessage GetTags()
        {
            var results = _imageLibrary.GetTags();
            return Request.CreateResponse(HttpStatusCode.OK, results);
        }

        [HttpGet]
        public HttpResponseMessage GetPrivateImage(string url)
        {
            return GetPrivateImage(url, "original");
        }

        [HttpGet]
        public HttpResponseMessage GetPrivateImage(string url, string size)
        {
            bool umbracoAccess = false;
            // Check to see if the user is logged into Umbraco
            var umbracoUser = UmbracoContext.Security.CurrentUser;

            if (umbracoUser == null)
            {
                var authTicket = new HttpContextWrapper(HttpContext.Current).GetUmbracoAuthTicket();
                if (authTicket != null)
                {
                    var userName = authTicket.Name;
                    umbracoUser = UmbracoContext.Application.Services.UserService.GetByUsername(userName);
                }
            }

            if (umbracoUser != null)
            {
                // TODO: Double check this is the way to do it
                if (umbracoUser.UserType.Alias.ToLowerInvariant() == "admin")
                {
                    umbracoAccess = true;
                }
            }

            // Check to see if the user is logged into the front-end site
            if (!umbracoAccess)
            {
                var authResult = Authenticate();
                if (authResult != null)
                {
                    return authResult;
                }
            }

            if (string.IsNullOrEmpty(url))
            {
                return ErrorMessage(HttpStatusCode.BadRequest, "URL of the required image isn't specified.");
            }

            HttpResponseMessage result = ErrorMessage(HttpStatusCode.NotFound, "Image not found");

            var image = _imageLibrary.GetImageByBlobUrl(url) as PrivateLibraryImage;
            var userAccess = false;
            if (_loggedInMember != null)
            {
                userAccess = image.Username == _loggedInMember.Username;
            }

            if (userAccess || umbracoAccess)
            {
                string blobId;
                switch (size.ToLowerInvariant())
                {
                    case "small":
                        blobId = image.SmallThumbBlobId;
                        break;
                    case "medium":
                    case "large":
                        blobId = image.LargeThumbBlobId;
                        break;
                    case "original":
                    default:
                        blobId = image.OriginalBlobId;
                        break;
                }

                //TODO: Get the expiry from config
                BlobStorageHelper blobHelper = new BlobStorageHelper(ConfigHelper.PrivateStorageConnectionString, ConfigHelper.PrivateMediaBlobStorageContainer);
                var accessUrl = blobHelper.GetBlobUrlWithSas(blobId, 60);

                result = Request.CreateResponse(HttpStatusCode.Moved);
                result.Headers.Location = new Uri(accessUrl);
            }
            else
            {
                result = ErrorMessage(HttpStatusCode.Forbidden, "You do not have permission to view this image.");
            }

            return result;
        }

        [HttpPost]
        public HttpResponseMessage UploadImage(ImageUploadViewModel imageUpload)
        {
            var authResult = Authenticate();
            if (authResult != null)
            {
                return authResult;
            }

            if (string.IsNullOrEmpty(imageUpload.Name))
            {
                return ErrorMessage(HttpStatusCode.BadRequest, "You must provide a name for the image");
            }

            byte[] bytes = null;
            if (imageUpload.ImageData != null)
            {
                bytes = imageUpload.ImageData;
            }
            else
            {
                try
                {
                    bytes = Convert.FromBase64String(imageUpload.ImageString);
                }
                catch (Exception ex)
                {
                    _logger.Exception(this.GetType().Name, "UploadBase64Image", ex);
                    _logger.Error(this.GetType().Name, "UploadBase64Image", "A user tried to upload an invalid image.");
                }
            }

            if (bytes != null)
            {
                return UploadImage(bytes, imageUpload.Name);
            }
            else
            {
                return ErrorMessage(HttpStatusCode.BadRequest, "The image provided was not in the correct format.");
            }
        }

        private HttpResponseMessage UploadImage(byte[] bytes, string name)
        {
            // todo: check size of bytes[]

            PCL.Models.IMedia media = null;
            try
            {
                media = _imageLibrary.AddImage(bytes, name, _loggedInMember);
            }
            catch (Exception ex)
            {
                _logger.Exception(this.GetType().Name, "UploadImage", ex);
                _logger.Error(this.GetType().Name, "UploadImage", "Unable to save image to Umbraco.");
            }

            if (media == null)
            {
                return ErrorMessage(HttpStatusCode.InternalServerError, "Unable to save image.");
            }

            return Request.CreateResponse(HttpStatusCode.Created, media);
        }

        [HttpDelete]
        public HttpResponseMessage DeleteImage(int id)
        {
            var authResult = Authenticate();
            if (authResult != null)
            {
                return authResult;
            }

            // Find the image
            var image = _imageLibrary.GetImage(id, false) as PrivateLibraryImage;
            if (image == null)
            {
                return ErrorMessage(HttpStatusCode.NotFound, "The image was not found");
            }

            if (image.Username != _loggedInMember.Username)
            {
                return ErrorMessage(HttpStatusCode.Forbidden, "You do not have permission to delete this image");
            }

            bool success = _imageLibrary.DeleteImage(image);

            if (!success)
            {
                return ErrorMessage(HttpStatusCode.InternalServerError, "Unable to delete image.");
            }

            return Request.CreateResponse(HttpStatusCode.NoContent);
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