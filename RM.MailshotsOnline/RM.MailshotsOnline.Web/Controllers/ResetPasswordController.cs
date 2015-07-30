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
using RM.MailshotsOnline.PCL.Services;

namespace RM.MailshotsOnline.Web.Controllers
{
    public class ResetPasswordController : GlassController
    {
        private readonly IMembershipService _membershipService;

        public ResetPasswordController(IUmbracoService umbracoService, IMembershipService membershipService)
            : base(umbracoService)
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

            ViewBag.BadToken = "Your token is the worst";
            
            return View("~/Views/ResetPassword/RequestResetPassword.cshtml", pageModel);
        }
    }
}