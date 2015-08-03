using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RM.MailshotsOnline.Data.Services;
using RM.MailshotsOnline.Entities.PageModels;
using RM.MailshotsOnline.Entities.ViewModels;
using RM.MailshotsOnline.Web.Extensions;
using Umbraco.Web.Mvc;

namespace RM.MailshotsOnline.Web.Controllers.SurfaceControllers
{
    public class LoginSurfaceController : SurfaceController
    {
        // GET: Login
        [ChildActionOnly]
        public ActionResult ShowLoginForm(Login model)
        {
            var viewModel = new LoginViewModel() { PageModel = model };
            viewModel.ResetPasswordUrl = model.PasswordResetPage.Url(Umbraco);
            return PartialView("~/Views/Login/Partials/ShowLoginForm.cshtml", viewModel);
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
                var returnUrl = Request.QueryString["ReturnUrl"];
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                return Redirect("/");
            }

            ModelState.AddModelError("BadLogin", model.PageModel.BadLoginMessage);
            return CurrentUmbracoPage();
        }
    }
}