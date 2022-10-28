using System;
using System.Data;
using System.Data.Common;
using CA.Blocks.SqliteDataAccessUnitTests.Base;
using NUnit.Framework;

namespace CA.Blocks.SqliteDataAccessUnitTests.SQLLite
{
    [TestFixture]
    public class SqlLiteDataAccessExceptionMasterTests : UnitTestBadConnection
    {
        public bool TraceErrorCalled = false;

        protected override void TraceGeneralError(IDbCommand cmd, Exception ex)
        {
            TestContext.WriteLine($"Error with cmd - {cmd.CommandText}");
            TestContext.WriteLine($"Error Detail - {ex.Message}");
            base.TraceGeneralError(cmd, ex); // to trigger code coverage
            TraceErrorCalled = true;
        }

        protected override void TraceDbError(IDbCommand cmd, DbException ex)
        {
            TestContext.WriteLine($"Error with cmd - {cmd.CommandText}");
            TestContext.WriteLine($"Error Detail - {ex.Message}");
            base.TraceGeneralError(cmd, ex); // to trigger code coverage
            TraceErrorCalled = true;
        }

        [Test]
        public void TestGeneralError()
        {
            TraceErrorCalled = false;
            var cmd = CreateTextCommand("Select * from sqlite_master");
            try
            {
                var result = ExecuteDataTable(cmd);
            }
            catch (System.Exception ex)
            {
                TestContext.WriteLine(ex.Message);
            }
            Assert.IsTrue(TraceErrorCalled);
        }
    }
}