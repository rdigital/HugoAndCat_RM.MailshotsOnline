using Glass.Mapper.Umb;
using HC.RM.Common.PCL.Helpers;
using RM.MailshotsOnline.PCL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Models;
using Profile = RM.MailshotsOnline.Entities.PageModels.Profile.Profile;

namespace RM.MailshotsOnline.Web.Controllers
{
    [Authorize]
    public class ProfileController : GlassController
    {
        private readonly IMembershipService _membershipService;

        public ProfileController(IUmbracoService umbracoService, IMembershipService membershipService, ILogger logger)
            : base(umbracoService, logger)
        {
            _membershipService = membershipService;
        }

        // GET: Profile
        public override ActionResult Index(RenderModel model)
        {
            var pageModel = GetModel<Profile>();
            pageModel.Member = _membershipService.GetCurrentMember();

            return View("~/Views/Profile/Overview.cshtml", pageModel);
        }
    }
}