using CA.Blocks.DataAccess.Translator.Basic;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Providers;
using CA.Blocks.DataAccess.Translator.Extensions;
using CA.Blocks.SQLServerDataAccess;
using CA.Blocks.SQLServerDataAccessUnitTests.Base;

namespace CA.Blocks.SQLServerDataAccessUnitTests.Translator.DbTypeTests
{
    [Collection("DbIntegrationTests")]
    public class DbTypeShortTests : UnitTestDataAccess, IDisposable
    {

        private class ShortDataType
        {
            public short Col { get; set; }
        }

        private void InsertTestDataSQL(short data)
        {
            ExecuteNonQuery(InsertTestDataCommand(data));
        }

        public DbTypeShortTests()
        {
            ExecuteNonQuery(DropTestTableCommand());
            ExecuteNonQuery(CreateTestTableCommand("smallint"));
            InsertTestDataSQL(-1);
            InsertTestDataSQL(0);
            InsertTestDataSQL(123);
            InsertTestDataSQL(246);
            InsertTestDataSQL(short.MaxValue);
        }

        public void Dispose()
        {
            ExecuteNonQuery(DropTestTableCommand());
        }

        [Fact]
        public void SelectAllData()
        {
            //Setup 
            var t = new ShortTranslator(UNIT_TEST_COL_NAME);
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
            var data = Execute(cmd).ToListOf<ShortDataType>();
            //Assert
            Assert.Equal(5, data.Count);
            Assert.Equal(short.MaxValue, data[4].Col);
        }


        [Fact]
        public void SelectAllDataIntWithFilter ()
        {
            //setup
            const short testvalue = 123;
            var t = new ShortTranslator(UNIT_TEST_COL_NAME);
            var cmd = BasicSelectCommand($"WHERE {UNIT_TEST_COL_NAME:[]} >= {testvalue:@testValue}");

            //Act
            var data = t.Translate(Execute(cmd).ToDataTable());

            //Asert
            Assert.Equal(3, data.Count);
        }
        
        [Fact]
        public void SelectAllDataIntWithFilterWithTranslator ()
        {
            //setup
            const short testvalue = 123;
            var t = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<ShortDataType>();
            var cmd = BasicSelectCommand($"WHERE {UNIT_TEST_COL_NAME:[]} >= {testvalue:@testValue}");

            //Act
            var data = t.Translate(Execute(cmd).ToDataTable());

            //Asert
            Assert.Equal(3, data.Count);
        }

    }
}




