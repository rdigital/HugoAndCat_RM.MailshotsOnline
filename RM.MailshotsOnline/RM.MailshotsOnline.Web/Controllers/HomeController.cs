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
    public class HomeController : GlassController
    {
        public HomeController(IUmbracoService umbracoService) : base(umbracoService)
        {
        }

        // GET: Home
        public override ActionResult Index(RenderModel model)
        {
            // Fetch the Glass model of the home page
            var homepageModel = GetModel<Home>();
            return View("~/Views/Home.cshtml", homepageModel);
        }
    }
}