using System;
using CA.Blocks.DataAccess.Translator.Basic;
using CA.Blocks.DataAccess.Translator.Extensions;
using CA.Blocks.SQLServerDataAccess;
using CA.Blocks.SQLServerDataAccessUnitTests.Base;

namespace CA.Blocks.SQLServerDataAccessUnitTests.Translator.DbTypeTests
{
    [Collection("DbIntegrationTests")]
    public class DbTypeVarCharTests : UnitTestDataAccess, IDisposable
    {
        private const string  TEST_DATA = "varchar data";

        private void InsertTestData(string data)
        {
            ExecuteNonQuery(InsertTestDataCommand(data));
        }

        public DbTypeVarCharTests()
        {
            ExecuteNonQuery(DropTestTableCommand());
            var create = CreateTestTableCommand("varchar");
            create.CommandText = create.CommandText.Replace("[varchar]", "[varchar](50)");
            ExecuteNonQuery(create);
            InsertTestData(TEST_DATA);
            InsertTestData(Guid.NewGuid().ToString());
            InsertTestData(Guid.NewGuid().ToString());
            InsertTestData(Guid.NewGuid().ToString());
            InsertTestData(Guid.NewGuid().ToString());
        }

        public void Dispose()
        {
            ExecuteNonQuery(DropTestTableCommand());
        }

        [Fact]
        public void SelectAllDataBinary()
        {
            //Setup 
            var t = new StringTranslator(UNIT_TEST_COL_NAME);
            var cmd = BasicSelectCommand();
            //Act
            var data = t.Translate(Execute(cmd).ToDataTable());
            //Assert
            Assert.Equal(5, data.Count);
            Assert.Equal(TEST_DATA, data[0]);
            var streamed = Execute(BasicSelectCommand()).ToSingleNamedColumnList<string>(UNIT_TEST_COL_NAME);
            Assert.Equal(TEST_DATA, streamed[0]);
        }

        
        [Fact]
        public void SelectDataBinaryWithFilter()
        {
            //setup
            const string testvalue = TEST_DATA;
            var t = new StringTranslator(UNIT_TEST_COL_NAME);
            var cmd = BasicSelectCommand($"WHERE {UNIT_TEST_COL_NAME:[]} = {testvalue:@testValue}");

            //Act
            var data = t.Translate(Execute(cmd).ToDataTable());

            //Asert
            Assert.Single(data);
        }
    }
}




