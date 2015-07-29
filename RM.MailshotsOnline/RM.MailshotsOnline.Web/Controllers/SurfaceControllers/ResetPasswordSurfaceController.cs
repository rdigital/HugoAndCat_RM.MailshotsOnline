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
    public class ResetPasswordSurfaceController : SurfaceController
    {
        private readonly MembershipService _membershipService = new MembershipService();
        private readonly EmailService _emailService = new EmailService();

        private const string CompletedFlag = "ResetComplete";

        [ChildActionOnly]
        public ActionResult ShowRequestResetForm(RequestResetPassword model)
        {
            if (TempData[CompletedFlag] != null && (bool)TempData[CompletedFlag])
            {
                return RequestComplete(model);
            }

            model.ViewModel = new RequestResetPasswordViewModel();

            return PartialView("~/Views/ResetPassword/Partials/ShowRequestResetForm.cshtml", model);
        }

        [HttpPost]
        public ActionResult RequestResetForm(RequestResetPassword model)
        {
            if (!ModelState.IsValid)
            {
                return CurrentUmbracoPage();
            }

            var token = _membershipService.RequestPasswordReset(model.ViewModel.Email);

            if (token != null)
            {
                _emailService.SendMail("msol@hugoandcat.com", "jgriffin@hugoandcat.com", "Password reset", $"Your token: {token}");
            }

            TempData[CompletedFlag] = true;

            return CurrentUmbracoPage();
        }

        public ActionResult RequestComplete(RequestResetPassword model)
        {
            return PartialView("~/Views/ResetPassword/Partials/RequestComplete.cshtml", model);
        }

        [ChildActionOnly]
        public ActionResult ShowResetForm(ResetPassword model)
        {
            if (TempData[CompletedFlag] != null && (bool) TempData[CompletedFlag])
            {
                return ResetComplete(model);
            }

            model.ViewModel = new ResetPasswordViewModel();

            return PartialView("~/Views/ResetPassword/Partials/ShowResetForm.cshtml", model);
        }
        [HttpPost]
        public ActionResult ResetForm(ResetPassword model)
        {
            if (!ModelState.IsValid)
            {
                return CurrentUmbracoPage();
            }

            _membershipService.SetNewPassword(_membershipService.GetCurrentMember(), model.ViewModel.Password);

            TempData[CompletedFlag] = true;

            return CurrentUmbracoPage();
        }

        public ActionResult ResetComplete(ResetPassword model)
        {
            return PartialView("~/Views/ResetPassword/Partials/ResetComplete.cshtml", model);
        }
    }
}