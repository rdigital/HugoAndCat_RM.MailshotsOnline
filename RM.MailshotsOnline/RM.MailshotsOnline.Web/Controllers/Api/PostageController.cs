using HC.RM.Common.PCL.Helpers;
using RM.MailshotsOnline.PCL.Models;
using RM.MailshotsOnline.PCL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web;

namespace RM.MailshotsOnline.Web.Controllers.Api
{
    public class PostageController : ApiBaseController
    {
        private IPricingService _pricingService;

        public PostageController(IMembershipService membershipService, IPricingService pricingService, UmbracoContext umbracoContext, ILogger logger)
            : base(membershipService, umbracoContext, logger)
        {
            _pricingService = pricingService;
        }

        public PostageController(IMembershipService membershipService, IPricingService pricingService, ILogger logger)
            : base(membershipService, logger)
        {
            _pricingService = pricingService;        
        }

        [HttpGet]
        public IEnumerable<IPostalOption> Get()
        {
            return _pricingService.GetPostalOptions();
        }

        [HttpGet]
        public IEnumerable<IPostalOption> GetForFormat(int id)
        {
            return _pricingService.GetPostalOptions(id);
        }
    }
}