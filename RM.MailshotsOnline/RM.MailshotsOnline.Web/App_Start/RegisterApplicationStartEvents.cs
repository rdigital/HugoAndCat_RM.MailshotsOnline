﻿using RM.MailshotsOnline.Data.Migrations;
using System.Data.Entity.Migrations;
using Umbraco.Core;

namespace RM.MailshotsOnline.Web.App_Start
{
    public class RegisterApplicationStartEvents : ApplicationEventHandler
    {
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            base.ApplicationStarted(umbracoApplication, applicationContext);
            var dbMigrator = new DbMigrator(new Configuration());
            dbMigrator.Update();
        }
    }
}