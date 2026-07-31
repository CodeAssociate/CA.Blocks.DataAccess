using System.Data;
using CA.Blocks.DataAccess;
using CA.Blocks.DataAccess.Extensions;
using CA.Blocks.DataAccess.Translator.Extensions;
using CA.Blocks.SQLServerDataAccess;
using CA.Blocks.SQLServerDataAccessUnitTests.Base;

namespace CA.Blocks.SQLServerDataAccessUnitTests.SQLServer
{
    [Collection("DbIntegrationTests")]
    public class SqlServerCompressionTests : UnitTestDataAccess
    {
        private string dropTempTestTable = @"If Exists (Select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = '_tempSqlServerSqlServerCompressionTests') 
BEGIN
    drop table _tempSqlServerSqlServerCompressionTests
END";
        private string CreateTempTestTable = @"Create table _tempSqlServerSqlServerCompressionTests (Id int, dataValue varbinary(max))";

        private string InsertData =
            "insert into _tempSqlServerSqlServerCompressionTests (id, dataValue) values (@id, @data)";

        public class TestDataObj
        {
            public int Id { get; set; }

            public string? dataValue { get; set; }
        }

        public class TestDataObjTranslator : CA.Blocks.DataAccess.Translator.SimpleDbRow2ObjectTranslator<TestDataObj>
        {
            protected override TestDataObj CustomTranslate(DataRow dr)
            {
                var result = new TestDataObj();
                result.Id = dr.AsInt("Id");
                result.dataValue = dr.AsBinary("DataValue").DecompressToSqlNVarcharString();
                return result;
            }
        }

        private void SetupTest()
        {
            var cmd1 = CreateTextCommand(dropTempTestTable);
            ExecuteNonQuery(cmd1);

            var cmd2 = CreateTextCommand(CreateTempTestTable);
            ExecuteNonQuery(cmd2);
        }

        private void CleanUpTest()
        {
            var cmd = CreateTextCommand(dropTempTestTable);
            ExecuteNonQuery(cmd);
        }

        [Fact]
        public void CompressToSQLNVarcharStringTests()
        {
            // setup 
            string testData = $"{Guid.NewGuid()}-{Guid.NewGuid()}-{Guid.NewGuid()}";
            testData += testData;

            SetupTest();
            // Act
            var cmd = CreateTextCommand(InsertData);
            cmd.Parameters.Add(1.ToSqlParameter("@id"));
            cmd.Parameters.Add(testData.CompressToSqlNVarcharString().ToSqlParameter("@data"));
            ExecuteNonQuery(cmd);

            // Read data as binary
            var readAsBinary = CreateTextCommand("Select Id, dataValue from _tempSqlServerSqlServerCompressionTests where id = 1");
            var t = new TestDataObjTranslator();
            var dt = Execute(readAsBinary).ToDataTable();
            var bresult = t.Translate(dt.Rows[0]);
            
            // Read data as string 

            var readAsString = CreateTextCommand("Select Id, cast(decompress(dataValue) as nvarchar(max)) as dataValue from _tempSqlServerSqlServerCompressionTests where id = 1");
            var tresult = ExecuteTo<TestDataObj>(readAsString);
            // assert

            Assert.Equal(bresult.Id, tresult.Id);
            Assert.Equal(testData, bresult.dataValue);
            Assert.Equal(testData,tresult.dataValue);
           
            CleanUpTest();
        }
    }
}




