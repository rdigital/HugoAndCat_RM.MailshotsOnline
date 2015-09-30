using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Xml.Linq;
using ClientDependency.Core;
using CsvHelper;
using HC.RM.Common.Azure.EntryPoints;
using HC.RM.Common.Network;
using HC.RM.Common.PCL.Persistence;
using Microsoft.WindowsAzure.Storage.Queue;
using HC.RM.Common.Azure.Helpers;
using RM.MailshotsOnline.Data.Services.Reporting;
using RM.MailshotsOnline.PCL.Models.Reporting;
using RM.MailshotsOnline.PCL.Services;
using RM.MailshotsOnline.PCL.Services.Reporting;
using RM.MailshotsOnline.Web.Controllers.Api;
using umbraco.cms.businesslogic;
using Constants = RM.MailshotsOnline.Data.Constants.Constants;

namespace RM.MailshotsOnline.WorkerRole.EntryPoints
{
    public class ReportGeneratorWorker : WorkerEntryPoint
    {
        private static readonly object SyncLock = new object();
        private static bool _working;
        private static TimeSpan _queueInterval;
        private static IBlobService _blobService;
        private static IFtpService _ftpService;
        private static ICryptographicService _cryptographicService;

        public ReportGeneratorWorker(ILogger logger, IBlobService blobService, IFtpService ftpService,
            ICryptographicService cryptographicService, string connectionString, string queueName,
            TimeSpan? queueInterval = null)
            : base(logger, connectionString, queueName)
        {
            _blobService = blobService;
            _ftpService = ftpService;
            _cryptographicService = cryptographicService;
            _queueInterval = queueInterval ?? new TimeSpan(0, 15, 0);
        }

        public override void Run()
        {
            // ensure only 1 instance is processing queue messages
            lock (SyncLock)
            {
                if (_working)
                {
                    return;
                }

                _working = true;
            }

            try
            {
                // Get the message at the head of the queue. Note: FIFO is
                // not guaranteed
                var queueMsg = Queue.GetMessage();

                while (queueMsg != null)
                {
                    Logger.Info(GetType().Name, "Run", "Processing Message: {0}", queueMsg.Id);

                    /*
                     * Message should look something like:
                     * <?xml version="1.0" encoding="utf-16"?>
                     * <StorageQueueMessage xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
                     *   <ExecutionTag>e927cf76d24da9625608248ce654cfa7</ExecutionTag>
                     *   <ClientRequestId>3c4b84e2-aee5-43ac-913e-c05c8827ac41</ClientRequestId>
                     *   <ExpectedExecutionTime>2015-09-10T00:00:00</ExpectedExecutionTime>
                     *   <SchedulerJobId>rm-photopost-creditreports</SchedulerJobId>
                     *   <SchedulerJobCollectionId>bdhcjobs</SchedulerJobCollectionId>
                     *   <Region>West Europe</Region>
                     *   <Message>the message</Message>
                     * </StorageQueueMessage>
                     */

                    // Parse the message as an XDocument
                    var messageDoc = XDocument.Parse(queueMsg.AsString);

                    // Grab the Message Element
                    var messageElement = messageDoc.Descendants("Message").SingleOrDefault();

                    if (messageElement == null)
                    {
                        Logger.Error(GetType().Name, "Run", "Queue Message did not contain a Message element: {0}: {1}", queueMsg.Id, queueMsg.AsString);

                        Queue.DeleteMessage(queueMsg);
                        queueMsg = Queue.GetMessage();

                        continue;
                    }

                    // Grab the message
                    var message = messageElement.Value.ToLower();

                    if (string.IsNullOrEmpty(message))
                    {
                        Logger.Error(GetType().Name, "Run", "Queue Message was parsed successfully but didn't contain a message: {0}: {1}", queueMsg.Id, queueMsg.AsString);

                        Queue.DeleteMessage(queueMsg);
                        queueMsg = Queue.GetMessage();

                        continue;
                    }

                    switch (message)
                    {
                        case "membership":
                        case "transactions":

                            Logger.Info(GetType().Name, "Run", $"Message: {message}. Actioning...");

                            var newToken = Guid.NewGuid().ToString();
                            var url = $"{Constants.Apis.TokenAuthApi}/settoken/{newToken}";

                            if (SendHttpPost(url, newToken) == HttpStatusCode.OK)
                            {
                                SendHttpPost($"{Constants.Apis.ReportsApi}/reports?type={message}&token={newToken}");
                            }
                            else
                            {
                                // log failure
                            }

                            Logger.Info(GetType().Name, "Run", "Action complete");


                            break;

                        default:

                            Logger.Error(GetType().Name, "Run", "Queue Message was not an expected value: {0}: {1}", queueMsg.Id, message);

                            Queue.DeleteMessage(queueMsg);
                            queueMsg = Queue.GetMessage();

                            continue;
                    }

                    // Finally - delete the message off the queue. We don't need it any more.
                    Queue.DeleteMessage(queueMsg);

                    // get the next queueMsg
                    queueMsg = Queue.GetMessage();
                }
            }
            catch (SystemException se)
            {
                Logger.Exception(GetType().Name, "Run", se);
                throw;
            }
            catch (Exception e)
            {
                Logger.Critical(GetType().Name, "Run", "Exception Message:{0}, StackTrace: {1}", e.Message, e.StackTrace);
            }
            finally
            {
                _working = false;
            }

            // Reports only really happen once a day
            Thread.Sleep(_queueInterval);
        }

        HttpStatusCode SendHttpPost(string url, object data = null)
        {
            var tokenRequest = WebRequest.Create(url);
            tokenRequest.Method = WebRequestMethods.Http.Post;
            var postData = data?.ToString();
            var byteArray = Encoding.UTF8.GetBytes(postData ?? "");
            tokenRequest.ContentLength = byteArray.Length;
            using (var dataStream = tokenRequest.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);

                using (var response = (HttpWebResponse)tokenRequest.GetResponse())
                {
                    return response.StatusCode;
                }
            }
        }
    }
}
