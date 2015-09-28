using Glass.Mapper.Umb;
using HC.RM.Common.PCL.Helpers;
using RM.MailshotsOnline.Entities.PageModels;
using RM.MailshotsOnline.PCL.Services;
using RM.MailshotsOnline.Web.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Models;

namespace RM.MailshotsOnline.Web.Controllers
{
    public class CreateCanvasController : GlassController
    {
        private IMembershipService _membershipService;
        private IMailshotsService _mailshotService;

        public CreateCanvasController(IUmbracoService umbracoService, ILogger logger, IMembershipService membershipService, IMailshotsService mailshotService)
            : base(umbracoService, logger)
        {
            _membershipService = membershipService;
            _mailshotService = mailshotService;
        }

        // GET: CreateCanvas
        public override ActionResult Index(RenderModel model)
        {
            // Fetch the Glass model of the page
            var contentPageModel = GetModel<CreateCanvas>();

            // Check if there's a mailshot ID in the query string and confirm the user owns it
            Guid mailshotId = Guid.Empty;
            if (Guid.TryParse(Request.QueryString["mailshotId"], out mailshotId))
            {
                // Check if the user is logged in
                var loggedInMember = _membershipService.GetCurrentMember();
                if (loggedInMember == null)
                {
                    var currentUrl = Request.RawUrl;
                    var loginRedirect = string.Format("{0}?returnUrl={1}", contentPageModel.LoginPage.Url(), Server.UrlEncode(currentUrl));
                    return Redirect(loginRedirect);
                }

                // Check if the user owns the mailshot
                if (!_mailshotService.MailshotBelongsToUser(mailshotId, loggedInMember.Id))
                {
                    return Redirect(contentPageModel.MyCampaignsPage.Url());
                }
            }

            return View("~/Views/CreateCanvas.cshtml", contentPageModel);
        }
    }
}