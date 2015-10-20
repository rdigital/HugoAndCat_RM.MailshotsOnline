using HC.RM.Common;
using HC.RM.Common.Network;
using HC.RM.Common.PCL.Helpers;
using RM.MailshotsOnline.Data.Constants;
using RM.MailshotsOnline.Data.Extensions;
using RM.MailshotsOnline.Data.Helpers;
using RM.MailshotsOnline.Entities.MemberModels;
using RM.MailshotsOnline.Entities.PageModels;
using RM.MailshotsOnline.Entities.ViewModels;
using RM.MailshotsOnline.PCL.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Security;
using Umbraco.Web.Mvc;
using RM.MailshotsOnline.Web.Extensions;

namespace RM.MailshotsOnline.Web.Controllers.SurfaceControllers
{
    public class RegisterSurfaceController : SurfaceController
    {
        private readonly IMembershipService _membershipService;
        private readonly IEmailService _emailService;
        private readonly ILogger _logger;
        private readonly ICryptographicService _cryptographicService;
        private const string CompletedFlag = "RegistrationComplete";

        public RegisterSurfaceController(IMembershipService membershipService, IEmailService emailService, ILogger logger, ICryptographicService cryptographicService)
        {
            _membershipService = membershipService;
            _emailService = emailService;
            _logger = logger;
            _cryptographicService = cryptographicService;
        }

        [ChildActionOnly]
        public ActionResult ShowRegisterForm(Register model)
        {
            if (TempData[CompletedFlag] != null && (bool)TempData[CompletedFlag])
            {
                return Complete(model);
            }

            //model.TitleOptions = Services.DataTypeService.GetPreValuesWithPlaceholder("Title Dropdown", "", "Please choose");
            model.TitleOptions = Services.DataTypeService.GetPreValues("Title Dropdown");

            var termsAndConditionsLink = string.Format("<a href=\"{0}\">{1}</a>", model.TermsAndConditionsPage.Url(), model.TermsAndConditionsLinkText);
            model.TermsAndConditionsLabelWithLink = string.Format(model.AgreeToTermsAndConditionsLabel, termsAndConditionsLink);

            model.ViewModel = new RegisterViewModel();

            model.IsStage1Valid = IsStage1Valid();
            model.IsStage3Valid = IsStage3Valid();

            return PartialView("~/Views/Register/Partials/ShowRegisterForm.cshtml", model);
        }

