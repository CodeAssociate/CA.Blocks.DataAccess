using System.Collections.Generic;
using System.Data;
using System.Linq;
using CA.Blocks.DataAccess.DI;
using CA.Blocks.DataAccess.Translator.Extensions;
using CA.Blocks.SQLServerDataAccess;


namespace CA.Blocks.SQLServerDataAccessUnitTests.Samples.AdventureWorks.AllowList
{

    public class AdventureWorksDynamicTableQuery : SqlServerDataAccess
    {
        public AdventureWorksDynamicTableQuery() : base(new SimpleConnectionStringDataAccessConfig(
            "Server=(local);Database=AdventureWorks2019;Integrated Security=SSPI;TrustServerCertificate=True"))

        {

        }

        // This method is private it provide the allow list, this list can be hard coded or dynamic 
        private IList<string> GetAllowedListFor(string schema)
        {
            var sql = $"Select TABLE_NAME from INFORMATION_SCHEMA.TABLES where TABLE_SCHEMA = @schema";
            var cmd = CreateTextCommand(sql).WithParameter(schema.ToSqlParameter("@schema"));
            return Execute(cmd).ToSingleNamedColumnList<string>("TABLE_NAME");
        }

        // Example of execute a sql statement that cannot be parameterised but using a allow list to prevent an injection
        public DataTable SelectDynamicTableFromSalesSchema(string tableName)
        {
            var schema = "Sales";
            // we need to validate the tableName first against allowList.
            var allowedList = GetAllowedListFor(schema);
            if (allowedList.Any( x=> x == tableName))
            {
                var sql = $"Select * from [{schema}].[{tableName}]";
                var cmd = CreateTextCommand(sql);
                return ExecuteDataTable(cmd);
            }
            else
            {
                throw new DataException($"{tableName} is not allowed");
            }
        }
    }
}
