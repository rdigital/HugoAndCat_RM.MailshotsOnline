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
    public class RegisterController : GlassController
    {
        public RegisterController(IUmbracoService umbracoService, ILogger logger)
            : base(umbracoService, logger)
        {
        }

        // GET: Register
        public override ActionResult Index(RenderModel model)
        {
            var registerPageModel = GetModel<Register>();

            return View("~/Views/Register/Register.cshtml", registerPageModel);
        }
    }
}