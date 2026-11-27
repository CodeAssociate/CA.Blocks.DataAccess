using CA.Blocks.DataAccess.Translator.Basic;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Providers;
using CA.Blocks.DataAccess.Translator.Extensions;
using CA.Blocks.SQLServerDataAccess;
using CA.Blocks.SQLServerDataAccessUnitTests.Base;

namespace CA.Blocks.SQLServerDataAccessUnitTests.Translator.DbTypeTests
{
    [Collection("DbIntegrationTests")]
    public class DbTypeUIntTests : UnitTestDataAccess, IDisposable
    {

        private class UIntDataType
        {
            public uint Col { get; set; }
        }

        private void InsertTestDataSQL(uint data)
        {
            ExecuteNonQuery(InsertTestDataCommand((long)data));
        }

        public DbTypeUIntTests()
        {
            ExecuteNonQuery(DropTestTableCommand());
            ExecuteNonQuery(CreateTestTableCommand("bigint")); // bigint is the Closest SQL Datatype Match
            InsertTestDataSQL(0);
            InsertTestDataSQL(0);
            InsertTestDataSQL(123);
            InsertTestDataSQL(246);
            InsertTestDataSQL(uint.MaxValue);
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
            var data = Execute(cmd).ToSingleNamedColumnList<uint>(UNIT_TEST_COL_NAME);
            //Assert
            Assert.Equal(5, data.Count);
        }

        [Fact]
        public void SelectAllDataToListOf()
        {
            //Setup 

            var cmd = BasicSelectCommand();
            //Act
            var data = Execute(cmd).ToListOf<UIntDataType>();
            //Assert
            Assert.Equal(5, data.Count);
            Assert.Equal(uint.MaxValue, data[4].Col);
        }


        [Fact]
        public void SelectAllDataWithFilter ()
        {
            //setup
            const long testvalue = 123;
            var cmd = BasicSelectCommand($"WHERE {UNIT_TEST_COL_NAME:[]} >= {testvalue:@testValue}");

            //Act
            var data = Execute(cmd).ToListOf<UIntDataType>();

            //Asert
            Assert.Equal(3, data.Count);
        }
        
        [Fact]
        public void SelectAllDataWithFilterWithTranslator ()
        {
            //setup
            const long testvalue = 123;
            var t = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<UIntDataType>();
            var cmd = BasicSelectCommand($"WHERE {UNIT_TEST_COL_NAME:[]} >= {testvalue:@testValue}");

            //Act
            var data = t.Translate(Execute(cmd).ToDataTable());

            //Asert
            Assert.Equal(3, data.Count);
        }
    }
}




