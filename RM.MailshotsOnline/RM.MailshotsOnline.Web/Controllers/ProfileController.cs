using Glass.Mapper.Umb;
using RM.MailshotsOnline.Entities.PageModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Umbraco.Web.Models;

namespace RM.MailshotsOnline.Web.Controllers
{
    public class ProfileController : GlassController
    {
        public ProfileController(IUmbracoService umbracoService)
            : base(umbracoService)
        {
        }

        // GET: Profile
        public override ActionResult Index(RenderModel model)
        {
            var pageModel = GetModel<Profile>();

            return View("~/Views/Profile/Overview.cshtml", pageModel);
        }
    }
}