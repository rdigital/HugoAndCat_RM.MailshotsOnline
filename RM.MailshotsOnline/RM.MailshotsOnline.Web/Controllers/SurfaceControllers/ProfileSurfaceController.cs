using Glass.Mapper.Umb;
using HC.RM.Common.Azure.Extensions;
using HC.RM.Common.PCL.Helpers;
using RM.MailshotsOnline.Data.Extensions;
using RM.MailshotsOnline.Entities.MemberModels;
using RM.MailshotsOnline.Entities.PageModels.Profile;
using RM.MailshotsOnline.Entities.ViewModels;
using RM.MailshotsOnline.PCL.Models;
using RM.MailshotsOnline.PCL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Umbraco.Web.Models.ContentEditing;
using Umbraco.Web.Mvc;
using MarketingPreferences = RM.MailshotsOnline.Entities.PageModels.Profile.MarketingPreferences;
using UmbracoContext = Umbraco.Web.UmbracoContext;

namespace RM.MailshotsOnline.Web.Controllers.SurfaceControllers
{
    [Authorize]
    public class ProfileSurfaceController : BaseSurfaceController
    {
        private const string UpdatedFlag = "MarketingPreferencesUpdated";

        public ProfileSurfaceController(IMembershipService membershipService, ILogger logger) 
            : base(membershipService, logger)
        {
        }

        public ProfileSurfaceController(IMembershipService membershipService, UmbracoContext umbracoContext, ILogger logger)
            : base(membershipService, umbracoContext, logger)
        {
        }

        [ChildActionOnly]
        public ActionResult ShowEditPersonalDetails(PersonalDetails model)
        {
            if (TempData[UpdatedFlag] != null)
            {
                ViewBag.UpdateSuccess = (bool) TempData[UpdatedFlag];
            }

            model.TitleOptions = Services.DataTypeService.GetPreValues("Title Dropdown");

            var viewModel = new PersonalDetailsViewModel()
            {
                Title = LoggedInMember.Title,
                FirstName = LoggedInMember.FirstName,
                LastName = LoggedInMember.LastName,
                EmailAddress = LoggedInMember.EmailAddress
            };

            viewModel.PageModel = model;

            return PartialView("~/Views/Profile/Partials/EditPersonalDetails.cshtml", viewModel);
        }

        [HttpPost]
        public ActionResult EditPersonalDetails(PersonalDetailsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return CurrentUmbracoPage();
            }

            var originalEmailAddress = LoggedInMember.EmailAddress;
            LoggedInMember.Title = model.Title;
            LoggedInMember.FirstName = model.FirstName;
            LoggedInMember.LastName = model.LastName;
            LoggedInMember.EmailAddress = model.EmailAddress;

            try
            {
                if (MembershipService.Save(originalEmailAddress, LoggedInMember))
                {
                    Log.Info(this.GetType().Name, "EditPersonalDetails", "Personal details updated for user {0}.", LoggedInMember.Username);
                    TempData[UpdatedFlag] = true;
                    return CurrentUmbracoPage();
                }
            }
            catch (Exception ex)
            {
                Log.Error(this.GetType().Name, "EditPersonalDetails", "Error updating personal details for user {0}: {1}.", LoggedInMember.Username, ex.Message);
            }

            Log.Warn(this.GetType().Name, "EditPersonalDetails", "Personal details update failed for user {0}.", LoggedInMember.Username);
            TempData[UpdatedFlag] = false;
            return CurrentUmbracoPage();
        }

        [ChildActionOnly]
        public ActionResult ShowChangePassword(PersonalDetails model)
        {
            if (TempData[UpdatedFlag] != null)
            {
                ViewBag.UpdateSuccess = (bool) TempData[UpdatedFlag];
            }

            var viewModel = new PasswordViewModel();

            viewModel.PageModel = model;

            return PartialView("~/Views/Profile/Partials/ChangePassword.cshtml", viewModel);
        }

        [HttpPost]
        public ActionResult ChangePassword(PasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return CurrentUmbracoPage();
            }

            if (!Membership.Provider.ValidateUser(LoggedInMember.Username, model.CurrentPassword))
            {
                ModelState.AddModelError("WrongPassword", "Your current password is incorrect. Please enter it again.");
                Log.Warn(this.GetType().Name, "ChangePassword", "Incorrect password entered when user {0} attempted to change password.", LoggedInMember.Username);
                return CurrentUmbracoPage();
            }

            try
            {
                if (MembershipService.SetNewPassword(LoggedInMember, model.NewPassword))
                {
                    Log.Info(this.GetType().Name, "ChangePassword", "Password changed for user {0}.", LoggedInMember.Username);
                    TempData[UpdatedFlag] = true;
                    return CurrentUmbracoPage();
                }
            }
            catch (Exception ex)
            {
                Log.Error(this.GetType().Name, "ChangePassword", "Error changing password for user {0}: {1}.", LoggedInMember.Username, ex.Message);
            }

