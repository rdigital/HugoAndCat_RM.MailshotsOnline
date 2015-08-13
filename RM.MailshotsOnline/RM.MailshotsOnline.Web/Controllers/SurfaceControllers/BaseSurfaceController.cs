using HC.RM.Common.PCL.Helpers;
using RM.MailshotsOnline.PCL.Models;
using RM.MailshotsOnline.PCL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Web;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;

namespace RM.MailshotsOnline.Web.Controllers.SurfaceControllers
{
    public class BaseSurfaceController : SurfaceController
    {
        internal IMember LoggedInMember;
        internal readonly IMembershipService MembershipService;
        internal readonly ILogger Log;

        public BaseSurfaceController(IMembershipService membershipService, UmbracoContext umbracoContext, ILogger logger)
            : base(umbracoContext)
        {
            MembershipService = membershipService;
            LoggedInMember = MembershipService.GetCurrentMember();
            Log = logger;
        }

        public BaseSurfaceController(IMembershipService membershipService, ILogger logger)
        {
            MembershipService = membershipService;
            LoggedInMember = MembershipService.GetCurrentMember();
            Log = logger;
        }
    }
}