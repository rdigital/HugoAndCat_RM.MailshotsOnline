using Glass.Mapper.Umb;
using HC.RM.Common.Network;
using HC.RM.Common.PCL.Helpers;
using RM.MailshotsOnline.Data.Helpers;
using RM.MailshotsOnline.Entities.PageModels;
using RM.MailshotsOnline.Entities.ViewModels;
using RM.MailshotsOnline.PCL.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Configuration;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Mvc;

namespace RM.MailshotsOnline.Web.Controllers.SurfaceControllers
{
    public class ResetPasswordSurfaceController : SurfaceController
    {
        private readonly IMembershipService _membershipService;
        private readonly IEmailService _emailService;
        private readonly ILogger _logger;

        private const string RequestCompleteFlag = "RequestComplete";
        private const string ResetCompleteFlag = "ResetComplete";

        public ResetPasswordSurfaceController(IMembershipService membershipService, IEmailService emailService, ILogger logger)
        {
            _membershipService = membershipService;
            _emailService = emailService;
            _logger = logger;
        }

        [ChildActionOnly]
        public ActionResult ShowRequestResetForm(ResetPassword model)
        {
            if (TempData[ResetCompleteFlag] != null && (bool)TempData[ResetCompleteFlag])
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
            var emailBody = string.Empty;
            if (UmbracoContext.PageId.HasValue)
            {
                var pageModel = Umbraco.Content(UmbracoContext.PageId);
                emailBody = pageModel.RequestCompleteEmail.ToString();
            }

            if (!ModelState.IsValid)
            {
                return CurrentUmbracoPage();
            }

            var token = _membershipService.RequestPasswordReset(model.RequestViewModel.Email);

            if (token != null)
            {
                var resetLink = $"https://{Request.Url.Authority}/reset-password?token={token}";

                var recipients = new List<string>() { model.RequestViewModel.Email };
                var sender = new System.Net.Mail.MailAddress(ConfigHelper.SystemEmailAddress);
                _emailService.SendEmail(
                    recipients, 
                    "Password reset",
                    emailBody.Replace("##resetLink", $"<a href='{resetLink}'>{resetLink}</a>"),
                    System.Net.Mail.MailPriority.Normal,
                    sender);

                _logger.Info(this.GetType().Name, "RequestRestForm", "Password reset email sent to {0}.", model.RequestViewModel.Email);
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
            if (TempData[ResetCompleteFlag] != null && (bool)TempData[ResetCompleteFlag])
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

            var success = _membershipService.RedeemPasswordResetToken(Request.QueryString["token"], model.ResetViewModel.Password);

            if (success)
            {
                TempData[ResetCompleteFlag] = true;
                _logger.Info(this.GetType().Name, "ResetForm", "Password reset successful using token {0}", Request.QueryString["token"]);
            }
            else
            {
                _logger.Warn(this.GetType().Name, "ResetForm", "Password reset failed for token {0}", Request.QueryString["token"]);
            }

            return CurrentUmbracoPage();
        }

        public ActionResult ResetComplete(ResetPassword model)
        {
            return PartialView("~/Views/ResetPassword/Partials/ResetComplete.cshtml", model);
        }
    }
}