using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RM.MailshotsOnline.Entities.PageModels;
using RM.MailshotsOnline.Entities.ViewModels;
using RM.MailshotsOnline.Web.Extensions;
using Umbraco.Web.Mvc;

namespace RM.MailshotsOnline.Web.Controllers.SurfaceControllers
{
    public class LoginSurfaceController : SurfaceController
    {
        private const string BadLoginFlag = "BadLogin";

        [ChildActionOnly]
        public ActionResult ShowLoginForm(Login model)
        {
            if (TempData[BadLoginFlag] != null && (bool) TempData[BadLoginFlag])
            {
                ViewBag.ErrorMessage = model.BadLoginMessage;
            }

            var viewModel = new LoginViewModel() { PageModel = model };

            return PartialView("~/Views/Login/Partials/ShowLoginForm.cshtml", viewModel);
        }

        [HttpPost]
        public ActionResult LoginForm(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return CurrentUmbracoPage();
            }

            var member = Services.MemberService.GetByEmail(model.Email);

            if (member != null)
            {
                if (Members.Login(member.Username, model.Password))
                {
                    // we're logged in
                    var returnUrl = Request.QueryString["ReturnUrl"];
                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }

                    return Redirect("/");
                }
            }

            TempData[BadLoginFlag] = true;
            return CurrentUmbracoPage();
        }
    }
}