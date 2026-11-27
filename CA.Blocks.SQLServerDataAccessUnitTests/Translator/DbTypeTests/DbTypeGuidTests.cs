using CA.Blocks.DataAccess.Translator.Extensions;
using CA.Blocks.DataAccess.Translator.Basic;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Providers;
using CA.Blocks.SQLServerDataAccess;
using CA.Blocks.SQLServerDataAccessUnitTests.Base;
using Microsoft.AspNetCore.Components;

namespace CA.Blocks.SQLServerDataAccessUnitTests.Translator.DbTypeTests
{
    [Collection("DbIntegrationTests")]
    public class DbTypeGuidTests : UnitTestDataAccess, IDisposable
    {
        private const string TestGuidValue = "CE69B300-F9EA-4F3B-BBA8-676D12737E3E";
        private class GuidDataType
        {
            public Guid Col { get; set; }
        }


        private void InsertTestDataSQL(Guid data)
        {
            ExecuteNonQuery(InsertTestDataCommand(data));
        }

        public DbTypeGuidTests()
        {
            ExecuteNonQuery(DropTestTableCommand());
            ExecuteNonQuery(CreateTestTableCommand("UniqueIdentifier"));
            InsertTestDataSQL(Guid.Empty);
            InsertTestDataSQL(Guid.NewGuid());
            InsertTestDataSQL(Guid.NewGuid());
            InsertTestDataSQL(Guid.NewGuid());
            InsertTestDataSQL(Guid.Parse(TestGuidValue));
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
            var data = Execute(cmd).ToSingleNamedColumnList<Guid>(UNIT_TEST_COL_NAME);
            //Assert
            Assert.Equal(5, data.Count);
        }



        [Fact]
        public void SelectAllDataToListOf()
        {
            //Setup 

            var cmd = BasicSelectCommand();
            //Act
            var data = Execute(cmd).ToListOf<GuidDataType>();
            //Assert
            Assert.Equal(5, data.Count);
            Assert.Equal(Guid.Parse(TestGuidValue), data[4].Col);
        }

        [Fact]
        public void SelectAllDataTimeWithFilter()
        {
            //setup
            Guid testvalue = Guid.Parse(TestGuidValue);
            var cmd = BasicSelectCommand($"WHERE {UNIT_TEST_COL_NAME:[]} = {testvalue:@testValue}");

    
            //Act
            var data = Assert.Single(Execute(cmd).ToListOf<GuidDataType>());

            //Asert
            Assert.Equal(testvalue, data.Col);
        }

        [Fact]
        public void SelectAllDataWithWithTranslator()
        {
            //setup
            Guid testValue = Guid.Parse(TestGuidValue);
            var cmd = BasicSelectCommand($"WHERE {UNIT_TEST_COL_NAME:[]} = {testValue:@value}");
            var t = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<GuidDataType>();
            //Act
            var data = t.Translate(Execute(cmd).ToDataRow());
            Assert.NotNull(data);
            Assert.Equal(testValue, data.Col);
        }
    }
}




