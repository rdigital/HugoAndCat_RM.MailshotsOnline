using HC.RM.Common.PCL.Helpers;
using RM.MailshotsOnline.Entities.PageModels;
using RM.MailshotsOnline.Entities.ViewModels;
using RM.MailshotsOnline.PCL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RM.MailshotsOnline.Web.Controllers.SurfaceControllers
{
    [Authorize]
    public class CampaignSurfaceController : BaseSurfaceController
    {
        public CampaignSurfaceController(IMembershipService membershipService, ILogger logger) 
            : base(membershipService, logger)
        {
        }

        [ChildActionOnly]
        public ActionResult ShowCheckoutButton(CampaignHub model)
        {
            var viewModel = new CampaignHubCheckoutViewModel()
            {
                PageModel = model
            };

            return PartialView("~/Views/CampaignHub/Partials/CheckoutButton.cshtml", viewModel);
        }

        public ActionResult Checkout(CampaignHubCheckoutViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return CurrentUmbracoPage();
            }

            // Try to get paypal stuff going
            bool paypalStuffWorked = true;
            string redirectUrl = "http://www.google.com/";

            if (paypalStuffWorked)
            {
                return Redirect(redirectUrl);
            }

            return CurrentUmbracoPage();
        }
    }
}