using System.Diagnostics;
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


        public class SpWhoResult
        {
            public int spid { get; set; }
            public string status { get; set; }
            
            public string loginame { get; set; }
            public string hostname { get; set; }
            public string blk { get; set; }
            public string dbname { get; set; }
            public string cmd { get; set; }
            public int request_id { get; set; }
        }
        

        [Test]
        public void ExecuteSpwhoWithNoReturnValue()
        {
            var cmd = CreateStoredProcedureCommand("sp_who");
            var result = ExecuteDataTable(cmd);
            Assert.IsTrue(result.Rows.Count > 0);
        }

        [Test]
        public void ExecuteSpwhoWithNoReturnValueToObject()
        {
            var cmd = CreateStoredProcedureCommand("sp_who");
            var result = ExecuteToListOf<SpWhoResult>(cmd);
            Assert.IsTrue(result.Count > 0);
        }

        [Test]
        public void ExecuteSpWhoWithParameter()
        {
            string loginName = "sa";
            var cmd = CreateStoredProcedureCommand("sp_who").WithParameter(loginName.ToSqlParameter("@loginame"));
            var result = ExecuteToListOf<SpWhoResult>(cmd);
            Assert.IsTrue(result.Count > 0);
        }

        [Test]
        public void ExecuteSpWhoWithReturnValue()
        {
            var cmd = CreateStoredProcedureCommand("sp_who").WithReturnResult();
            var result = ExecuteDataTable(cmd);
            var spReturnValue = cmd.GetReturnResult();
            Assert.AreEqual(0, spReturnValue);
            Assert.IsTrue(result.Rows.Count > 0);
        }


        [Test]
        public void Execute_CA_Blocks_SQLServerDataAccessUnitTests_SQLServer_Output()
        {
            int intOutput = 0;
            var sqlOutputParamParam = intOutput.ToSqlParameter("@TestOutputValue").AsOutput();
            var cmd = CreateStoredProcedureCommand("CA_Blocks_SQLServerDataAccessUnitTests_SQLServer_Output").WithParameter(sqlOutputParamParam);
            ExecuteNonQuery(cmd);
            intOutput = sqlOutputParamParam.ToValue<int>();
            Assert.AreEqual(123, intOutput);
        }

        [Test]
        public void CA_Blocks_SQLServerDataAccessUnitTests_SQLServer_InputOutput()
        {
            int intInput = 123;
            var sqlInOutParam = intInput.ToSqlParameter("@TestOutputValue").AsInputOutput();
            var cmd = CreateStoredProcedureCommand("CA_Blocks_SQLServerDataAccessUnitTests_SQLServer_InputOutput").WithParameter(sqlInOutParam);
            ExecuteNonQuery(cmd);
            intInput = sqlInOutParam.ToValue<int>();
            Assert.AreEqual(246, intInput);
        }



    }
}
