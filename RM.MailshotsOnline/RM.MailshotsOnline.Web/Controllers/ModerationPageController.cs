using Glass.Mapper.Umb;
using HC.RM.Common.PCL.Helpers;
using RM.MailshotsOnline.Entities.PageModels;
using RM.MailshotsOnline.PCL.Models;
using RM.MailshotsOnline.PCL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Models;

namespace RM.MailshotsOnline.Web.Controllers
{
    public class ModerationPageController : GlassController
    {
        private ICampaignService _campaignService;

        public ModerationPageController(IUmbracoService umbracoService, ILogger logger, ICampaignService campaignService)
            : base(umbracoService, logger)
        {
            _campaignService = campaignService;
        }

        // GET: ModerationPage
        public override ActionResult Index(RenderModel model)
        {
            // Fetch the Glass model of the page
            var pageModel = GetModel<ModerationPage>();

            // Get the campaign from the moderation ID
            Guid moderationId = Guid.Empty;
            ICampaign campaign = null;
            if (Guid.TryParse(Request.QueryString["moderationId"], out moderationId))
            {
                campaign = _campaignService.GetCampaignByModerationId(moderationId);
            }

            if (campaign == null)
            {
                _logger.Error(this.GetType().Name, "Index", "Unable to get campaign for moderation ID {0}.", Request.QueryString["moderationId"]);
                return Redirect("/");
            }

            if (string.IsNullOrEmpty(Request.QueryString["action"]))
            {
                return Redirect("/");
            }

            var action = Request.QueryString["action"].ToLowerInvariant();
            bool noAction = false;
            switch (action)
            {
                case "approve":
                    pageModel.DisplayApprovedMessage = true;
                    break;
                case "reject":
                    pageModel.DisplayRejectedMessage = true;
                    break;
                default:
                    noAction = true;
                    break;
            }

            if (noAction)
            {
                return Redirect("/");
            }

            return View("~/Views/ModerationPage.cshtml", pageModel);
        }
    }
}