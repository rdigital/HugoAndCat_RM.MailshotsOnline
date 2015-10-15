using System;
using System.IO;
using System.Reflection;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using HC.RM.Common.Azure;
using HC.RM.Common.Azure.EntryPoints;
using HC.RM.Common.Azure.Helpers;
using RM.MailshotsOnline.Data.DAL;
using RM.MailshotsOnline.Data.Services;
using RM.MailshotsOnline.PCL.Services;

namespace RM.MailshotsOnline.WorkerRole.Installers
{
    public class WorkerRoleInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Classes.FromThisAssembly().BasedOn<ThreadedRoleEntryPoint>().LifestyleTransient(),
                Component.For<ILogger>().ImplementedBy<Logger>().LifestyleTransient(),
                Component.For<HC.RM.Common.PCL.Helpers.ILogger>().ImplementedBy<Logger>().Named("OverridingImplementation").IsDefault().LifestyleTransient(),
                Component.For<StorageContext>().ImplementedBy<StorageContext>().LifestyleTransient(),
                Component.For<IAuthTokenService>().ImplementedBy<AuthTokenService>().LifestyleTransient());
        }
    }
}