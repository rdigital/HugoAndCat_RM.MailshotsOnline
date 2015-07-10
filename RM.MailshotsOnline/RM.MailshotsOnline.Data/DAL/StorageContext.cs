using RM.MailshotsOnline.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.MailshotsOnline.Data.DAL
{
    [DbConfigurationType(typeof(AzureConfiguration))]
    public class StorageContext : DbContext
    {
        public StorageContext() : this("StorageContext")
        { }

        public StorageContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
            // Set so that XML Serialisation Works - note that LazyLoading will not work any more.
            base.Configuration.ProxyCreationEnabled = false;
        }

        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<DistributionList> DistributionLists { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<PostalOption> PostalOptions { get; set; }
        public DbSet<Mailshot> Mailshots { get; set; }
        public DbSet<MailshotContent> MailshotContents { get; set; }

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
