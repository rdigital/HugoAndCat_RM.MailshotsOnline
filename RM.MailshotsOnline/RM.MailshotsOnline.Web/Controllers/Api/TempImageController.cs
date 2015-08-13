using HC.RM.Common.PCL.Helpers;
using RM.MailshotsOnline.Data.Helpers;
using RM.MailshotsOnline.PCL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using RM.MailshotsOnline.Web.Helpers;
using Umbraco.Web;

namespace RM.MailshotsOnline.Web.Controllers.Api
{
    public class TempImageController : ApiBaseController
    {
        public TempImageController(IMembershipService membershipService, ILogger logger) 
            : base(membershipService, logger)
        {

        }
        // GET: TempImage
        public HttpResponseMessage Get(int id)
        {
            bool umbracoAccess = false;
            // Check to see if the user is logged into Umbraco
            var umbracoUser = UmbracoContext.Security.CurrentUser;
            if (umbracoUser != null)
            {
                // TODO: Double check this is the way to do it
                if (umbracoUser.UserType.Alias.ToLowerInvariant() == "administrator")
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

            // Find image with given ID
            var mediaItem = Umbraco.TypedMedia(id);
            if (mediaItem != null)
            {
                if (mediaItem.DocumentTypeAlias.ToLowerInvariant() == ConfigHelper.PrivateImageContentTypeAlias.ToLowerInvariant())
                {
                    if (!umbracoAccess)
                    {
                        // Check that the user has access
                        var ownerId = mediaItem.GetPropertyValue<int>("ownerid");
                        if (ownerId != _loggedInMember.Id)
                        {
                            return ErrorMessage(HttpStatusCode.Forbidden, "No access to requested media item.");
                        }
                    }

                    // Get the blob information
                    var blobId = mediaItem.GetPropertyValue<string>("blobid");
                    var blobConnectionString = ConfigHelper.StorageConnectionString;
                    var blobContainerName = ConfigHelper.PrivateMediaBlobStorageContainer;

                    BlobStorageHelper blobStore = new BlobStorageHelper(blobConnectionString, blobContainerName);
                    var bytes = blobStore.FetchBytes(blobId);

                    var result = Request.CreateResponse(HttpStatusCode.OK, bytes);
                    result.Content.Headers.ContentType.MediaType = "application/pdf";
                }
                else
                {
                    // Wrong media type
                    return ErrorMessage(HttpStatusCode.BadRequest, "The requested media item is not a private image.");
                }
            }
            else
            {
                // Media item not found
                return ErrorMessage(HttpStatusCode.NotFound, "The requested media item was not found.");
            }

            // TODO: Remove this!
            return Request.CreateResponse(HttpStatusCode.InternalServerError, new { error = "Didn't work!  Not ready yet." });
        }
    }
}