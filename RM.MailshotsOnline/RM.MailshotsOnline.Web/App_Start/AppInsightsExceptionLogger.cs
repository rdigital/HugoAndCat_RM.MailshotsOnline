using Microsoft.ApplicationInsights;
using Microsoft.WindowsAzure.ServiceRuntime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.ExceptionHandling;

namespace RM.MailshotsOnline.Web.App_Start
{
    public class AppInsightsExceptionLogger : ExceptionLogger
    {
        public override void Log(ExceptionLoggerContext context)
        {
            if (!RoleEnvironment.IsEmulated)
            {
                if (context != null && context.Exception != null)
                {
                    // General exception logger, no way to choose "shared" TelemetryClient. 
                    var telemetry = new TelemetryClient();
                    telemetry.TrackException(context.Exception);
                }
            }
            base.Log(context);
        }
    }
}