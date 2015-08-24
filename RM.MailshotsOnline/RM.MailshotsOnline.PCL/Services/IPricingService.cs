using RM.MailshotsOnline.PCL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RM.MailshotsOnline.PCL.Services
{
    public interface IPricingService
    {
        /// <summary>
        /// Gets the Postal Options
        /// </summary>
        IEnumerable<IPostalOption> GetPostalOptions();

        /// <summary>
        /// Gets a specific postal option
        /// </summary>
        /// <param name="postalOptionId">The ID of the postal option</param>
        /// <returns>Postal option object</returns>
        IPostalOption GetPostalOption(Guid postalOptionId);

        /// <summary>
        /// Gets a specific postal option by Umbraco ID
        /// </summary>
        /// <param name="umbracoId">Umbraco ID of the postal option</param>
        /// <returns>Postal option object</returns>
        IPostalOption GetPostalOptionByUmbracoId(int umbracoId);

        /// <summary>
        /// Saves a postal option to the database
        /// </summary>
        /// <param name="postalOption">Postal option</param>
        /// <returns>The updated postal option</returns>
        IPostalOption SavePostalOption(IPostalOption postalOption);

        /// <summary>
        /// Generates a price breakdown based on a campaign
        /// </summary>
        /// <param name="campaign">The Campaign to get the price breakdown for</param>
        /// <returns>Price breakdown</returns>
        ICampaignPriceBreakdown GetPriceBreakdown(ICampaign campaign);
    }
}
