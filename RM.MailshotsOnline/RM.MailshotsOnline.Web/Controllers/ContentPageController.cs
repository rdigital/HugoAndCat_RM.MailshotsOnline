using Glass.Mapper.Umb;
using HC.RM.Common.PCL.Helpers;
using RM.MailshotsOnline.Entities.PageModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Models;

namespace RM.MailshotsOnline.Web.Controllers
{
    public class ContentPageController : GlassController
    {
        public ContentPageController(IUmbracoService umbracoService, ILogger logger)
            : base(umbracoService, logger)
        {
        }

        // GET: ContentPage
        public override ActionResult Index(RenderModel model)
        {
            // Fetch the Glass model of the home page
            var contentPageModel = GetModel<ContentPage>();

            return View("~/Views/ContentPage.cshtml", contentPageModel);
        }

        public ActionResult MemberAPITestPage(RenderModel model)
        {
            // Fetch the Glass model of the home page
            var contentPageModel = GetModel<ContentPage>();

            return View("~/Views/MemberAPITestPage.cshtml", contentPageModel);
        }

        public ActionResult ImageLibraryAPITestPage(RenderModel model)
        {
            // Fetch the Glass model of the home page
            var contentPageModel = GetModel<ContentPage>();

            return View("~/Views/ImageLibraryAPITestPage.cshtml", contentPageModel);
        }

        public ActionResult MyCampaignsAPITestPage(RenderModel model)
        {
            // Fetch the Glass model of the home page
            var contentPageModel = GetModel<ContentPage>();

            return View("~/Views/MyCampaignsAPITestPage.cshtml", contentPageModel);
        }
    }
}