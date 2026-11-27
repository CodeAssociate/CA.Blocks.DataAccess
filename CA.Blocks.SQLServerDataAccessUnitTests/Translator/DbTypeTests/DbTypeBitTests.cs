using CA.Blocks.DataAccess.Translator.Extensions;
using CA.Blocks.DataAccess;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Providers;
using CA.Blocks.SQLServerDataAccess;
using CA.Blocks.SQLServerDataAccessUnitTests.Base;

namespace CA.Blocks.SQLServerDataAccessUnitTests.Translator.DbTypeTests
{
    [Collection("DbIntegrationTests")]
    public class DbTypeBitTests : UnitTestDataAccess, IDisposable
    {
        private class BoolDataType
        {
            public bool Col { get; set; }
        }

        private void InsertTestDataSQL(bool data)
        {
            ExecuteNonQuery(InsertTestDataCommand(data));
        }

        public DbTypeBitTests()
        {
            ExecuteNonQuery(DropTestTableCommand());
            ExecuteNonQuery(CreateTestTableCommand("bit"));
            InsertTestDataSQL(true);
            InsertTestDataSQL(false);
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
            var data = Execute(cmd).ToSingleNamedColumnList<bool>(UNIT_TEST_COL_NAME);
            //Assert
            Assert.Equal(2, data.Count);
        }

        [Fact]
        public void SelectAllDataToListOf()
        {
            //Setup 

            var cmd = BasicSelectCommand();
            //Act
            var data = Execute(cmd).ToListOf<BoolDataType>();
            //Assert
            Assert.Equal(2, data.Count);
            Assert.True(data[0].Col);
        }

        [Fact]
        public void SelectAllDataWithFilter ()
        {
            //setup
            const bool testvalue = true;
            var cmd = BasicSelectCommand($"WHERE {UNIT_TEST_COL_NAME:[]} = {testvalue:@testValue}");

            //Act
            var data = Execute(cmd).ToSingleNamedColumnList<bool>(UNIT_TEST_COL_NAME);

            //Asert
            Assert.Single(data);
        }

        [Fact]
        public void SelectAllDataWithWithTranslator()
        {
            //setup
            const bool testvalue = true;
            var cmd = BasicSelectCommand($"WHERE {UNIT_TEST_COL_NAME:[]} = {testvalue:@value}");
            
            var t = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<BoolDataType>();
            //Act
            var data = t.Translate(Execute(cmd).ToDataRow());
            
            Assert.Equal(testvalue, data.Col);
        }
        
    }
}




