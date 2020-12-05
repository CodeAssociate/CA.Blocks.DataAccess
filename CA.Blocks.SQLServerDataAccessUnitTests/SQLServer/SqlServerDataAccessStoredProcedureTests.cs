using CA.Blocks.SQLServerDataAccessUnitTests.Base;
using NUnit.Framework;

namespace CA.Blocks.SQLServerDataAccessUnitTests.SQLServer
{
    [TestFixture]
    public class SqlServerDataAccessStoredProcedureTests : UnitTestDataAccess
    {
        [Test]
        public void ExecuteSpwhoWithNoReturnValue()
        {
            var cmd = CreateBlankStoredProcedureCommand("sp_who", false);
            var result = ExecuteDataTable(cmd);
            Assert.IsTrue(result.Rows.Count > 0);
            ////VIEWOUTPUT
            // Trace.Write(DataTableToText(result));
        }


        [Test]
        public void ExecuteSpwhoWithReturnValue()
        {
            var cmd = CreateBlankStoredProcedureCommand("sp_who", true);
            var result = ExecuteDataTable(cmd);
            var spresult = this.GetStoredProcedureReturnValue(cmd);
            Assert.AreEqual(0, spresult);
            Assert.IsTrue(result.Rows.Count > 0);
        }
    }
}
