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
    public class CreateCanvasController : GlassController
    {
        public CreateCanvasController(IUmbracoService umbracoService, ILogger logger)
            : base(umbracoService, logger)
        {
        }

        // GET: CreateCanvas
        public override ActionResult Index(RenderModel model)
        {
            // Fetch the Glass model of the home page
            var contentPageModel = GetModel<CreateCanvas>();

            return View("~/Views/CreateCanvas.cshtml", contentPageModel);
        }
    }
}