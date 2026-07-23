using System.Dynamic;
using System.Globalization;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using CA.Blocks.SQLServerDataAccess;
using Microsoft.Data.SqlClient;
using CA.Blocks.SQLServerDataAccessUnitTests.Base;

namespace CA.Blocks.SQLServerDataAccessUnitTests.SQLServer
{
    internal class TempNonQueryDate
    {
        public int id { get; set; }
        public string name { get; set; }
    }


    [Collection("DbIntegrationTests")]
    public class SqlServerDataAccessExecuteNonQueryTests : UnitTestDataAccess
    {
        private string CreateTempTable = "Create table _tempSqlServerDataAccessExecuteNonQueryTests (id int, name varchar(10))";
        private string Insert1 = "insert into _tempSqlServerDataAccessExecuteNonQueryTests values (1, 'test')";
        private string Insert2 = "insert into _tempSqlServerDataAccessExecuteNonQueryTests values (2, 'tes2t')";
        private string NoDelete = "delete from _tempSqlServerDataAccessExecuteNonQueryTests where id = 3";
        private string Delete = "delete from _tempSqlServerDataAccessExecuteNonQueryTests";
        private string DropTable = "drop table _tempSqlServerDataAccessExecuteNonQueryTests";


        [Fact]
        public void ExecuteNonQueryTests()
        {
            SqlCommand cmd1 = CreateTextCommand(CreateTempTable);
            var cmd1result = ExecuteNonQuery(cmd1);
            Assert.Equal(-1, cmd1result);


            SqlCommand cmd2 = CreateTextCommand(Insert1);
            var cmd2result = ExecuteNonQuery(cmd2);
            Assert.Equal(1, cmd2result);


            SqlCommand cmd3 = CreateTextCommand(Insert2);
            var cmd3result = ExecuteNonQuery(cmd3);
            Assert.Equal(1, cmd3result);


            SqlCommand cmd4 = CreateTextCommand(NoDelete);
            var cmd4result = ExecuteNonQuery(cmd4);
            Assert.Equal(0, cmd4result);

            SqlCommand cmd5 = CreateTextCommand(Delete);
            var cmd5result = ExecuteNonQuery(cmd5);
            Assert.Equal(2, cmd5result);


            SqlCommand cmd6 = CreateTextCommand(DropTable);
            var cmd6result = ExecuteNonQuery(cmd6);
            Assert.Equal(-1, cmd1result);

        }
        #region async Tests

        [Fact]
        public async Task ExecuteNonQueryTestsAsync()
        {
            SqlCommand cmd1 = CreateTextCommand(CreateTempTable);
            var cmd1result = await ExecuteNonQueryAsync(cmd1);
            Assert.Equal(-1, cmd1result);


            SqlCommand cmd2 = CreateTextCommand(Insert1);
            var cmd2result = await ExecuteNonQueryAsync(cmd2);
            Assert.Equal(1, cmd2result);


            SqlCommand cmd3 = CreateTextCommand(Insert2);
            var cmd3result = await ExecuteNonQueryAsync(cmd3);
            Assert.Equal(1, cmd3result);


            SqlCommand cmd4 = CreateTextCommand(NoDelete);
            var cmd4result = await ExecuteNonQueryAsync(cmd4);
            Assert.Equal(0, cmd4result);

            SqlCommand cmd5 = CreateTextCommand(Delete);
            var cmd5result = await ExecuteNonQueryAsync(cmd5);
            Assert.Equal(2, cmd5result);


            SqlCommand cmd6 = CreateTextCommand(DropTable);
            var cmd6result = await ExecuteNonQueryAsync(cmd6);
            Assert.Equal(-1, cmd1result);
        }

        #endregion 
    }

}




