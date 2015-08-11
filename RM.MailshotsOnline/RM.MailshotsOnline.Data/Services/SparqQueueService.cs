using HC.RM.Common.Azure.ServiceBus;
using HC.RM.Common.Orders;
using Microsoft.ServiceBus.Messaging;
using RM.MailshotsOnline.Business.Processors;
using RM.MailshotsOnline.Data.Helpers;
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
        public SparqQueueService()
        {
        }

        /// <summary>
        /// Send a mailshot off for rendering and printing
        /// </summary>
        /// <param name="mailshot">Mailshot to be printed</param>
        /// <returns>True on success</returns>
        public async Task<bool> SendRenderAndPrintJob(IMailshot mailshot)
        {
            return await SendJob(mailshot, true);
        }

        /// <summary>
        /// Send a mailshot for rendering only
        /// </summary>
        /// <param name="mailshot">Mailshot to be rendered</param>
        /// <returns>True on success</returns>
        public async Task<bool> SendRenderJob(IMailshot mailshot)
        {
            return await SendJob(mailshot, false);
        }

        /// <summary>
        /// Send a mailshot to the Sparq Queue
        /// </summary>
        /// <param name="mailshot">Mailshot to send</param>
        /// <param name="printAfterRender">Set to true to the resulting PDF sent to St. Ive's for printing</param>
        /// <returns>True on success</returns>
        private async Task<bool> SendJob(IMailshot mailshot, bool printAfterRender)
        {
            var success = true;
            // Generate XML and XSL from Mailshot
            var mailshotProcessor = new MailshotsProcessor();
            var xmlAndXsl = mailshotProcessor.GetXmlAndXslForMailshot(mailshot);

            if (xmlAndXsl == null)
            {
                // Error creating XML and XSL
                // Should have thrown an exception
                success = false;
            }
            else
            {
                // Create new message for queue
                var baseUrl = string.Format("{0}://{1}:{2}", ConfigHelper.HostedScheme, ConfigHelper.HostedDomain, ConfigHelper.HostedPort);
                var postbackUrl = string.Format("{0}/Umbraco/Api/ProofPdf/ProofReady/{1}", baseUrl, mailshot.MailshotId);
                var ftpPostbackUrl = printAfterRender ? string.Format("{0}/Umbraco/Api/ProofPdf/PrintPdfReady/{1}", baseUrl, mailshot.MailshotId) : null;
                var orderPriority = printAfterRender ? SparqOrderPriority.Low : SparqOrderPriority.High;
                var orderType = printAfterRender ? SparqOrderType.RenderAndPrint : SparqOrderType.RenderOnly;
                var groupOrder = printAfterRender; // TODO: Confirm that this is correct
                var order = new SparqOrder(mailshot.MailshotId, // Mailshot ID
                                           Encoding.UTF8.GetBytes(xmlAndXsl.Item1), // XML Bytes
                                           Encoding.UTF8.GetBytes(xmlAndXsl.Item2), // XSL Bytes
                                           baseUrl, // Base URL for additional assets
                                           orderPriority, // Priority
                                           orderType, // Order type
                                           groupOrder, // Order is a group order
                                           postbackUrl, // Proof is ready postback URL
                                           ftpPostbackUrl); // Print PDF is ready postback URL

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
