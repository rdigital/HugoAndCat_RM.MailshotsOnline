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
    public class RegisterSurfaceController : SurfaceController
    {
        // GET: Register
        [ChildActionOnly]
        public ActionResult ShowRegisterForm(Register model)
        {
            // todo: get valid titles.

            var viewModel = new RegisterViewModel() { PageModel = model };

            return PartialView("~/Views/Partials/Register.cshtml", viewModel);
        }

        [HttpPost]
        public ActionResult RegisterForm(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Something", "Something went wrong in RegisterForm() (oh dear!)");
                return CurrentUmbracoPage();
            }

            return Redirect("/?registered=true");
        }
    }
}