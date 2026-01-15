#if NET6_0_OR_GREATER
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace CA.Blocks.SqliteDataAccess.Builder
{
    [InterpolatedStringHandler]
    public readonly struct SqlStringHandler
    {
        private readonly SqlliteSqlStringBuilder _builder;

        public SqlStringHandler(int literalLength, int formattedCount)
        {
            _builder = new SqlliteSqlStringBuilder();
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
            if (format == "``" && t != null && t is string)
            {
                var sqObjectlName = t as string;

                if (sqObjectlName.Contains('`'))
                {
                    throw new SqlBuilderException("Invalid character '`' in SQL identifier.");
                }
         
                _builder.AppendSql('`' + sqObjectlName + '`');
                return;
            }
            _builder.AppendNewSqlParameter(t, format);
        }

        internal string GetFormattedText() => _builder.ToSqlStatement();

        internal IList<SqliteParameter> GetParameters()
        {
            return _builder.GetParameters();
        }
    }
}
#endif