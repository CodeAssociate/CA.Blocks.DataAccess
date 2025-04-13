#if NET6_0_OR_GREATER

using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Runtime.CompilerServices;


namespace CA.Blocks.SQLServerDataAccess.Builder
{
    [InterpolatedStringHandler]
    public readonly struct SqlStringHandler
    {
        private readonly SqlServerSqlStringBuilder _builder;

        public SqlStringHandler(int literalLength, int formattedCount)
        {
            _builder = new SqlServerSqlStringBuilder();
        }

        public void AppendLiteral(string s)
        {
            _builder.AppendSql(s);
        }

        public void AppendFormatted<T>(T t)
        {
            _builder.AppendSqlParameter(t);
        }

        public void AppendFormatted<T>(T t, string format)
        {
            _builder.AppendNewSqlParameter(t, format);
        }

        internal string GetFormattedText() => _builder.ToSqlStatement();

        internal IList<SqlParameter> GetParameters()
        {
            return _builder.GetParameters();
        }
    }
}

#endif