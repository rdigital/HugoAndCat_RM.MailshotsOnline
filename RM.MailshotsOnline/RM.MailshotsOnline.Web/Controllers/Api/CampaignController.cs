using HC.RM.Common.PCL.Helpers;
using RM.MailshotsOnline.Entities.DataModels;
using RM.MailshotsOnline.Entities.ViewModels;
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
        private IPricingService _pricingService;

        public CampaignController(ICampaignService campaignService, IMailshotsService mailshotService, IPricingService pricingService, IMembershipService membershipService, ILogger logger)
            : base (membershipService, logger)
        {
            _campaignService = campaignService;
            _mailshotService = mailshotService;
            _pricingService = pricingService;
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
            ICampaign originalCampaign;
            // Make sure user can access campaign
            var validateResponse = ValidateRequest("GetPriceBreakdown", id, out originalCampaign);
            if (validateResponse != null)
            {
                return validateResponse;
            }

            // Return the campaign
            var campaignData = (Campaign)originalCampaign;
            return Request.CreateResponse(HttpStatusCode.OK, campaignData);
        }

        public HttpResponseMessage GetPriceBreakdown(Guid id)
        {
            ICampaign originalCampaign;
            // Make sure user can access campaign
            var validateResponse = ValidateRequest("GetPriceBreakdown", id, out originalCampaign);
            if (validateResponse != null)
            {
                return validateResponse;
            }

            // Return the campaign price breakdown
            ICampaignPriceBreakdown priceBreakdown = null;
            try
            {
                priceBreakdown = _pricingService.GetPriceBreakdown(originalCampaign);
            }
            catch (Exception ex)
            {
                _logger.Exception(this.GetType().Name, "GetPriceBreakdown", ex);
                _logger.Error(this.GetType().Name, "GetPriceBreakdown", "Server error getting price information for Campaign {0}: {1}", id, ex.Message);
            }

            if (priceBreakdown != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, priceBreakdown);
            }

            return ErrorMessage(HttpStatusCode.InternalServerError, "Unable to get price information.");
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

        [HttpPost]
        public HttpResponseMessage Rename(Guid id, RenameViewModel renameRequest)
        {
            ICampaign originalCampaign;
            // Make sure user can access campaign
            var validateResponse = ValidateRequest("Rename", id, out originalCampaign);
            if (validateResponse != null)
            {
                return validateResponse;
            }

            // Confirm mailshot can be updated
            var canUpdateMailshotResponse = ConfirmCanBeUpdated("Rename", originalCampaign);
            if (canUpdateMailshotResponse != null)
            {
                return canUpdateMailshotResponse;
            }

            if (string.IsNullOrEmpty(renameRequest.Name))
            {
                return ErrorMessage(HttpStatusCode.BadRequest, "You must enter a name for the campaign.");
            }

            // Fill in the blanks
            originalCampaign.UpdatedDate = DateTime.UtcNow;
            originalCampaign.Name = renameRequest.Name;

            // Save the changse
            try
            {
                _campaignService.SaveCampaign(originalCampaign);
            }
            catch (Exception ex)
            {
                _logger.Exception(this.GetType().Name, "Update", ex);
                _logger.Error(this.GetType().Name, "Update", "Unable to Rename campaign {0}: {1}", id, ex.Message);
                return ErrorMessage(HttpStatusCode.InternalServerError, "Unable to Rename due to server error");
            }

            // Return the result
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }

        [HttpPost]
        public HttpResponseMessage SetDataApproval(Guid id, SetApprovalViewModel approval)
        {
            ICampaign originalCampaign;
            // Make sure user can access campaign
            var validateResponse = ValidateRequest("SetDataApproval", id, out originalCampaign);
            if (validateResponse != null)
            {
                return validateResponse;
            }

            // Confirm mailshot can be updated
            var canUpdateMailshotResponse = ConfirmCanBeUpdated("SetDataApproval", originalCampaign);
            if (canUpdateMailshotResponse != null)
            {
                return canUpdateMailshotResponse;
            }

            // Update data approval and check other properties to set status
            originalCampaign.DataSetsApproved = approval.Approved;
            return SaveAndReturnStatus("SetMailshotApproval", originalCampaign);
        }

        [HttpPost]
        public HttpResponseMessage SetMailshotApproval(Guid id, SetApprovalViewModel approval)
        {
            ICampaign originalCampaign;
            // Make sure user can access campaign
            var validateResponse = ValidateRequest("SetMailshotApproval", id, out originalCampaign);
            if (validateResponse != null)
            {
                return validateResponse;
            }

            // Confirm mailshot can be updated
            var canUpdateMailshotResponse = ConfirmCanBeUpdated("SetMailshotApproval", originalCampaign);
            if (canUpdateMailshotResponse != null)
            {
                return canUpdateMailshotResponse;
            }

            // Update mailshot status and check other properties to set status
            if (originalCampaign.Mailshot == null)
            {
                return ErrorMessage(HttpStatusCode.Conflict, "No mailshot has been created for the current campaign.");
            }

            originalCampaign.Mailshot.Draft = !approval.Approved;
            return SaveAndReturnStatus("SetMailshotApproval", originalCampaign);
        }

        public HttpResponseMessage SetPostalOption(Guid id, SetPostageViewModel postage)
        {
            ICampaign originalCampaign;
            // Make sure user can access campaign
            var validateResponse = ValidateRequest("SetPostalOption", id, out originalCampaign);
            if (validateResponse != null)
            {
                return validateResponse;
            }

            // Confirm mailshot can be updated
            var canUpdateMailshotResponse = ConfirmCanBeUpdated("SetPostalOption", originalCampaign);
            if (canUpdateMailshotResponse != null)
            {
                return canUpdateMailshotResponse;
            }

            // Update mailshot status and check other properties to set status
            if (originalCampaign.Mailshot == null)
            {
                return ErrorMessage(HttpStatusCode.Conflict, "No mailshot has been created for the current campaign.");
            }

            var postalOption = _pricingService.GetPostalOption(postage.PostalOptionId);
            if (postage == null)
            {
                return ErrorMessage(HttpStatusCode.Conflict, "No postage option exists with that ID.");
            }

            originalCampaign.PostalOptionId = postalOption.PostalOptionId;
            originalCampaign.PostalOption = postalOption;
            return SaveAndReturnStatus("SetPostalOption", originalCampaign);
        }

        [Authorize]
        public HttpResponseMessage Update(Guid id, Campaign campaign)
        {
            ICampaign originalCampaign;
            // Make sure user can access campaign
            var validateResponse = ValidateRequest("Update", id, out originalCampaign);
            if (validateResponse != null)
            {
                return validateResponse;
            }

            // Confirm mailshot can be updated
            var canUpdateMailshotResponse = ConfirmCanBeUpdated("Update", originalCampaign);
            if (canUpdateMailshotResponse != null)
            {
                return canUpdateMailshotResponse;
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
            ICampaign originalCampaign;
            // Make sure user can access campaign
            var validateResponse = ValidateRequest("GetCopy", id, out originalCampaign);
            if (validateResponse != null)
            {
                return validateResponse;
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
            ICampaign originalCampaign;
            // Make sure user can access campaign
            var validateResponse = ValidateRequest("Delete", id, out originalCampaign);
            if (validateResponse != null)
            {
                return validateResponse;
            }

            // Confirm mailshot can be updated
            var canUpdateMailshotResponse = ConfirmCanBeUpdated("Delete", originalCampaign);
            if (canUpdateMailshotResponse != null)
            {
                return canUpdateMailshotResponse;
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
            ICampaign originalCampaign;
            // Make sure user can access campaign
            var validateResponse = ValidateRequest("LinkMailshotToCampaign", id, out originalCampaign);
            if (validateResponse != null)
            {
                return validateResponse;
            }

            // Confirm mailshot can be updated
            var canUpdateMailshotResponse = ConfirmCanBeUpdated("LinkMailshotToCampaign", originalCampaign);
            if (canUpdateMailshotResponse != null)
            {
                return canUpdateMailshotResponse;
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

        private HttpResponseMessage ValidateRequest(string methodName, Guid id, out ICampaign campaign)
        {
            campaign = null;

            // Confirm the user is logged in
            var authResult = Authenticate();
            if (authResult != null)
            {
                _logger.Error(this.GetType().Name, methodName, "Unauthenticated attempt to update campaign with ID {0}.", id);
                return authResult;
            }

            // Confirm that the original campaign exists
            campaign = _campaignService.GetCampaign(id);
            if (campaign == null)
            {
                _logger.Error(this.GetType().Name, methodName, "Attempt to link unknown campaign with ID {0}.", id);
                return ErrorMessage(HttpStatusCode.NotFound, "Campaign not found");
            }

            // Confirm that the user has access to the campaign
            if (campaign.UserId != _loggedInMember.Id)
            {
                _logger.Error(this.GetType().Name, methodName, "Unauthorised attempt to udpate campaign with ID {0}.", id);
                return ErrorMessage(HttpStatusCode.Forbidden, "No access to specified campaign");
            }

            return null;
        }

        private HttpResponseMessage ConfirmCanBeUpdated(string methodName, ICampaign campaign)
        {
            // Confirm that the campaign can be updated
            var updatableStatuses = new List<PCL.Enums.CampaignStatus>()
            {
                PCL.Enums.CampaignStatus.Draft,
                PCL.Enums.CampaignStatus.ReadyToCheckout
            };

            if (!updatableStatuses.Contains(campaign.Status))
            {
                _logger.Error(this.GetType().Name, methodName, "Attempt to modify in-progress campaign with ID {0}.", campaign.CampaignId);
                return ErrorMessage(HttpStatusCode.Conflict, "Campaign is in process and can no longer be modified.");
            }

            return null;
        }

        /// <summary>
        /// Tries to save changes to a campaign then returns a summary of the status
        /// </summary>
        private HttpResponseMessage SaveAndReturnStatus(string methodName, ICampaign originalCampaign)
        {
            originalCampaign.UpdatedDate = DateTime.UtcNow;

            if (originalCampaign.DataSetsApproved && originalCampaign.MailshotApproved && originalCampaign.PostalOption != null)
            {
                originalCampaign.Status = PCL.Enums.CampaignStatus.ReadyToCheckout;
            }
            else
            {
                originalCampaign.Status = PCL.Enums.CampaignStatus.Draft;
            }

            // Save the changse
            try
            {
                _campaignService.SaveCampaign(originalCampaign);
            }
            catch (Exception ex)
            {
                _logger.Exception(this.GetType().Name, methodName, ex);
                _logger.Error(this.GetType().Name, methodName, "Unable to update campaign {0}: {1}", originalCampaign.CampaignId, ex.Message);
                return ErrorMessage(HttpStatusCode.InternalServerError, "Unable to update due to server error");
            }

            var response = new CampaignStatusViewModel()
            {
                DataSetsApproved = originalCampaign.DataSetsApproved,
                MailshotApproved = originalCampaign.MailshotApproved,
                PostageSet = originalCampaign.PostalOption != null,
                CampaignStatus = originalCampaign.Status
            };

            // Return the result
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }


    }
}