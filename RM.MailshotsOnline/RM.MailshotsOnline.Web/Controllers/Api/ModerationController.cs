using HC.RM.Common.PCL.Helpers;
using RM.MailshotsOnline.PCL.Models;
using RM.MailshotsOnline.PCL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace RM.MailshotsOnline.Web.Controllers.Api
{
    public class ModerationController : ApiBaseController
    {
        private ICampaignService _campaignService;
        private IMailshotsService _mailshotService;
        private IPricingService _pricingService;
        private IInvoiceService _invoiceService;

        public ModerationController(
            ICampaignService campaignService, 
            IMailshotsService mailshotService, 
            IPricingService pricingService, 
            IMembershipService membershipService, 
            ILogger logger,
            IInvoiceService invoiceService)
            : base (membershipService, logger)
        {
            _campaignService = campaignService;
            _mailshotService = mailshotService;
            _pricingService = pricingService;
            _invoiceService = invoiceService;
        }

        [HttpGet]
        public HttpResponseMessage Approve(Guid id)
        {
            // Get Campaign based on ID
            ICampaign campaign = null;
            var result = GetCampaignByModerationId(id, out campaign);
            if (result != null)
            {
                return result;
            }

            campaign.SystemNotes += string.Format("{0}{1:yyyy-MM-dd HH:mm:ss} - Campaign has passed moderation.", Environment.NewLine, DateTime.UtcNow);
            campaign.Status = PCL.Enums.CampaignStatus.ReadyForFulfilment;
            _campaignService.SaveCampaign(campaign);

            return Request.CreateResponse(HttpStatusCode.OK, "Thank you - the campaign has been approved");
        }

        private HttpResponseMessage GetCampaignByModerationId(Guid moderationId, out ICampaign campaign)
        {
            campaign = _campaignService.GetCampaignByModerationId(moderationId);

            if (campaign == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "Unable to find campaign with the given ID");
            }

            if (campaign.Status != PCL.Enums.CampaignStatus.PendingModeration)
            {
                _logger.Warn(this.GetType().Name, "GetCampaignByModerationId", "Campaign with moderation ID {0} is not awaiting moderation.", moderationId);
                return Request.CreateResponse(HttpStatusCode.Conflict, "The campaign is not currently awaiting moderation.");
            }

            return null;
        }
    }
}