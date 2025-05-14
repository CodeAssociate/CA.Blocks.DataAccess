#if NET6_0_OR_GREATER
using Microsoft.Data.Sqlite;
using System;
using CA.Blocks.DataAccess.Builders;

namespace CA.Blocks.SqliteDataAccess.Builder
{
    public class SqlliteSqlStringBuilder : SqlStringBuilder<SqliteParameter>
    {
        public override SqliteParameter CreateNewParameterFor(Type t, string name, string targetDbType)
        {
            return new SqliteParameter
             { ParameterName = name, SqliteType = DefaultTypeToSqliteTypeProvider.DefaultInstance.Resolve(t, targetDbType) };
        }
    }
}
#endif