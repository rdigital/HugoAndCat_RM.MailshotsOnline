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
using HC.RM.Common.Azure.Extensions;
using HC.RM.Common.PCL.Helpers;
using HC.RM.Common;
using RM.MailshotsOnline.Data.Constants;
using HC.RM.Common.Network;
using RM.MailshotsOnline.Data.Helpers;

namespace RM.MailshotsOnline.Web.Controllers.Api
{
    public class MembersController : ApiBaseController
    {
        private readonly ICryptographicService _cryptographicService;
        private readonly IEmailService _emailService;

        public MembersController(IMembershipService membershipService, ILogger logger, ICryptographicService cryptographicService, IEmailService emailService)
            : base(membershipService, logger)
        {
            _cryptographicService = cryptographicService;
            _emailService = emailService;
        }
        
        /// <summary>
        /// Checks if there's a user currently logged in
        /// </summary>
        /// <returns>HTTP OK with an object containing the status</returns>
        [HttpGet]
        public HttpResponseMessage GetLoggedInStatus()
        {
            bool loggedIn = false;
            Authenticate();

            if (_loggedInMember != null)
            {
                loggedIn = true;
            }

            return Request.CreateResponse(HttpStatusCode.OK, new { loggedIn });
        }

        /// <summary>
        /// Attempts to log a user in
        /// </summary>
        /// <param name="login">Login viewmodel</param>
        /// <returns>HTTP OK if the login was successful, HTTP Bad Request if not</returns>
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

            var emailSalt = _cryptographicService.GenerateEmailSalt(login.Email);
            var encryptedEmail = _cryptographicService.Encrypt(login.Email, emailSalt);

            var member = Services.MemberService.GetByEmail(encryptedEmail);

            if (member == null)
            {
                _logger.Error(this.GetType().Name, "Login", "Invalid login attempt with email address {0}.", login.Email);
                return ErrorMessage(HttpStatusCode.BadRequest, "Login credentials incorrect");
            }

            if (Members.Login(member.Username, login.Password))
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { loggedIn = true });
            }
            else
            {
                //TODO: Get the error messages out of Umbraco somewhere
                _logger.Error(this.GetType().Name, "Login", "Invalid login attempt with email address {0}.", login.Email);
                return ErrorMessage(HttpStatusCode.BadRequest, "Login credentials incorrect");
            }
        }

        /// <summary>
        /// Sends a password reset link to the specified email address
        /// </summary>
        /// <param name="email">The user's email address</param>
        /// <returns>HTTP OK if the user is found, 404 otherwise</returns>
        [HttpPost]
        public HttpResponseMessage SendPasswordResetLink(RequestResetPasswordViewModel resetRequest)
        {
            if (!ModelState.IsValid)
            {
                var errorResponse = GetErrors("Unable to send a password reset link.");
                return Request.CreateResponse(HttpStatusCode.BadRequest, errorResponse);
            }

            var headerNavSettings = Umbraco.Content(Constants.Settings.HeaderNavSettingsId);
            var passwordResetPage = Umbraco.Content((int)headerNavSettings.passwordResetPage);
            var emailBody = passwordResetPage.RequestCompleteEmail.ToString();
            var token = _membershipService.RequestPasswordReset(resetRequest.Email);

            if (token != null)
            {
                var resetLink = $"{ConfigHelper.HostedScheme}://{ConfigHelper.HostedDomain}{passwordResetPage.Url}?token={token}";

                var recipients = new List<string>() { resetRequest.Email };
                var sender = new System.Net.Mail.MailAddress(ConfigHelper.SystemEmailAddress);
                _emailService.SendEmail(
                    recipients,
                    "Password reset",
                    emailBody.Replace("##resetLink", $"<a href='{resetLink}'>{resetLink}</a>"),
                    System.Net.Mail.MailPriority.Normal,
                    sender);

                _logger.Info(this.GetType().Name, "SendPasswordResetLink", "Password reset email sent to {0}.", resetRequest.Email);
                //return Request.CreateResponse(HttpStatusCode.OK, new { emailSent = true });
            }

            //return Request.CreateResponse(HttpStatusCode.InternalServerError, new { emailSent = false });
            return Request.CreateResponse(HttpStatusCode.OK, new { success = true });
        }

        /// <summary>
        /// Perform a logout
        /// </summary>
        /// <returns>HTTP OK</returns>
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

        /// <summary>
        /// Register a new user
        /// </summary>
        /// <param name="registration">Registration view model</param>
        /// <returns>HTTP OK if the registration was successful</returns>
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

            var emailSalt = _cryptographicService.GenerateEmailSalt(registration.Email);
            var encryptedEmail = _cryptographicService.Encrypt(registration.Email, emailSalt);
            
            if (Members.GetByEmail(encryptedEmail) != null)
            {
                _logger.Error(this.GetType().Name, "Register", "Attempt to register with email {0} which has already been used.", registration.Email);
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
            catch (Exception ex)
            {
                _logger.Error(this.GetType().Name, "Register", "Attempt to register with email {0} resulted in an error: {1}", registration.Email, ex.Message);
                return ErrorMessage(HttpStatusCode.InternalServerError, "There was an error attempting to create your registration.");
            }

            bool registered = true;
            var encryptedEmailAddress = _cryptographicService.EncryptEmailAddress(registration.Email);
            var newMember = UmbracoContext.Application.Services.MemberService.GetByEmail(encryptedEmailAddress);

            bool loggedIn = Members.Login(newMember.Username, registration.Password);

            return Request.CreateResponse(HttpStatusCode.OK, new { registered, loggedIn });
        }

        /// <summary>
        /// Create the newly registered member
        /// </summary>
        /// <param name="model">View model</param>
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

        /// <summary>
        /// Get additional field errors from the view state
        /// </summary>
        /// <param name="mainError">Main error message</param>
        /// <returns>Error view model</returns>
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