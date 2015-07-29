using System.Runtime.InteropServices;
using System.Web.Mvc;
using System.Web.Security;
using RM.MailshotsOnline.Data.Services;
using RM.MailshotsOnline.Entities.MemberModels;
using RM.MailshotsOnline.Entities.PageModels;
using RM.MailshotsOnline.Entities.ViewModels;
using Umbraco.Web.Mvc;

namespace RM.MailshotsOnline.Web.Controllers.SurfaceControllers
{
    public class RegisterSurfaceController : SurfaceController
    {
        private readonly MembershipService _membershipService = new MembershipService();

        private const string CompletedFlag = "RegistrationComplete";

        // GET: Register
        [ChildActionOnly]
        public ActionResult ShowRegisterForm(Register model)
        {
            // todo: get valid titles.
            if (TempData[CompletedFlag] != null && (bool) TempData[CompletedFlag])
            {
                return Complete(model);
            }

            model.ViewModel = new RegisterViewModel();

            return PartialView("~/Views/Register/Partials/ShowRegisterForm.cshtml", model);
        }

        [HttpPost]
        public ActionResult RegisterForm(Register model)
        {
            if (!ModelState.IsValid)
            {
                return CurrentUmbracoPage();
            }

            if (Members.GetByEmail(model.ViewModel.Email) != null)
            {
                ModelState.AddModelError("ViewModel.Email",
                    "You already appear to be registered with us. Please use the login page instead.");
                return CurrentUmbracoPage();
            }

            try
            {
                CreateMember(model.ViewModel);
            }
            catch (MembershipPasswordException)
            {
                ModelState.AddModelError("PasswordError",
                    "Your password does not meet the minimum requirements. Please use 6 alphanumeric characters.");
                return CurrentUmbracoPage();
            }

            TempData[CompletedFlag] = true;

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
                CanWeContactByPost = model.RoyalMailContactOptions.Post,
                CanWeContactByEmail = model.RoyalMailContactOptions.Email,
                CanWeContactByPhone = model.RoyalMailContactOptions.Phone,
                CanWeContactBySmsAndOther = model.RoyalMailContactOptions.SmsAndOther,
                CanThirdPatiesContactByPost = model.ThirdPartyContactOptions.Post,
                CanThirdPatiesContactByEmail = model.ThirdPartyContactOptions.Email,
                CanThirdPatiesContactByPhone = model.ThirdPartyContactOptions.Phone,
                CanThirdPatiesContactBySmsAndOther = model.ThirdPartyContactOptions.SmsAndOther,
                Postcode = model.Postcode,
                OrganisationName = model.OrganisationName,
                JobTitle = model.JobTitle,
                FlatNumber = model.FlatNumber,
                BuildingNumber = model.BuildingNumber,
                BuildingName = model.BuildingName,
                Address1 = model.Address1,
                Address2 = model.Address2,
                City = model.City,
                Country = model.Country,
                WorkPhoneNumber = model.WorkPhoneNumber,
                MobilePhoneNumber = model.MobilePhoneNumber
            }, model.Password);
        }
    }
}