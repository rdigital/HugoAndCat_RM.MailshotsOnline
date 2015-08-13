using HC.RM.Common.PCL.Helpers;
using RM.MailshotsOnline.Data.Extensions;
using RM.MailshotsOnline.Entities.MemberModels;
using RM.MailshotsOnline.Entities.PageModels;
using RM.MailshotsOnline.Entities.ViewModels;
using RM.MailshotsOnline.PCL.Services;
using System;
using System.Configuration;
using System.Runtime.InteropServices;
using System.Web.Mvc;
using System.Web.Security;
using umbraco;
using Umbraco.Web.Mvc;

namespace RM.MailshotsOnline.Web.Controllers.SurfaceControllers
{
    public class RegisterSurfaceController : SurfaceController
    {
        private readonly IMembershipService _membershipService;
        private readonly IEmailService _emailService;
        private const string CompletedFlag = "RegistrationComplete";
        private readonly ILogger _logger;

        public RegisterSurfaceController(IMembershipService membershipService, IEmailService emailService, ILogger logger)
        {
            _membershipService = membershipService;
            _emailService = emailService;
            _logger = logger;
        }

        // GET: Register
        [ChildActionOnly]
        public ActionResult ShowRegisterForm(Register model)
        {
            if (TempData[CompletedFlag] != null && (bool) TempData[CompletedFlag])
            {
                return Complete(model);
            }

            model.TitleOptions = Services.DataTypeService.GetPreValues("Title Dropdown");
            model.ViewModel = new RegisterViewModel();

            return PartialView("~/Views/Register/Partials/ShowRegisterForm.cshtml", model);
        }

        [HttpPost]
        public ActionResult RegisterForm(Register model)
        {
            var emailBody = string.Empty;
            if (UmbracoContext.PageId.HasValue)
            {
                var pageModel = Umbraco.Content(UmbracoContext.PageId);
                emailBody = pageModel.RegisterCompleteEmail.ToString();
            }

            if (!ModelState.IsValid)
            {
                return CurrentUmbracoPage();
            }

            if (Members.GetByEmail(model.ViewModel.Email) != null)
            {
                _logger.Info(this.GetType().Name, "RegisterForm", "Duplicate registration attempted.");
                ModelState.AddModelError("ViewModel.Email", model.AlreadyRegisteredMessage);
                return CurrentUmbracoPage();
            }

            try
            {
                CreateMember(model.ViewModel);
            }
            catch (MembershipPasswordException)
            {
                _logger.Info(this.GetType().Name, "RegisterForm", "Registration attempt with low quality password.");
                ModelState.AddModelError("PasswordError",
                    model.PasswordErrorMessage);
                return CurrentUmbracoPage();
            }
            catch (Exception ex)
            {
                _logger.Error(this.GetType().Name, "RegisterForm", "Registration attempt failed with error: {0}", ex.Message);
                ModelState.AddModelError("PasswordError", "There was an error and you have not been registered.  Please try again later.");
                return CurrentUmbracoPage();
            }

            TempData[CompletedFlag] = true;
            _logger.Info(this.GetType().Name, "RegisterForm", "New registration complete.");
            _emailService.SendMail(ConfigurationManager.AppSettings["SystemEmailAddress"], model.ViewModel.Email,
                "Welcome to the Royal Mail Business Portal",
                emailBody.Replace("##firstName", model.ViewModel.FirstName)
                    .Replace("##email", model.ViewModel.Email));

            return CurrentUmbracoPage();
        }

        public ActionResult Complete(Register model)
        {
            return PartialView("~/Views/Register/Partials/RegisterComplete.cshtml", model);
        }

        private void CreateMember(RegisterViewModel model)
        {
            _membershipService.CreateMember(new Member()
            {
                EmailAddress = model.Email,
                Title = model.Title,
                FirstName = model.FirstName,
                LastName = model.LastName,
                IsApproved = false,
                IsLockedOut = false,
                RoyalMailMarketingPreferences = new ContactOptions()
                {
                    Post = model.MarketingPreferencesViewModel.RoyalMailMarketingPreferences.Post,
                    Email = model.MarketingPreferencesViewModel.RoyalMailMarketingPreferences.Email,
                    Phone = model.MarketingPreferencesViewModel.RoyalMailMarketingPreferences.Phone,
                    SmsAndOther = model.MarketingPreferencesViewModel.RoyalMailMarketingPreferences.SmsAndOther
                },
                ThirdPartyMarketingPreferences = new ContactOptions()
                {
                    Post = model.MarketingPreferencesViewModel.ThirdPartyMarketingPreferences.Post,
                    Email = model.MarketingPreferencesViewModel.ThirdPartyMarketingPreferences.Email,
                    Phone = model.MarketingPreferencesViewModel.ThirdPartyMarketingPreferences.Phone,
                    SmsAndOther = model.MarketingPreferencesViewModel.ThirdPartyMarketingPreferences.SmsAndOther
                },
                Postcode = model.OrganisationDetailsViewModel.Postcode,
                OrganisationName = model.OrganisationDetailsViewModel.OrganisationName,
                JobTitle = model.OrganisationDetailsViewModel.JobTitle,
                FlatNumber = model.OrganisationDetailsViewModel.FlatNumber,
                BuildingNumber = model.OrganisationDetailsViewModel.BuildingNumber,
                BuildingName = model.OrganisationDetailsViewModel.BuildingName,
                Address1 = model.OrganisationDetailsViewModel.Address1,
                Address2 = model.OrganisationDetailsViewModel.Address2,
                City = model.OrganisationDetailsViewModel.City,
                Country = model.OrganisationDetailsViewModel.Country,
                WorkPhoneNumber = model.OrganisationDetailsViewModel.WorkPhoneNumber,
                MobilePhoneNumber = model.OrganisationDetailsViewModel.MobilePhoneNumber
            }, model.Password);
        }
    }
}