using RM.MailshotsOnline.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure;
using RM.MailshotsOnline.Entities.DataModels.MailshotSettings;

namespace RM.MailshotsOnline.Data.DAL
{
    [DbConfigurationType(typeof(AzureConfiguration))]
    public class StorageContext : DbContext
    {
        public StorageContext() : this(CloudConfigurationManager.GetSetting("StorageContextEF") ?? "StorageContextEF")
        { }

        public StorageContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
            // Set so that XML Serialisation Works - note that LazyLoading will not work any more.
            base.Configuration.ProxyCreationEnabled = false;
        }

        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<CampaignDistributionList> CampaignDistributionLists { get; set; }
        public DbSet<CmsImage> CmsImages { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<DataSearch> DataSearch { get; set; }
        public DbSet<DistributionList> DistributionLists { get; set; }
        public DbSet<Format> Formats { get; set; }
        public DbSet<MailshotContent> MailshotContents { get; set; }
        public DbSet<MailshotDefaultContent> MailshotDefaultContent { get; set; }
        public DbSet<MailshotImageUse> MailshotImageUse { get; set; }
        public DbSet<Mailshot> Mailshots { get; set; }
        public DbSet<PostalOption> PostalOptions { get; set; }
        public DbSet<Template> Templates { get; set; }
        public DbSet<Theme> Themes { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceLineItem> InvoiceLineItems { get; set; }
        public DbSet<SettingsFromCms> Settings { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Add(new DateTime2Convention());
        }
    }

    public class DateTime2Convention : Convention
    {
        public DateTime2Convention()
        {
            this.Properties<DateTime>()
                .Configure(c => c.HasColumnType("datetime2"));
        }
    }
}
