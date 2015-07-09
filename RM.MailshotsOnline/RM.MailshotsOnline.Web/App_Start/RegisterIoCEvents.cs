using Castle.Windsor;
using Castle.Windsor.Installer;
using RM.MailshotsOnline.Web.Plumbing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Mvc;
using umbraco.cms.businesslogic.web;
using Umbraco.Core;
using Umbraco.Web.Mvc;

namespace RM.MailshotsOnline.Web.App_Start
{
    public class RegisterIoCEvents : ApplicationEventHandler
    {
        /// <summary>
        /// The IoC container
        /// </summary>
        private static IWindsorContainer container;

        /// <summary>
        /// Overrides the Application Start for Umbraco
        /// </summary>
        protected override void ApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            base.ApplicationStarting(umbracoApplication, applicationContext);
            BootstrapContainer();
        }

        /// <summary>
        /// Runs immediately after the Application Start for Umbraco.  Register new events here
        /// </summary>
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            base.ApplicationStarted(umbracoApplication, applicationContext);

            // Dispose of the IoC container
            umbracoApplication.Disposed += new EventHandler((object sender, EventArgs e) => { container.Dispose(); });
        }

        /// <summary>
        /// Creates the IoC container and its properties
        /// </summary>
        private static void BootstrapContainer()
        {
            container = new WindsorContainer().Install(FromAssembly.This());

            var controllerFactory = new WindsorControllerFactory(container.Kernel);
            ControllerBuilder.Current.SetControllerFactory(controllerFactory);

            // See: http://stackoverflow.com/questions/21835555/umbraco-mvc-with-castle-windsor
            // See also: https://gist.github.com/florisrobbemont/5821863
            // Needed to allow injectino for API controllers
            GlobalConfiguration.Configuration.Services.Replace(typeof(IHttpControllerActivator), new WindsorCompositionRoot(container.Kernel));

            GlassMapperUmbCustom.CastleConfig(container);
            GlassMapperUmb.Start();
        }
    }
}