#if NET6_0_OR_GREATER
using System;
using CA.Blocks.DataAccess.Builders;
using Microsoft.Data.SqlClient;

namespace CA.Blocks.SQLServerDataAccess.Builder
{
    public class SqlServerSqlStringBuilder : SqlStringBuilder<SqlParameter>
    {
        public override SqlParameter CreateNewParameterFor(Type t, string name, string targetDbType)
        {
            return new SqlParameter
                { ParameterName = name, SqlDbType = DefaultTypeToSqlDbTypeProvider.DefaultInstance.Resolve(t, targetDbType) };
        }
    }
}
#endif