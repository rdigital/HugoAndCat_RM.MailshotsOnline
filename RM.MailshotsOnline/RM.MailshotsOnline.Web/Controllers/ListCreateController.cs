using System;
using System.Web.Mvc;
using Glass.Mapper.Umb;
using HC.RM.Common.PCL.Helpers;
using RM.MailshotsOnline.Entities.DataModels;
using RM.MailshotsOnline.Entities.PageModels;
using RM.MailshotsOnline.PCL;
using RM.MailshotsOnline.PCL.Services;
using Umbraco.Web.Models;
using RM.MailshotsOnline.Web.Extensions;
using RM.MailshotsOnline.Web.Helpers;

namespace RM.MailshotsOnline.Web.Controllers
{
    [Authorize]
    public class ListCreateController : GlassController
    {
        private readonly IDataService _dataService;
        private readonly IMembershipService _membershipService;

        public ListCreateController(IUmbracoService umbracoService, ILogger logger, IDataService dataService, IMembershipService membershipService)
            : base(umbracoService, logger)
        {
            _dataService = dataService;
            _membershipService = membershipService;
        }

        //// GET: ListCreate
        //public override ActionResult Index(RenderModel model)
        //{
        //    return Index(model, Guid.Empty);
        //}

        // GET: ListCreate
        public ActionResult Index(RenderModel model, Guid? campaignId)
        {
            var pageModel = GetModel<ListCreate>();

            if (!campaignId.HasValue)
            {
                pageModel.BackUrl = pageModel.BackPage.Url();
                pageModel.ListDetailBaseUrl = pageModel.BackUrl;
            }
            else
            {
                pageModel.CampaignId = campaignId.ToString();
                pageModel.BackUrl = SiteUrlHelper.GetUrlForCampaignData(campaignId.Value);
                pageModel.ListDetailBaseUrl = pageModel.BackUrl + "/";
            }

            string rawListId = Request.QueryString["listId"];

            Guid listId = Guid.Empty;
            bool guidParsed = false;

            if (!string.IsNullOrEmpty(rawListId))
            {
                _logger.Info("ListCreateController", "Index", "Found listId {0} on query string.");

                guidParsed = Guid.TryParse(rawListId, out listId);

                if (!guidParsed)
                {
                    _logger.Warn("ListCreateController", "Index", "Unable to parse \"{0}\" into a valid Guid.");
                }
            }

            if (guidParsed)
            {
                var loggedInMember = _membershipService.GetCurrentMember();

                pageModel.DistributionList = _dataService.GetDistributionListForUser(loggedInMember.Id, listId);

                if (pageModel.DistributionList == null)
                {
                    _logger.Error("ListCreateController", "Index", "Distribution list \"{0}\" does not belong to user {1}.", listId, loggedInMember.Id);

                    return HttpNotFound("No distribution list found for that user.");
                }

                pageModel.CurrentStep = pageModel.DistributionList.ListState;
            }
            else
            {
                pageModel.CurrentStep = Enums.DistributionListState.AddNewContacts;

                pageModel.DistributionList = new DistributionList();
            }

            return View("~/Views/Lists/Create.cshtml", pageModel);
        }
    }
}