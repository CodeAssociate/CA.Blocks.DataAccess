using System;
using System.Data;
using System.Data.Common;
using CA.Blocks.SqliteDataAccessUnitTests.Base;

namespace CA.Blocks.SqliteDataAccessUnitTests.SQLLite
{
    public class SqlLiteDataAccessExceptionMasterTests : UnitTestBadConnection
    {
        private bool TraceErrorCalled = false;

        protected override void TraceGeneralError(IDbCommand cmd, Exception ex)
        {
            Console.WriteLine($"Error with cmd - {cmd.CommandText}");
            Console.WriteLine($"Error Detail - {ex.Message}");
            base.TraceGeneralError(cmd, ex); // to trigger code coverage
            TraceErrorCalled = true;
        }

        protected override void TraceDbError(IDbCommand cmd, DbException ex)
        {
            Console.WriteLine($"Error with cmd - {cmd.CommandText}");
            Console.WriteLine($"Error Detail - {ex.Message}");
            base.TraceGeneralError(cmd, ex); // to trigger code coverage
            TraceErrorCalled = true;
        }

        [Fact]
        public void TestGeneralError()
        {
            TraceErrorCalled = false;
            var cmd = CreateTextCommand("Select * from sqlite_master");
            try
            {
                var result = ExecuteDataTable(cmd);
                Assert.Fail($"Got {result.Rows.Count} rows ?" );
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Assert.True(TraceErrorCalled);
        }
    }
}



