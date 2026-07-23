using System.Linq;
using CA.Blocks.DataAccess.Translator.Basic;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Providers;
using CA.Blocks.SQLServerDataAccess;
using CA.Blocks.SQLServerDataAccessUnitTests.Base;

namespace CA.Blocks.SQLServerDataAccessUnitTests.Translator.DbTypeTests
{
    [Collection("DbIntegrationTests")]
    public class DbTypeNVarCharMaxTests : UnitTestDataAccess, IDisposable
    {
        // https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/configuring-parameters-and-parameter-data-types

        // This implicit conversion will fail if the string is larger than the maximum size of an NVarChar which is 4000 we testing above and below
        
        private string testDataValueForMax = string.Empty;
        private string testDataValueShort = string.Empty; 
        private class StringDataType
        {
            public string Col { get; set; }
        }


        private void InsertTestDataAsText(string data)
        {
            ExecuteNonQuery(InsertTestDataSQL(string.Format("N'{0}'", data)));
        }

        public DbTypeNVarCharMaxTests()
        {
            testDataValueForMax = string.Concat(Enumerable.Repeat("0123456789?", 500)); // Create a string 5500 char long
            testDataValueShort = string.Concat(Enumerable.Repeat("0123456789?", 300)); // Create a string 3300 char long
            ExecuteNonQuery(DropTestTableSQL());
            ExecuteNonQuery(CreateTestTable("NVarChar(Max) not null"));
            InsertTestDataAsText(testDataValueForMax);
            InsertTestDataAsText(testDataValueShort);
        }

        public void Dispose()
        {
            ExecuteNonQuery(DropTestTableSQL());
        }

        [Fact]
        public void SelectAllData()
        {
            //Setup 
            var t = new StringTranslator(UNIT_TEST_COL_NAME);
            var cmd = CreateTextCommand(SelectTestDataSQL());
            //Act
            var data = t.Translate(ExecuteDataTable(cmd));
            //Assert
            Assert.Equal(2, data.Count);
            Assert.Equal(testDataValueForMax, data[0] );
            Assert.Equal(testDataValueShort, data[1]);

        }


        [Fact]
        public void SelectAllDataToListOf()
        {
            //Setup 

            var cmd = CreateTextCommand(SelectTestDataSQL());
            //Act
            var data = ExecuteToListOf<StringDataType>(cmd);
            //Assert
            Assert.Equal(2, data.Count);
            Assert.Equal(testDataValueForMax, data[0].Col);
            Assert.Equal(testDataValueShort, data[1].Col);
        }

        [Fact]
        public void SelectDataWithLargeFilter()
        {
            //setup
            var t = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<StringDataType>();
            var cmd = CreateTextCommand(SelectTestDataSQL(), "Where col = @testValue");
            cmd.Parameters.Add(testDataValueForMax.ToSqlParameter("@testValue"));

            //Act
            var data = t.Translate(ExecuteDataTable(cmd));

            //Asert
            Assert.Single(data);
        }

        [Fact]
        public void SelectDataWithLargeSmallFilter()
        {
            //setup
            var t = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<StringDataType>();
            var cmd = CreateTextCommand(SelectTestDataSQL(), "Where col = @testValue");
            cmd.Parameters.Add(testDataValueShort.ToSqlParameter("@testValue"));
            //Act
            var data = t.Translate(ExecuteDataTable(cmd));

            //Asert
            Assert.Single(data);
        }

    }
}



