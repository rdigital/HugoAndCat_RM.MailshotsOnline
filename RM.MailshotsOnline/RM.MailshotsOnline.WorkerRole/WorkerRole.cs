using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using Castle.Windsor;
using Castle.Windsor.Installer;
using HC.RM.Common.Azure;
using HC.RM.Common.Azure.EntryPoints;
using Microsoft.ApplicationInsights;
using Microsoft.Azure;
using RM.MailshotsOnline.PCL.Services;
using RM.MailshotsOnline.Web.Plumbing;
using RM.MailshotsOnline.WorkerRole.EntryPoints;
using RM.MailshotsOnline.WorkerRole.Installers;

namespace RM.MailshotsOnline.WorkerRole
{
    public class WorkerRole : ThreadedRoleEntryPoint
    {
        private static readonly string ReportQueueName = CloudConfigurationManager.GetSetting("ReportsServiceQueueName");
        private static readonly string ServiceBusConnectionString = CloudConfigurationManager.GetSetting("ReportsServiceBusConnectionString");
        private static IAuthTokenService _authTokenService;

        private const string WorkerRoleName = "MailshotsOnline.WorkerRole";

        public WorkerRole()
        {
            var container = new WindsorContainer();
            container.Install(FromAssembly.This());
            _authTokenService = container.Resolve<IAuthTokenService>();
        }

        public WorkerRole(IAuthTokenService authTokenService)
        {
            _authTokenService = authTokenService;
        }

        public override void Run()
        {
            Logger.Info(GetType().Name, "Run", $"{WorkerRoleName} is running");

            var workers = new List<WorkerEntryPoint>
            {
                new ReportGeneratorWorker(Logger, _authTokenService, ServiceBusConnectionString, ReportQueueName)
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
            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 12;

            try
            {
                TelemetryConfig.SetupTelemetry();
                var telemetry = new TelemetryClient();
                Logger = new Logger(telemetry);
            }
            catch (Exception e)
            {
                
            }

            Logger.Info(GetType().Name, "OnStart", $"{WorkerRoleName} is starting");

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.
            var result = base.OnStart();

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
