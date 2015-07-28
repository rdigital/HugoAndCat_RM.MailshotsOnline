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
            try
            {
                Authenticate();
            }
            catch (HttpException ex)
            {
                return Request.CreateResponse((HttpStatusCode)ex.GetHttpCode(), ex.Message);
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
            try
            {
                Authenticate();
            }
            catch (HttpException ex)
            {
                return Request.CreateResponse((HttpStatusCode)ex.ErrorCode, ex.Message);
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

            return Request.CreateResponse(HttpStatusCode.OK, mailshot);
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
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Please provide mailshot data to save.");
            }

            if (string.IsNullOrEmpty(mailshot.ContentText) || string.IsNullOrEmpty(mailshot.Name))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Please provide mailshot data to save.");
            }

            try
            {
                Authenticate();
            }
            catch (HttpException ex)
            {
                return Request.CreateResponse((HttpStatusCode)ex.ErrorCode, ex.Message);
            }

            IMailshot mailshotData = mailshot;

            mailshotData.Content = new MailshotContent() { Content = mailshot.ContentText };
            mailshotData.UserId = _loggedInMember.Id;
            mailshotData.UpdatedDate = DateTime.UtcNow;

            _mailshotsService.SaveMailshot(mailshotData);

            return Request.CreateResponse(HttpStatusCode.OK, mailshotData.MailshotId);
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
            try
            {
                Authenticate();
            }
            catch (HttpException ex)
            {
                return Request.CreateResponse((HttpStatusCode)ex.ErrorCode, ex.Message);
            }

            if (mailshot == null || string.IsNullOrEmpty(mailshot.ContentText))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Please provide mailshot data to save.");
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

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        /// <summary>
        /// Gets the URL of the proof PDF
        /// </summary>
        /// <param name="id">ID of the mailshot for proofing</param>
        /// <returns>URL of the proof PDF</returns>
        [HttpGet]
        public HttpResponseMessage GetProofPdf(Guid id)
        {
            try
            {
                Authenticate();
            }
            catch (HttpException ex)
            {
                return Request.CreateResponse((HttpStatusCode)ex.ErrorCode, ex.Message);
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
            return Request.CreateResponse(HttpStatusCode.OK, "This will be the PDF URL");
        }

        private HttpResponseMessage MailshotNotFound()
        {
            return Request.CreateResponse(HttpStatusCode.NotFound, "No mailshot found with that ID");
        }

        private HttpResponseMessage MailshotForbidden()
        {
            return Request.CreateResponse(HttpStatusCode.Forbidden, "Forbidden.");
        }
    }
}