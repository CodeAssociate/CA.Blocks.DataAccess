using System.Data;
using System.Data.Common;
using CA.Blocks.DataAccess.DI;
using CA.Blocks.SQLLiteDataAccess;
using CA.Blocks.SQLLiteDataAccessUnitTests.Base;
using NUnit.Framework;

namespace CA.Blocks.SQLLiteDataAccessUnitTests.SQLLite
{
    internal class sqliteMaster
    {
        public string name { get; set; }
        public string type { get; set; }
        public int rootpage { get; set; }
        public string sql { get; set; }
    }

    // Shows how to use the ExecuteObjectList to get a list of dyanmic objects from a SQL query.
    // This is handy for very quick development
    [TestFixture]
    public class SqlLiteDataAccessMasterTests : UnitTestDataAccess
    {

        public bool TraceCalled = false;
        public bool DbErrorCalled = false;
        public SqlLiteDataAccessMasterTests() : base(new DataAccessConfigOptions
            {DebugTrace = true, TraceExceptions = true})
        {

        }

        [SetUp]
        public void CreateMasterTestTable()
        {
            var cmd = CreateTextCommand("create table if not exists CABLOCKS_TestMasterTable (id int identity(1,1), col int )");
            ExecuteNonQuery(cmd);
        }
        
        protected override void TraceDbStatement(IDbCommand cmd)
        {
            TestContext.WriteLine($"Trace = {cmd.CommandText}");
            base.TraceDbStatement(cmd); // to trigger code coverage
            TraceCalled = true;
        }

        protected override void TraceDbError(IDbCommand cmd, DbException ex)
        {
            TestContext.WriteLine($"Error with cmd - {cmd.CommandText}");
            TestContext.WriteLine($"Error Detail - {ex.Message}");
            base.TraceDbError(cmd, ex); // to trigger code coverage
            DbErrorCalled = true;
        }
        
        [Test]
        public void GetsqliteMasterData()
        {
            var cmd = CreateTextCommand("Select * from sqlite_master");
            var result = ExecuteToListOf<sqliteMaster>(cmd);
            foreach (var o in result)
            {
                TestContext.WriteLine($"{o.name},{o.type},{o.rootpage},{o.sql}");
            }
        }
        
        [Test]
        public void GetsqliteMasterDataDynamicList()
        {
            TraceCalled = false;
            var cmd = CreateTextCommand("Select * from sqlite_master where name = @tableName");
            cmd.Parameters.Add("CABLOCKS_TestMasterTable".ToSqlParameter("@tableName"));
            var result = ExecuteObjectList(cmd);
            Assert.IsTrue(result.Count > 0);
        }
        
        [Test]
        public void GetsqliteMasterDataDynamicSingle()
        {
            TraceCalled = false;
            var cmd = CreateTextCommand("Select * from sqlite_master where name = @tableName");
            cmd.Parameters.Add("CABLOCKS_TestMasterTable".ToSqlParameter("@tableName"));
            var result = ExecuteObject(cmd);
            Assert.AreEqual("CABLOCKS_TestMasterTable", result.name);
        }

        
        
        [Test]
        public void GetsqliteMasterData_AssertTrace()
        {
            TraceCalled = false;
            var cmd = CreateTextCommand("Select * from sqlite_master");
            var result = ExecuteToListOf<sqliteMaster>(cmd);
            Assert.IsTrue(TraceCalled);
        }

        
        [Test]
        public void GetsqliteMasterData_AssertTraceWithScalar()
        {
            TraceCalled = false;
            var cmd = CreateTextCommand("Select name from sqlite_master where name = @tableName");
            cmd.Parameters.Add("CABLOCKS_TestMasterTable".ToSqlParameter("@tableName"));
            var result = ExecuteScalarAs<string>(cmd);
            Assert.AreEqual("CABLOCKS_TestMasterTable", result);
            Assert.IsTrue(TraceCalled);
        }
        
        [Test]
        public void GetsqliteMasterData_AssertTraceWithScalarAsync()
        {
            TraceCalled = false;
            var cmd = CreateTextCommand("Select name from sqlite_master where name = @tableName");
            cmd.Parameters.Add("CABLOCKS_TestMasterTable".ToSqlParameter("@tableName"));
            var result = ExecuteScalarAsAsync<string>(cmd);
            result.Wait();
            Assert.AreEqual("CABLOCKS_TestMasterTable", result.Result);
            Assert.IsTrue(TraceCalled);
        }

        [Test]
        public void GetsqliteMasterData_ExecuteToAsync()
        {
            TraceCalled = false;
            var cmd = CreateTextCommand("Select * from sqlite_master where name = @tableName");
            cmd.Parameters.Add("CABLOCKS_TestMasterTable".ToSqlParameter("@tableName"));
            var result = ExecuteToAsync<sqliteMaster>(cmd);
            result.Wait();
            Assert.AreEqual("CABLOCKS_TestMasterTable", result.Result.name);
            Assert.IsTrue(TraceCalled);
        }
        
        
        [Test]
        public void GetsqliteMasterData_ExecuteToListOfAsync()
        {
            TraceCalled = false;
            var cmd = CreateTextCommand("Select * from sqlite_master where name = @tableName");
            cmd.Parameters.Add("CABLOCKS_TestMasterTable".ToSqlParameter("@tableName"));
            var result = ExecuteToListOfAsync<sqliteMaster>(cmd);
            result.Wait();
            Assert.AreEqual(1, result.Result.Count);
            Assert.AreEqual("CABLOCKS_TestMasterTable", result.Result[0].name);
            Assert.IsTrue(TraceCalled);
        }
        
        [Test]
        public void GetsqliteMasterData_AssertTraceWithExecuteScalarWithConvertAsAsync()
        {
            TraceCalled = false;
            var cmd = CreateTextCommand("Select rootpage from sqlite_master where name = @tableName");
            cmd.Parameters.Add("CABLOCKS_TestMasterTable".ToSqlParameter("@tableName"));
            // the data will be in from db return as string
            var result = ExecuteScalarWithConvertAsAsync<string>(cmd);
            result.Wait();
            Assert.IsTrue(TraceCalled);
        }
        
        [Test]
        public void GetsqliteMasterData_AssertDbErrorCalled()
        {
            DbErrorCalled = false;
            var cmd = CreateTextCommand("Select * from BadTableName");
            try
            {
                var result = ExecuteDataTable(cmd);
            }
            catch 
            {
            }
            Assert.IsTrue(DbErrorCalled);
        }
        
        




    }
}
