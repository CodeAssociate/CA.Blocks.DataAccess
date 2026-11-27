using CA.Blocks.DataAccess.Translator.Extensions;
using CA.Blocks.DataAccess;
using CA.Blocks.DataAccess.Translator.Basic;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Providers;
using CA.Blocks.SQLServerDataAccess;
using CA.Blocks.SQLServerDataAccessUnitTests.Base;

namespace CA.Blocks.SQLServerDataAccessUnitTests.Translator.DbTypeTests
{
    [Collection("DbIntegrationTests")]
    public class DbTypeIntTests : UnitTestDataAccess, IDisposable
    {

        private class IntDataType
        {
            public int Col { get; set; }
        }


        private void InsertTestDataSQL(int data)
        {
            ExecuteNonQuery(InsertTestDataCommand(data));
        }

        public DbTypeIntTests()
        {
            ExecuteNonQuery(DropTestTableCommand());
            ExecuteNonQuery(CreateTestTableCommand("int"));
            InsertTestDataSQL(-1);
            InsertTestDataSQL(0);
            InsertTestDataSQL(123);
            InsertTestDataSQL(246);
            InsertTestDataSQL(int.MaxValue);
        }

        public void Dispose()
        {
            ExecuteNonQuery(DropTestTableCommand());
        }

        [Fact]
        public void SelectAllData()
        {
            //Setup 
            var t = new IntTranslator(UNIT_TEST_COL_NAME);
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
            var data = Execute(cmd).ToListOf<IntDataType>();
            //Assert
            Assert.Equal(5, data.Count);
            Assert.Equal(-1, data[0].Col);
        }

        [Fact]
        public void SelectAllDataFilter ()
        {
            //setup
            const int testvalue = 123; 
            var t = new IntTranslator(UNIT_TEST_COL_NAME);
            var cmd = BasicSelectCommand($"WHERE {UNIT_TEST_COL_NAME:[]} >= {testvalue:@testValue}");

            //Act
            var data = t.Translate(Execute(cmd).ToDataTable());

            //Asert
            Assert.Equal(3, data.Count);
        }

        [Fact]
        public void SelectAllDataWithWithTranslator()
        {
            //setup
            int testValue = 123;
            var cmd = BasicSelectCommand($"WHERE {UNIT_TEST_COL_NAME:[]} = {testValue:@value}");
            var t = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<IntDataType>();
            //Act
            var data = t.Translate(Execute(cmd).ToDataRow());
            
            Assert.Equal(testValue, data.Col);
        }
    }
}




