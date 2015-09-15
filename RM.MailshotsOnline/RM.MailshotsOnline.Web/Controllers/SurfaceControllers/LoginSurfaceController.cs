using HC.RM.Common.Azure;
using HC.RM.Common.PCL.Helpers;
using RM.MailshotsOnline.Entities.PageModels;
using RM.MailshotsOnline.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using HC.RM.Common;
using RM.MailshotsOnline.Data.Constants;
using Umbraco.Web.Mvc;

namespace RM.MailshotsOnline.Web.Controllers.SurfaceControllers
{
    public class LoginSurfaceController : SurfaceController
    {
        private const string BadLoginFlag = "BadLogin";
        private readonly ILogger _log;

        public LoginSurfaceController(ILogger logger)
        {
            _log = logger;
        }

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

            var computedSalt = Encryption.ComputedSalt(model.Email, model.Email);
            var b64Salt = Encoding.UTF8.GetBytes(computedSalt);
            var encryptedEmail = Encryption.Encrypt(model.Email, Constants.Encryption.EncryptionKey, b64Salt);

            var member = Services.MemberService.GetByEmail(encryptedEmail);

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
                else
                {
                    _log.Warn(this.GetType().Name, "LoginForm", "Bad login request for email {0}.", model.Email);
                }
            }

            TempData[BadLoginFlag] = true;
            return CurrentUmbracoPage();
        }
    }
}