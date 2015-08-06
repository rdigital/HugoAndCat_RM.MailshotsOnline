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
            context.PostalOptions.AddOrUpdate(
                o => new { o.Name, o.FormatId },
                new PostalOption { Name = "First class", FormatId = 1, Currency = "GBP", PricePerUnit = 0.63m, Tax = 0.12m, TaxCode = "V" },
                new PostalOption { Name = "Second class", FormatId = 1, Currency = "GBP", PricePerUnit = 0.54m, Tax = 0.10m, TaxCode = "V" },
                new PostalOption { Name = "First class", FormatId = 2, Currency = "GBP", PricePerUnit = 0.70m, Tax = 0.14m, TaxCode = "V" },
                new PostalOption { Name = "Second class", FormatId = 2, Currency = "GBP", PricePerUnit = 0.60m, Tax = 0.12m, TaxCode = "V" }
                );
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
