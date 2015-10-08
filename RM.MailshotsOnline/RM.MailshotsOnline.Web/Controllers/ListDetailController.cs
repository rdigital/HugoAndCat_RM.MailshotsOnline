using Glass.Mapper.Umb;
using HC.RM.Common.PCL.Helpers;
using RM.MailshotsOnline.Entities.PageModels;
using System;
using System.Web.Mvc;
using Umbraco.Web.Models;

namespace RM.MailshotsOnline.Web.Controllers
{
    [Authorize]
    public class ListDetailController : GlassController
    {
        public ListDetailController(IUmbracoService umbracoService, ILogger logger)
            : base(umbracoService, logger)
        {
        }

        // GET: ListDetail
        public ActionResult Index(RenderModel model, Guid distributionListId)
        {
            // Fetch the Glass model of the page
            var pageModel = GetModel<ListDetail>();

            pageModel.DistributionListId = distributionListId;

            return View("~/Views/ListDetail.cshtml", pageModel);
        }

        public ActionResult ListDetailEmpty(RenderModel model)
        {
            // Fetch the Glass model of the page
            var pageModel = GetModel<ListDetail>();

            return View("~/Views/ListDetailEmpty.cshtml", pageModel);
        }
    }
}