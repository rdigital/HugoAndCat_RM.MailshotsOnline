using RM.MailshotsOnline.Entities.MemberModels;
using RM.MailshotsOnline.PCL.Models;
using RM.MailshotsOnline.PCL.Services;
using RM.MailshotsOnline.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Web;
using Umbraco.Web.WebApi;

namespace RM.MailshotsOnline.Web.Controllers.Api
{
    public abstract class ApiBaseController : UmbracoApiController
    {
        internal IMember loggedInMember;

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
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                throw new HttpException(401, "You must be logged in.");
            }

            loggedInMember = _membershipService.GetCurrentMember();
        }
    }
}