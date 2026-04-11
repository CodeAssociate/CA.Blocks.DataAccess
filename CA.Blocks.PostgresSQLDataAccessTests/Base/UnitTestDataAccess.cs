using CA.Blocks.DataAccess.DI;
using CA.Blocks.PostgreSQLDataAccess;
using Npgsql;

namespace CA.Blocks.PostgreSQLDataAccessUnitTests.Base
{


    public static class TestConnectionStrings
    {
        public static string TestDataBaseConnectionString { get; set; }
    }

    public class LocalSqlServerUnitTestStringsResolver : IDataAccessKeyToConnectionStringResolver
    {
        /// <summary>
        /// Provides the mapping from the name in code to the name in the Connection string Stored in the app.config or web.config file
        /// </summary>
        /// <param name="connectionStringKey">The Connection string known to the code</param>
        /// <returns> The Connection string to be used by the ADO.NET provider.</returns>
        public string GetConnectionString(string connectionStringKey)
        {
            return TestConnectionStrings.TestDataBaseConnectionString;
        }
    }

    // this class exposes the internal workings so we can test
    public class UnitTestDataAccess : PostgresDataAccess
    {

        public UnitTestDataAccess() : this(new DataAccessConfigOptions
        { 
                ConnectionStringKey = "localsqlserverhost" })
        {
        }

        public UnitTestDataAccess(DataAccessConfigOptions options) : base(
            new DataAccessConfig(options, new LocalSqlServerUnitTestStringsResolver())
        )
        {
        }


        protected string unitTestTableName = "ca_blocks_unittest_temp_testtable";


        public const string UNIT_TEST_COL_NAME = "Col";

        protected string DropTestTableSQL()
        {
            return @$"
                drop table if exists {unitTestTableName}";
        }

        protected string CreateTestTable(string coltype)
        {
            return $"Create table if not exists {unitTestTableName} (id int GENERATED ALWAYS AS IDENTITY, col {coltype} )";

        }

        protected string InsertTestDataSQL(string data)
        {
            return $"Insert into {unitTestTableName} (col) values ({data})";
        }

        protected string InsertTestDataSQLWithDataParam()
        {
            return $"Insert into {unitTestTableName} (col) values (@data)";
        }

        protected string SelectTestDataSQL(string filter = "")
        {
            return $"Select col from {unitTestTableName} {filter}";
        }

        
        // This is a backdoor used for unit testing to setup and teardown test data in the local sql server
        //  this is a helper function and bypasses all the security features around the block.
        public void ExecuteNonQuery(string query)
        {
            NpgsqlCommand cmd = CreateTextCommand(query);
            ExecuteNonQuery(cmd);
        }

        public void ExecuteNonQueryCmd(string query)
        {
            NpgsqlCommand cmd = CreateTextCommand(query);
            ExecuteNonQuery(cmd);
        }
    }
}

