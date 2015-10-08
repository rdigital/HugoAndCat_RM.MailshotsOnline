using RM.MailshotsOnline.PCL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RM.MailshotsOnline.PCL.Services
{
    public interface ICampaignService
    {
        /// <summary>
        /// Gets all campaigns
        /// </summary>
        /// <returns>Collection of campaigns</returns>
        IEnumerable<ICampaign> GetCampaigns();

        /// <summary>
        /// Gets campaigns that satisfy the given filter
        /// </summary>
        /// <param name="filter">Filter method</param>
        /// <returns>Collection of campaigns</returns>
        IEnumerable<ICampaign> GetCampaigns(Func<ICampaign, bool> filter);

        /// <summary>
        /// Gets the campaigns for a given user ID
        /// </summary>
        /// <param name="userId">The ID of the user to search on</param>
        /// <returns>Collection of Campaigns</returns>
        IEnumerable<ICampaign> GetCampaignsForUser(int userId);

        /// <summary>
        /// Gets orders (campaigns that are in progress or completed) for a given user ID
        /// </summary>
        /// <param name="userId">The ID of the user to search on</param>
        /// <returns>Collection of Campaigns</returns>
        IEnumerable<ICampaign> GetOrdersForUser(int userId);

        /// <summary>
        /// Gets a specific campaign
        /// </summary>
        /// <param name="campaignId">ID of the campaign to find</param>
        /// <returns>Campaign object</returns>
        ICampaign GetCampaign(Guid campaignId);

        /// <summary>
        /// Gets a specific campaign (including the attached invoices)
        /// </summary>
        /// <param name="campaignId">ID of the campaign</param>
        /// <returns>Campaign object</returns>
        ICampaign GetCampaignWithInvoices(Guid campaignId);

        /// <summary>
        /// Gets a specific campaign based on its moderation ID
        /// </summary>
        /// <param name="moderationId">The Moderation ID</param>
        /// <returns></returns>
        ICampaign GetCampaignByModerationId(Guid moderationId);

        /// <summary>
        /// Saves a campaign to the database
        /// </summary>
        /// <param name="campaign">The campaign to save</param>
        /// <returns>Saved campaign object</returns>
        ICampaign SaveCampaign(ICampaign campaign);

        /// <summary>
        /// Deletes a campaign
        /// </summary>
        /// <param name="campaign">The campaign to delete</param>
        /// <returns>Bool if successful, false otherwise</returns>
        bool DeleteCampaign(ICampaign campaign);

        /// <summary>
        /// Deletes a campaign
        /// </summary>
        /// <param name="campaignId">The ID of the campaign to delete</param>
        /// <returns>Bool if successful, false otherwise</returns>
        bool DeleteCampaignWithId(Guid campaignId);

        /// <summary>
        /// Adds test data to a campaign
        /// </summary>
        /// <param name="campaign">The campaign</param>
        /// <returns>True on success</returns>
        bool AddTestDataToCampaign(ICampaign campaign);
    }
}
