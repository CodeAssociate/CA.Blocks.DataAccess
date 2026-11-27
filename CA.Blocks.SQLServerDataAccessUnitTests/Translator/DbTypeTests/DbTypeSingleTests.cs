using System;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Providers;
using CA.Blocks.DataAccess.Translator.Extensions;
using CA.Blocks.SQLServerDataAccess;
using CA.Blocks.SQLServerDataAccessUnitTests.Base;

namespace CA.Blocks.SQLServerDataAccessUnitTests.Translator.DbTypeTests
{
    [Collection("DbIntegrationTests")]
    public class DbTypeSingleTests : UnitTestDataAccess, IDisposable
    {

        private class SingleDataType
        {
            public Single Col { get; set; }
        }


        private void InsertTestDataSQL(float data)
        {
            ExecuteNonQuery(InsertTestDataCommand(data));
        }

        public DbTypeSingleTests()
        {
            ExecuteNonQuery(DropTestTableCommand());
            ExecuteNonQuery(CreateTestTableCommand("real"));
            InsertTestDataSQL((float)-1.2);
            InsertTestDataSQL(0);
            InsertTestDataSQL((float)123.456);
            InsertTestDataSQL(int.MaxValue);
            InsertTestDataSQL((float)123456789.987654321);
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
            var data = Execute(cmd).ToSingleNamedColumnList<float>(UNIT_TEST_COL_NAME);
            //Assert
            Assert.Equal(5, data.Count);
        }


        [Fact]
        public void SelectAllDataToListOf()
        {
            //Setup 
            var cmd = BasicSelectCommand();
            //Act
            var data = Execute(cmd).ToListOf<SingleDataType>();
            //Assert
            Assert.Equal(5, data.Count);
            Assert.Equal(-(float)1.2, data[0].Col);
        }

        [Fact]
        public void SelectAllDataFilter ()
        {
            //setup
            const float testvalue = 123;
            var cmd = BasicSelectCommand($"WHERE {UNIT_TEST_COL_NAME:[]} > {testvalue:@testValue}");

            //Act
            var data = Execute(cmd).ToListOf<SingleDataType>();

            //Asert
            Assert.Equal(3, data.Count);
        }
        
        [Fact]
        public void SelectAllDataIntWithFilterWithTranslator ()
        {
            //setup
            const float testvalue = 123;
            var t = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<SingleDataType>();
            var cmd = BasicSelectCommand($"WHERE {UNIT_TEST_COL_NAME:[]} >= {testvalue:@testValue}");

            //Act
            var data = t.Translate(Execute(cmd).ToDataTable());

            //Asert
            Assert.Equal(3, data.Count);
        }
    }
}




