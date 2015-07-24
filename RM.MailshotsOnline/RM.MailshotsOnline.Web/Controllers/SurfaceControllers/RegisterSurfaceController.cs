using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
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

        // GET: Register
        [ChildActionOnly]
        public ActionResult ShowRegisterForm(Register model)
        {
            // todo: get valid titles.

            var viewModel = new RegisterViewModel() { Page = 1, PageModel = model };

            return PartialView("~/Views/Register/Partials/Register.cshtml", viewModel);
        }

        [HttpPost]
        public ActionResult RegisterForm(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return CurrentUmbracoPage();
            }

            if (Members.GetByEmail(model.Email) != null)
            {
                ModelState.AddModelError("AlreadyRegistered",
                    "You already appear to be registered with us. Please use the login page instead.");
                return CurrentUmbracoPage();
            }

            try
            {
                CreateMember(model);
            }
            catch (MembershipPasswordException)
            {
                ModelState.AddModelError("PasswordError",
                    "Your password does not meet the minimum requirements. Please use 6 alphanumeric characters.");
                return CurrentUmbracoPage();
            }

            return Redirect("/?registered=true");
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
                CanThirdPatiesContactBySmsAndOther = model.ThirdPartyContactOptions.SmsAndOther
            }, model.Password);
        }
    }
}