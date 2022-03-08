using System.Dynamic;
using System.Globalization;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using CA.Blocks.SQLServerDataAccess;
using Microsoft.Data.SqlClient;
using CA.Blocks.SQLServerDataAccessUnitTests.Base;
using NUnit.Framework;

namespace CA.Blocks.SQLServerDataAccessUnitTests.SQLServer
{
    internal class TempNonQueryDate
    {
        public int id { get; set; }
        public string name { get; set; }
    }


    [TestFixture]
    public class SqlServerDataAccessExecuteNonQueryTests : UnitTestDataAccess
    {
        private string CreateTempTable = "Create table _tempSqlServerDataAccessExecuteNonQueryTests (id int, name varchar(10))";
        private string Insert1 = "insert into _tempSqlServerDataAccessExecuteNonQueryTests values (1, 'test')";
        private string Insert2 = "insert into _tempSqlServerDataAccessExecuteNonQueryTests values (2, 'tes2t')";
        private string NoDelete = "delete from _tempSqlServerDataAccessExecuteNonQueryTests where id = 3";
        private string Delete = "delete from _tempSqlServerDataAccessExecuteNonQueryTests";
        private string DropTable = "drop table _tempSqlServerDataAccessExecuteNonQueryTests";


        [Test]
        public void ExecuteNonQueryTests()
        {
            SqlCommand cmd1 = CreateTextCommand(CreateTempTable);
            var cmd1result = ExecuteNonQuery(cmd1);
            Assert.AreEqual(cmd1result, -1);


            SqlCommand cmd2 = CreateTextCommand(Insert1);
            var cmd2result = ExecuteNonQuery(cmd2);
            Assert.AreEqual(cmd2result, 1);


            SqlCommand cmd3 = CreateTextCommand(Insert2);
            var cmd3result = ExecuteNonQuery(cmd3);
            Assert.AreEqual(cmd3result, 1);


            SqlCommand cmd4 = CreateTextCommand(NoDelete);
            var cmd4result = ExecuteNonQuery(cmd4);
            Assert.AreEqual(cmd4result, 0);

            SqlCommand cmd5 = CreateTextCommand(Delete);
            var cmd5result = ExecuteNonQuery(cmd5);
            Assert.AreEqual(cmd5result, 2);


            SqlCommand cmd6 = CreateTextCommand(DropTable);
            var cmd6result = ExecuteNonQuery(cmd6);
            Assert.AreEqual(cmd1result, -1);

        }
        #region async Tests

        [Test]
        public async Task ExecuteNonQueryTestsAsync()
        {
            SqlCommand cmd1 = CreateTextCommand(CreateTempTable);
            var cmd1result = await ExecuteNonQueryAsync(cmd1);
            Assert.AreEqual(cmd1result, -1);


            SqlCommand cmd2 = CreateTextCommand(Insert1);
            var cmd2result = await ExecuteNonQueryAsync(cmd2);
            Assert.AreEqual(cmd2result, 1);


            SqlCommand cmd3 = CreateTextCommand(Insert2);
            var cmd3result = await ExecuteNonQueryAsync(cmd3);
            Assert.AreEqual(cmd3result, 1);


            SqlCommand cmd4 = CreateTextCommand(NoDelete);
            var cmd4result = await ExecuteNonQueryAsync(cmd4);
            Assert.AreEqual(cmd4result, 0);

            SqlCommand cmd5 = CreateTextCommand(Delete);
            var cmd5result = await ExecuteNonQueryAsync(cmd5);
            Assert.AreEqual(cmd5result, 2);


            SqlCommand cmd6 = CreateTextCommand(DropTable);
            var cmd6result = await ExecuteNonQueryAsync(cmd6);
            Assert.AreEqual(cmd1result, -1);
        }

        #endregion 
    }

}
