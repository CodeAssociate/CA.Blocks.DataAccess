using CA.Blocks.DataAccess.DI;
using CA.Blocks.DataAccess.Generic;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Interfaces;
using Npgsql;

namespace CA.Blocks.PostgreSQLDataAccess
{
    public class PostgresDataAccess : AbstractedDbDataAccessConnector<NpgsqlConnection, NpgsqlDataAdapter, NpgsqlCommand>
    {

        public PostgresDataAccess(IDataAccessConfig config, bool pooled = false, IDbRowTranslatorProvider? dbRowTranslatorProvider = null)
            : base(config, pooled, dbRowTranslatorProvider)
        {

        }
    }
}