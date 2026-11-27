using CA.Blocks.DataAccess.Translator.Extensions;
using CA.Blocks.DataAccess.Translator.Basic;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Providers;
using CA.Blocks.SQLServerDataAccessUnitTests.Base;

namespace CA.Blocks.SQLServerDataAccessUnitTests.Translator.DbTypeTests
{
    [Collection("DbIntegrationTests")]
    public class DbTypeByteTests : UnitTestDataAccess, IDisposable
    {
        private class ByteDataType
        {
            public byte Col { get; set; }
        }

        private void InsertTestDataSQL(byte data)
        {
            ExecuteNonQuery(InsertTestDataCommand(data));
        }

        public DbTypeByteTests()
        {
            ExecuteNonQuery(DropTestTableCommand());
            ExecuteNonQuery(CreateTestTableCommand("tinyint"));
            InsertTestDataSQL(0);
            InsertTestDataSQL(1);
            InsertTestDataSQL(2);
            InsertTestDataSQL(4);
            InsertTestDataSQL(byte.MaxValue);
        }

        public void Dispose()
        {
            ExecuteNonQuery(DropTestTableCommand());
        }

        [Fact]
        public void SelectAllData()
        {
            //Setup 
            var t = new ByteTranslator(UNIT_TEST_COL_NAME);
            var cmd = BasicSelectCommand();
            //Act
            var data = t.Translate(Execute(cmd).ToDataTable());
            //Assert
            Assert.Equal(5, data.Count);
        }

        [Fact]
        public void SelectAllDataToListOf()
        {
            //Setup 

            var cmd = BasicSelectCommand();
            //Act
            var data = Execute(cmd).ToListOf<ByteDataType>();
            //Assert
            Assert.Equal(5, data.Count);
            Assert.Equal(byte.MaxValue, data[4].Col);
        }

        [Fact]
        public void SelectAllDataByteWithFilter ()
        {
            //setup
            const byte testvalue = 123; 
            var t = new ByteTranslator(UNIT_TEST_COL_NAME);
            var cmd = BasicSelectCommand($"WHERE {UNIT_TEST_COL_NAME:[]} >= {testvalue:@testValue}");

            //Act
            var data = t.Translate(Execute(cmd).ToDataTable());

            //Asert
            Assert.Single(data);
        }

        [Fact]
        public void SelectAllDataWithWithTranslator()
        {
            //setup
            const byte testValue = 1;
            var cmd = BasicSelectCommand($"WHERE {UNIT_TEST_COL_NAME:[]} = {testValue:@value}");
            var t = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<ByteDataType>();
            //Act
            var data = t.Translate(Execute(cmd).ToDataRow());
            
            Assert.Equal(testValue, data.Col);
        }

    }
}




