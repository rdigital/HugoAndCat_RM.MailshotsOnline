using HC.RM.Common.Azure.Extensions;
using HC.RM.Common.PCL.Helpers;
using Newtonsoft.Json;
using RM.MailshotsOnline.Entities.DataModels;
using RM.MailshotsOnline.Entities.DataModels.MailshotSettings;
using RM.MailshotsOnline.Entities.Extensions;
using RM.MailshotsOnline.Entities.JsonModels;
using RM.MailshotsOnline.Entities.ViewModels;
using RM.MailshotsOnline.PCL.Models;
using RM.MailshotsOnline.PCL.Models.MailshotSettings;
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
    [MemberAuthorize()]
    public class MailshotsController : ApiBaseController
    {
        private IMailshotsService _mailshotsService;
        private IMailshotSettingsService _settingsService;

        public MailshotsController(IMailshotSettingsService settingsService, IMailshotsService mailshotsService, IMembershipService membershipService, ILogger logger)
            : base(membershipService, logger)
        {
            _mailshotsService = mailshotsService;
            _settingsService = settingsService;
        }

        /// <summary>
        /// Gets all Mailshots belonging to the logged in user
        /// </summary>
        /// <returns>Collection of Mailshot view model objects</returns>
        public HttpResponseMessage GetAll()
        {
            // Confirm the user is logged in
            var authResult = Authenticate();
            if (authResult != null)
            {
                return authResult;
            }

            // Find the mailshots that belong to the user
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
            // Confirm the user is logged in
            var authResult = Authenticate();
            if (authResult != null)
            {
                _logger.Error(this.GetType().Name, "Get", "Unauthenticated attempt to get mailshot with ID {0}.", id);
                return authResult;
            }

            // Confirm that the mailshot exists
            var mailshot = _mailshotsService.GetMailshot(id);
            if (mailshot == null)
            {
                _logger.Info(this.GetType().Name, "Get", "Attempt to get unknown mailshot with ID {0}.", id);
                return MailshotNotFound();
            }

            // Confirm that the user has access to the mailshot
            if (mailshot.UserId != _loggedInMember.Id)
            {
                _logger.Error(this.GetType().Name, "Get", "Unauthorised attempt to get mailshot with ID {0}.", id);
                return MailshotForbidden();
            }

            // Return the mailshot
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
            // Confirm the user is logged in
            var authResult = Authenticate();
            if (authResult != null)
            {
                _logger.Error(this.GetType().Name, "Save", "Unauthenticated attempt to save new mailshot.");
                return authResult;
            }

            // Confirm that the request is okay
            if (mailshot == null)
            {
                return ErrorMessage(HttpStatusCode.BadRequest, "Please provide mailshot data to save.");
            }

            // Confirm that the request contains the required information
            if (string.IsNullOrEmpty(mailshot.ContentText) || string.IsNullOrEmpty(mailshot.Name))
            {
                return ErrorMessage(HttpStatusCode.BadRequest, "Please provide mailshot data to save.");
            }

            // Get the Format, Template and Theme from the JSON
            IFormat format = null;
            ITemplate template = null;
            ITheme theme = null;
            MailshotEditorContent parsedContent = null;
            var jsonCheckResult = ConfirmJsonContentIsCorrect(mailshot.ContentText, out format, out template, out theme, out parsedContent);

            if (jsonCheckResult != null)
            {
                // Error while checking the JSON - return this
                return jsonCheckResult;
            }

            // Save the mailshot
            IMailshot mailshotData = mailshot;

            mailshotData.Content = new MailshotContent() { Content = mailshot.ContentText };
            mailshotData.UserId = _loggedInMember.Id;
            mailshotData.UpdatedDate = DateTime.UtcNow;
            mailshotData.FormatId = format.FormatId;
            mailshotData.TemplateId = template.TemplateId;
            mailshotData.ThemeId = theme.ThemeId;

            var linkedImages = parsedContent.Elements.Where(e => !string.IsNullOrEmpty(e.Src)).Select(e => e.Src);

            try
            {
                var savedMailshot = _mailshotsService.SaveMailshot(mailshotData);
                _mailshotsService.UpdateLinkedImages(savedMailshot, linkedImages);
                return Request.CreateResponse(HttpStatusCode.Created, new { id = savedMailshot.MailshotId });
            }
            catch (Exception ex)
            {
                _logger.Error(this.GetType().Name, "Save", "Error when attempting to save mailshot: {0}", ex.Message);
            }

            return ErrorMessage(HttpStatusCode.InternalServerError, "Unable to save mailshot data.");
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
            // Confirm the user is logged in
            var authResult = Authenticate();
            if (authResult != null)
            {
                _logger.Error(this.GetType().Name, "Update", "Unauthenticated attempt to update mailshot with ID {0}.", id);
                return authResult;
            }

            // Confirm that the request is okay
            if (mailshot == null || string.IsNullOrEmpty(mailshot.ContentText))
            {
                return ErrorMessage(HttpStatusCode.BadRequest, "Please provide mailshot data to save.");
            }

            // Confirm that mailshot exists
            var mailshotData = _mailshotsService.GetMailshot(id);
            if (mailshotData == null)
            {
                _logger.Info(this.GetType().Name, "Update", "Attempt to update unknown mailshot with ID {0}.", id);
                return MailshotNotFound();
            }

            // Confirm that the user has access to the mailshot
            if (mailshotData.UserId != _loggedInMember.Id)
            {
                _logger.Error(this.GetType().Name, "Update", "Unauthorised attempt to update mailshot with ID {0}.", id);
                return MailshotForbidden();
            }

            // Get the Format, Template and Theme from the JSON
            IFormat format = null;
            ITemplate template = null;
            ITheme theme = null;
            MailshotEditorContent parsedContent = null;
            var jsonCheckResult = ConfirmJsonContentIsCorrect(mailshot.ContentText, out format, out template, out theme, out parsedContent);

            if (jsonCheckResult != null)
            {
                // Error while checking the JSON - return this
                return jsonCheckResult;
            }

            // Save new data
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
            mailshotData.FormatId = format.FormatId;
            mailshotData.TemplateId = template.TemplateId;
            mailshotData.ThemeId = theme.ThemeId;
            mailshotData.ProofPdfStatus = PCL.Enums.PdfRenderStatus.None;

            var linkedImages = parsedContent.Elements.Where(e => !string.IsNullOrEmpty(e.Src)).Select(e => e.Src);

            var success = false;
            try
            {
                _mailshotsService.SaveMailshot(mailshotData);
                _mailshotsService.UpdateLinkedImages(mailshotData, linkedImages);
                success = true;
            }
            catch (Exception ex)
            {
                _logger.Error(this.GetType().Name, "Update", "Error updating mailshot with ID {0}: {1}", id, ex.Message);
            }

            return Request.CreateResponse(HttpStatusCode.OK, new { success = success });
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
            // Confirm the user is logged in
            var authResult = Authenticate();
            if (authResult != null)
            {
                return authResult;
            }

            // Confirm that mailshot exists
            var mailshot = _mailshotsService.GetMailshot(id);
            if (mailshot == null)
            {
                return MailshotNotFound();
            }

            // Confirm that the user has access to the mailshot
            if (mailshot.UserId != _loggedInMember.Id)
            {
                return MailshotForbidden();
            }

            // Check for the proof PDF
            HttpResponseMessage result;

            switch (mailshot.ProofPdfStatus)
            {
                case PCL.Enums.PdfRenderStatus.Complete:
                    result = Request.CreateResponse(HttpStatusCode.OK, new { url = mailshot.ProofPdfUrl });
                    break;
                case PCL.Enums.PdfRenderStatus.Pending:
                    result = ErrorMessage(HttpStatusCode.PreconditionFailed, "The PDF is not ready.");
                    break;
                case PCL.Enums.PdfRenderStatus.None:
                default:
                    result = ErrorMessage(HttpStatusCode.NotFound, "No proof PDF has been requested for this mailshot.");
                    break;
            }

            return result;
        }

        /// <summary>
        /// Deletes a mailshot
        /// </summary>
        /// <param name="id">ID of the mailshot to delete</param>
        /// <returns>HTTP No Content (204) on success</returns>
        [HttpDelete]
        [HttpPost]
        public HttpResponseMessage Delete(Guid id)
        {
            // Confirm the user is logged in
            var authResult = Authenticate();
            if (authResult != null)
            {
                _logger.Error(this.GetType().Name, "Delete", "Unauthenticated attempt to delete mailshot with ID {0}.", id);
                return authResult;
            }

            // Confirm that mailshot exists
            var mailshot = _mailshotsService.GetMailshot(id);
            if (mailshot == null)
            {
                _logger.Info(this.GetType().Name, "Delete", "Attempt to delete unknown mailshot with ID {0}.", id);
                return MailshotNotFound();
            }

            // Confirm that the user has access to the mailshot
            if (mailshot.UserId != _loggedInMember.Id)
            {
                _logger.Error(this.GetType().Name, "Delete", "Unauthorised attempt to delete mailshot with ID {0}.", id);
                return MailshotForbidden();
            }

            // Confirm that the mailshot isn't used in any campaigns
            if (_mailshotsService.MailshotIsUsedInCampaign(mailshot))
            {
                _logger.Error(this.GetType().Name, "Delete", "Attempt to delete mailshot that is in use with ID {0}.", id);
                return ErrorMessage(HttpStatusCode.Conflict, "Unable to delete mailshot that is in use for a campaign.");
            }

            // Delete the mailshot
            _mailshotsService.Delete(mailshot);

            return Request.CreateResponse(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Parses the content to be saved to the Mailshot to ensure it conforms to the required structure and uses known templates
        /// </summary>
        /// <param name="content">The content to be parsed</param>
        /// <param name="format">The Format of the mailshot</param>
        /// <param name="template">The Template used for the mailshot</param>
        /// <param name="theme">The Theme used for the mailshot</param>
        /// <returns>Returns an HTTP response if the JSON is incorrect</returns>
        private HttpResponseMessage ConfirmJsonContentIsCorrect(string content, out IFormat format, out ITemplate template, out ITheme theme, out MailshotEditorContent parsedContent)
        {
            format = null;
            template = null;
            theme = null;

            // Confirm that the JSON content is correct
            parsedContent = null;
            try
            {
                parsedContent = JsonConvert.DeserializeObject<MailshotEditorContent>(content);
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.Error(this.GetType().Name, "ConfirmJsonContentIsCorrect", "Error parsing JSON content: {0}", ex.Message);
            }

            if (parsedContent == null || parsedContent.Elements == null)
            {
                return ErrorMessage(HttpStatusCode.BadRequest, "The JSON content is incorrect.");
            }

            // Confirm that the selected Formats exist
            format = _settingsService.GetFormatByJsonIndex(parsedContent.FormatId);
            template = _settingsService.GetTemplateByJsonIndex(parsedContent.TemplateId, parsedContent.FormatId);
            theme = _settingsService.GetThemeByJsonIndex(parsedContent.ThemeId);
            if (format == null)
            {
                _logger.Error(this.GetType().Name, "ConfirmJsonContentIsCorrect", "Attempt to use unknown format: {0}", parsedContent.FormatId);
                return ErrorMessage(HttpStatusCode.BadRequest, "The specified format does not exist.");
            }

            if (template == null)
            {
                _logger.Error(this.GetType().Name, "ConfirmJsonContentIsCorrect", "Attempt to use unknown template: {0}", parsedContent.TemplateId);
                return ErrorMessage(HttpStatusCode.BadRequest, "The specified template does not exist.");
            }

            if (template.FormatUmbracoPageId != format.UmbracoPageId)
            {
                _logger.Error(this.GetType().Name, "ConfirmJsonContentIsCorrect", "Attempt to use template {0} which does not match format {1}.", parsedContent.TemplateId, parsedContent.FormatId);
                return ErrorMessage(HttpStatusCode.BadRequest, "The selected template does not belong to the selected format.");
            }

            if (theme == null)
            {
                _logger.Error(this.GetType().Name, "ConfirmJsonContentIsCorrect", "Attempt to use unknown theme: {0}", parsedContent.TemplateId);
                return ErrorMessage(HttpStatusCode.BadRequest, "The specified theme does not exist.");
            }

            return null;
        }

        /// <summary>
        /// Return a Not Found message
        /// </summary>
        private HttpResponseMessage MailshotNotFound()
        {
            return ErrorMessage(HttpStatusCode.NotFound, "No mailshot found with that ID");
        }

        /// <summary>
        /// Return a Forbidden message
        /// </summary>
        private HttpResponseMessage MailshotForbidden()
        {
            return ErrorMessage(HttpStatusCode.Forbidden, "Forbidden");
        }
    }
}