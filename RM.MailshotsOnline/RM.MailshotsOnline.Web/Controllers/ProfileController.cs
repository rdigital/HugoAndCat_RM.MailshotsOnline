using Glass.Mapper.Umb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using RM.MailshotsOnline.Data.Extensions;
using RM.MailshotsOnline.Entities.PageModels.Profile;
using RM.MailshotsOnline.PCL.Services;
using RM.MailshotsOnline.Web.Extensions;
using umbraco;
using Umbraco.Core.Services;
using Umbraco.Web.Models;

namespace RM.MailshotsOnline.Web.Controllers
{
    [Authorize]
    public class ProfileController : GlassController
    {
        private readonly IMembershipService _membershipService;

        public ProfileController(IUmbracoService umbracoService, IMembershipService membershipService)
            : base(umbracoService)
        {
            _membershipService = membershipService;
        }

        // GET: Profile
        public override ActionResult Index(RenderModel model)
        {
            var pageModel = GetModel<Overview>();
            var member = _membershipService.GetCurrentMember();

            if (member == null)
            {
                return RedirectToRoute("/");
            }

            if(!string.IsNullOrEmpty(member.Title))
            {
                var title = Services.DataTypeService.GetPreValues("Title Dropdown").FirstOrDefault(x => x.Key.Equals(member.Title));

                if (title.Value != null)
                {
                    member.Title = title.Value;
                }
            }

            pageModel.Member = member;

            return View("~/Views/Profile/Overview.cshtml", pageModel);
        }
    }
}