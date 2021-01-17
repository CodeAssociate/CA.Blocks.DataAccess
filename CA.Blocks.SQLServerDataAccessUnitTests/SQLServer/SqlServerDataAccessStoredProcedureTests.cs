using CA.Blocks.SQLServerDataAccess;
using CA.Blocks.SQLServerDataAccessUnitTests.Base;
using NUnit.Framework;

namespace CA.Blocks.SQLServerDataAccessUnitTests.SQLServer
{
    [TestFixture]
    public class SqlServerDataAccessStoredProcedureTests : UnitTestDataAccess
    {

        
        private void DropExistingTesttoredProcedure(string SpName)
        {
            var cmd = CreateTextCommand($"If Exists (Select * from sysobjects where name = '{SpName}' and xtype = 'P') BEGIN drop procedure {SpName} END");
            ExecuteNonQuery(cmd);
        }


        [OneTimeSetUp]
        public void Setup()
        {
            DropExistingTesttoredProcedure("CA_Blocks_SQLServerDataAccessUnitTests_SQLServer_Output");
            string sqlTestProc =
                @"CREATE PROCEDURE CA_Blocks_SQLServerDataAccessUnitTests_SQLServer_Output @TestOutputValue INT OUTPUT AS
            BEGIN
            SELECT @TestOutputValue = 123
            END";
            ExecuteNonQuery(CreateTextCommand(sqlTestProc));


            DropExistingTesttoredProcedure("CA_Blocks_SQLServerDataAccessUnitTests_SQLServer_InputOutput");
            string sqlTestProc2 =
                @"CREATE PROCEDURE CA_Blocks_SQLServerDataAccessUnitTests_SQLServer_InputOutput @TestOutputValue INT OUTPUT AS
            BEGIN
            SELECT @TestOutputValue = @TestOutputValue * 2
            END";
            ExecuteNonQuery(CreateTextCommand(sqlTestProc2));
        }

        [OneTimeTearDown]
        public void Teardown()
        {
            DropExistingTesttoredProcedure("CA_Blocks_SQLServerDataAccessUnitTests_SQLServer_Output");
            DropExistingTesttoredProcedure("CA_Blocks_SQLServerDataAccessUnitTests_SQLServer_InputOutput");

        }


        [Test]
        public void ExecuteSpwhoWithNoReturnValue()
        {
            var cmd = CreateBlankStoredProcedureCommand("sp_who");
            var result = ExecuteDataTable(cmd);
            Assert.IsTrue(result.Rows.Count > 0);
        }
        
        [Test]
        public void ExecuteSpWhoWithReturnValue()
        {
            var cmd = CreateBlankStoredProcedureCommand("sp_who").WithReturnResult();
            var result = ExecuteDataTable(cmd);
            var spReturnValue = cmd.GetReturnResult();
            Assert.AreEqual(0, spReturnValue);
            Assert.IsTrue(result.Rows.Count > 0);
        }


        [Test]
        public void Execute_CA_Blocks_SQLServerDataAccessUnitTests_SQLServer_Output()
        {
            int test = 0;
            var outParam = test.ToSqlParameter("@TestOutputValue").AsOutout();
            var cmd = CreateBlankStoredProcedureCommand("CA_Blocks_SQLServerDataAccessUnitTests_SQLServer_Output").WithParameter(outParam);
            ExecuteNonQuery(cmd);
            test = outParam.ToValue<int>();
            Assert.AreEqual(123, test);
        }

        [Test]
        public void CA_Blocks_SQLServerDataAccessUnitTests_SQLServer_InputOutput()
        {
            int test = 123;
            var inoutParam = test.ToSqlParameter("@TestOutputValue").AsInputOutput();
            var cmd = CreateBlankStoredProcedureCommand("CA_Blocks_SQLServerDataAccessUnitTests_SQLServer_InputOutput").WithParameter(inoutParam);
            ExecuteNonQuery(cmd);
            test = inoutParam.ToValue<int>();
            Assert.AreEqual(246, test);
        }

    }
}
