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
            return _context.Campaigns
                .Include("Mailshot")
                .Include("Mailshot.Format")
                .Include("CampaignDistributionLists")
                .Include("CampaignDistributionLists.DistributionList")
                .Include("DataSearches")
                .Include("PostalOption")
                .Where(filter);
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
        /// Gets orders (campaigns that are in progress or completed) for a given user ID
        /// </summary>
        /// <param name="userId">The ID of the user to search on</param>
        /// <returns>Collection of Campaigns</returns>
        public IEnumerable<ICampaign> GetOrdersForUser(int userId)
        {
            var inProgressOrCompletedStatuses = new List<PCL.Enums.CampaignStatus>()
            {
                PCL.Enums.CampaignStatus.Exception,
                PCL.Enums.CampaignStatus.Fulfilled,
                PCL.Enums.CampaignStatus.PendingModeration,
                PCL.Enums.CampaignStatus.ReadyForFulfilment,
                PCL.Enums.CampaignStatus.SentForFulfilment,
                PCL.Enums.CampaignStatus.Cancelled,
                PCL.Enums.CampaignStatus.PaymentFailed
            };

            return _context.Campaigns
                .Include("Invoices")
                .Include("Invoices.LineItems")
                .Include("Invoices.LineItems.Product")
                .Include("PostalOption")
                .Where(c => c.UserId == userId && inProgressOrCompletedStatuses.Contains(c.Status))
                .OrderByDescending(c => c.UpdatedDate);
        }

        /// <summary>
        /// Gets a specific campaign
        /// </summary>
        /// <param name="campaignId">ID of the campaign to find</param>
        /// <returns>Campaign object</returns>
        public ICampaign GetCampaign(Guid campaignId)
        {
            return _context.Campaigns
                .Include("Mailshot")
                .Include("Mailshot.Format")
                .Include("CampaignDistributionLists")
                .Include("CampaignDistributionLists.DistributionList")
                .Include("DataSearches")
                .Include("PostalOption")
                .FirstOrDefault(c => c.CampaignId == campaignId);
        }

        /// <summary>
        /// Gets a specific campaign (including the attached invoices)
        /// </summary>
        /// <param name="campaignId">ID of the campaign</param>
        /// <returns>Campaign object</returns>
        public ICampaign GetCampaignWithInvoices(Guid campaignId)
        {
            return _context.Campaigns
                .Include("Mailshot")
                .Include("Mailshot.Format")
                .Include("CampaignDistributionLists")
                .Include("CampaignDistributionLists.DistributionList")
                .Include("DataSearches")
                .Include("PostalOption")
                .Include("Invoices")
                .Include("Invoices.LineItems")
                .Include("Invoices.LineItems.Product")
                .FirstOrDefault(c => c.CampaignId == campaignId);
        }

        /// <summary>
        /// Gets a specific campaign based on its moderation ID
        /// </summary>
        /// <param name="moderationId">The Moderation ID</param>
        /// <returns></returns>
        public ICampaign GetCampaignByModerationId(Guid moderationId)
        {
            return _context.Campaigns
                .Include("Mailshot")
                .Include("Mailshot.Format")
                .Include("CampaignDistributionLists")
                .Include("CampaignDistributionLists.DistributionList")
                .Include("DataSearches")
                .Include("PostalOption")
                .FirstOrDefault(c => c.ModerationId == moderationId);
        }

        /// <summary>
        /// Saves a campaign to the database
        /// </summary>
        /// <param name="campaign">The campaign to save</param>
        /// <returns>Saved campaign object</returns>
        public ICampaign SaveCampaign(ICampaign campaign)
        {
            campaign.UpdatedDate = DateTime.UtcNow;
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

        /// <summary>
        /// Adds test data to a campaign
        /// </summary>
        /// <param name="campaign">The campaign</param>
        /// <returns>True on success</returns>
        public bool AddTestDataToCampaign(ICampaign campaign)
        {
            var random = new Random();
            random.Next(500, 1500);
            var dataAdded = false;

            if (!_context.CampaignDistributionLists.Any(cdl => cdl.CampaignId == campaign.CampaignId))
            {
                Guid distListId = Guid.Empty;
                var existingDataList = _context.DistributionLists.FirstOrDefault(dl => dl.UserId == campaign.UserId);
                if (existingDataList != null)
                {
                    distListId = existingDataList.DistributionListId;
                }
                else
                {
                    var newDistributionList = new DistributionList()
                    {
                        Name = "Test list",
                        RecordCount = random.Next(500, 1500),
                        UserId = campaign.UserId
                    };
                    var savedList = _context.DistributionLists.Add(newDistributionList);
                    distListId = savedList.DistributionListId;
                }

                if (distListId != Guid.Empty)
                {
                    _context.CampaignDistributionLists.Add(new CampaignDistributionList() { CampaignId = campaign.CampaignId, DistributionListId = distListId });
                    dataAdded = true;
                }

                _context.SaveChanges();
            }

            if (!_context.DataSearch.Any(ds => ds.CampaignId == campaign.CampaignId))
            {
                _context.DataSearch.Add(new DataSearch()
                {
                    CampaignId = campaign.CampaignId,
                    Name = "Test data search",
                    SearchCriteria = "Test",
                    Status = PCL.Enums.DataSearchStatus.Draft,
                    ThirdPartyIdentifier = "Test",
                    RecordCount = random.Next(500, 1500)
                });
                _context.SaveChanges();
                dataAdded = true;
            }

            return dataAdded;
        }
    }
}
