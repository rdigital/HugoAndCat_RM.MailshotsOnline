using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RM.MailshotsOnline.Entities.PageModels;
using Umbraco.Core.Models;
using Umbraco.Web.Mvc;

namespace RM.MailshotsOnline.Web.Controllers.SurfaceControllers
{
    public class LogoutSurfaceController : SurfaceController
    {
        // GET: LogoutSurface
        public ActionResult Index(Logout model)
        {
            Members.Logout();

            return PartialView("~/Views/Logout/Partials/Logout.cshtml", model);
        }
    }
}