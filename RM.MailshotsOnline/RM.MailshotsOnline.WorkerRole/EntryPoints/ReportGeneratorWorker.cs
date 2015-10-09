﻿using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Xml.Linq;
using HC.RM.Common.Azure.EntryPoints;
using HC.RM.Common.Azure.Helpers;
using RM.MailshotsOnline.Data.Constants;
using RM.MailshotsOnline.PCL.Models;
using RM.MailshotsOnline.PCL.Services;

namespace RM.MailshotsOnline.WorkerRole.EntryPoints
{
    public class ReportGeneratorWorker : WorkerEntryPoint
    {
        private static readonly object SyncLock = new object();
        private static bool _working;
        private static TimeSpan _queueInterval;
        private static IAuthTokenService _authTokenService;

        public ReportGeneratorWorker(ILogger logger, IAuthTokenService authTokenService, string connectionString, string queueName,
            TimeSpan? queueInterval = null)
            : base(logger, connectionString, queueName)
        {
            _queueInterval = queueInterval ?? new TimeSpan(0, 15, 0);
            _authTokenService = authTokenService;
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
                      <?xml version="1.0" encoding="utf-16"?>
                      <StorageQueueMessage xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
                        <ExecutionTag>e927cf76d24da9625608248ce654cfa7</ExecutionTag>
                        <ClientRequestId>3c4b84e2-aee5-43ac-913e-c05c8827ac41</ClientRequestId>
                        <ExpectedExecutionTime>2015-09-10T00:00:00</ExpectedExecutionTime>
                        <SchedulerJobId>rm-photopost-creditreports</SchedulerJobId>
                        <SchedulerJobCollectionId>bdhcjobs</SchedulerJobCollectionId>
                        <Region>West Europe</Region>
                        <Message>membership</Message>
                      </StorageQueueMessage>
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


                            IAuthToken token;
                            try
                            {
                                token = _authTokenService.Create(GetType().Name);
                            }
                            catch
                            {
                                Logger.Info(GetType().Name, "Run", "Failed to set token before starting report generation.");

                                return;
                            }

                            if (token != null)
                            {
                                var response =
                                    SendHttpPost($"{Constants.Apis.ReportsApi}/generatereport?type={message}&token={token.AuthTokenId}&service={GetType().Name}");

                                Logger.Info(GetType().Name, "Run", $"Reports API responded with status {response}");
                            }
                            else
                            {
                                Logger.Info(GetType().Name, "Run", "The token received from the token auth API was null");
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

        private HttpStatusCode SendHttpPost(string url, object data = null)
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
