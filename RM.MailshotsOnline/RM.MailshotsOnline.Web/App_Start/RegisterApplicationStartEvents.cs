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
using Umbraco.Web.Models;

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

            // Set up telemetry
            TelemetryConfig.SetupTelemetry();

            // Application Insights error handling
            GlobalFilters.Filters.Add(new AppInsightsHandleErrorAttribute());
            if (Microsoft.WindowsAzure.ServiceRuntime.RoleEnvironment.IsAvailable)
            {
            }
        }

        private static void setupCustomRouting()
        {
            RouteTable.Routes.MapUmbracoRoute(
                                              name: "DownloadListDetails",
                                              url: "lists/{distributionListId}/Download",
                                              defaults: new { Controller = "ListDetail", Action = "ListDetailDownload", ParentId = "1407" },
                                              virtualNodeHandler:
                                                  new FilteredDocumentTypeNodeRouteHandler(typeof (ListDetail).Name),
                                              constraints: new {distributionListId = new GuidRouteConstraint()}
                );

            RouteTable.Routes.MapUmbracoRoute(
                                              name: "ListDetails",
                                              url: "lists/{distributionListId}/",
                                              defaults: new { Controller = "ListDetail", Action = "ListDetail", ParentId = "1407" },
                                              virtualNodeHandler:
                                                  new FilteredDocumentTypeNodeRouteHandler(typeof (ListDetail).Name),
                                              constraints: new {distributionListId = new GuidRouteConstraint()}
                );

            RouteTable.Routes.MapUmbracoRoute(
                                              name: "CampaignData",
                                              url: "campaigns/{campaignId}/data",
                                              defaults: new { Controller = "Lists", Action = "Lists" },
                                              virtualNodeHandler:
                                                  new DocumentTypeNodeRouteHandler(typeof(Lists).Name),
                                              constraints: new { campaignId = new GuidRouteConstraint() }
                );

            RouteTable.Routes.MapUmbracoRoute(
                                              name: "CampaignListCreate",
                                              url: "campaigns/{campaignId}/data/create",
                                              defaults: new { Controller = "ListCreate", Action = "ListCreate" },
                                              virtualNodeHandler:
                                                  new DocumentTypeNodeRouteHandler(typeof(ListCreate).Name),
                                              constraints: new { campaignId = new GuidRouteConstraint() }
                );

            RouteTable.Routes.MapUmbracoRoute(
                                              name: "CampaignListDetails",
                                              url: "campaigns/{campaignId}/data/{distributionListId}",
                                              defaults: new { Controller = "ListDetail", Action = "ListDetail" },
                                              virtualNodeHandler:
                                                  new DocumentTypeNodeRouteHandler(typeof(ListDetail).Name),
                                              constraints: new { campaignId = new GuidRouteConstraint(), distributionListId = new GuidRouteConstraint() }
                );
        }
    }
}