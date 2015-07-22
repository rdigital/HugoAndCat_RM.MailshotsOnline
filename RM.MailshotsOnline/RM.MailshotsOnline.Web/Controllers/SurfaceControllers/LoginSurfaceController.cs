using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RM.MailshotsOnline.Data.Services;
using RM.MailshotsOnline.Entities.PageModels;
using RM.MailshotsOnline.Entities.ViewModels;
using Umbraco.Web.Mvc;

namespace RM.MailshotsOnline.Web.Controllers.SurfaceControllers
{
    public class LoginSurfaceController : SurfaceController
    {
        private MembershipService _membershipService = new MembershipService();

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
                return CurrentUmbracoPage();
            }

            if (Members.Login(model.Email, model.Password))
            {
                // we're logged in

                TempData["LoggedIn"] = true;
                return Redirect("/?loggedin=true");
            }

            ModelState.AddModelError("BadLogin",
                "Your login has not been recognised. Please check that you have entered your details correctly");
            return CurrentUmbracoPage();
        }
    }
}