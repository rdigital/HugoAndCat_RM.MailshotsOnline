using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Configuration;
using System.Web;
using System.Web.Mvc;
using Glass.Mapper.Umb;
using RM.MailshotsOnline.Data.Services;
using RM.MailshotsOnline.Entities.PageModels;
using RM.MailshotsOnline.Entities.ViewModels;
using RM.MailshotsOnline.PCL.Services;
using Umbraco.Web.Mvc;

namespace RM.MailshotsOnline.Web.Controllers.SurfaceControllers
{
    public class ResetPasswordSurfaceController : SurfaceController
    {
        private readonly IMembershipService _membershipService;
        private readonly IEmailService _emailService;

        private const string RequestCompleteFlag = "RequestComplete";
        private const string ResetCompleteFlag = "ResetComplete";

        public ResetPasswordSurfaceController(IMembershipService membershipService, IEmailService emailService)
        {
            _membershipService = membershipService;
            _emailService = emailService;
        }

        [ChildActionOnly]
        public ActionResult ShowRequestResetForm(ResetPassword model)
        {
            if (TempData[ResetCompleteFlag] != null && (bool) TempData[ResetCompleteFlag])
            {
                return ResetComplete(model);
            }

            if (TempData[RequestCompleteFlag] != null && (bool)TempData[RequestCompleteFlag])
            {
                return RequestComplete(model);
            }

            model.RequestViewModel = new RequestResetPasswordViewModel();

            return PartialView("~/Views/ResetPassword/Partials/ShowRequestResetForm.cshtml", model);
        }

        [HttpPost]
        public ActionResult RequestResetForm(ResetPassword model)
        {
            if (!ModelState.IsValid)
            {
                return CurrentUmbracoPage();
            }

            var token = _membershipService.RequestPasswordReset(model.RequestViewModel.Email);

            if (token != null)
            {
                var resetLink = Request.Url.Authority + "/reset-password?token=" + token;

            _emailService.SendMail(ConfigurationManager.AppSettings["SystemEmailAddress"],
                    model.RequestViewModel.Email, "Password reset",
                    $"Click here mate: <a href='{resetLink}'>{resetLink}</a>");
            }

            TempData[RequestCompleteFlag] = true;

            return CurrentUmbracoPage();
        }

        public ActionResult RequestComplete(ResetPassword model)
        {
            return PartialView("~/Views/ResetPassword/Partials/RequestComplete.cshtml", model);
        }

        [ChildActionOnly]
        public ActionResult ShowResetForm(ResetPassword model)
        {
            if (TempData[ResetCompleteFlag] != null && (bool) TempData[ResetCompleteFlag])
            {
                return ResetComplete(model);
            }

            model.ResetViewModel = new ResetPasswordViewModel();

            return PartialView("~/Views/ResetPassword/Partials/ShowResetForm.cshtml", model);
        }

        [HttpPost]
        public ActionResult ResetForm(ResetPassword model)
        {
            if (!ModelState.IsValid)
            {
                return CurrentUmbracoPage();
            }

            _membershipService.RedeemPasswordResetToken(Request.QueryString["token"], model.ResetViewModel.Password);

            TempData[ResetCompleteFlag] = true;

            return CurrentUmbracoPage();
        }

        public ActionResult ResetComplete(ResetPassword model)
        {
            return PartialView("~/Views/ResetPassword/Partials/ResetComplete.cshtml", model);
        }
    }
}