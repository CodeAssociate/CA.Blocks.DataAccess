using System.Data;
using System.Data.Common;
using CA.Blocks.DataAccess;
using CA.Blocks.DataAccess.DI;
using CA.Blocks.DataAccess.Translator.Extensions;
using CA.Blocks.SqliteDataAccess;
using CA.Blocks.SqliteDataAccessUnitTests.Base;

namespace CA.Blocks.SqliteDataAccessUnitTests.SQLLite
{
    public class sqliteMaster
    {
        public required string name { get; set; }
        public required string type { get; set; }
        public int rootpage { get; set; }
        public required string sql { get; set; }
    }

    // Shows how to use the ExecuteObjectList to get a list of dyanmic objects from a SQL query.
    // This is handy for very quick development
    public class SqlLiteDataAccessMasterTests : UnitTestDataAccess, IDisposable
    {

        public bool TraceCalled = false;
        public bool DbErrorCalled = false;
        public SqlLiteDataAccessMasterTests()
            : base(new DataAccessConfigOptions { DebugTrace = true, TraceExceptions = true })
        {
            var cmd = CreateTextCommand("create table if not exists CABLOCKS_TestMasterTable (id int identity(1,1), col int )");
            ExecuteNonQuery(cmd);
        }

        public new void Dispose()
        {
        }
        
        protected override void TraceDbStatement(IDbCommand cmd)
        {
            Console.WriteLine($"Trace = {cmd.CommandText}");
            base.TraceDbStatement(cmd); // to trigger code coverage
            TraceCalled = true;
        }

        protected override void TraceDbError(IDbCommand cmd, DbException ex)
        {
            Console.WriteLine($"Error with cmd - {cmd.CommandText}");
            Console.WriteLine($"Error Detail - {ex.Message}");
            base.TraceDbError(cmd, ex); // to trigger code coverage
            DbErrorCalled = true;
        }
        
        [Fact]
        public void GetsqliteMasterData()
        {
            var cmd = CreateTextCommand("Select * from sqlite_master");
            var result = Execute(cmd).ToListOf<sqliteMaster>();
            foreach (var o in result)
            {
                Console.WriteLine($"{o.name},{o.type},{o.rootpage},{o.sql}");
            }
        }
        
        [Fact]
        public void GetsqliteMasterDataDynamicList()
        {
            TraceCalled = false;
            var cmd = CreateTextCommand("Select * from sqlite_master where name = @tableName");
            cmd.Parameters.Add("CABLOCKS_TestMasterTable".ToSqlParameter("@tableName"));
            var result = ExecuteObjectList(cmd);
            Assert.True(result.Count > 0);
        }
        
        [Fact]
        public void GetsqliteMasterDataDynamicSingle()
        {
            TraceCalled = false;
            var cmd = CreateTextCommand("Select * from sqlite_master where name = @tableName");
            cmd.Parameters.Add("CABLOCKS_TestMasterTable".ToSqlParameter("@tableName"));
            var result = ExecuteObject(cmd);
            Assert.Equal("CABLOCKS_TestMasterTable", result.name);
        }

        
        
        [Fact]
        public void GetsqliteMasterData_AssertTrace()
        {
            TraceCalled = false;
            var cmd = CreateTextCommand("Select * from sqlite_master");
            var result = Execute(cmd).ToListOf<sqliteMaster>();
            Assert.True(TraceCalled);
        }


        private sqliteMaster CustomReader(IDataReader dr)
        {
            var result = new sqliteMaster
            {
                name = dr.AsString("name"),
                rootpage = dr.AsInt("rootpage"),
                sql = dr.AsString("sql"),
                type = dr.AsString("type")
            };

            return result;

        }

        [Fact]
        public void GetsqliteMasterData_AssertTrace1()
        {
            TraceCalled = false;
            var cmd = CreateTextCommand("Select * from sqlite_master limit 1");
            var result = Execute(cmd).ToListOf<sqliteMaster>(CustomReader);
            Assert.True(TraceCalled);
        }


        [Fact]
        public void GetsqliteMasterData_AssertTraceWithScalar()
        {
            TraceCalled = false;
            var cmd = CreateTextCommand("Select name from sqlite_master where name = @tableName");
            cmd.Parameters.Add("CABLOCKS_TestMasterTable".ToSqlParameter("@tableName"));
            var result = ExecuteScalarAs<string>(cmd);
            Assert.Equal("CABLOCKS_TestMasterTable", result);
            Assert.True(TraceCalled);
        }
        
        [Fact]
        public async Task GetsqliteMasterData_AssertTraceWithScalarAsync()
        {
            TraceCalled = false;
            var cmd = CreateTextCommand("Select name from sqlite_master where name = @tableName");
            cmd.Parameters.Add("CABLOCKS_TestMasterTable".ToSqlParameter("@tableName"));
            var result = await ExecuteScalarAsAsync<string>(cmd);
        
            Assert.Equal("CABLOCKS_TestMasterTable", result);
            Assert.True(TraceCalled);
        }

        [Fact]
        public async Task GetsqliteMasterData_ExecuteToAsync()
        {
            TraceCalled = false;
            var cmd = CreateTextCommand("Select * from sqlite_master where name = @tableName");
            cmd.Parameters.Add("CABLOCKS_TestMasterTable".ToSqlParameter("@tableName"));
            var result = await ExecuteAsync(cmd, TestContext.Current.CancellationToken).ToSingleOrDefault<sqliteMaster>();
    
            Assert.Equal("CABLOCKS_TestMasterTable", result.name);
            Assert.True(TraceCalled);
        }
        
        
        [Fact]
        public async Task GetsqliteMasterData_ExecuteToListOfAsync()
        {
            TraceCalled = false;
            var cmd = CreateTextCommand("Select * from sqlite_master where name = @tableName");
            cmd.Parameters.Add("CABLOCKS_TestMasterTable".ToSqlParameter("@tableName"));
            var result = await ExecuteAsync(cmd, TestContext.Current.CancellationToken).ToListOf<sqliteMaster>();
            Assert.Single(result);
            Assert.Equal("CABLOCKS_TestMasterTable", result[0].name);
            Assert.True(TraceCalled);
        }
        
        [Fact]
        public async Task GetsqliteMasterData_AssertTraceWithExecuteScalarWithConvertAsAsync()
        {
            TraceCalled = false;
            var cmd = CreateTextCommand("Select rootpage from sqlite_master where name = @tableName");
            cmd.Parameters.Add("CABLOCKS_TestMasterTable".ToSqlParameter("@tableName"));
            // the data will be in from db return as string
            await ExecuteScalarWithConvertAsAsync<string>(cmd);
            Assert.True(TraceCalled);
        }
        
        [Fact]
        public void GetsqliteMasterData_AssertDbErrorCalled()
        {
            DbErrorCalled = false;
            var cmd = CreateTextCommand("Select * from BadTableName");
            try
            {
                var result = Execute(cmd).ToDataTable();
            }
            catch 
            {
            }
            Assert.True(DbErrorCalled);
        }
        
        




    }
}





