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
using UmbracoContext = Umbraco.Web.UmbracoContext;

namespace RM.MailshotsOnline.Web.Controllers.SurfaceControllers
{
    public class ProfileSurfaceController : BaseSurfaceController
    {
        public ProfileSurfaceController(IMembershipService membershipService) : base(membershipService)
        {
        }

        public ProfileSurfaceController(IMembershipService membershipService, UmbracoContext umbracoContext)
            : base(membershipService, umbracoContext)
        {
        }

        [ChildActionOnly]
        public ActionResult ShowEditMarketingPreferences(MarketingPreferences model)
        {
            return PartialView("~/Views/Profile/Partials/EditMarketingPreferences.cshtml", model);
        }

        [HttpPost]
        public ActionResult EditMarketingPreferences(MarketingPreferences model)
        {
            LoggedInMember.CanWeContactByEmail = model.ViewModel.RoyalMailContactPreferences.Email;
            LoggedInMember.CanWeContactByPhone = model.ViewModel.RoyalMailContactPreferences.Phone;
            LoggedInMember.CanWeContactByPost = model.ViewModel.RoyalMailContactPreferences.Post;
            LoggedInMember.CanWeContactBySmsAndOther = model.ViewModel.RoyalMailContactPreferences.SmsAndOther;

            LoggedInMember.CanThirdPatiesContactByEmail = model.ViewModel.ThirdPartyContactPreferences.Email;
            LoggedInMember.CanThirdPatiesContactByPhone = model.ViewModel.ThirdPartyContactPreferences.Phone;
            LoggedInMember.CanThirdPatiesContactByPost = model.ViewModel.ThirdPartyContactPreferences.Post;
            LoggedInMember.CanThirdPatiesContactBySmsAndOther = model.ViewModel.ThirdPartyContactPreferences.SmsAndOther;

            if (MembershipService.Save(LoggedInMember))
            {
                ViewBag.UpdateSuccess = model.UpdateSuccessMessage;
                return CurrentUmbracoPage();
            }

            ViewBag.UpdateSuccess = model.UpdateFailMessage;
            return CurrentUmbracoPage();
        }
    }
}