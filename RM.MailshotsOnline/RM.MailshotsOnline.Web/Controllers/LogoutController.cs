using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Glass.Mapper.Umb;
using RM.MailshotsOnline.Entities.PageModels;
using umbraco;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;
using Umbraco.Web.Security;

namespace RM.MailshotsOnline.Web.Controllers
{
    public class LogoutController : GlassController
    {
        public LogoutController(IUmbracoService umbracoService)
            : base(umbracoService)
        {
        }

        // GET: Logout
        public override ActionResult Index(RenderModel model)
        {
            var memberShipHelper = new MembershipHelper(UmbracoContext);
            if (memberShipHelper.IsLoggedIn())
            {
                memberShipHelper.Logout();

                return Redirect(Request.RawUrl);
            }

            var logoutPageModel = GetModel<Logout>();

            return View("~/Views/Logout/Logout.cshtml", logoutPageModel);
        }
    }
}