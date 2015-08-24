using RM.MailshotsOnline.PCL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RM.MailshotsOnline.PCL.Models;
using RM.MailshotsOnline.Data.DAL;
using RM.MailshotsOnline.Entities.DataModels;
using RM.MailshotsOnline.Business.Processors;
using RM.MailshotsOnline.Data.Helpers;

namespace RM.MailshotsOnline.Data.Services
{
    public class CampaignService : ICampaignService
    {
        private StorageContext _context;

        public CampaignService() 
            : this(new StorageContext())
        { }

        public CampaignService(StorageContext storageContext)
        {
            _context = storageContext;
        }
        
        /// <summary>
        /// Gets all campaigns
        /// </summary>
        /// <returns>Collection of campaigns</returns>
        public IEnumerable<ICampaign> GetCampaigns()
        {
            return _context.Campaigns.AsEnumerable();
        }

        /// <summary>
        /// Gets campaigns that satisfy the given filter
        /// </summary>
        /// <param name="filter">Filter method</param>
        /// <returns>Collection of campaigns</returns>
        public IEnumerable<ICampaign> GetCampaigns(Func<ICampaign, bool> filter)
        {
            return _context.Campaigns.Include("Mailshot").Include("CampaignDistributionLists").Include("DataSearches").Include("PostalOption").Include("CampaignDistributionLists.DistributionList").Where(filter);
        }

        /// <summary>
        /// Gets the campaigns for a given user ID
        /// </summary>
        /// <param name="userId">The ID of the user to search on</param>
        /// <returns>Collection of Campaigns</returns>
        public IEnumerable<ICampaign> GetCampaignsForUser(int userId)
        {
            return GetCampaigns(c => c.UserId == userId).OrderByDescending(c => c.UpdatedDate);
        }

        /// <summary>
        /// Gets a specific campaign
        /// </summary>
        /// <param name="campaignId">ID of the campaign to find</param>
        /// <returns>Campaign object</returns>
        public ICampaign GetCampaign(Guid campaignId)
        {
            return _context.Campaigns.Include("Mailshot").Include("CampaignDistributionLists").Include("DataSearches").Include("PostalOption").Include("CampaignDistributionLists.DistributionList").FirstOrDefault(c => c.CampaignId == campaignId);
        }

        /// <summary>
        /// Saves a campaign to the database
        /// </summary>
        /// <param name="campaign">The campaign to save</param>
        /// <returns>Saved campaign object</returns>
        public ICampaign SaveCampaign(ICampaign campaign)
        {
            if (campaign.CampaignId == Guid.Empty)
            {
                _context.Campaigns.Add((Campaign)campaign);
            }

            _context.SaveChanges();
            return campaign;
        }

        /// <summary>
        /// Deletes a campaign
        /// </summary>
        /// <param name="campaign">The campaign to delete</param>
        /// <returns>Bool if successful, false otherwise</returns>
        public bool DeleteCampaign(ICampaign campaign)
        {
            var success = true;
            try
            {
                _context.Campaigns.Remove((Campaign)campaign);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                // TODO: Log the exception
                success = false;
            }

            return success;
        }

        /// <summary>
        /// Deletes a campaign
        /// </summary>
        /// <param name="campaignId">The ID of the campaign to delete</param>
        /// <returns>Bool if successful, false otherwise</returns>
        public bool DeleteCampaignWithId(Guid campaignId)
        {
            var campaign = GetCampaign(campaignId);
            if (campaign != null)
            {
                return DeleteCampaign(campaign);
            }
            else
            {
                return false;
            }
        }
    }
}
