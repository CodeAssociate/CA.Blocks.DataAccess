using System.Data;
using System.Text;
using Npgsql;

namespace CA.Blocks.PostgresDataAccess.Builder;

public class SafeSqlBuilder
{
    private StringBuilder _sb = new StringBuilder();
    private List<NpgsqlParameter> _parameters = [];

    public SafeSqlBuilder()
    {

    }

    public SafeSqlBuilder(string sql)
    {
        AddSql(sql);
    }

    public SafeSqlBuilder(SqlStringHandler builder)
    {
        AddSql(builder);
    }


    public void AddSql(string sql)
    {
        _sb.Append(sql);
    }

    /// <summary>
    /// Will append the SQL text and parameters from the SqlStringHandler
    /// </summary>
    /// <param name="builder"></param>
    public void AddSql(SqlStringHandler builder)
    {
        _sb.Append(builder.GetFormattedText());
        var sqlParams = builder.GetParameters();
        if (sqlParams.Count > 0)
        {
            _parameters.AddRange(sqlParams);
        }
    }

    public void AddSqlLine(string sql)
    {
        AddSql(sql);
        _sb.Append(Environment.NewLine);
    }

    public void AddSqlLine(SqlStringHandler builder)
    {
        AddSql(builder);
        _sb.Append(Environment.NewLine);
    }

    public NpgsqlCommand BuildSqlCommand()
    {
        var cmd = new NpgsqlCommand { CommandText = _sb.ToString(), CommandType = CommandType.Text};
        cmd.Parameters.AddRange(_parameters.ToArray());
        return cmd;
    }

    public string GetSqlStatement()
    {
        return _sb.ToString();
    }
    public IList<NpgsqlParameter> GetParameters()
    {
        return _parameters;
    }
}