using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Glass.Mapper.Umb;
using RM.MailshotsOnline.Data.Services;
using RM.MailshotsOnline.Entities.PageModels;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;

namespace RM.MailshotsOnline.Web.Controllers
{
    public class ResetPasswordController : GlassController
    {
        private readonly MembershipService _membershipService = new MembershipService();

        public ResetPasswordController(IUmbracoService umbracoService)
            : base(umbracoService)
        {
        }

        // GET: ResetPassword
        public override ActionResult Index(RenderModel model)
        {
            var pageModel = GetModel<RequestResetPassword>();

            return View("~/Views/ResetPassword/RequestResetPassword.cshtml", pageModel);
        }

        // GET: Validate
        public ActionResult Validate(string token)
        {
            var member = _membershipService.GetMemberByPasswordResetToken(token);

            if (member != null)
            {
                var pageModel = GetModel<ResetPassword>();

                return View("~/Views/ResetPassword/ResetPassword.cshtml", pageModel);
            }

            ModelState.AddModelError("BadToken",
                "The login link you're trying to use is expired or invalid. Please request a new one using the form below.");

            return Index(GetModel<RenderModel>());
        }
    }
}