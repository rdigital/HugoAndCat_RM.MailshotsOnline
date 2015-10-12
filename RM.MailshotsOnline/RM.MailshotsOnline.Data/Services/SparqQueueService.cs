using HC.RM.Common.Azure;
using HC.RM.Common.Azure.Persistence;
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
        private string _baseUrl;

        public SparqQueueService()
        {
            _log = new Logger();
            _baseUrl = string.Format("{0}://{1}:{2}", ConfigHelper.HostedScheme, ConfigHelper.HostedDomain, ConfigHelper.HostedPort);
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
        /// Send a render job to the queue
        /// </summary>
        /// <param name="data">XML and XSL data</param>
        /// <param name="orderNumber">Order number</param>
        /// <param name="formatId">Format ID</param>
        /// <param name="renderPostbackUrl">Render postback URL</param>
        /// <returns>True on success</returns>
        public async Task<bool> SendRenderJob(IXmlAndXslData data, string orderNumber, string formatId, string renderPostbackUrl)
        {
            return await SendJob(data, orderNumber, formatId, false, renderPostbackUrl, null);
        }

        /// <summary>
        /// Sends a job to the queue
        /// </summary>
        /// <param name="data">XML and XSL data</param>
        /// <param name="orderNumber">Order number</param>
        /// <param name="formatId">Format ID</param>
        /// <param name="printAfterRender">Job should be printed after rendering</param>
        /// <param name="renderPostbackUrl">Render postback URL</param>
        /// <param name="ftpPostbackUrl">FTP postback URL</param>
        /// <returns></returns>
        private async Task<bool> SendJob(IXmlAndXslData data, string orderNumber, string formatId, bool printAfterRender, string renderPostbackUrl, string ftpPostbackUrl)
        {
            bool success = true;

            // Create new message for queue
            var orderPriority = printAfterRender ? SparqOrderPriority.Low : SparqOrderPriority.High;
            var orderType = printAfterRender ? SparqOrderType.RenderAndPrint : SparqOrderType.RenderOnly;
            var groupOrder = printAfterRender; // TODO: Confirm that this is correct

            var order = new SparqOrder(orderNumber,
                "",
                formatId,
                Encoding.UTF8.GetBytes(data.XmlData),
                Encoding.UTF8.GetBytes(data.XslStylesheet),
                _baseUrl,
                orderPriority,
                orderType,
                groupOrder,
                renderPostbackUrl,
                ftpPostbackUrl);

            _log.Info(this.GetType().Name, "SendJob", $@"Sending job with the following parameters:
    Order Number: {orderNumber},
    Base URL: {_baseUrl},
    Postback URL: {renderPostbackUrl},
    FTP postback URL: {ftpPostbackUrl},
    Order Priority: {orderPriority},
    Order Type: {orderType},
    Group Order: {groupOrder}");

            try
            {
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
            }
            catch (Exception ex)
            {
                success = false;
                _log.Exception(this.GetType().Name, "SendJob", ex);
                _log.Error(this.GetType().Name, "SendJob", "Error sending job with order number {0} to the queue.", orderNumber);
            }

            return success;
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
            var blobStorage = new BlobStorage(ConfigHelper.PrivateStorageConnectionString);
            var blobService = new BlobService(blobStorage, ConfigHelper.PrivateMediaBlobStorageContainer);
            var mailshotProcessor = new MailshotsProcessor(_log, blobService);
            XmlAndXslData xmlAndXsl = null;

            try
            {
                xmlAndXsl = await mailshotProcessor.GetXmlAndXslForMailshot(mailshot, ConfigHelper.SparqServiceIpRangeStart, ConfigHelper.SparqServiceIpRangeEnd);
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
                if (ConfigHelper.SaveMailshotInfoForDebug)
                {
                    // Save the XML and XSL to blob storage so we can see what's being saved
                    DateTime saveDate = DateTime.UtcNow;
                    await blobService.StoreAsync(Encoding.UTF8.GetBytes(xmlAndXsl.XmlData), string.Format("debug/{0}/{1}/{2:yyyyMMdd-HHmmss}/xmlData.xml", mailshot.UserId, mailshot.MailshotId, saveDate), "text/xml");
                    await blobService.StoreAsync(Encoding.UTF8.GetBytes(xmlAndXsl.XslStylesheet), string.Format("debug/{0}/{1}/{2:yyyyMMdd-HHmmss}/stylesheet.xslt", mailshot.UserId, mailshot.MailshotId, saveDate), "text/xml");
                }

                var ftpPostbackUrl = printAfterRender ? string.Format("{0}/Umbraco/Api/ProofPdf/PrintPdfReady/{1}", _baseUrl, mailshot.MailshotId) : null;
                var orderNumber = mailshot.ProofPdfOrderNumber.ToString();
                success = await SendJob(xmlAndXsl, orderNumber, mailshot.FormatId.ToString(), printAfterRender, postbackUrl, ftpPostbackUrl);
            }

            return success;
        }
    }
}
