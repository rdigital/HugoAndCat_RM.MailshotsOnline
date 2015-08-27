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
    [Authorize]
    public class CampaignHubController : GlassController
    {
        private readonly ICampaignService _campaignService;
        private readonly IMembershipService _membershipService;
        private readonly IPricingService _pricingService;

        public CampaignHubController(IUmbracoService umbracoService, ILogger logger, ICampaignService campaignService, IMembershipService membershipService, IPricingService pricingService)
            : base(umbracoService, logger)
        {
            _campaignService = campaignService;
            _membershipService = membershipService;
            _pricingService = pricingService;
        }


        // GET: CampaignHub
        public override ActionResult Index(RenderModel model)
        {
            // Fetch the Glass model of the home page
            var pageModel = GetModel<CampaignHub>();

            // Check to see if there's a Campaign in the query string
            Guid campaignId = Guid.Empty;
            if (!Guid.TryParse(Request.QueryString["campaignId"], out campaignId))
            {
                pageModel.DisplayNoCampaignMessage = true;
                return ReturnView(pageModel);
            }

            var campaign = _campaignService.GetCampaign(campaignId);
            if (campaign == null)
            {
                pageModel.DisplayCampaignErrorMessage = true;
                return ReturnView(pageModel);
            }

            var loggedInMember = _membershipService.GetCurrentMember();
            if (campaign.UserId != loggedInMember.Id)
            {
                pageModel.DisplayCampaignErrorMessage = true;
                return ReturnView(pageModel);
            }

            pageModel.Campaign = campaign;
            pageModel.LoggedInMember = loggedInMember;

            var postageOptions = _pricingService.GetPostalOptions();
            pageModel.PostalOptions = postageOptions;

            var priceSummary = _pricingService.GetPriceBreakdown(campaign);
            pageModel.PriceBreakdown = priceSummary;

            return View("~/Views/CampaignHub.cshtml", pageModel);
        }

        private ActionResult ReturnView(CampaignHub pageModel)
        {
            return View("~/Views/CampaignHub.cshtml", pageModel);
        }
    }
}