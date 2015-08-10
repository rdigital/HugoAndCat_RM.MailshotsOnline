using HC.RM.Common.AzureWeb.Attributes;
using RM.MailshotsOnline.Data.Migrations;
using System.Data.Entity.Migrations;
using System.Web.Mvc;
using Umbraco.Core;

namespace RM.MailshotsOnline.Web.App_Start
{
    public class RegisterApplicationStartEvents : ApplicationEventHandler
    {
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            base.ApplicationStarted(umbracoApplication, applicationContext);

            // Entity framework DB update
            var dbMigrator = new DbMigrator(new Configuration());
            dbMigrator.Update();

            // Application Insights error handling
            GlobalFilters.Filters.Add(new AppInsightsHandleErrorAttribute());
        }
    }
}