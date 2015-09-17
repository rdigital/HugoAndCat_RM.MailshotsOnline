using Glass.Mapper.Umb;
using HC.RM.Common.PCL.Helpers;
using RM.MailshotsOnline.Entities.PageModels;
using RM.MailshotsOnline.PCL.Models;
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
    [Authorize]
    public class CaptureDetailsController : GlassController
    {
        private readonly ICampaignService _campaignService;
        private readonly IInvoiceService _invoiceService;
        private readonly IMembershipService _membershipService;

        public CaptureDetailsController(IUmbracoService umbracoService, ILogger logger, ICampaignService campaignService, IMembershipService membershipService, IInvoiceService invoiceService)
            : base(umbracoService, logger)
        {
            _campaignService = campaignService;
            _invoiceService = invoiceService;
            _membershipService = membershipService;
        }

        // GET: CaptureDetails
        public override ActionResult Index(RenderModel model)
        {
            // Fetch the Glass model of the page
            var pageModel = GetModel<CaptureDetails>();

            // Fetch the campaign
            ICampaign campaign = null;
            Guid campaignId = Guid.Empty;
            if (Guid.TryParse(Request.QueryString["campaignId"], out campaignId))
            {
                try
                {
                    campaign = _campaignService.GetCampaign(campaignId);
                }
                catch (Exception ex)
                {
                    _logger.Exception(this.GetType().Name, "Index", ex);
                    _logger.Error(this.GetType().Name, "Index", "Unable to get campaign for ID {0}", campaignId);
                }
            }

            if (campaign == null)
            {
                return Redirect(pageModel.CampaignListingPage.Url());
            }

            // Check that the user owns the campaign
            var loggedInMember = _membershipService.GetCurrentMember();
            if (campaign.UserId != loggedInMember.Id)
            {
                _logger.Error(this.GetType().Name, "Index", "Unauthorised attempt to get to campaign {0} by user {1}.", campaignId, loggedInMember.Id);
                return Redirect(pageModel.CampaignListingPage.Url());
            }

            var campaignHubUrl = pageModel.CampaignHubPage.Url();
            string token = "?";
            if (campaignHubUrl.Contains(token))
            {
                token = "&";
            }
            pageModel.BackToCampaignUrl = string.Format("{0}{1}campaignId={2}", campaignHubUrl, token, Request.QueryString["campaignId"]);

            // Check that an invoice has been created, is in draft and has an approval URL
            var invoice = _invoiceService
                .GetInvoicesForCampaign(campaign)
                .Where(i => i.Status == PCL.Enums.InvoiceStatus.Draft && !string.IsNullOrEmpty(i.PaypalApprovalUrl))
                .OrderByDescending(i => i.UpdatedDate)
                .FirstOrDefault();

            if (invoice == null)
            {
                // No good!  Send the user back to the campaign
                return Redirect(pageModel.BackToCampaignUrl);
            }

            return View("~/Views/CaptureDetails.cshtml", pageModel);
        }
    }
}