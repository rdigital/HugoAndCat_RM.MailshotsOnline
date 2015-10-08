using HC.RM.Common.Azure;
using HC.RM.Common.AzureWeb.Attributes;
using RM.MailshotsOnline.Data.Migrations;
using System.Data.Entity.Migrations;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Mvc.Routing.Constraints;
using System.Web.Routing;
using RM.MailshotsOnline.Entities.PageModels;
using RM.MailshotsOnline.Web.Handlers;
using Umbraco.Core;
using Umbraco.Web;

namespace RM.MailshotsOnline.Web.App_Start
{
    public class RegisterApplicationStartEvents : ApplicationEventHandler
    {
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            base.ApplicationStarted(umbracoApplication, applicationContext);
			
            GlobalConfiguration.Configure(WebApiConfig.Register);

            // Entity framework DB update
            var dbMigrator = new DbMigrator(new Configuration());
            dbMigrator.Update();

            setupCustomRouting();

            if (Microsoft.WindowsAzure.ServiceRuntime.RoleEnvironment.IsAvailable)
            {
                // Set up telemetry
                TelemetryConfig.SetupTelemetry();

                // Application Insights error handling
                GlobalFilters.Filters.Add(new AppInsightsHandleErrorAttribute());
            }
        }

        private static void setupCustomRouting()
        {
            RouteTable.Routes.MapUmbracoRoute(
                                              name: "ListDetails",
                                              url: "lists/{distributionListId}/",
                                              defaults: new {Controller = "ListDetail", Action = "ListDetail"},
                                              virtualNodeHandler:
                                                  new DocumentTypeNodeRouteHandler(typeof (ListDetail).Name),
                                              constraints: new {distributionListId = new GuidRouteConstraint()}
                );
        }
    }
}