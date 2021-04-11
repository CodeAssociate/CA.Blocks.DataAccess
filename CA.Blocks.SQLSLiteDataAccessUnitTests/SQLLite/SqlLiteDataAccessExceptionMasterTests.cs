using System;
using System.Data;
using CA.Blocks.SQLLiteDataAccessUnitTests.Base;
using NUnit.Framework;

namespace CA.Blocks.SQLLiteDataAccessUnitTests.SQLLite
{
    [TestFixture]
    public class SqlLiteDataAccessExceptionMasterTests : UnitTestBadConnection
    {
        public bool TraceErrorCalled = false;

        protected override void TraceGenralError(IDbCommand cmd, Exception ex)
        {
            TestContext.WriteLine($"Error with cmd - {cmd.CommandText}");
            TestContext.WriteLine($"Error Detail - {ex.Message}");
            base.TraceGenralError(cmd, ex); // to trigger code coverage
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