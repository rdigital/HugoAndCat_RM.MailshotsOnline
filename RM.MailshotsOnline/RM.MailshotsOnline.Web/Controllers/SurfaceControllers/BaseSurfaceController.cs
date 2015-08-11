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
using System.Web.Mvc;
using Umbraco.Web;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;

namespace RM.MailshotsOnline.Web.Controllers.SurfaceControllers
{
    public class BaseSurfaceController : SurfaceController
    {
        internal IMember LoggedInMember;
        internal readonly IMembershipService MembershipService;
        internal readonly TelemetryClient Telemetry = new TelemetryClient();

        public BaseSurfaceController(IMembershipService membershipService, UmbracoContext umbracoContext)
            : base(umbracoContext)
        {
            MembershipService = membershipService;
            LoggedInMember = MembershipService.GetCurrentMember();
        }

        public BaseSurfaceController(IMembershipService membershipService)
        {
            MembershipService = membershipService;
            LoggedInMember = MembershipService.GetCurrentMember();
        }
    }
}