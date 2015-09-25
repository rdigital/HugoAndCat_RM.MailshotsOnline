﻿using Glass.Mapper.Umb;
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
    [Authorize]
    public class OrderDetailsController : GlassController
    {
        private IMembershipService _membershipService;
        private ICampaignService _campaignService;
        private ISettingsService _settingsService;

        public OrderDetailsController(IUmbracoService umbracoService, ILogger logger, IMembershipService membershipService, ICampaignService campaignService, ISettingsService settingsService)
            : base(umbracoService, logger)
        {
            _membershipService = membershipService;
            _campaignService = campaignService;
            _settingsService = settingsService;
        }

        // GET: OrderDetails
        public override ActionResult Index(RenderModel model)
        {
            // Fetch the Glass model of the page
            var pageModel = GetModel<OrderDetails>();

            var loggedInMember = _membershipService.GetCurrentMember();

            ICampaign campaign = null;
            Guid campaignId;
            if (Guid.TryParse(Request.QueryString["campaignId"], out campaignId))
            {
                campaign = _campaignService.GetCampaignWithInvoices(campaignId);
            }

            if (campaign == null)
            {
                // Display error message
                pageModel.DisplayCampaignErrorMessage = true;
                return View("~/Views/OrderDetails.cshtml", pageModel);
            }

            if (campaign.UserId != loggedInMember.Id)
            {
                // Display error message
                pageModel.DisplayCampaignErrorMessage = true;
                return View("~/Views/OrderDetails.cshtml", pageModel);
            }

            pageModel.Campaign = campaign;

            // Get the My Orders page (should be the parent page)
            var parentPageId = CurrentPage.Parent.Id;
            var myOrdersPage = _umbracoService.CreateType<MyOrders>(_umbracoService.ContentService.GetPublishedVersion(parentPageId), false, false);
            pageModel.MyOrdersPage = myOrdersPage;

            // Get the settings
            pageModel.MyOrdersPage.Settings = _settingsService.GetCurrentSettings();

            return View("~/Views/OrderDetails.cshtml", pageModel);
        }
    }
}