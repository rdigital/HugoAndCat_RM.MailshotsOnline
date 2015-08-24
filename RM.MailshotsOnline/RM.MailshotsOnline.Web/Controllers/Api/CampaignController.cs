using HC.RM.Common.PCL.Helpers;
using RM.MailshotsOnline.Entities.DataModels;
using RM.MailshotsOnline.PCL.Models;
using RM.MailshotsOnline.PCL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace RM.MailshotsOnline.Web.Controllers.Api
{
    public class CampaignController : ApiBaseController
    {
        private ICampaignService _campaignService;
        private IMailshotsService _mailshotService;

        public CampaignController(ICampaignService campaignService, IMailshotsService mailshotService, IMembershipService membershipService, ILogger logger)
            : base (membershipService, logger)
        {
            _campaignService = campaignService;
            _mailshotService = mailshotService;
        }

        [Authorize]
        public HttpResponseMessage GetAll()
        {
            // Confirm the user is logged in
            var authResult = Authenticate();
            if (authResult != null)
            {
                _logger.Error(this.GetType().Name, "Get", "Unauthenticated attempt to get campaigns");
                return authResult;
            }

            // Get the user's campaigns
            var campaigns = _campaignService.GetCampaignsForUser(_loggedInMember.Id).Select(c => (Campaign)c);

            return Request.CreateResponse(HttpStatusCode.OK, campaigns);
        }

        [Authorize]
        public HttpResponseMessage Get(Guid id)
        {
            // Confirm the user is logged in
            var authResult = Authenticate();
            if (authResult != null)
            {
                _logger.Error(this.GetType().Name, "Get", "Unauthenticated attempt to get campaign with ID {0}.", id);
                return authResult;
            }

            // Confirm that the campaign exists
            var campaign = _campaignService.GetCampaign(id);
            if (campaign == null)
            {
                _logger.Error(this.GetType().Name, "Get", "Attempt to get unknown campaign with ID {0}.", id);
                return ErrorMessage(HttpStatusCode.NotFound, "Campaign not found");
            }

            // Confirm that the user has access to the campaign
            if (campaign.UserId != _loggedInMember.Id)
            {
                _logger.Error(this.GetType().Name, "Get", "Unauthorised attempt to get campaign with ID {0}.", id);
                return ErrorMessage(HttpStatusCode.Forbidden, "No access to specified campaign");
            }

            // Return the campaign
            var campaignData = (Campaign)campaign;
            return Request.CreateResponse(HttpStatusCode.OK, campaignData);
        }

        [Authorize]
        public HttpResponseMessage Save(Campaign campaign)
        {
            // Confirm the user is logged in
            var authResult = Authenticate();
            if (authResult != null)
            {
                _logger.Error(this.GetType().Name, "Save", "Unauthenticated attempt to save new campaign.");
                return authResult;
            }

            // Confirm that the required fields are filled out
            var validationResponse = ValidateCampaign(campaign, true);
            if (validationResponse != null)
            {
                return validationResponse;
            }

            // Fill in the blanks
            ICampaign campaignData = campaign;
            campaignData.UpdatedDate = DateTime.UtcNow;
            campaignData.UserId = _loggedInMember.Id;
            campaignData.Status = PCL.Enums.CampaignStatus.Draft;

            // Save the campaign
            ICampaign savedCampaign;
            try
            {
                savedCampaign = (Campaign)_campaignService.SaveCampaign(campaignData);
            }
            catch (Exception ex)
            {
                _logger.Exception(this.GetType().Name, "Save", ex);
                _logger.Error(this.GetType().Name, "Save", "Unable to save new campaign");
                return ErrorMessage(HttpStatusCode.InternalServerError, "Unable to save campaign due to server error");
            }

            // Return the result
            return Request.CreateResponse(HttpStatusCode.OK, savedCampaign);
        }

        [Authorize]
        public HttpResponseMessage Update(Guid id, Campaign campaign)
        {
            // Confirm the user is logged in
            var authResult = Authenticate();
            if (authResult != null)
            {
                _logger.Error(this.GetType().Name, "Update", "Unauthenticated attempt to update campaign with ID {0}.", id);
                return authResult;
            }

            // Confirm that the original campaign exists
            var originalCampaign = _campaignService.GetCampaign(id);
            if (originalCampaign == null)
            {
                _logger.Error(this.GetType().Name, "Update", "Attempt to update unknown campaign with ID {0}.", id);
                return ErrorMessage(HttpStatusCode.NotFound, "Campaign not found");
            }

            // Confirm that the user has access to the campaign
            if (originalCampaign.UserId != _loggedInMember.Id)
            {
                _logger.Error(this.GetType().Name, "Update", "Unauthorised attempt to udpate campaign with ID {0}.", id);
                return ErrorMessage(HttpStatusCode.Forbidden, "No access to specified campaign");
            }

            // Confirm that the required fields are filled out
            var validationResponse = ValidateCampaign(campaign, false);
            if (validationResponse != null)
            {
                return validationResponse;
            }

            // Fill in the blanks
            originalCampaign.UpdatedDate = DateTime.UtcNow;
            originalCampaign.Name = campaign.Name;
            originalCampaign.Notes = campaign.Notes;
            originalCampaign.MailshotId = campaign.MailshotId;
            originalCampaign.PostalOptionId = campaign.PostalOptionId;

            // Save the changse
            try
            {
                _campaignService.SaveCampaign(originalCampaign);
            }
            catch (Exception ex)
            {
                _logger.Exception(this.GetType().Name, "Update", ex);
                _logger.Error(this.GetType().Name, "Update", "Unable to update campaign {0}: {1}", id, ex.Message);
                return ErrorMessage(HttpStatusCode.InternalServerError, "Unable to update due to server error");
            }

            // Return the result
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }

        [HttpGet]
        public HttpResponseMessage GetCopy(Guid id)
        {
            // Confirm the user is logged in
            var authResult = Authenticate();
            if (authResult != null)
            {
                _logger.Error(this.GetType().Name, "GetCopy", "Unauthenticated attempt to copy campaign with ID {0}.", id);
                return authResult;
            }

            // Confirm that the original campaign exists
            var originalCampaign = _campaignService.GetCampaign(id);
            if (originalCampaign == null)
            {
                _logger.Error(this.GetType().Name, "GetCopy", "Attempt to copy unknown campaign with ID {0}.", id);
                return ErrorMessage(HttpStatusCode.NotFound, "Campaign not found");
            }

            // Confirm that the user has access to the campaign
            if (originalCampaign.UserId != _loggedInMember.Id)
            {
                _logger.Error(this.GetType().Name, "GetCopy", "Unauthorised attempt to copy campaign with ID {0}.", id);
                return ErrorMessage(HttpStatusCode.Forbidden, "No access to specified campaign");
            }

            ICampaign newCampaign = new Campaign();
            //TODO: Create a copy of the Mailshot
            newCampaign.MailshotId = originalCampaign.MailshotId;
            newCampaign.PostalOptionId = originalCampaign.PostalOptionId;
            newCampaign.Name = $"{originalCampaign.Name} - Copy";
            newCampaign.Notes = $"Copied from {originalCampaign.Name} on {DateTime.UtcNow.ToString("d MMMM yyyy")}.{Environment.NewLine}{originalCampaign.Notes}";
            newCampaign.Status = PCL.Enums.CampaignStatus.Draft;
            newCampaign.SystemNotes = $"Copied from {originalCampaign.Name} on {DateTime.UtcNow.ToString("d MMMM yyyy")}.{Environment.NewLine}{originalCampaign.SystemNotes}";
            newCampaign.UpdatedDate = DateTime.UtcNow;
            newCampaign.UserId = _loggedInMember.Id;
            //TODO: Copy Data usage information

            // Save the campaign
            ICampaign savedCampaign;
            try
            {
                savedCampaign = (Campaign)_campaignService.SaveCampaign(newCampaign);
            }
            catch (Exception ex)
            {
                _logger.Exception(this.GetType().Name, "GetCopy", ex);
                _logger.Error(this.GetType().Name, "GetCopy", "Unable to save new campaign");
                return ErrorMessage(HttpStatusCode.InternalServerError, "Unable to save campaign due to server error");
            }

            // Return the result
            return Request.CreateResponse(HttpStatusCode.OK, savedCampaign);
        }

        [HttpDelete]
        [Authorize]
        public HttpResponseMessage Delete(Guid id)
        {
            // Confirm the user is logged in
            var authResult = Authenticate();
            if (authResult != null)
            {
                _logger.Error(this.GetType().Name, "Delete", "Unauthenticated attempt to delete campaign with ID {0}.", id);
                return authResult;
            }

            // Confirm that the original campaign exists
            var originalCampaign = _campaignService.GetCampaign(id);
            if (originalCampaign == null)
            {
                _logger.Error(this.GetType().Name, "Delete", "Attempt to delete unknown campaign with ID {0}.", id);
                return ErrorMessage(HttpStatusCode.NotFound, "Campaign not found");
            }

            // Confirm that the user has access to the campaign
            if (originalCampaign.UserId != _loggedInMember.Id)
            {
                _logger.Error(this.GetType().Name, "Delete", "Unauthorised attempt to delete campaign with ID {0}.", id);
                return ErrorMessage(HttpStatusCode.Forbidden, "No access to specified campaign");
            }

            // Confirm that the campaign can be updated
            List<PCL.Enums.CampaignStatus> updatableStatuses = new List<PCL.Enums.CampaignStatus>() { PCL.Enums.CampaignStatus.Draft };
            if (!updatableStatuses.Contains(originalCampaign.Status))
            {
                _logger.Error(this.GetType().Name, "Delete", "Attempt to delete in-progress campaign with ID {0}.", id);
                return ErrorMessage(HttpStatusCode.Conflict, "Campaign is in process and can no longer be delete.");
            }

            //TODO: Confirm: Delete mailshot too?

            bool success = false;
            try
            {
                success = _campaignService.DeleteCampaignWithId(id);
            }
            catch (Exception ex)
            {
                _logger.Exception(this.GetType().Name, "Delete", ex);
            }

            if (!success)
            {
                _logger.Error(this.GetType().Name, "Delete", "Unable to delete campaign with ID {0}.", id);
                return ErrorMessage(HttpStatusCode.InternalServerError, "Unable to delete campaign because of a server error.");
            }

            return Request.CreateResponse(HttpStatusCode.NoContent);
        }

        [Authorize]
        public HttpResponseMessage LinkMailshotToCampaign(Guid id, Guid mailshotId)
        {
            // Confirm the user is logged in
            var authResult = Authenticate();
            if (authResult != null)
            {
                _logger.Error(this.GetType().Name, "LinkMailshotToCampaign", "Unauthenticated attempt to update campaign with ID {0}.", id);
                return authResult;
            }

            // Confirm that the original campaign exists
            var originalCampaign = _campaignService.GetCampaign(id);
            if (originalCampaign == null)
            {
                _logger.Error(this.GetType().Name, "LinkMailshotToCampaign", "Attempt to link unknown campaign with ID {0}.", id);
                return ErrorMessage(HttpStatusCode.NotFound, "Campaign not found");
            }

            // Confirm that the user has access to the campaign
            if (originalCampaign.UserId != _loggedInMember.Id)
            {
                _logger.Error(this.GetType().Name, "LinkMailshotToCampaign", "Unauthorised attempt to udpate campaign with ID {0}.", id);
                return ErrorMessage(HttpStatusCode.Forbidden, "No access to specified campaign");
            }

            // Confirm that the campaign can be updated
            List<PCL.Enums.CampaignStatus> updatableStatuses = new List<PCL.Enums.CampaignStatus>() { PCL.Enums.CampaignStatus.Draft, PCL.Enums.CampaignStatus.Exception };
            if (!updatableStatuses.Contains(originalCampaign.Status))
            {
                _logger.Error(this.GetType().Name, "LinkMailshotToCampaign", "Attempt to udpate in-progress campaign with ID {0}.", id);
                return ErrorMessage(HttpStatusCode.Conflict, "Campaign is in process and can no longer be updated.");
            }

            // Confirm that the mailshot exists
            var mailshot = _mailshotService.GetMailshot(mailshotId);
            if (mailshot == null)
            {
                _logger.Error(this.GetType().Name, "LinkMailshotToCampaign", "Attempt to link unknown mailshot with ID {0}.", id);
                return ErrorMessage(HttpStatusCode.NotFound, "Mailshot not found");
            }

            // Confirm that the user has access to the mailshot
            if (mailshot.UserId != _loggedInMember.Id)
            {
                _logger.Error(this.GetType().Name, "LinkMailshotToCampaign", "Unauthorised attempt to link mailshot with ID {0}.", id);
                return ErrorMessage(HttpStatusCode.Forbidden, "No access to specified mailshot");
            }

            // Update campaign
            originalCampaign.MailshotId = mailshot.MailshotId;

            // Save the changse
            try
            {
                _campaignService.SaveCampaign(originalCampaign);
            }
            catch (Exception ex)
            {
                _logger.Exception(this.GetType().Name, "LinkMailshotToCampaign", ex);
                _logger.Error(this.GetType().Name, "LinkMailshotToCampaign", "Unable to update campaign {0}: {1}", id, ex.Message);
                return ErrorMessage(HttpStatusCode.InternalServerError, "Unable to update due to server error");
            }

            // Return the result
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }

        private HttpResponseMessage ValidateCampaign(Campaign campaign, bool isNew)
        {
            // Confirm that the required fields are filled out
            if (string.IsNullOrEmpty(campaign.Name))
            {
                return ErrorMessage(HttpStatusCode.BadRequest, "Please provide a name for the campaign");
            }

            // Confirm that the selected mailshot exists and belongs to the user
            if (campaign.MailshotId.HasValue)
            {
                var mailshot = _mailshotService.GetMailshot(campaign.MailshotId.Value);
                if (mailshot == null)
                {
                    return ErrorMessage(HttpStatusCode.BadRequest, "The selected mailshot does not exist");
                }

                if (mailshot.UserId != _loggedInMember.Id)
                {
                    return ErrorMessage(HttpStatusCode.BadRequest, "The selected mailshot does not belong to the logged in user.");
                }
            }

            return null;
        }
    }
}