using Npgsql;
using System.Runtime.CompilerServices;
using CA.Blocks.DataAccess.Builders;


namespace CA.Blocks.PostgreSQLDataAccess.Builder;


[InterpolatedStringHandler]
public readonly struct SqlStringHandler
{
    private readonly PostgreSqlStringBuilder _builder;

    public SqlStringHandler(int literalLength, int formattedCount)
    {
        _builder = new PostgreSqlStringBuilder();
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
            if (sqObjectlName == null)
            {
                throw new SqlBuilderException("SQL identifier cannot be null.");
            }

            if (sqObjectlName.Contains('"'))
            {
                throw new SqlBuilderException("Invalid character '\"' in SQL identifier.");
            }
            _builder.AppendSql("\"" + sqObjectlName + "\"");
            return;
        }
        _builder.AppendNewSqlParameter(t, format);
    }

    internal string GetFormattedText() => _builder.ToSqlStatement();

    internal IList<NpgsqlParameter> GetParameters()
    {
        return _builder.GetParameters();
    }
}
