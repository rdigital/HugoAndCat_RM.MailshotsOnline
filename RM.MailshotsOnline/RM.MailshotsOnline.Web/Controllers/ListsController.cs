using System;
using Glass.Mapper.Umb;
using HC.RM.Common.PCL.Helpers;
using RM.MailshotsOnline.Entities.PageModels;
using System.Web.Mvc;
using RM.MailshotsOnline.Entities.DataModels;
using RM.MailshotsOnline.PCL.Models;
using RM.MailshotsOnline.PCL.Services;
using Umbraco.Web.Models;

namespace RM.MailshotsOnline.Web.Controllers
{
    [Authorize]
    public class ListsController : GlassController
    {
        private readonly IDataService _dataService;
        private readonly IMembershipService _membershipService;
        private readonly ICampaignService _campaignService;

        public ListsController(IUmbracoService umbracoService, ILogger logger, IDataService dataService,
            IMembershipService membershipService, ICampaignService campaignService)
            : base(umbracoService, logger)
        {
            _dataService = dataService;
            _membershipService = membershipService;
            _campaignService = campaignService;
        }

        // GET: Lists
        public override ActionResult Index(RenderModel model)
        {
            // Fetch the Glass model of the page
            var pageModel = GetModel<Lists>();

            var loggedInMember = _membershipService.GetCurrentMember();

            var distributionLists = _dataService.GetDistributionListsForUser(loggedInMember.Id);

            pageModel.UsersLists = distributionLists;

            return View("~/Views/Lists.cshtml", pageModel);
        }

        // GET: Lists
        public ActionResult Index(RenderModel model, Guid? campaignId)
         {
            // Fetch the Glass model of the page
            var pageModel = GetModel<Lists>();

            ICampaign campaign = null;
            if (campaignId.HasValue)
            {
                campaign = _campaignService.GetCampaign(campaignId.Value);
            }

            if (campaign == null)
            {
                return HttpNotFound();
            }

            var currentMember = _membershipService.GetCurrentMember();

            if (currentMember == null || currentMember.Id != campaign.UserId)
            {
                return HttpNotFound();
            }

            var distributionLists = _dataService.GetDistributionListsForUser(currentMember.Id);

            pageModel.UsersLists = distributionLists;

            //pageModel.UsersLists = campaign.DistributionLists;
            pageModel.Campaign = campaign;

            return View("~/Views/Lists.cshtml", pageModel);
        }

        public ActionResult ListsEmpty(RenderModel model)
        {
            // Fetch the Glass model of the page
            var pageModel = GetModel<Lists>();

            return View("~/Views/ListsEmpty.cshtml", pageModel);
        }
    }
}