﻿using Glass.Mapper.Umb;
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
using Profile = RM.MailshotsOnline.Entities.PageModels.Profile.Profile;

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
            var pageModel = GetModel<Profile>();
            pageModel.Member = _membershipService.GetCurrentMember();

            return View("~/Views/Profile/Overview.cshtml", pageModel);
        }
    }
}