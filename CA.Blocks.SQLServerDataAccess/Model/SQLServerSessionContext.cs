using System;
using Microsoft.Data.SqlClient;

namespace CA.Blocks.SQLServerDataAccess.Model
{
    // https://docs.microsoft.com/en-us/sql/relational-databases/system-stored-procedures/sp-set-session-context-transact-sql

    public abstract class SqlServerSessionContext
    {
        public required string Key { get; init; }

        // The value for the specified key, of type sql_variant. Setting a value of NULL frees the memory. The maximum size is 8,000 bytes
        public bool ReadOnly { get; init; } = true;

        public abstract SqlParameter ValueAsSqlParameter(string strParameterName);
    }

    public class SqlServerIntSessionContext : SqlServerSessionContext
    {
        public required int Value { get; init; }

        public override SqlParameter ValueAsSqlParameter(string strParameterName)
        {
            return Value.ToSqlParameter(strParameterName);
        }
    }

    public class SqlServerStringSessionContext : SqlServerSessionContext
    {
        public required string Value { get; init; }

        public override SqlParameter ValueAsSqlParameter(string strParameterName)
        {
            return Value.ToSqlParameter(strParameterName);
        }
    }

    public class SqlServerGuidSessionContext : SqlServerSessionContext
    {
        public required Guid Value { get; init; }

        public override SqlParameter ValueAsSqlParameter(string strParameterName)
        {
            return Value.ToSqlParameter(strParameterName);
        }
    }
    
    
}