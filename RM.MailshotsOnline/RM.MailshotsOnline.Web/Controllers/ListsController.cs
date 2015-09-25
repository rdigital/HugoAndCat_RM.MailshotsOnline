using Glass.Mapper.Umb;
using HC.RM.Common.PCL.Helpers;
using RM.MailshotsOnline.Entities.PageModels;
using System.Web.Mvc;
using RM.MailshotsOnline.PCL.Services;
using Umbraco.Web.Models;

namespace RM.MailshotsOnline.Web.Controllers
{
    [Authorize]
    public class ListsController : GlassController
    {
        private readonly IDataService _dataService;
        private readonly IMembershipService _membershipService;

        public ListsController(IUmbracoService umbracoService, ILogger logger, IDataService dataService, IMembershipService membershipService)
            : base(umbracoService, logger)
        {
            _dataService = dataService;
            _membershipService = membershipService;
        }

        // GET: Lists
        public override ActionResult Index(RenderModel model)
        {
            // Fetch the Glass model of the page
            var pageModel = GetModel<Lists>();

            var loggedInMember = _membershipService.GetCurrentMember();

            var distributionLists = _dataService.GetDistributionListsForUser(loggedInMember.Id);

            pageModel.UsersLists = distributionLists;

            return View("~/Views/Lists/Lists.cshtml", pageModel);
        }

        public ActionResult ListsEmpty(RenderModel model)
        {
            // Fetch the Glass model of the page
            var pageModel = GetModel<Lists>();

            return View("~/Views/ListsEmpty.cshtml", pageModel);
        }
    }
}