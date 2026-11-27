using CA.Blocks.DataAccess.Translator.Extensions;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Providers;
using CA.Blocks.SQLServerDataAccessUnitTests.Base;

namespace CA.Blocks.SQLServerDataAccessUnitTests.Translator.DbTypeTests
{
    [Collection("DbIntegrationTests")]
    public class DbTypeBigIntTests : UnitTestDataAccess, IDisposable
    {
        private class BigIntDataType
        {
            public long Col { get; set; }
        }

        private void InsertTestData(long value)
        {
            ExecuteNonQuery(InsertTestDataCommand(value));
        }

        public DbTypeBigIntTests()
        {
            
            ExecuteNonQuery(DropTestTableCommand());
            ExecuteNonQuery(CreateTestTableCommand("bigint"));
            InsertTestData(-1);
            InsertTestData(0);
            InsertTestData(123);
            InsertTestData(246);
            InsertTestData((long)int.MaxValue + (long)int.MaxValue);
        }

        public void Dispose()
        {
            ExecuteNonQuery(DropTestTableCommand());
        }

        [Fact]
        public void SelectAllData()
        {
            //Setup 
            var cmd = BasicSelectCommand();
            //Act
            var data = Execute(cmd).ToSingleNamedColumnList<long>(UNIT_TEST_COL_NAME);
            //Assert
            Assert.Equal(5, data.Count);
        }

        [Fact]
        public void SelectAllDataToListOf()
        {
            //Setup 
            var cmd = BasicSelectCommand();
            //Act
            var data = Execute(cmd).ToListOf<BigIntDataType>();
            //Assert
            Assert.Equal(5, data.Count);
            Assert.Equal(-1, data[0].Col);
        }


        [Fact]
        public void SelectAllDataBigIntWithFilter ()
        {
            //setup
            const long testvalue = 123; 
            var cmd = BasicSelectCommand($"WHERE {UNIT_TEST_COL_NAME:[]} >= {testvalue:@testValue}");
            //Act
            var data = Execute(cmd).ToSingleNamedColumnList<long>(UNIT_TEST_COL_NAME);

            //Assert
            Assert.Equal(3, data.Count);
        }

        [Fact]
        public void SelectAllDataWithWithTranslator()
        {
            //setup
            const long testvalue = 123;
            var cmd = BasicSelectCommand($"WHERE {UNIT_TEST_COL_NAME:[]} = {testvalue:@testValue}");
            var t = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<BigIntDataType>();
            //Act
            var data = t.Translate(Execute(cmd).ToDataRow());
            
            Assert.Equal(123, data.Col);
        }
    }
}




