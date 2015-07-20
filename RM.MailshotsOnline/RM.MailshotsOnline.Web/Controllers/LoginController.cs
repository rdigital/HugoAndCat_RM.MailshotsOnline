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
    public class LoginController : GlassController
    {
        public LoginController(IUmbracoService umbracoService)
            : base(umbracoService)
        {
        }

        // GET: Login
        public override ActionResult Index(RenderModel model)
        {
            var loginPageModel = GetModel<Login>();

            return View("~/Views/Login.cshtml", loginPageModel);
        }
    }
}