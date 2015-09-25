using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Xml.Linq;
using CsvHelper;
using HC.RM.Common.Azure.EntryPoints;
using HC.RM.Common.Network;
using HC.RM.Common.PCL.Persistence;
using Microsoft.WindowsAzure.Storage.Queue;
using HC.RM.Common.Azure.Helpers;
using RM.MailshotsOnline.PCL.Models.Reporting;
using RM.MailshotsOnline.PCL.Services.Reporting;

namespace RM.MailshotsOnline.WorkerRole.EntryPoints
{
    public class ReportGeneratorWorker : WorkerEntryPoint
    {
        private static readonly object SyncLock = new object();
        private static bool _working;
        private static TimeSpan _queueInterval;
        private static IBlobService _blobService;
        private static IFtpService _ftpService;
        private static IReportingService _reportingService;

        public ReportGeneratorWorker(ILogger logger, IReportingService reportingService, string connectionString,
            string queueName, IBlobService blobService, IFtpService ftpService, TimeSpan? queueInterval = null)
            : base(logger, connectionString, queueName)
        {
            _reportingService = reportingService;
            _blobService = blobService;
            _ftpService = ftpService;
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
                     *  <StorageQueueMessage xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
                     *    <ExecutionTag>e927cf76d24da9625608248ce654cfa7</ExecutionTag>
                     *    <ClientRequestId>3c4b84e2-aee5-43ac-913e-c05c8827ac41</ClientRequestId>
                     *    <ExpectedExecutionTime>2015-09-10T00:00:00</ExpectedExecutionTime>
                     *    <SchedulerJobId>rm-photopost-creditreports</SchedulerJobId>
                     *    <SchedulerJobCollectionId>bdhcjobs</SchedulerJobCollectionId>
                     *    <Region>West Europe</Region>
                     *    <Message>the message</Message>
                     *  </StorageQueueMessage>
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
                    var message = messageElement.Value;

                    if (string.IsNullOrEmpty(message))
                    {
                        Logger.Error(GetType().Name, "Run", "Queue Message was parsed successfully but didn't contain a message: {0}: {1}", queueMsg.Id, queueMsg.AsString);

                        Queue.DeleteMessage(queueMsg);
                        queueMsg = Queue.GetMessage();

                        continue;
                    }

                    IReport report;

                    switch (message.ToLower())
                    {
                        case "membership":

                            report = _reportingService.MembershipReportGenerator.Generate();
                            ProcessReport(report, ((IMembershipReport)report).Members);

                            break;

                        case "transactions":

                            report = _reportingService.TransactionsReportGenerator.Generate();
                            ProcessReport(report, ((ITransactionsReport)report).Transactions);

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

        private void ProcessReport(IReport report, IEnumerable data)
        {
            var reportBytes = SerializeReport(data);
            var fileName = $"{report.Name} - {report.CreatedDate}.csv";

            StoreBlob(report, reportBytes, fileName);

            TransferFile(reportBytes, fileName);
        }

        private bool TransferFile(byte[] data, string fileName)
        {
            var stream = new MemoryStream(data);
            _ftpService.Put(stream, fileName);

            return true;
        }

        private string StoreBlob(IReport report, IEnumerable data, string fileName)
        {
            var bytes = SerializeReport(data);

            Logger.Info(GetType().Name, "Run", $"Storing report in blob storage: {fileName}");

            return _blobService.Store(bytes, fileName, "text/csv");

        }

        private byte[] SerializeReport(IEnumerable data)
        {
            byte[] csvBytes;
            using (var memoryStream = new MemoryStream())
            {
                using (var streamWriter = new StreamWriter(memoryStream))
                using (var csvWriter = new CsvWriter(streamWriter))
                {
                    csvWriter.WriteRecords(data);
                }

                csvBytes = memoryStream.ToArray();
            }

            return csvBytes;
        }
    }
}
