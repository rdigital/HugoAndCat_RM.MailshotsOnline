using Microsoft.ApplicationInsights;
using RM.MailshotsOnline.Entities.MemberModels;
using RM.MailshotsOnline.PCL.Models;
using RM.MailshotsOnline.PCL.Services;
using RM.MailshotsOnline.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using Umbraco.Web;
using Umbraco.Web.WebApi;

namespace RM.MailshotsOnline.Web.Controllers.Api
{
    public abstract class ApiBaseController : UmbracoApiController
    {
        internal IMember _loggedInMember;

        internal IMembershipService _membershipService;

        internal readonly TelemetryClient _telemetry = new TelemetryClient();

        // Used for mocking
        public ApiBaseController(IMembershipService membershipService, UmbracoContext umbracoContext)
            : base(umbracoContext)
        {
            _membershipService = membershipService;
        }

        public ApiBaseController(IMembershipService membershipService)
        {
            _membershipService = membershipService;
        }

        internal HttpResponseMessage Authenticate()
        {
            HttpResponseMessage result = null;

            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["SpoofUser"]))
            {
                int userId = int.Parse(ConfigurationManager.AppSettings["SpoofUser"]);
                _loggedInMember = new Member()
                {
                    Id = userId,
                    FirstName = "Test",
                    LastName = "User",
                    EmailAddress = "ext-mradford@hugoandcat.com",
                    Title = "Mr"
                };
            }
            else
            {
                if (!HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    result = Request.CreateResponse(HttpStatusCode.Unauthorized, new { error = "You must be logged in.", statusCode = HttpStatusCode.Unauthorized });
                }
                else
                {
                    _loggedInMember = _membershipService.GetCurrentMember();
                }
            }

            return result;
        }

        internal HttpResponseMessage ErrorMessage(HttpStatusCode statusCode, string errorMessage)
        {
            return Request.CreateResponse(statusCode, new { error = errorMessage, statusCode = statusCode });
        }
    }
}