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
                return ErrorMessage(HttpStatusCode.BadRequest, "You are already logged in.");
            }

            if (!ModelState.IsValid)
            {
                var errorMessage = new StringBuilder("Unable to login.  Please correct the following errors:");
                
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        if (!string.IsNullOrEmpty(error.ErrorMessage))
                        {
                            errorMessage.AppendLine();
                            errorMessage.Append(error.ErrorMessage);
                        }
                        else if (error.Exception != null)
                        {
                            errorMessage.AppendLine();
                            errorMessage.Append(error.Exception.Message);
                        }
                    }
                }

                return Request.CreateResponse(HttpStatusCode.BadRequest, new { error = errorMessage.ToString() });
            }

            if (Members.Login(login.Email, login.Password))
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { loggedIn = true });
            }
            else
            {
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
                return ErrorMessage(HttpStatusCode.BadRequest, "You are already logged in.");
            }

            if (!ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { error = "Unable to register. Please correct the following errors:", fieldErrors = ModelState.Values.Select(s => s.Errors.Select(e => e.ErrorMessage))});
            }

            if (Members.GetByEmail(registration.Email) != null)
            {
                return ErrorMessage(HttpStatusCode.Conflict, "The email address provided has already been used to register.");
            }

            try
            {
                CreateMember(registration);
            }
            catch (MembershipPasswordException)
            {
                return ErrorMessage(HttpStatusCode.BadRequest, "Your password does not meet the minimum requirements.");
            }

            bool registered = true;
            bool loggedIn = Members.Login(registration.Email, registration.Password);

            return Request.CreateResponse(HttpStatusCode.OK, new { registered, loggedIn });
        }

        private void CreateMember(SimplifiedRegisterViewModel model)
        {
            _membershipService.CreateMember(new Member()
            {
                EmailAddress = model.Email,
                Title = model.Title,
                FirstName = model.FirstName,
                LastName = model.LastName,
                IsApproved = false,
                IsLockedOut = false
            }, model.Password);
        }
    }
}