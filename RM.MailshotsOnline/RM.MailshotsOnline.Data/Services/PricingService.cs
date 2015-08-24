using Microsoft.Azure;
using RM.MailshotsOnline.Business.Processors;
using RM.MailshotsOnline.Data.DAL;
using RM.MailshotsOnline.Data.Helpers;
using RM.MailshotsOnline.Entities.DataModels;
using RM.MailshotsOnline.PCL.Models;
using RM.MailshotsOnline.PCL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.MailshotsOnline.Data.Services
{
    public class PricingService : IPricingService
    {
        private StorageContext _context;

        /// <summary>
        /// Creates a new instance of the PricingService class
        /// </summary>
        public PricingService()
        {
            _context = new StorageContext();
        }

        /// <summary>
        /// Creates a new instance of the PricingService class, specifying the connection string name to pass to the Storage Context.
        /// Added to allow unit tests to specify the connection string properly.
        /// </summary>
        /// <param name="connectionStringName">Connection string name</param>
        public PricingService(string connectionStringName)
        {
            _context = new StorageContext(connectionStringName);
        }

        /// <summary>
        /// Gets the Postal Options
        /// </summary>
        public IEnumerable<IPostalOption> GetPostalOptions()
        {
            return _context.PostalOptions.AsEnumerable();
        }

        /// <summary>
        /// Gets a specific postal option
        /// </summary>
        /// <param name="postalOptionId">The ID of the postal option</param>
        /// <returns>Postal option object</returns>
        public IPostalOption GetPostalOption(Guid postalOptionId)
        {
            return _context.PostalOptions.FirstOrDefault(p => p.PostalOptionId == postalOptionId);
        }

        /// <summary>
        /// Gets a specific postal option by Umbraco ID
        /// </summary>
        /// <param name="umbracoId">Umbraco ID of the postal option</param>
        /// <returns>Postal option object</returns>
        public IPostalOption GetPostalOptionByUmbracoId(int umbracoId)
        {
            return _context.PostalOptions.FirstOrDefault(p => p.UmbracoId == umbracoId);
        }

        /// <summary>
        /// Saves a postal option to the database
        /// </summary>
        /// <param name="postalOption">Postal option</param>
        /// <returns>The updated postal option</returns>
        public IPostalOption SavePostalOption(IPostalOption postalOption)
        {
            if (postalOption.PostalOptionId == Guid.Empty)
            {
                _context.PostalOptions.Add((PostalOption)postalOption);
            }

            _context.SaveChanges();
            return postalOption;
        }

        /// <summary>
        /// Generates a price breakdown based on a campaign
        /// </summary>
        /// <param name="campaign">The Campaign to get the price breakdown for</param>
        /// <returns>Price breakdown</returns>
        public ICampaignPriceBreakdown GetPriceBreakdown(ICampaign campaign)
        {
            if (campaign.Mailshot == null && campaign.MailshotId.HasValue && campaign.MailshotId != Guid.Empty)
            {
                var mailshot = _context.Mailshots.Include("Format").FirstOrDefault(m => m.MailshotId == campaign.MailshotId);
                campaign.Mailshot = mailshot;
            }

            if (campaign.Mailshot != null && campaign.Mailshot.Format == null && campaign.Mailshot.FormatId != Guid.Empty)
            {
                var format = _context.Formats.FirstOrDefault(f => f.FormatId == campaign.Mailshot.FormatId);
                campaign.Mailshot.Format = format;
            }

            if (campaign.PostalOption == null && campaign.PostalOptionId.HasValue && campaign.PostalOptionId != Guid.Empty)
            {
                var postalOption = _context.PostalOptions.FirstOrDefault(p => p.PostalOptionId == campaign.PostalOptionId);
                campaign.PostalOption = postalOption;
            }

            var result = new Entities.JsonModels.CampaignPriceBreakdown();
            if (campaign.HasDataSearches || campaign.HasDistributionLists)
            {
                result.PrintCount = campaign.OwnDataRecipientCount + campaign.RentedDataRecipientCount;
            }

            result.TaxRate = ConfigHelper.TaxRate;
            result.DataRentalRate = ConfigHelper.PricePerRentedDataRecord;
            if (campaign.HasDataSearches)
            {
                result.DataRentalCount = campaign.RentedDataRecipientCount;
            }

            if (campaign.PostalOption != null)
            {
                result.PostageRate = campaign.PostalOption.PricePerUnit;
            }
            
            if (campaign.Mailshot != null && campaign.Mailshot.Format != null)
            {
                result.ServiceFee = campaign.Mailshot.Format.OnceOffPrice;
                result.PrintingRate = campaign.Mailshot.Format.PricePerPrint;
            }

            return result;
        }
    }
}
