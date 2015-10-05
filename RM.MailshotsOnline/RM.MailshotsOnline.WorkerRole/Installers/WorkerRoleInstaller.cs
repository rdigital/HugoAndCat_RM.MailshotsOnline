using System;
using System.IO;
using System.Reflection;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using HC.RM.Common.Azure;
using HC.RM.Common.Azure.EntryPoints;
using HC.RM.Common.Azure.Helpers;
using RM.MailshotsOnline.Data.Services;
using RM.MailshotsOnline.PCL.Services;

namespace RM.MailshotsOnline.WorkerRole.Installers
{
    public class WorkerRoleInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            // Register filtered controllers only (stops Umbraco from breaking)
            // See: http://stackoverflow.com/questions/21835555/umbraco-mvc-with-castle-windsor
            // Also register Glass Mapper items and DAL Services
            container.Register(Classes.FromThisAssembly().BasedOn<ThreadedRoleEntryPoint>().LifestyleTransient(),
                Component.For<ILogger>().ImplementedBy<Logger>().LifestyleTransient(),
                Component.For<IAuthTokenService>().ImplementedBy<AuthTokenService>().LifestyleTransient());
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