using Umbraco.Web;
using RM.MailshotsOnline.PCL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Net;
using RM.MailshotsOnline.Entities.ViewModels;
using RM.MailshotsOnline.Entities.MemberModels;
using System.Web.Security;
using System.Text;
using RM.MailshotsOnline.PCL.Models;

namespace RM.MailshotsOnline.Web.Controllers.Api
{
    public class MembersController : ApiBaseController
    {
        public MembersController(IMembershipService membershipService)
            : base(membershipService)
        {
        }
        
        [HttpGet]
        public HttpResponseMessage CurrentlyLoggedIn()
        {
            bool loggedIn = false;
            Authenticate();

            if (_loggedInMember != null)
            {
                loggedIn = true;
            }

            return Request.CreateResponse(HttpStatusCode.OK, new { loggedIn });
        }

        [HttpPost]
        public HttpResponseMessage Login(LoginViewModel login)
        {
            Authenticate();
            if (_loggedInMember != null)
            {
                //TODO: Get the error messages out of Umbraco somewhere
                return ErrorMessage(HttpStatusCode.BadRequest, "You are already logged in.");
            }

            if (!ModelState.IsValid)
            {
                //TODO: Get the error messages out of Umbraco somewhere
                var errorResponse = GetErrors("Unable to login.  Please correct the following errors:");
                return Request.CreateResponse(HttpStatusCode.BadRequest, errorResponse);
            }

            if (Members.Login(login.Email, login.Password))
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { loggedIn = true });
            }
            else
            {
                //TODO: Get the error messages out of Umbraco somewhere
                return ErrorMessage(HttpStatusCode.BadRequest, "Login credentials incorrect");
            }
        }

        [HttpPost]
        public HttpResponseMessage Logout()
        {
            Authenticate();
            if (_loggedInMember != null)
            {
                Members.Logout();
            }

            return Request.CreateResponse(HttpStatusCode.OK, new { loggedIn = false });
        }

        [HttpPost]
        public HttpResponseMessage Register(SimplifiedRegisterViewModel registration)
        {
            Authenticate();
            if (_loggedInMember != null)
            {
                //TODO: Get the error messages out of Umbraco somewhere
                return ErrorMessage(HttpStatusCode.BadRequest, "You are already logged in.");
            }

            if (!ModelState.IsValid)
            {
                //TODO: Get the error messages out of Umbraco somewhere
                var errorResponse = GetErrors("Unable to register. Please correct the following errors:");
                return Request.CreateResponse(HttpStatusCode.BadRequest, errorResponse);
            }

            if (Members.GetByEmail(registration.Email) != null)
            {
                //TODO: Get the error messages out of Umbraco somewhere
                return ErrorMessage(HttpStatusCode.Conflict, "The email address provided has already been used to register.");
            }

            try
            {
                CreateMember(registration);
            }
            catch (MembershipPasswordException)
            {
                //TODO: Get the error messages out of Umbraco somewhere
                return ErrorMessage(HttpStatusCode.BadRequest, "Your password does not meet the minimum requirements.");
            }

            bool registered = true;
            bool loggedIn = Members.Login(registration.Email, registration.Password);

            return Request.CreateResponse(HttpStatusCode.OK, new { registered, loggedIn });
        }

        private void CreateMember(SimplifiedRegisterViewModel model)
        {
            var rmContactOptions = new ContactOptions()
            {
                Email = model.AgreeToRoyalMailContact,
                Phone = model.AgreeToRoyalMailContact,
                Post = model.AgreeToRoyalMailContact,
                SmsAndOther = model.AgreeToRoyalMailContact
            };

            var thirdPartyContactOptions = new ContactOptions()
            {
                Email = model.AgreeToThirdPartyContact,
                Phone = model.AgreeToThirdPartyContact,
                Post = model.AgreeToThirdPartyContact,
                SmsAndOther = model.AgreeToThirdPartyContact
            };
            
            _membershipService.CreateMember(new Member()
            {
                EmailAddress = model.Email,
                Title = model.Title,
                FirstName = model.FirstName,
                LastName = model.LastName,
                IsApproved = false,
                IsLockedOut = false,
                RoyalMailMarketingPreferences = rmContactOptions,
                ThirdPartyMarketingPreferences = thirdPartyContactOptions
            }, model.Password);
        }

        private ErrorViewModel GetErrors(string mainError)
        {
            var errorResponse = new ErrorViewModel() { Error = mainError };
            errorResponse.FieldErrors = new List<string>();
            foreach (var modelState in ModelState.Values)
            {
                foreach (var error in modelState.Errors)
                {
                    if (error.Exception != null)
                    {
                        errorResponse.FieldErrors.Add(error.Exception.Message);
                    }
                    else
                    {
                        errorResponse.FieldErrors.Add(error.ErrorMessage);
                    }
                }
            }

            return errorResponse;
        }
    }
}