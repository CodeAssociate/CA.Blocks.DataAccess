using System.Data;
using System.Text;
using CA.Blocks.DataAccess.DI;
using CA.Blocks.SQLLiteDataAccess;
using Microsoft.Data.Sqlite;

namespace CA.Blocks.SQLLiteDataAccessUnitTests.Base
{

    public class UnitTRestInMemDB : IDataAccessKeyToConnectionStringResolver
    {
        public string GetConnectionString(string connectionStringKey)
        {
            return "Data Source=ca_blocks_unittest;Mode=Memory;Cache=Shared";
        }
    }

    // this class exposes the internal workings so we can test
    public class UnitTestDataAccess : SqlLiteDataAccess
    {
        private SqliteConnection _dbcontext; 

        public UnitTestDataAccess()
            : base(new UnitTRestInMemDB(), 
                new DataAccessConfigOptions{DebugTrace  = false})
        {
            // we need to hold a conneciton open for in mem
            _dbcontext = new SqliteConnection(ConnectionString);
            _dbcontext.Open();
        }


        private const string unitTestTableName = "CA_BLOCKS_UNITTEST_TEMP_TESTTABLE";
        public const string UNIT_TEST_COL_NAME = "col";

        protected string DropTestTableSQL()
        {
            return $"drop table if exists {unitTestTableName}";
        }

        protected string CreateTestTable(string coltype)
        {
            return $"create table if not exists  {unitTestTableName} (id int identity(1,1), col {coltype} )";

        }

        protected string InsertTestDataSQL(string data)
        {
            return  $"Insert into {unitTestTableName} (col) values ({data})";
        }

        protected string SelectTestDataSQL()
        {
            return $"Select col from {unitTestTableName} /*##FILTER##*/";
        }

        protected string SelectTestDataSQL( string where)
        {
            return $"Select col from {unitTestTableName} {@where}";
        }


        // This is a backdoor used for unit testing to setup and teardown test data in the local sql server
        //  this is a helper function and bypasses all the security features around the block.
        public void ExecuteNonQuery(string query)
        {
            var cmd = CreateTextCommand(query);
            ExecuteNonQuery(cmd); 
        }


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
                        int length = row[i].ToString().Length;

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
                            ? row[i].ToString().PadRight(maxLengths[i] + 2)
                            : new string(' ', maxLengths[i] + 2));
                    }

                    sb.AppendLine();
                }
            }
            return sb.ToString();
        }
    }
}
