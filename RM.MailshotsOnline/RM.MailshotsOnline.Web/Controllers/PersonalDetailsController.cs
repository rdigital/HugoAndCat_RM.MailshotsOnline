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
    public class PersonalDetailsController : GlassController
    {
        private readonly IMembershipService _membershipService;

        public PersonalDetailsController(IUmbracoService umbracoService, IMembershipService membershipService)
            : base(umbracoService)
        {
            _membershipService = membershipService;
        }

        public override ActionResult Index(RenderModel model)
        {
            var pageModel = GetModel<PersonalDetails>();
            pageModel.Member = _membershipService.GetCurrentMember();

            return View("~/Views/Profile/PersonalDetails.cshtml", pageModel);
        }
    }
}