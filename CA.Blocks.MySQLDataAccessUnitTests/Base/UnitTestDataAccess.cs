using CA.Blocks.DataAccess.DI;
using CA.Blocks.DataAccess.Model.Paging;
using CA.Blocks.MySQLDataAccess;
using MySqlConnector;
using System.Data;
using System.Text;

namespace CA.Blocks.MySQLDataAccessUnitTests.Base
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
    

    public class UnitTestDataAccess : MySqlDataAccess
    {
        protected UnitTestDataAccess() : this(new DataAccessConfigOptions
        { 
            ConnectionStringKey = "localsqlserverhost" })
        {
        }

        private UnitTestDataAccess(DataAccessConfigOptions options) : base(
            new DataAccessConfig(options, new LocalUnitTestStringsResolver())
        )
        {
        }


        private const string unitTestTableName = "CA_BLOCKS_UNITTEST_TEMP_TESTTABLE";
        public const string UNIT_TEST_COL_NAME = "Col";

        protected string DropTestTableSQL()
        {
            return $"DROP TABLE IF EXISTS {unitTestTableName};";
        }

        protected string CreateTestTable(string coltype)
        {
            return $"CREATE TABLE {unitTestTableName} (id int NOT NULL AUTO_INCREMENT, col {coltype},  PRIMARY KEY (id) )";

        }

        protected string InsertTestDataSQL(string data)
        {
            return  $"Insert into {unitTestTableName} (col) values ({data})";
        }

        protected string SelectTestDataSQL()
        {
            return $"Select col from {unitTestTableName} /*##FILTER##*/";
        }


        // This is a backdoor used for unit testing to setup and teardown test data in the local sql server
        //  this is a helper function and bypasses all the security features around the block.
        public void ExecuteNonQuery(string query)
        {
            var cmd = CreateTextCommand(query);
            ExecuteNonQuery(cmd); 
        }


        public new DataTable ExecuteDataTable(MySqlCommand cmd, PagingRequest page)
        {
           return base.ExecuteDataTable(cmd, page);
        }


        //public new IList<T> ExecuteToListOf<T>(SqlCommand cmd) where T : new()
        //{
        //    return base.ExecuteToListOf<T>(cmd);
        //}

        protected string DataTableToText(DataTable dt)
        {
            var maxLengths = new int[dt.Columns.Count];

            for (int i = 0; i < dt.Columns.Count; i++)
            {
                maxLengths[i] = dt.Columns[i].ColumnName.Length;

                foreach (DataRow row in dt.Rows)
                {
                    if (!row.IsNull(i))
                    {
                        int length = row[i].ToString()!.Length;

                        if (length > maxLengths[i])
                        {
                            maxLengths[i] = length;
                        }
                    }
                }
            }

            var sb = new StringBuilder();
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    sb.Append(dt.Columns[i].ColumnName.PadRight(maxLengths[i] + 2));
                }

                sb.AppendLine();

                foreach (DataRow row in dt.Rows)
                {
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        sb.Append(!row.IsNull(i)
                            ? row[i].ToString()!.PadRight(maxLengths[i] + 2)
                            : new string(' ', maxLengths[i] + 2));
                    }

                    sb.AppendLine();
                }
            }
            return sb.ToString();
        }

    }
}
