using RM.MailshotsOnline.PCL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.MailshotsOnline.PCL.Services
{
    public interface ISparqQueueService
    {
        /// <summary>
        /// Send a mailshot for rendering only
        /// </summary>
        /// <param name="mailshot">Mailshot to be rendered</param>
        /// <param name="postbackUrl">The URL the service needs to post back to</param>
        /// <returns>True on success</returns>
        Task<bool> SendRenderJob(IMailshot mailshot, string postbackUrl);

        /// <summary>
        /// Send a mailshot off for rendering and printing
        /// </summary>
        /// <param name="mailshot">Mailshot to be printed</param>
        /// <param name="postbackUrl">The URL the service needs to post back to</param>
        /// <param name="ftpPostbackUrl">FTP postback URL</param>
        /// <returns>True on success</returns>
        Task<bool> SendRenderAndPrintJob(IMailshot mailshot, string postbackUrl, string ftpPostbackUrl);

        /// <summary>
        /// Send a render job to the queue
        /// </summary>
        /// <param name="data">XML and XSL data</param>
        /// <param name="orderNumber">Order number</param>
        /// <param name="formatId">Format ID</param>
        /// <param name="renderPostbackUrl">Render postback URL</param>
        /// <param name="ftpPostbackUrl">FTP postback URL</param>
        /// <returns>True on success</returns>
        Task<bool> SendRenderJob(IXmlAndXslData data, string orderNumber, string formatId, string renderPostbackUrl);
    }
}
