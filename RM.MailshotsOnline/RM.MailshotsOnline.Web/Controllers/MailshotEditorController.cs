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
    //[Authorize]
    public class MailshotEditorController : GlassController
    {
        public MailshotEditorController(IUmbracoService umbracoService) 
            : base(umbracoService)
        {
        }

        // GET: Editor
        public override ActionResult Index(RenderModel model)
        {
            // Fetch the Glass model of the home page
            var editorModel = GetModel<MailshotEditor>();

            return View("~/Views/MailshotEditor.cshtml", editorModel);
        }
    }
}