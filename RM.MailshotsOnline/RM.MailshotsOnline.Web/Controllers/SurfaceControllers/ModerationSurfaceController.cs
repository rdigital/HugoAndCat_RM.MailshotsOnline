using HC.RM.Common.PCL.Helpers;
using RM.MailshotsOnline.Data.Helpers;
using RM.MailshotsOnline.Entities.PageModels;
using RM.MailshotsOnline.Entities.ViewModels;
using RM.MailshotsOnline.PCL.Models;
using RM.MailshotsOnline.PCL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RM.MailshotsOnline.Web.Controllers.SurfaceControllers
{
    public class ModerationSurfaceController : BaseSurfaceController
    {
        private ICampaignService _campaignService;
        private ISparqQueueService _sparqService;
        private const string CompletedFlag = "ModerationResultSaved";

        public ModerationSurfaceController(IMembershipService membershipService, ILogger logger, ICampaignService campaignService, ISparqQueueService sparqService)
            : base(membershipService, logger)
        {
            _campaignService = campaignService;
            _sparqService = sparqService;
        }

        [ChildActionOnly]
        public ActionResult ShowApprovalConfirmationButton(ModerationPage pageModel)
        {
            if (TempData[CompletedFlag] != null && (bool)TempData[CompletedFlag])
            {
                return Complete(pageModel);
            }

            Guid moderationId = Guid.Parse(Request.QueryString["moderationId"]);
            var viewModel = new ModerationApprovalViewModel() { PageModel = pageModel, ModerationId = moderationId };
            return PartialView("~/Views/Moderation/Partials/ApprovalConfirmation.cshtml", viewModel);
        }

        public ActionResult Approve(ModerationApprovalViewModel viewModel)
        {
            ICampaign campaign = null;
            try
            {
                campaign = _campaignService.GetCampaignByModerationId(viewModel.ModerationId);
            }
            catch (Exception ex)
            {
                Log.Exception(this.GetType().Name, "Approve", ex);
            }

            if (campaign == null)
            {
                Log.Error(this.GetType().Name, "Approve", "Campaign with Moderation ID {0} could not be found.");
                ModelState.AddModelError("ModerationId", "No campaign found with the given ID.");
                return CurrentUmbracoPage();
            }

            campaign.Status = PCL.Enums.CampaignStatus.ReadyForFulfilment;
            campaign.SystemNotes += string.Format("{0}{1:yyyy-MM-dd HH:mm:ss} - Campaign has been approved.", Environment.NewLine, DateTime.UtcNow);
            _campaignService.SaveCampaign(campaign);

            Log.Info(this.GetType().Name, "Approve", "Campaign with ID {0} has been approved", campaign.CampaignId);

            //TODO: Send the PDF off to print
            var baseUrl = string.Format("{0}://{1}:{2}", ConfigHelper.HostedScheme, ConfigHelper.HostedDomain, ConfigHelper.HostedPort);
            var postbackUrl = string.Format("{0}/Umbraco/Api/ProofPdf/JobReadyForPrint/{1}&campaignId={2}", baseUrl, campaign.Mailshot.MailshotId, campaign.CampaignId);
            _sparqService.SendRenderAndPrintJob(campaign.Mailshot, postbackUrl);

            TempData[CompletedFlag] = true;
            return CurrentUmbracoPage();
        }

        private ActionResult Complete(ModerationPage pageModel)
        {
            return PartialView("~/Views/Moderation/Partials/Complete.cshtml", pageModel);
        }
    }
}