            Log.Warn(this.GetType().Name, "ChangePassword", "Unable to change password for user {0}.", LoggedInMember.Username);
            TempData[UpdatedFlag] = false;
            return CurrentUmbracoPage();
        }

        [ChildActionOnly]
        public ActionResult ShowEditOrganisationDetails(OrganisationDetails model)
        {
            if (TempData[UpdatedFlag] != null)
            {
                ViewBag.UpdateSuccess = (bool)TempData[UpdatedFlag];
            }

            var viewModel = new OrganisationDetailsViewModel() { PageModel = model };
            viewModel.OrganisationName = model.Member.OrganisationName;
            viewModel.JobTitle = model.Member.JobTitle;
            viewModel.FlatNumber = model.Member.FlatNumber;
            viewModel.BuildingNumber = model.Member.BuildingNumber;
            viewModel.BuildingName = model.Member.BuildingName;
            viewModel.Address1 = model.Member.Address1;
            viewModel.Address2 = model.Member.Address2;
            viewModel.City = model.Member.City;
            viewModel.Postcode = model.Member.Postcode;
            viewModel.Country = model.Member.Country;
            viewModel.WorkPhoneNumber = model.Member.WorkPhoneNumber;
            viewModel.MobilePhoneNumber = model.Member.MobilePhoneNumber;

            return PartialView("~/Views/Profile/Partials/EditOrganisationDetails.cshtml", viewModel);
        }

        [HttpPost]
        public ActionResult EditOrganisationDetails(OrganisationDetailsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return CurrentUmbracoPage();
            }

            LoggedInMember.OrganisationName = model.OrganisationName;
            LoggedInMember.JobTitle = model.JobTitle;
            LoggedInMember.FlatNumber = model.FlatNumber;
            LoggedInMember.BuildingNumber = model.BuildingNumber;
            LoggedInMember.BuildingName = model.BuildingName;
            LoggedInMember.Address1 = model.Address1;
            LoggedInMember.Address2 = model.Address2;
            LoggedInMember.City = model.City;
            LoggedInMember.Postcode = model.Postcode;
            LoggedInMember.Country = model.Country;
            LoggedInMember.WorkPhoneNumber = model.WorkPhoneNumber;
            LoggedInMember.MobilePhoneNumber = model.MobilePhoneNumber;

            try
            {
                if (MembershipService.Save(LoggedInMember.EmailAddress, LoggedInMember))
                {
                    Log.Info(this.GetType().Name, "EditOrganisationDetails", "Organisation details updated for user {0}.", LoggedInMember.Username);
                    TempData[UpdatedFlag] = true;
                    return CurrentUmbracoPage();
                }
            }
            catch(Exception ex)
            {
                Log.Error(this.GetType().Name, "EditOrganisationDetails", "Error when updating organisation details for user {0}: {1}", LoggedInMember.Username, ex.Message);
            }

            Log.Warn(this.GetType().Name, "EditOrganisationDetails", "Unable to update Organisation details for user {0}.", LoggedInMember.Username);
            TempData[UpdatedFlag] = false;
            return CurrentUmbracoPage();
        }

        [ChildActionOnly]
        public ActionResult ShowEditMarketingPreferences(MarketingPreferences model)
        {
            if (TempData[UpdatedFlag] != null)
            {
                ViewBag.UpdateSuccess = (bool) TempData[UpdatedFlag];
            }

            var viewModel = new MarketingPreferencesViewModel() {PageModel = model};
            viewModel.ThirdPartyMarketingPreferences = (ContactOptions) model.Member.ThirdPartyMarketingPreferences;
            viewModel.RoyalMailMarketingPreferences = (ContactOptions) model.Member.RoyalMailMarketingPreferences;

            return PartialView("~/Views/Profile/Partials/EditMarketingPreferences.cshtml", viewModel);
        }

        [HttpPost]
        public ActionResult EditMarketingPreferences(MarketingPreferencesViewModel model)
        {
            LoggedInMember.RoyalMailMarketingPreferences = model.RoyalMailMarketingPreferences;
            LoggedInMember.ThirdPartyMarketingPreferences = model.ThirdPartyMarketingPreferences;

            try
            {
                if (MembershipService.Save(LoggedInMember.EmailAddress, LoggedInMember))
                {
                    Log.Info(this.GetType().Name, "EditMarketingPreferences", "Marketing prefernces updated for user {0}.", LoggedInMember.Username);
                    TempData[UpdatedFlag] = true;
                    return CurrentUmbracoPage();
                }
            }
            catch (Exception ex)
            {
                Log.Error(this.GetType().Name, "EditMarketingPreferences", "Error when updating marketing preferences for user {0}: {1}", LoggedInMember.Username, ex.Message);
            }

            Log.Warn(this.GetType().Name, "EditMarketingPreferences", "Unable to update marketing preferences for user {0}.", LoggedInMember.Username);
            TempData[UpdatedFlag] = false;
            return CurrentUmbracoPage();
        }
    }
}