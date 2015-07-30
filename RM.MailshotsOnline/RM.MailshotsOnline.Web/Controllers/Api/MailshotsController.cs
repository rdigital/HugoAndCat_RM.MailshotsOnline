using RM.MailshotsOnline.Entities.DataModels;
using RM.MailshotsOnline.Entities.Extensions;
using RM.MailshotsOnline.Entities.ViewModels;
using RM.MailshotsOnline.PCL.Models;
using RM.MailshotsOnline.PCL.Services;
using RM.MailshotsOnline.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.WebApi;

namespace RM.MailshotsOnline.Web.Controllers.Api
{
    //[MemberAuthorize()]
    public class MailshotsController : ApiBaseController
    {
        private IMailshotsService _mailshotsService;

        public MailshotsController(IMailshotsService mailshotsService, IMembershipService membershipService)
            : base(membershipService)
        {
            _mailshotsService = mailshotsService;
        }

        /// <summary>
        /// Gets all Mailshots belonging to the logged in user
        /// </summary>
        /// <returns>Collection of Mailshot view model objects</returns>
        public HttpResponseMessage GetAll()
        {
            var authResult = Authenticate();
            if (authResult != null)
            {
                return authResult;
            }

            var mailshots = _mailshotsService.GetUsersMailshots(_loggedInMember.Id);

            return Request.CreateResponse(HttpStatusCode.OK, mailshots);
        }

        /// <summary>
        /// Gets an individual mailshot
        /// </summary>
        /// <param name="id">ID of the mailshot</param>
        /// <returns>The Mailshot view model object</returns>
        [HttpGet]
        public HttpResponseMessage Get(Guid id)
        {
            var authResult = Authenticate();
            if (authResult != null)
            {
                return authResult;
            }

            var mailshot = _mailshotsService.GetMailshot(id);
            if (mailshot == null)
            {
                return MailshotNotFound();
            }

            if (mailshot.UserId != _loggedInMember.Id)
            {
                return MailshotForbidden();
            }

            var mailshotData = (Mailshot)mailshot;
            if (mailshotData.Content != null)
            {
                mailshotData.ContentText = mailshotData.Content.Content;
            }

            return Request.CreateResponse(HttpStatusCode.OK, mailshotData);
        }

        /// <summary>
        /// Saves a new Mailshot
        /// </summary>
        /// <param name="mailshot">The mailshot data to save</param>
        /// <returns>ID of the saved Mailshot</returns>
        [HttpPost]
        public HttpResponseMessage Save(Mailshot mailshot)
        {
            if (mailshot == null)
            {
                return ErrorMessage(HttpStatusCode.BadRequest, "Please provide mailshot data to save.");
            }

            if (string.IsNullOrEmpty(mailshot.ContentText) || string.IsNullOrEmpty(mailshot.Name))
            {
                return ErrorMessage(HttpStatusCode.BadRequest, "Please provide mailshot data to save.");
            }

            var authResult = Authenticate();
            if (authResult != null)
            {
                return authResult;
            }

            IMailshot mailshotData = mailshot;

            mailshotData.Content = new MailshotContent() { Content = mailshot.ContentText };
            mailshotData.UserId = _loggedInMember.Id;
            mailshotData.UpdatedDate = DateTime.UtcNow;

            _mailshotsService.SaveMailshot(mailshotData);

            return Request.CreateResponse(HttpStatusCode.Created, new { id = mailshotData.MailshotId });
        }

        /// <summary>
        /// Updates an existing Mailshot
        /// </summary>
        /// <param name="id">ID of the mailshot to update</param>
        /// <param name="mailshot">New content for the mailshot</param>
        /// <returns>HTTP OK on success</returns>
        [HttpPost]
        public HttpResponseMessage Update(Guid id, Mailshot mailshot)
        {
            var authResult = Authenticate();
            if (authResult != null)
            {
                return authResult;
            }

            if (mailshot == null || string.IsNullOrEmpty(mailshot.ContentText))
            {
                return ErrorMessage(HttpStatusCode.BadRequest, "Please provide mailshot data to save.");
            }

            var mailshotData = _mailshotsService.GetMailshot(id);
            if (mailshotData == null)
            {
                return MailshotNotFound();
            }

            if (mailshotData.UserId != _loggedInMember.Id)
            {
                return MailshotForbidden();
            }

            mailshotData.Name = mailshot.Name;
            if (mailshotData.Content == null)
            {
                MailshotContent contentData = new MailshotContent() { Content = mailshot.ContentText };
                mailshotData.Content = contentData;
            }
            else
            {
                mailshotData.Content.Content = mailshot.ContentText;
            }

            mailshotData.Draft = mailshot.Draft;
            mailshotData.UpdatedDate = DateTime.UtcNow;
            mailshotData.TemplateId = mailshot.TemplateId;
            mailshotData.FormatId = mailshot.FormatId;
            mailshotData.ThemeId = mailshot.ThemeId;

            _mailshotsService.SaveMailshot(mailshotData);

            return Request.CreateResponse(HttpStatusCode.OK, new { success = true });
        }

        /// <summary>
        /// Gets the URL of the proof PDF
        /// </summary>
        /// <param name="id">ID of the mailshot for proofing</param>
        /// <returns>URL of the proof PDF</returns>
        [Obsolete("The ProofPdfController has generation / access actions for proof PDFs")]
        [HttpGet]
        public HttpResponseMessage GetProofPdf(Guid id)
        {
            var authResult = Authenticate();
            if (authResult != null)
            {
                return authResult;
            }

            var mailshot = _mailshotsService.GetMailshot(id);
            if (mailshot == null)
            {
                return MailshotNotFound();
            }

            if (mailshot.UserId != _loggedInMember.Id)
            {
                return MailshotForbidden();
            }

            //TODO: Actually produce the XML
            return Request.CreateResponse(HttpStatusCode.OK, new { url = "This will be the PDF URL" });
        }

        private HttpResponseMessage MailshotNotFound()
        {
            return ErrorMessage(HttpStatusCode.NotFound, "No mailshot found with that ID");
        }

        private HttpResponseMessage MailshotForbidden()
        {
            return ErrorMessage(HttpStatusCode.Forbidden, "Forbidden");
        }
    }
}