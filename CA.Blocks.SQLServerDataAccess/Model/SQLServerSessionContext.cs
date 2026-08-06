using System;
using Microsoft.Data.SqlClient;

namespace CA.Blocks.SQLServerDataAccess.Model
{
    // https://docs.microsoft.com/en-us/sql/relational-databases/system-stored-procedures/sp-set-session-context-transact-sql

    public abstract class SqlServerSessionContext
    {
        public string? Key { get; set; }

        // The value for the specified key, of type sql_variant. Setting a value of NULL frees the memory. The maximum size is 8,000 bytes
        public bool ReadOnly { get; set; }

        public abstract SqlParameter ValueAsSqlParameter(string strParameterName);
    }

    public class SqlServerIntSessionContext : SqlServerSessionContext
    {
        public int Value { get; set; }

        public override SqlParameter ValueAsSqlParameter(string strParameterName)
        {
            return Value.ToSqlParameter(strParameterName);
        }
    }

    public class SqlServerStringSessionContext : SqlServerSessionContext
    {
        public string? Value { get; set; }

        public override SqlParameter ValueAsSqlParameter(string strParameterName)
        {
            return Value.ToSqlParameter(strParameterName);
        }
    }

    public class SqlServerGuidSessionContext : SqlServerSessionContext
    {
        public Guid Value { get; set; }

        public override SqlParameter ValueAsSqlParameter(string strParameterName)
        {
            return Value.ToSqlParameter(strParameterName);
        }
    }
}