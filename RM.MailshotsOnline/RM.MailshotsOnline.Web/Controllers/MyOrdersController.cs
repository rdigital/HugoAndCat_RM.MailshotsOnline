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

        public MyOrdersController(IUmbracoService umbracoService, ILogger logger, IMembershipService membershipService, ICampaignService campaignService)
            : base(umbracoService, logger)
        {
            _membershipService = membershipService;
            _campaignService = campaignService;
        }

        // GET: MyOrders
        public override ActionResult Index(RenderModel model)
        {
            // Fetch the Glass model of the page
            var pageModel = GetModel<MyOrders>();

            var loggedInMember = _membershipService.GetCurrentMember();

            // Get Orders (campaigns that have invoices) from the service
            var campaigns = _campaignService.GetOrdersForUser(loggedInMember.Id);
            pageModel.Campaigns = campaigns;

            return View("~/Views/MyOrders.cshtml", pageModel);
        }
    }
}