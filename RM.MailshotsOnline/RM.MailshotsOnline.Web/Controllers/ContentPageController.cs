using Glass.Mapper.Umb;
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
        public ContentPageController(IUmbracoService umbracoService)
            : base(umbracoService)
        {
        }

        // GET: ContentPage
        public ActionResult Index(RenderModel model)
        {
            // Fetch the Glass model of the home page
            var contentPageModel = GetModel<ContentPage>();
            return View("~/Views/ContentPage.cshtml", contentPageModel);
        }
    }
}