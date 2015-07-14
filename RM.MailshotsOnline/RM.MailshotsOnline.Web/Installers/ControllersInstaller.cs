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
using Umbraco.Core.Services;
using Umbraco.Web;

namespace RM.MailshotsOnline.Web.Installers
{
    public class ControllersInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            // Register filtered controllers only (stops Umbraco from breaking)
            // See: http://stackoverflow.com/questions/21835555/umbraco-mvc-with-castle-windsor
            // Also register Glass Mapper items
            container.Register(
                Classes.FromAssemblyInDirectory(new AssemblyFilter(AssemblyDirectory)).BasedOn<IController>().LifestyleTransient(),
                Component.For<IUmbracoService>().ImplementedBy<UmbracoService>().LifestyleTransient(),
                Component.For<IContentService>().ImplementedBy<ContentService>().LifestyleTransient(),
                Component.For<IMailshotsService>().ImplementedBy<MailshotsService>().LifestyleTransient());
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