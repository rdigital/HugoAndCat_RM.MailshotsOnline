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
    [Authorize]
    public class MyOrdersController : GlassController
    {
        private IMembershipService _membershipService;
        private ICampaignService _campaignService;
        private ISettingsService _settingsService;

        public MyOrdersController(IUmbracoService umbracoService, ILogger logger, IMembershipService membershipService, ICampaignService campaignService, ISettingsService settingsService)
            : base(umbracoService, logger)
        {
            _membershipService = membershipService;
            _campaignService = campaignService;
            _settingsService = settingsService;
        }

        // GET: MyOrders
        public override ActionResult Index(RenderModel model)
        {
            // Fetch the Glass model of the page
            var pageModel = GetModel<MyOrders>();

            var loggedInMember = _membershipService.GetCurrentMember();

            // Get the settings
            pageModel.Settings = _settingsService.GetCurrentSettings();

            // Get Orders (campaigns that have invoices) from the service
            var campaigns = _campaignService.GetOrdersForUser(loggedInMember.Id);
            pageModel.Campaigns = campaigns;

            return View("~/Views/MyOrders.cshtml", pageModel);
        }
    }
}