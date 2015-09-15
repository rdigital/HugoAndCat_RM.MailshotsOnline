using HC.RM.Common.Azure;
using HC.RM.Common.Azure.ServiceBus;
using HC.RM.Common.Orders;
using HC.RM.Common.PCL.Helpers;
using Microsoft.ServiceBus.Messaging;
using RM.MailshotsOnline.Business.Processors;
using RM.MailshotsOnline.Data.Helpers;
using RM.MailshotsOnline.Entities.ViewModels;
using RM.MailshotsOnline.PCL.Models;
using RM.MailshotsOnline.PCL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.MailshotsOnline.Data.Services
{
    public class SparqQueueService : ISparqQueueService
    {
        private ILogger _log;

        public SparqQueueService()
        {
            _log = new Logger();
        }

        /// <summary>
        /// Send a mailshot off for rendering and printing
        /// </summary>
        /// <param name="mailshot">Mailshot to be printed</param>
        /// <param name="postbackUrl">The URL the service needs to post back to</param>
        /// <returns>True on success</returns>
        public async Task<bool> SendRenderAndPrintJob(IMailshot mailshot, string postbackUrl)
        {
            return await SendJob(mailshot, true, postbackUrl);
        }

        /// <summary>
        /// Send a mailshot for rendering only
        /// </summary>
        /// <param name="mailshot">Mailshot to be rendered</param>
        /// <param name="postbackUrl">The URL the service needs to post back to</param>
        /// <returns>True on success</returns>
        public async Task<bool> SendRenderJob(IMailshot mailshot, string postbackUrl)
        {
            return await SendJob(mailshot, false, postbackUrl);
        }

        /// <summary>
        /// Send a mailshot to the Sparq Queue
        /// </summary>
        /// <param name="mailshot">Mailshot to send</param>
        /// <param name="printAfterRender">Set to true to the resulting PDF sent to St. Ive's for printing</param>
        /// <returns>True on success</returns>
        private async Task<bool> SendJob(IMailshot mailshot, bool printAfterRender, string postbackUrl)
        {
            var success = true;
            // Generate XML and XSL from Mailshot
            var mailshotProcessor = new MailshotsProcessor();
            ProcessedMailshotData xmlAndXsl = null;

            try
            {
                xmlAndXsl = mailshotProcessor.GetXmlAndXslForMailshot(mailshot);
            }
            catch (Exception ex)
            {
                _log.Exception(this.GetType().Name, "SendJob", ex);
                _log.Error(this.GetType().Name, "SendJob", "Unable to send job to Sparq queue for Mailshot {0}", mailshot.MailshotId);
            }

            if (xmlAndXsl == null)
            {
                // Error creating XML and XSL
                success = false;
            }
            else
            {
                // Create new message for queue
                var baseUrl = string.Format("{0}://{1}:{2}", ConfigHelper.HostedScheme, ConfigHelper.HostedDomain, ConfigHelper.HostedPort);
                //var postbackUrl = string.Format("{0}/Umbraco/Api/ProofPdf/ProofReady/{1}", baseUrl, mailshot.MailshotId);
                var ftpPostbackUrl = printAfterRender ? string.Format("{0}/Umbraco/Api/ProofPdf/PrintPdfReady/{1}", baseUrl, mailshot.MailshotId) : null;
                var orderPriority = printAfterRender ? SparqOrderPriority.Low : SparqOrderPriority.High;
                var orderType = printAfterRender ? SparqOrderType.RenderAndPrint : SparqOrderType.RenderOnly;
                var groupOrder = printAfterRender; // TODO: Confirm that this is correct

                var order = new SparqOrder(mailshot.ProofPdfOrderNumber.ToString(),
                    "",
                    mailshot.FormatId.ToString(),
                    Encoding.UTF8.GetBytes(xmlAndXsl.XmlData),
                    Encoding.UTF8.GetBytes(xmlAndXsl.XslStylesheet),
                    baseUrl,
                    orderPriority,
                    orderType,
                    groupOrder,
                    postbackUrl,
                    ftpPostbackUrl);

                _log.Info(this.GetType().Name, "SendJob", $@"Sending job with the following parameters:
                    Mailshot ID: {mailshot.MailshotId},
                    Base URL: {baseUrl},
                    Postback URL: {postbackUrl},
                    FTP postback URL: {ftpPostbackUrl},
                    Order Priority: {orderPriority},
                    Order Type: {orderType},
                    Group Order: {groupOrder}");

                // Send to queue
                var message = new BrokeredMessage(order);

                if (SparqQueueConnector.RenderOnlyQueue == null || SparqQueueConnector.RenderAndPrintQueueClient == null)
                {
                    SparqQueueConnector.Initialize();
                }

                if (printAfterRender)
                {
                    await SparqQueueConnector.RenderAndPrintQueueClient.SendAsync(message);
                }
                else
                {
                    await SparqQueueConnector.RenderOnlyQueueClient.SendAsync(message);
                }

                success = true;
            }

            return success;
        }
    }
}
