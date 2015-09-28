using Glass.Mapper.Umb;
using HC.RM.Common.PCL.Helpers;
using RM.MailshotsOnline.Entities.PageModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Models;

namespace RM.MailshotsOnline.Web.Controllers
{
    [Authorize]
    public class MyCampaignsController : GlassController
    {
        public MyCampaignsController(IUmbracoService umbracoService, ILogger logger)
            : base(umbracoService, logger)
        {
        }

        // GET: MyCampaigns
        public override ActionResult Index(RenderModel model)
        {
            // Fetch the Glass model of the page
            var contentPageModel = GetModel<MyCampaigns>();

            return View("~/Views/MyCampaigns.cshtml", contentPageModel);
        }
    }
}