using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Glass.Mapper;
using Glass.Mapper.Umb;
using RM.MailshotsOnline.Data.Services;
using RM.MailshotsOnline.PCL.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Mvc;
using Umbraco.Core;
using Umbraco.Core.Services;
using Umbraco.Web;
using Umbraco.Web.Mvc;

namespace RM.MailshotsOnline.Web.Installers
{
    public class ControllersInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            // Register filtered controllers only (stops Umbraco from breaking)
            // See: http://stackoverflow.com/questions/21835555/umbraco-mvc-with-castle-windsor
            // Also register Glass Mapper items and DAL Services
            container.Register(
                Classes.FromAssemblyInDirectory(new AssemblyFilter(AssemblyDirectory)).BasedOn<IController>().LifestyleTransient(),
                Classes.FromAssemblyInDirectory(new AssemblyFilter(AssemblyDirectory)).BasedOn<IHttpController>().LifestyleTransient(),
                Component.For<IUmbracoService>().ImplementedBy<UmbracoService>().LifestyleTransient(),
                Component.For<IContentService>().ImplementedBy<ContentService>().LifestyleTransient(),
                Component.For<IMailshotsService>().ImplementedBy<MailshotsService>().LifestyleTransient(),
                Component.For<IMembershipService>().ImplementedBy<MembershipService>().LifestyleTransient(),
                Component.For<IPricingService>().ImplementedBy<PricingService>().LifestyleTransient(),
                Component.For<IEmailService>().ImplementedBy<EmailService>().LifestyleTransient(),
                Component.For<ISparqQueueService>().ImplementedBy<SparqQueueService>().LifestyleTransient(),
                Component.For<IMailshotSettingsService>().ImplementedBy<MailshotSettingsService>().LifestyleTransient(),
                Component.For<IImageLibraryService>().ImplementedBy<ImageLibraryService>().LifestyleTransient());
        }

        static public string AssemblyDirectory
        {
            //Snippet code from: https://gist.github.com/iamkoch/2344638
            get
            {
                var codeBase = Assembly.GetExecutingAssembly().CodeBase;
                var uri = new UriBuilder(codeBase);
                var path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }
    }
}