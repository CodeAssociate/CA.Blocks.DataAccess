using System.Data;
using Microsoft.Data.SqlClient;
using System.Text;
using CA.Blocks.DataAccess.DI;
using CA.Blocks.SQLServerDataAccess;
using CA.Blocks.DataAccess.Model.Paging;

namespace CA.Blocks.SQLServerDataAccessUnitTests.Base
{
    
    public class LocalSqlServerUnitTestStringsResolver : IDataAccessKeyToConnectionStringResolver
    {
        /// <summary>
        /// Provides the mapping from the name in code to the name in the Connection string Stored in the app.config or web.config file
        /// </summary>
        /// <param name="connectionStringKey">The Connection string known to the code</param>
        /// <returns> The Connection string to be used by the ADO.NET provider.</returns>
        public string GetConnectionString(string connectionStringKey)
        {
            return "Server=(local);Database=tempdb;Integrated Security=SSPI;TrustServerCertificate=True";
        }
    }

    /*
         <configuration>
            <connectionStrings>    
                <add name="localsqlserverhost" connectionString="Server=(local);Database=tempdb;Integrated Security=SSPI" providerName="System.Data.SqlClient"/>
            </connectionStrings>
         </configuration>
         */
    // this class exposes the internal workings so we can test
    public class UnitTestDataAccess : SqlServerDataAccess
    {
        public UnitTestDataAccess() : this (new DataAccessConfigOptions
            {ConnectionStringKey = "localsqlserverhost"})
        {
        }

        public UnitTestDataAccess(DataAccessConfigOptions options) : base (
            new DataAccessConfig("UnitTestDataAccess", options, new LocalSqlServerUnitTestStringsResolver())
        )
        {
        }


        private const string unitTestTableName = "CA_BLOCKS_UNITTEST_TEMP_TESTTABLE";
        public const string UNIT_TEST_COL_NAME = "Col";

        protected string DropTestTableSQL()
        {
            return $"if exists (select * from sysobjects where xtype = 'U' and id = object_id(N'{unitTestTableName}')) begin drop table {unitTestTableName} end";
        }

        protected string CreateTestTable(string coltype)
        {
            return $"Create table {unitTestTableName} (id int identity(1,1), col {coltype} )";

        }

        protected string InsertTestDataSQL(string data)
        {
            return  $"Insert into {unitTestTableName}  values ({data})";
        }

        protected string SelectTestDataSQL()
        {
            return $"Select col from {unitTestTableName} /*##FILTER##*/";
        }


        // This is a backdoor used for unit testing to setup and teardown test data in the local sql server
        //  this is a helper function and bypasses all the security features around the block.
        public void ExecuteNonQuery(string query)
        {
            SqlCommand cmd = CreateTextCommand(query);
            ExecuteNonQuery(cmd); 
        }


        public new DataTable ExecuteDataTable(SqlCommand cmd, PagingRequest page)
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
