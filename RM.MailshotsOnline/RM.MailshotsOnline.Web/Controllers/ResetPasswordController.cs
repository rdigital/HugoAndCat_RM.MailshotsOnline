using Glass.Mapper.Umb;
using HC.RM.Common.PCL.Helpers;
using RM.MailshotsOnline.Entities.PageModels;
using RM.MailshotsOnline.PCL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Models;

namespace RM.MailshotsOnline.Web.Controllers
{
    public class ResetPasswordController : GlassController
    {
        private readonly IMembershipService _membershipService;

        public ResetPasswordController(IUmbracoService umbracoService, IMembershipService membershipService, ILogger logger)
            : base(umbracoService, logger)
        {
            _membershipService = membershipService;
        }

        // GET: ResetPassword
        public ActionResult Index(RenderModel model, string token)
        {
            var pageModel = GetModel<ResetPassword>();

            if (token == null)
            {
                return View("~/Views/ResetPassword/RequestResetPassword.cshtml", pageModel);
            }

            var member = _membershipService.GetMemberByPasswordResetToken(token);

            if (member != null)
            {
                return View("~/Views/ResetPassword/ResetPassword.cshtml", pageModel);
            }

            ViewBag.BadTokenMessage = pageModel.BadTokenMessage;
            _logger.Info(this.GetType().Name, "Index", "Bad reset password token supplied to reset password page.");

            return View("~/Views/ResetPassword/RequestResetPassword.cshtml", pageModel);
        }
    }
}