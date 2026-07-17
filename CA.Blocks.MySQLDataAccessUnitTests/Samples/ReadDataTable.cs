using CA.Blocks.DataAccess.DataTableHelpers;
using CA.Blocks.DataAccess.DI;
using CA.Blocks.DataAccessTestDataForUnitTests.ConnectionStringResolver;
using CA.Blocks.MySQLDataAccess;
using Xunit;
using System.Data;

namespace CA.Blocks.MySQLDataAccessUnitTests.Samples
{
public class ReadDataTable
    {

        public class ReadDataTableDataAccess : MySqlDataAccess
        {
            public ReadDataTableDataAccess() : base(
                new DataAccessConfig(new DataAccessConfigOptions { ConnectionStringKey = "notused" },
                     new LocalFileConnectionStringResolver("MySQLDataAccessConnectionString.txt"))
            )
            {
            }

            public DataTable GetInformationSchema()
            {
                var cmd = CreateTextCommand("select *  from information_schema.tables");
                return base.ExecuteDataTable(cmd);
            }

        }
        [Fact]
public void GetGetInformationSchema()
        {
            var target = new ReadDataTableDataAccess();
            var executeResult = target.GetInformationSchema();
            Console.WriteLine(DataTableToTextHelper.OutPutAsAlignedText(executeResult));

        }
    }
}

