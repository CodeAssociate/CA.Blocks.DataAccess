using CA.Blocks.DataAccess;
using CA.Blocks.DataAccess.Translator.Extensions;
using CA.Blocks.SQLServerDataAccess;
using CA.Blocks.SQLServerDataAccessUnitTests.Base;
// ReSharper disable InconsistentNaming

namespace CA.Blocks.SQLServerDataAccessUnitTests.SQLServer
{
    [Collection("DbIntegrationTests")]
    public class SqlServerDataAccessStoredProcedureTests : UnitTestDataAccess, IDisposable
    {

        
        private void DropExistingTesttoredProcedure(string SpName)
        {
            var cmd = CreateTextCommand($"If Exists (Select * from sysobjects where name = '{SpName}' and xtype = 'P') BEGIN drop procedure {SpName} END");
            ExecuteNonQuery(cmd);
        }


        public SqlServerDataAccessStoredProcedureTests()
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

        public void Dispose()
        {
            DropExistingTesttoredProcedure("CA_Blocks_SQLServerDataAccessUnitTests_SQLServer_Output");
            DropExistingTesttoredProcedure("CA_Blocks_SQLServerDataAccessUnitTests_SQLServer_InputOutput");

        }


        public class SpWhoResult
        {
            public int spid { get; init; }
            public required string status { get; init; }
            
            public required string loginame { get; init; }
            public required string hostname { get; init; }
            public required string blk { get; init; }
            public required string dbname { get; init; }
            public required string cmd { get; init; }
            public int request_id { get; init; }
        }
        

        [Fact]
        public void ExecuteSpwhoWithNoReturnValue()
        {
            var cmd = CreateStoredProcedureCommand("sp_who");
            var result = Execute(cmd).ToDataTable();
            Assert.True(result.Rows.Count > 0);
        }

        [Fact]
        public void ExecuteSpwhoWithNoReturnValueToObject()
        {
            var cmd = CreateStoredProcedureCommand("sp_who");
            var result = Execute(cmd).ToListOf<SpWhoResult>();
            Assert.True(result.Count > 0);
        }

        [Fact]
        public void ExecuteSpWhoWithParameter()
        {
            string loginName = "sa";
            var cmd = CreateStoredProcedureCommand("sp_who").WithParameter(loginName.ToSqlParameter("@loginame"));
            var result = Execute(cmd).ToListOf<SpWhoResult>();
            Assert.True(result.Count > 0);
        }

        [Fact]
        public void ExecuteSpWhoWithReturnValue()
        {
            var cmd = CreateStoredProcedureCommand("sp_who").WithReturnResult();
            var result = Execute(cmd).ToDataTable();
            var spReturnValue = cmd.GetReturnResult();
            Assert.Equal(0, spReturnValue);
            Assert.True(result.Rows.Count > 0);
        }


        [Fact]
        public void Execute_CA_Blocks_SQLServerDataAccessUnitTests_SQLServer_Output()
        {
            int intOutput = 0;
            var sqlOutputParamParam = intOutput.ToSqlParameter("@TestOutputValue").AsOutput();
            var cmd = CreateStoredProcedureCommand("CA_Blocks_SQLServerDataAccessUnitTests_SQLServer_Output").WithParameter(sqlOutputParamParam);
            ExecuteNonQuery(cmd);
            intOutput = sqlOutputParamParam.ToValue<int>();
            Assert.Equal(123, intOutput);
        }

        [Fact]
        public void CA_Blocks_SQLServerDataAccessUnitTests_SQLServer_InputOutput()
        {
            int intInput = 123;
            var sqlInOutParam = intInput.ToSqlParameter("@TestOutputValue").AsInputOutput();
            var cmd = CreateStoredProcedureCommand("CA_Blocks_SQLServerDataAccessUnitTests_SQLServer_InputOutput").WithParameter(sqlInOutParam);
            ExecuteNonQuery(cmd);
            intInput = sqlInOutParam.ToValue<int>();
            Assert.Equal(246, intInput);
        }



    }
}



