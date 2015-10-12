using RM.MailshotsOnline.Data.DAL;

namespace RM.MailshotsOnline.Data.Migrations
{
    using RM.MailshotsOnline.Entities.DataModels;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Migrations.Model;
    using System.Data.Entity.SqlServer;
    using System.Linq;

    public sealed class Configuration : DbMigrationsConfiguration<RM.MailshotsOnline.Data.DAL.StorageContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            SetSqlGenerator("System.Data.SqlClient", new CustomSqlServerMigrationSqlGenerator());
        }

        protected override void Seed(RM.MailshotsOnline.Data.DAL.StorageContext context)
        {
            //context.PostalOptions.AddOrUpdate(
            //    o => new { o.Name },
            //    new PostalOption { Name = "First class", UmbracoId = 0, Currency = "GBP", PricePerUnit = 0.63m, Tax = 0.12m, TaxCode = "V" },
            //    new PostalOption { Name = "Second class", UmbracoId = 1, Currency = "GBP", PricePerUnit = 0.54m, Tax = 0.10m, TaxCode = "V" }
            //    );

            context.Products.AddOrUpdate(
                p => new { p.ProductSku },
                new Product { ProductSku = Constants.Constants.Products.PrintSku, Category = "Printing and delivery", Name = "Printing", UpdatedDate = DateTime.UtcNow },
                new Product { ProductSku = Constants.Constants.Products.PostSku, Category = "Printing and delivery", Name = "Postage", UpdatedDate = DateTime.UtcNow },
                new Product { ProductSku = Constants.Constants.Products.YourDataSku, Category = "Recipients", Name = "Your data", UpdatedDate = DateTime.UtcNow },
                new Product { ProductSku = Constants.Constants.Products.OurDataSku, Category = "Recipients", Name = "Our data", UpdatedDate = DateTime.UtcNow },
                new Product { ProductSku = Constants.Constants.Products.MsolServiceFeeSku, Category = "Service Fees", Name = "Service fee", UpdatedDate = DateTime.UtcNow },
                new Product { ProductSku = Constants.Constants.Products.DataSearchFeeSku, Category = "Service Fees", Name = "Data search fee", UpdatedDate = DateTime.UtcNow }
                );
            // Populate dummy rows for Testing
            //fillContextWithListData(context);
        }

        private void fillContextWithListData(StorageContext context)
        {
            int userId = 1504;
            for (int listId = 0; listId < 50; listId++)
            {
                context.DistributionLists.AddOrUpdate(dl => new {dl.Name, dl.UserId},
                                                      new DistributionList
                                                      {
                                                          Name = $"List Id: {listId:00}",
                                                          RecordCount = ((listId + 1)*13) - (listId*7),
                                                          UserId = userId,
                                                          UpdatedDate = DateTime.UtcNow,
                                                      });
            }
        }

        /// <summary>
        /// Provides custom additions to the SQL Migration generator.
        /// </summary>
        /// <remarks>
        /// Currently includes:
        /// <list type="bullet">
        /// <item>If columnName is "CreatedUtc" will add a default value of "GetUtcDate()" to the column definition.</item>
        /// </list>
        /// </remarks>
        public class CustomSqlServerMigrationSqlGenerator : SqlServerMigrationSqlGenerator
        {
            protected override void Generate(AddColumnOperation addColumnOperation)
            {
                setCreatedUtcColumn(addColumnOperation.Column);

                base.Generate(addColumnOperation);
            }

            protected override void Generate(CreateTableOperation createTableOperation)
            {
                setCreatedUtcColumn(createTableOperation.Columns);

                base.Generate(createTableOperation);
            }

            private static void setCreatedUtcColumn(IEnumerable<ColumnModel> columns)
            {
                foreach (var columnModel in columns)
                {
                    setCreatedUtcColumn(columnModel);
                }
            }

            private static void setCreatedUtcColumn(PropertyModel column)
            {
                if (column.Name == "CreatedUtc")
                {
                    column.DefaultValueSql = "GETUTCDATE()";
                }
            }
        }
    }
}
