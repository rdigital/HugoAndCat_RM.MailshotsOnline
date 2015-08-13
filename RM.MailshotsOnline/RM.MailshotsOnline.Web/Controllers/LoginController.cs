using Glass.Mapper.Umb;
using HC.RM.Common.PCL.Helpers;
using RM.MailshotsOnline.Entities.PageModels;
using RM.MailshotsOnline.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Models;

namespace RM.MailshotsOnline.Web.Controllers
{
    public class LoginController : GlassController
    {
        public LoginController(IUmbracoService umbracoService, ILogger logger)
            : base(umbracoService, logger)
        {
        }

        // GET: Login
        public override ActionResult Index(RenderModel model)
        {
            if (Umbraco.MemberIsLoggedOn())
            {
                return Redirect("/");
            }

            var loginPageModel = GetModel<Login>();

            return View("~/Views/Login/Login.cshtml", loginPageModel);
        }
    }
}