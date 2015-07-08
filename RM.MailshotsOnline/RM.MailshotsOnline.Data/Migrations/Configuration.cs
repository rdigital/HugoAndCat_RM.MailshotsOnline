namespace RM.MailshotsOnline.Data.Migrations
{
    using RM.MailshotsOnline.Data.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Migrations.Model;
    using System.Data.Entity.SqlServer;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<RM.MailshotsOnline.Data.DAL.StorageContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            SetSqlGenerator("System.Data.SqlClient", new CustomSqlServerMigrationSqlGenerator());
        }

        protected override void Seed(RM.MailshotsOnline.Data.DAL.StorageContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
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
