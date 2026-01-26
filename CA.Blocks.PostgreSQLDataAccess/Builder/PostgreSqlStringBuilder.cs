
using CA.Blocks.DataAccess.Builders;
using Npgsql;


namespace CA.Blocks.PostgreSQLDataAccess.Builder;

public class PostgreSqlStringBuilder : SqlStringBuilder<NpgsqlParameter>
{
    public override NpgsqlParameter CreateNewParameterFor(Type t, string name, string targetDbType)
    {
        return new NpgsqlParameter { ParameterName = name, NpgsqlDbType = DefaultTypeToSqlDbTypeProvider.DefaultInstance.Resolve(t, targetDbType) };
    }
}
