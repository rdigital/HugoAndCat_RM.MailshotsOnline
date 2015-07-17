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

        public PostageController(IMembershipService membershipService, IPricingService pricingService, UmbracoContext umbracoContext)
            : base(membershipService, umbracoContext)
        {
            _pricingService = pricingService;
        }

        public PostageController(IMembershipService membershipService, IPricingService pricingService)
            : base(membershipService)
        {
            _pricingService = pricingService;        
        }

        [HttpGet]
        public IEnumerable<IPostalOption> GetAllPostalOptions()
        {
            return _pricingService.GetPostalOptions();
        }

        [HttpGet]
        public IEnumerable<IPostalOption> GetPostalOptionForFormat(int formatId)
        {
            return _pricingService.GetPostalOptions(formatId);
        }
    }
}