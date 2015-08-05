using System.Configuration;
using System.Runtime.InteropServices;
using System.Web.Mvc;
using System.Web.Security;
using Castle.Windsor.Installer;
using RM.MailshotsOnline.Data.Extensions;
using RM.MailshotsOnline.Data.Services;
using RM.MailshotsOnline.Entities.MemberModels;
using RM.MailshotsOnline.Entities.PageModels;
using RM.MailshotsOnline.Entities.ViewModels;
using RM.MailshotsOnline.PCL.Services;
using RM.MailshotsOnline.Web.Extensions;
using umbraco;
using Umbraco.Web.Mvc;

namespace RM.MailshotsOnline.Web.Controllers.SurfaceControllers
{
    public class RegisterSurfaceController : SurfaceController
    {
        private readonly IMembershipService _membershipService;
        private readonly IEmailService _emailService;
        private const string CompletedFlag = "RegistrationComplete";


        public RegisterSurfaceController(IMembershipService membershipService, IEmailService emailService)
        {
            _membershipService = membershipService;
            _emailService = emailService;
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
                ModelState.AddModelError("ViewModel.Email", model.AlreadyRegisteredMessage);
                return CurrentUmbracoPage();
            }

            try
            {
                CreateMember(model.ViewModel);
            }
            catch (MembershipPasswordException)
            {
                ModelState.AddModelError("PasswordError",
                    model.PasswordErrorMessage);
                return CurrentUmbracoPage();
            }

            TempData[CompletedFlag] = true;

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
                CanWeContactByPost = model.MarketingPreferencesViewModel.RoyalMailContactPreferences.Post,
                CanWeContactByEmail = model.MarketingPreferencesViewModel.RoyalMailContactPreferences.Email,
                CanWeContactByPhone = model.MarketingPreferencesViewModel.RoyalMailContactPreferences.Phone,
                CanWeContactBySmsAndOther = model.MarketingPreferencesViewModel.RoyalMailContactPreferences.SmsAndOther,
                CanThirdPatiesContactByPost = model.MarketingPreferencesViewModel.RoyalMailContactPreferences.Post,
                CanThirdPatiesContactByEmail = model.MarketingPreferencesViewModel.RoyalMailContactPreferences.Email,
                CanThirdPatiesContactByPhone = model.MarketingPreferencesViewModel.RoyalMailContactPreferences.Phone,
                CanThirdPatiesContactBySmsAndOther = model.MarketingPreferencesViewModel.RoyalMailContactPreferences.SmsAndOther,
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