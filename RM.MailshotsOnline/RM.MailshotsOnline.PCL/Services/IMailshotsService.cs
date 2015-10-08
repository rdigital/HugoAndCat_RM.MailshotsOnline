using RM.MailshotsOnline.PCL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.MailshotsOnline.PCL.Services
{
    public interface IMailshotsService
    {
        /// <summary>
        /// Finds all Mailshots
        /// </summary>
        /// <returns>Collection of Mailshots</returns>
        IEnumerable<IMailshot> GetAllMailshots();

        /// <summary>
        /// Checks if a mailshot belongs to a user
        /// </summary>
        /// <param name="mailshotId">ID of the mailshot</param>
        /// <param name="userId">ID of the user</param>
        /// <returns>Returns true if the mailshot belongs to the user</returns>
        bool MailshotBelongsToUser(Guid mailshotId, int userId);

        /// <summary>
        /// Gets a user's mailshots
        /// </summary>
        /// <param name="userId">The ID of the user to search against</param>
        /// <param name="draftOnly">Indicates if only draft Mailshots should be returned</param>
        /// <returns>Collection of Mailshot objects</returns>
        IEnumerable<IMailshot> GetUsersMailshots(int userId, bool draftOnly = false);

        /// <summary>
        /// Gets a specific Mailshot
        /// </summary>
        /// <param name="mailshotId">The ID of the mailshot to get</param>
        /// <returns>Mailshot object</returns>
        IMailshot GetMailshot(Guid mailshotId);

        /// <summary>
        /// Gets a specific Mailshot
        /// </summary>
        /// <param name="mailshotId">The ID of the mailshot to get</param>
        /// <returns>Mailshot object</returns>
        Task<IMailshot> GetMailshotAsync(Guid mailshotId);

        /// <summary>
        /// Gets a specific Mailshot
        /// </summary>
        /// <param name="mailshotId">The ID of the mailshot to get</param>
        /// <returns>Mailshot object</returns>
        IMailshot GetMailshotWithCampaignData(Guid mailshotId);

        /// <summary>
        /// Gets a specific Mailshot
        /// </summary>
        /// <param name="mailshotId">The ID of the mailshot to get</param>
        /// <returns>Mailshot object</returns>
        Task<IMailshot> GetMailshotWithCampaignDataAsync(Guid mailshotId);

        /// <summary>
        /// Saves a Mailshot to the database
        /// </summary>
        /// <param name="mailshot"></param>
        /// <returns></returns>
        IMailshot SaveMailshot(IMailshot mailshot);

        /// <summary>
        /// Saves a Mailshot to the database
        /// </summary>
        /// <param name="mailshot"></param>
        /// <returns></returns>
        Task<IMailshot> SaveMailshotAsync(IMailshot mailshot);

        /// <summary>
        /// Deletes a Mailshot
        /// </summary>
        /// <param name="mailshot">The Mailshot to be deleted</param>
        /// <returns>True on success</returns>
        bool Delete(IMailshot mailshot);

        /// <summary>
        /// Updates the links between Mailshots and CMS images
        /// </summary>
        /// <param name="mailshot">Mailshot to link images to</param>
        /// <param name="linkedImages">List of image URLs to be linked</param>
        void UpdateLinkedImages(IMailshot mailshot, IEnumerable<string> linkedImages);

        /// <summary>
        /// Checks if the given mailshot is used in a campaign
        /// </summary>
        /// <param name="mailshot">Mailshot to check</param>
        /// <returns>True if the mailshot is used, false otherwise</returns>
        bool MailshotIsUsedInCampaign(IMailshot mailshot);
    }
}
