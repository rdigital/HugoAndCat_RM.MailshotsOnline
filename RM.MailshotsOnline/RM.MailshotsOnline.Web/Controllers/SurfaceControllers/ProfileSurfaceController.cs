using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Glass.Mapper.Umb;
using RM.MailshotsOnline.Data.Extensions;
using RM.MailshotsOnline.Entities.MemberModels;
using RM.MailshotsOnline.Entities.PageModels.Profile;
using RM.MailshotsOnline.Entities.ViewModels;
using RM.MailshotsOnline.PCL.Models;
using RM.MailshotsOnline.PCL.Services;
using Umbraco.Web.Models.ContentEditing;
using Umbraco.Web.Mvc;
using MarketingPreferences = RM.MailshotsOnline.Entities.PageModels.Profile.MarketingPreferences;
using UmbracoContext = Umbraco.Web.UmbracoContext;

namespace RM.MailshotsOnline.Web.Controllers.SurfaceControllers
{
    public class ProfileSurfaceController : BaseSurfaceController
    {
        private const string UpdatedFlag = "MarketingPreferencesUpdated";

        public ProfileSurfaceController(IMembershipService membershipService) : base(membershipService)
        {
        }

        public ProfileSurfaceController(IMembershipService membershipService, UmbracoContext umbracoContext)
            : base(membershipService, umbracoContext)
        {
        }

        [ChildActionOnly]
        public ActionResult ShowEditPersonalDetails(MarketingPreferences model)
        {
            return PartialView("~/Views/Profile/Partials/EditPersonalDetails.cshtml", model);
        }

        [HttpPost]
        public ActionResult EditPersonalDetails()
        {
            return CurrentUmbracoPage();
        }

        [ChildActionOnly]
        public ActionResult ShowEditOrganisationDetails(MarketingPreferences model)
        {
            return PartialView("~/Views/Profile/Partials/EditOrganisationDetails.cshtml", model);
        }

        [HttpPost]
        public ActionResult EditOrganisationDetails()
        {
            return CurrentUmbracoPage();
        }

        [ChildActionOnly]
        public ActionResult ShowEditMarketingPreferences(MarketingPreferences model)
        {
            //TODO: Check for the MarketingPreferencesUpdated flag and display the appropriate error message!  :D :D :D  WOOHOO!z0r
            if (TempData[UpdatedFlag] != null)
            {
                ViewBag.UpdateSuccess = (bool) TempData[UpdatedFlag];
            }

            var viewModel = new MarketingPreferencesViewModel() {PageModel = model};
            viewModel.ThirdPartyMarketingPreferences = (ContactOptions)model.Member.ThirdPartyMarketingPreferences;
            viewModel.RoyalMailMarketingPreferences = (ContactOptions)model.Member.RoyalMailMarketingPreferences;

            return PartialView("~/Views/Profile/Partials/EditMarketingPreferences.cshtml", viewModel);
        }

        [HttpPost]
        public ActionResult EditMarketingPreferences(MarketingPreferencesViewModel model)
        {
            LoggedInMember.RoyalMailMarketingPreferences = model.RoyalMailMarketingPreferences;
            LoggedInMember.ThirdPartyMarketingPreferences = model.ThirdPartyMarketingPreferences;

            if (MembershipService.Save(LoggedInMember))
            {
                TempData[UpdatedFlag] = true;
                return CurrentUmbracoPage();
            }

            TempData[UpdatedFlag] = false;
            //ViewBag.SuccessMessage = model.UpdateFailMessage;
            return CurrentUmbracoPage();
        }
    }
}