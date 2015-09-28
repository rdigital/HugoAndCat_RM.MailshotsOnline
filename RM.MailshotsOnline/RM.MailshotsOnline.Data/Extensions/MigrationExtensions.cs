using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations.Infrastructure;
using System.Data.Entity.Migrations.Model;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.MailshotsOnline.Data.Extensions
{
    internal static class MigrationExtensions
    {
        /// <summary>
        /// Removes default contstraints on a given table and column
        /// </summary>
        /// <param name="migration">The migration being run</param>
        /// <param name="tableName">The table name</param>
        /// <param name="columnName">The column name</param>
        /// <see cref="http://stackoverflow.com/a/30132507/1399087"/>
        public static void DeleteDefaultContstraint(this IDbMigration migration, string tableName, string columnName, bool suppressTransaction = false)
        {
            var sql = new SqlOperation(String.Format(@"DECLARE @SQL varchar(1000)
            SET @SQL='ALTER TABLE {0} DROP CONSTRAINT ['+(SELECT name
            FROM sys.default_constraints
            WHERE parent_object_id = object_id('{0}')
            AND col_name(parent_object_id, parent_column_id) = '{1}')+']';
            PRINT @SQL;
            EXEC(@SQL);", tableName, columnName)) { SuppressTransaction = suppressTransaction };
            migration.AddOperation(sql);
        }
    }
}
