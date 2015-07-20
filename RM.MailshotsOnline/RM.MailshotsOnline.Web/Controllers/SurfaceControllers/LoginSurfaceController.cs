using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RM.MailshotsOnline.Entities.PageModels;
using RM.MailshotsOnline.Entities.ViewModels;
using Umbraco.Web.Mvc;

namespace RM.MailshotsOnline.Web.Controllers.SurfaceControllers
{
    public class LoginSurfaceController : SurfaceController
    {
        // GET: Login
        [ChildActionOnly]
        public ActionResult ShowLoginForm(Login model)
        {
            var viewModel = new LoginViewModel() {PageModel = model};

            return PartialView("~/Views/Partials/Login.cshtml", viewModel);
        }

        [HttpPost]
        public ActionResult LoginForm(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Something","Something went wrong (oh dear!)");
                return CurrentUmbracoPage();
            }

            return Redirect("/?loggedin=true");
        }
    }
}