        [HttpPost]
        public ActionResult RegisterForm(Register model)
        {
            if (!ModelState.IsValid)
            {
                return CurrentUmbracoPage();
            }

            dynamic pageModel = Umbraco.Content(UmbracoContext.PageId);

            var emailSalt = _cryptographicService.GenerateEmailSalt(model.ViewModel.Email);
            var encryptedEmail = _cryptographicService.Encrypt(model.ViewModel.Email, emailSalt);

            if (Members.GetByEmail(encryptedEmail) != null)
            {
                _logger.Info(this.GetType().Name, "RegisterForm", "Duplicate registration attempted.");
                ModelState.AddModelError("ViewModel.Email", pageModel.AlreadyRegisteredMessage);

                return CurrentUmbracoPage();
            }

            try
            {
                CreateMember(model.ViewModel);
            }
            catch (MembershipPasswordException)
            {
                _logger.Info(this.GetType().Name, "RegisterForm", "Registration attempt with low quality password.");
                ModelState.AddModelError("PasswordError", pageModel.PasswordErrorMessage);

                return CurrentUmbracoPage();
            }
            catch (Exception ex)
            {
                _logger.Error(this.GetType().Name, "RegisterForm", "Registration attempt failed with error: {0}", ex.Message);
                ModelState.AddModelError("PasswordError", "There was an error and you have not been registered.  Please try again later.");

                return CurrentUmbracoPage();
            }

            try
            {
                var encryptedEmailAddress = _cryptographicService.EncryptEmailAddress(model.ViewModel.Email);
                var newMember = UmbracoContext.Application.Services.MemberService.GetByEmail(encryptedEmailAddress);

                Members.Login(newMember.Username, model.ViewModel.Password);
            }
            catch (Exception ex)
            {
                _logger.Error(this.GetType().Name, "RegisterForm", "Registration succeeded but couldn't automatically log the new user in: {0}", ex.Message);
            }

            TempData[CompletedFlag] = true;

            _logger.Info(this.GetType().Name, "RegisterForm", "New registration complete.");

            var recipients = new List<string>() { model.ViewModel.Email };
            var sender = new System.Net.Mail.MailAddress(ConfigHelper.SystemEmailAddress);
            _emailService.SendEmail(
                recipients,
                "Welcome to the Royal Mail Business Portal",
                pageModel.RegisterCompleteEmail.ToString()
                    .Replace("##firstName", model.ViewModel.FirstName)
                    .Replace("##email", model.ViewModel.Email),
                System.Net.Mail.MailPriority.Normal,
                sender);

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
                //RoyalMailMarketingPreferences = new ContactOptions()
                //{
                //    Post = model.MarketingPreferencesViewModel.RoyalMailMarketingPreferences.Post,
                //    Email = model.MarketingPreferencesViewModel.RoyalMailMarketingPreferences.Email,
                //    Phone = model.MarketingPreferencesViewModel.RoyalMailMarketingPreferences.Phone,
                //    SmsAndOther = model.MarketingPreferencesViewModel.RoyalMailMarketingPreferences.SmsAndOther
                //},
                //ThirdPartyMarketingPreferences = new ContactOptions()
                //{
                //    Post = model.MarketingPreferencesViewModel.ThirdPartyMarketingPreferences.Post,
                //    Email = model.MarketingPreferencesViewModel.ThirdPartyMarketingPreferences.Email,
                //    Phone = model.MarketingPreferencesViewModel.ThirdPartyMarketingPreferences.Phone,
                //    SmsAndOther = model.MarketingPreferencesViewModel.ThirdPartyMarketingPreferences.SmsAndOther
                //},
                RoyalMailMarketingPreferences = new ContactOptions()
                {
                    Post = model.AgreeToRoyalMailContact,
                    Email = model.AgreeToRoyalMailContact,
                    Phone = model.AgreeToRoyalMailContact,
                    SmsAndOther = model.AgreeToRoyalMailContact
                },
                ThirdPartyMarketingPreferences = new ContactOptions()
                {
                    Post = model.AgreeToThirdPartyContact,
                    Email = model.AgreeToThirdPartyContact,
                    Phone = model.AgreeToThirdPartyContact,
                    SmsAndOther = model.AgreeToThirdPartyContact
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

        private bool IsStage1Valid()
        {
            var keysToCheck = new[]
            {
                "ViewModel.Email",
                "ViewModel.ConfirmEmail",
                "ViewModel.FirstName",
                "ViewModel.LastName",
                "ViewModel.Title",
                "ViewModel.Password",
                "ViewModel.ConfirmPassword",
                "ViewModel.AgreeToTermsAndConditions",
                "PasswordError"
            };

            return !ViewData.ModelState.Keys.Intersect(keysToCheck).Any();
        }

        private bool IsStage3Valid()
        {
            var keysToCheck = new[]
            {
                "ViewModel.OrganisationDetailsViewModel.OrganisationName",
                "ViewModel.OrganisationDetailsViewModel.JobTitle",
                "ViewModel.OrganisationDetailsViewModel.FlatNumber",
                "ViewModel.OrganisationDetailsViewModel.BuildingNumber",
                "ViewModel.OrganisationDetailsViewModel.BuildingName",
                "ViewModel.OrganisationDetailsViewModel.Address1",
                "ViewModel.OrganisationDetailsViewModel.Address2",
                "ViewModel.OrganisationDetailsViewModel.City",
                "ViewModel.OrganisationDetailsViewModel.Postcode",
                "ViewModel.OrganisationDetailsViewModel.Country",
                "ViewModel.OrganisationDetailsViewModel.WorkPhoneNumber",
                "ViewModel.OrganisationDetailsViewModel.Country",
                "ViewModel.IsRecaptchaValidated"
            };

            return !ViewData.ModelState.Keys.Intersect(keysToCheck).Any();
        }
    }
}