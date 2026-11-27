using Microsoft.Data.SqlClient;
using CA.Blocks.DataAccess.DI;
using CA.Blocks.SQLServerDataAccess;
using CA.Blocks.SQLServerDataAccess.Builder;

namespace CA.Blocks.SQLServerDataAccessUnitTests.Base
{
    
    public static class TestConnectionStrings
    {
        public  static string? TestDataBaseConnectionString { get; set; }
    }

    public class LocalUnitTestStringsResolver : IDataAccessKeyToConnectionStringResolver
    {
        /// <summary>
        /// Provides the mapping from the name in code to the name in the Connection string Stored in the app.config or web.config file
        /// </summary>
        /// <param name="connectionStringKey">The Connection string known to the code</param>
        /// <returns> The Connection string to be used by the ADO.NET provider.</returns>
        public string GetConnectionString(string connectionStringKey)
        {
            return TestConnectionStrings.TestDataBaseConnectionString!;
        }
    }
    
    public class UnitTestDataAccess : SqlServerDataAccess
    {
        protected UnitTestDataAccess() : this (new DataAccessConfigOptions
            {ConnectionStringKey = "localsqlserverhost"})
        {
        }

        protected UnitTestDataAccess(DataAccessConfigOptions options) : base (
            new DataAccessConfig(options, new LocalUnitTestStringsResolver())
        )
        {
        }
        
        private const string unitTestTableName = "CA_BLOCKS_UNITTEST_TEMP_TESTTABLE";
        protected const string UNIT_TEST_COL_NAME = "Col";
        
        protected SqlCommand DropTestTableCommand()
        {
            var sqlBuilder = new SafeSqlBuilder();
            sqlBuilder.AddSqlLine($"IF EXISTS (SELECT 1 from sysobjects where xtype = 'U' and id = object_id({unitTestTableName:@unitTestTableName}))");
            sqlBuilder.AddSqlLine("BEGIN");
            sqlBuilder.AddSqlLine($"DROP TABLE {unitTestTableName:[]}");
            sqlBuilder.AddSqlLine("END");
            return sqlBuilder.BuildSqlCommand();
        }
        
        protected SqlCommand CreateTestTableCommand(string coltype, bool allowNull = false )
        {
            var sqlBuilder = new SafeSqlBuilder($"CREATE TABLE {unitTestTableName:[]}");
            sqlBuilder.AddSqlLine("(");
            sqlBuilder.AddSqlLine("id int identity(1,1),");
            sqlBuilder.AddSqlLine($"{UNIT_TEST_COL_NAME:[]} {coltype:[]} ");
            sqlBuilder.AddSql(allowNull ? "NULL" : "NOT NULL");
            sqlBuilder.AddSqlLine(");");
            return sqlBuilder.BuildSqlCommand();
        }
        
        protected SqlCommand InsertTestDataCommand<T>(T value)
        {
            var sqlBuilder = new SafeSqlBuilder($"INSERT INTO {unitTestTableName:[]} ({UNIT_TEST_COL_NAME:[]})  values ({value:@value})");
            return sqlBuilder.BuildSqlCommand();
        }
        
        protected SqlCommand BasicSelectCommand(SqlStringHandler filter)
        {
            var sqlBuilder = new SafeSqlBuilder($"SELECT {UNIT_TEST_COL_NAME:[]} FROM {unitTestTableName:[]}");
            sqlBuilder.AddSql(filter);
            return sqlBuilder.BuildSqlCommand();
        }
        protected SqlCommand BasicSelectCommand()
        {
            var sqlBuilder = new SafeSqlBuilder($"SELECT {UNIT_TEST_COL_NAME:[]} FROM {unitTestTableName:[]}");
            return sqlBuilder.BuildSqlCommand();
        }
    }
}




