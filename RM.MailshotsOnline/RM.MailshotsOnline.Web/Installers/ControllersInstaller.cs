using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Glass.Mapper;
using Glass.Mapper.Umb;
using HC.RM.Common.Azure;
using HC.RM.Common.PCL.Helpers;
using RM.MailshotsOnline.Data.Services;
using RM.MailshotsOnline.PCL.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Mvc;
using HC.RM.Common.Azure.Persistence;
using HC.RM.Common.Network;
using HC.RM.Common.PCL.Persistence;
using Microsoft.Azure;
using RM.MailshotsOnline.Data.DAL;
using RM.MailshotsOnline.Data.Services.Reporting;
using RM.MailshotsOnline.PCL.Services.Reporting;
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
                Classes.FromAssemblyInDirectory(new AssemblyFilter(AssemblyDirectory))
                    .BasedOn<IController>()
                    .LifestyleTransient(),
                Classes.FromAssemblyInDirectory(new AssemblyFilter(AssemblyDirectory))
                    .BasedOn<IHttpController>()
                    .LifestyleTransient(),
                Component.For<IUmbracoService>().ImplementedBy<UmbracoService>().LifestyleTransient(),
                Component.For<IContentService>().ImplementedBy<ContentService>().LifestyleTransient(),
                Component.For<IMailshotsService>().ImplementedBy<MailshotsService>().LifestyleTransient(),
                Component.For<IMembershipService>().ImplementedBy<MembershipService>().LifestyleTransient(),
                Component.For<IPricingService>().ImplementedBy<PricingService>().LifestyleTransient(),
                //Component.For<IEmailService>().ImplementedBy<EmailService>().LifestyleTransient(),
                Component.For<ISparqQueueService>().ImplementedBy<SparqQueueService>().LifestyleTransient(),
                Component.For<IMailshotSettingsService>().ImplementedBy<MailshotSettingsService>().LifestyleTransient(),
                Component.For<IImageLibraryService>().ImplementedBy<ImageLibraryService>().LifestyleSingleton(),
                Component.For<ICmsImageService>().ImplementedBy<CmsImageService>().LifestyleTransient(),
                Component.For<ICampaignService>().ImplementedBy<CampaignService>().LifestyleTransient(),
                Component.For<IInvoiceService>().ImplementedBy<InvoiceService>().LifestyleTransient(),
                Component.For<IDataService>().ImplementedBy<DataService>().LifestyleTransient(),
                Component.For<IEmailService>().ImplementedBy<SmtpService>().LifestyleTransient(),
                Component.For<ICryptographicService>().ImplementedBy<CryptographicService>().LifestyleTransient(),
                Component.For<ILogger>().ImplementedBy<Logger>().LifestyleTransient(),
                Component.For<ISettingsService>().ImplementedBy<SettingsService>().LifestyleTransient(),
                Component.For<IReportingService>().ImplementedBy<ReportingService>().LifestyleTransient(),
                Component.For<IMembershipReportGenerator>().ImplementedBy<MembershipReportGenerator>().LifestyleTransient(),
                Component.For<ITransactionsReportGenerator>().ImplementedBy<TransactionsReportGenerator>().LifestyleTransient(),
                Component.For<IFtpService>().ImplementedBy<FtpService>().LifestyleTransient(),
                Component.For<IReportingSftpService>().ImplementedBy<ReportingSftpService>().LifestyleTransient(),
                Component.For<IReportingBlobService>().ImplementedBy<ReportingBlobService>().LifestyleTransient(),
                Component.For<IReportingBlobStorage>().ImplementedBy<ReportingBlobStorage>().LifestyleTransient(),
                Component.For<StorageContext>().ImplementedBy<StorageContext>().LifestyleTransient(),
                Component.For<IBlobStorage>()
                    .ImplementedBy<BlobStorage>()
                    .DependsOn(Dependency.OnValue("connectionString", CloudConfigurationManager.GetSetting("StorageConnectionString")))
                    .LifestyleTransient(),
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