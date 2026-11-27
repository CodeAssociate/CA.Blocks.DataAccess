using CA.Blocks.DataAccess.Translator.DbRowToObject.Providers;
using CA.Blocks.DataAccess.Translator.Extensions;
using CA.Blocks.SQLServerDataAccess;
using CA.Blocks.SQLServerDataAccessUnitTests.Base;

namespace CA.Blocks.SQLServerDataAccessUnitTests.Translator.DbTypeTests
{
    [Collection("DbIntegrationTests")]
    public class DbTypeUShortTests : UnitTestDataAccess, IDisposable
    {

        private class UShortDataType
        {
            public ushort Col { get; set; }
        }

        private void InsertTestDataSQL(ushort data)
        {
            ExecuteNonQuery(InsertTestDataCommand((int)data));
        }

        public DbTypeUShortTests()
        {
            ExecuteNonQuery(DropTestTableCommand());
            ExecuteNonQuery(CreateTestTableCommand("int")); // int is the Closest SQL Datatype Match
            InsertTestDataSQL(0);
            InsertTestDataSQL(0);
            InsertTestDataSQL(123);
            InsertTestDataSQL(246);
            InsertTestDataSQL(ushort.MaxValue);
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
            var data = Execute(cmd).ToSingleNamedColumnList<ushort>(UNIT_TEST_COL_NAME);
            //Assert
            Assert.Equal(5, data.Count);
        }

        [Fact]
        public void SelectAllDataToListOf()
        {
            //Setup 

            var cmd = BasicSelectCommand();
            //Act
            var data = Execute(cmd).ToListOf<UShortDataType>();
            //Assert
            Assert.Equal(5, data.Count);
            Assert.Equal(ushort.MaxValue, data[4].Col);
        }


        [Fact]
        public void SelectAllDataWithFilter ()
        {
            //setup
            const int testvalue = 123;
            var cmd = BasicSelectCommand($"WHERE {UNIT_TEST_COL_NAME:[]} >= {testvalue:@testValue}");

            //Act
            var data = Execute(cmd).ToListOf<UShortDataType>();

            //Asert
            Assert.Equal(3, data.Count);
        }

        [Fact]
        public void SelectAllDataWithFilterWithTranslator ()
        {
            //setup
            const int testvalue = 123; 
            var t = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<UShortDataType>();
            var cmd = BasicSelectCommand($"WHERE {UNIT_TEST_COL_NAME:[]} >= {testvalue:@testValue}");

            //Act
            var data = t.Translate(Execute(cmd).ToDataTable());

            //Asert
            Assert.Equal(3, data.Count);
        }

    }
}




