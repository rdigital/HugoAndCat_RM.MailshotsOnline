using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Glass.Mapper.Umb;
using RM.MailshotsOnline.Entities.PageModels;
using RM.MailshotsOnline.Entities.ViewModels;
using Umbraco.Web.Models;

namespace RM.MailshotsOnline.Web.Controllers
{
    public class RegisterController : GlassController
    {
        public RegisterController(IUmbracoService umbracoService)
            : base(umbracoService)
        {
        }

        // GET: Register
        public override ActionResult Index(RenderModel model)
        {
            var registerPageModel = GetModel<Register>();

            return View("~/Views/Register.cshtml", registerPageModel);
        }
    }
}