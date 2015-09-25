using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using HC.RM.Common.Azure;
using HC.RM.Common.Azure.EntryPoints;
using HC.RM.Common.Azure.Persistence;
using HC.RM.Common.Network;
using HC.RM.Common.PCL.Persistence;
using Microsoft.ApplicationInsights;
using Microsoft.Azure;
using Microsoft.WindowsAzure;
using RM.MailshotsOnline.Data.Services;
using RM.MailshotsOnline.Data.Services.Reporting;
using RM.MailshotsOnline.PCL.Services;
using RM.MailshotsOnline.PCL.Services.Reporting;
using RM.MailshotsOnline.WorkerRole.EntryPoints;

namespace RM.MailshotsOnline.WorkerRole
{
    public class WorkerRole : ThreadedRoleEntryPoint
    {
        private static readonly string ReportQueueName = CloudConfigurationManager.GetSetting("ReportsQueue");
        private static readonly string StorageConnectionString =
            CloudConfigurationManager.GetSetting("StorageConnectionString");

        private const string WorkerRoleName = "MailshotsOnline.WorkerRole";

        private static IBlobService _reportStorageService;
        private static IFtpService _ftpService;
        private static IReportingService _reportingService;

        public WorkerRole(IReportingService reportingService)
        {
            _reportingService = reportingService;
        }

        public override void Run()
        {
            Logger.Info(GetType().Name, "Run", $"{WorkerRoleName} is running");

            var workers = new List<WorkerEntryPoint>
            {
                new ReportGeneratorWorker(Logger, _reportingService, StorageConnectionString, ReportQueueName,
                    _reportStorageService, _ftpService)
            };

            try
            {
                RunAsync(workers.ToArray(), TokenSource.Token).Wait();
            }
            finally
            {
                RunCompleteEvent.Set();
            }
        }

        public override bool OnStart()
        {
            Logger.Info(GetType().Name, "OnStart", $"{WorkerRoleName} is starting");

            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 12;

            TelemetryConfig.SetupTelemetry();
            var telemetry = new TelemetryClient();
            Logger = new Logger(telemetry);

            var blobStorage = new BlobStorage(StorageConnectionString);
            _reportStorageService = new BlobService(blobStorage, CloudConfigurationManager.GetSetting("ReportStorageContainer"));

            var assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty;

            _ftpService = new FtpService(Path.Combine(assemblyPath, CloudConfigurationManager.GetSetting("ReportSFTPPrivateKeyLocation")),
                    CloudConfigurationManager.GetSetting("ReportSFTPUsername"),
                    CloudConfigurationManager.GetSetting("ReportSFTPHost"), Logger);

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.
            bool result = base.OnStart();

            Logger.Info(GetType().Name, "OnStart", $"{WorkerRoleName} has been started");

            return result;
        }

        public override void OnStop()
        {
            Logger.Info(GetType().Name, "OnStop", $"{WorkerRoleName} is stopping");

            TokenSource.Cancel();
            RunCompleteEvent.WaitOne();

            base.OnStop();

            Logger.Info(GetType().Name, "OnStop", $"{WorkerRoleName} has stopped");
        }
    }
}
