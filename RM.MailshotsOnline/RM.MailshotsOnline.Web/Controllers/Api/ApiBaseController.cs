using RM.MailshotsOnline.Entities.MemberModels;
using RM.MailshotsOnline.PCL.Models;
using RM.MailshotsOnline.PCL.Services;
using RM.MailshotsOnline.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Umbraco.Web;
using Umbraco.Web.WebApi;

namespace RM.MailshotsOnline.Web.Controllers.Api
{
    public abstract class ApiBaseController : UmbracoApiController
    {
        internal IMember _loggedInMember;

        internal IMembershipService _membershipService;

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

        internal void Authenticate()
        {
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
                    throw new HttpException(401, "You must be logged in.");
                }

                _loggedInMember = _membershipService.GetCurrentMember();
            }
        }
    }
}