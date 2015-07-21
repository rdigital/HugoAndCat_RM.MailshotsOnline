using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Azure;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Umbraco.Core;

namespace RM.MailshotsOnline.Web.App_Start
{
    public class RegisterApplicationStartEvents : ApplicationEventHandler
    {
        protected override void ApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            base.ApplicationStarting(umbracoApplication, applicationContext);
            TelemetryConfiguration.Active.InstrumentationKey = CloudConfigurationManager.GetSetting("AppInsightsKey") ?? ConfigurationManager.AppSettings["AppInsightsKey"];
        }
    }
}