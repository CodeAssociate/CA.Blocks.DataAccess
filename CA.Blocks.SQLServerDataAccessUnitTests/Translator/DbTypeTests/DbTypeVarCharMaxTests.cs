using CA.Blocks.DataAccess.Translator.Basic;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Providers;
using CA.Blocks.DataAccess.Translator.Extensions;
using CA.Blocks.SQLServerDataAccess;
using CA.Blocks.SQLServerDataAccessUnitTests.Base;

namespace CA.Blocks.SQLServerDataAccessUnitTests.Translator.DbTypeTests
{
    [Collection("DbIntegrationTests")]
    public class DbTypeVarCharMaxTests : UnitTestDataAccess, IDisposable
    {
        // https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/configuring-parameters-and-parameter-data-types

        // This implicit conversion will fail if the string is larger than the maximum size of an NVarChar which is 4000 we testing above and below
        private string testDataValueForMax = string.Empty;
        private string testDataValueShort = string.Empty;
        private class StringDataType
        {
            public string? Col { get; set; }
        }



        private void InsertTestDataAsText(string data)
        {
            ExecuteNonQuery(InsertTestDataCommand(data));
        }

        public DbTypeVarCharMaxTests()
        {
            testDataValueForMax = string.Concat(Enumerable.Repeat("0123456789", 1000)); // Create a string 10000 char long
            testDataValueShort = string.Concat(Enumerable.Repeat("0123456789", 600)); // Create a string 6000 char long
            ExecuteNonQuery(DropTestTableCommand());
            var create = CreateTestTableCommand("varchar");
            create.CommandText = create.CommandText.Replace("[varchar]", "[varchar](max)");
            ExecuteNonQuery(create);
            InsertTestDataAsText(testDataValueForMax);
            InsertTestDataAsText(testDataValueShort);
        }

        public void Dispose()
        {
            ExecuteNonQuery(DropTestTableCommand());
        }

        [Fact]
        public void SelectAllData()
        {
            //Setup 
            var t = new StringTranslator(UNIT_TEST_COL_NAME);
            var cmd = BasicSelectCommand();
            //Act
            var data = t.Translate(Execute(cmd).ToDataTable());
            //Assert
            Assert.Equal(2, data.Count);
            Assert.Equal(testDataValueForMax, data[0]);
            Assert.Equal(testDataValueShort, data[1]);
        }

        [Fact]
        public void SelectAllDataToListOf()
        {
            //Setup 
            var cmd = BasicSelectCommand();
            //Act
            var data = Execute(cmd).ToListOf<StringDataType>();
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
            var cmd = BasicSelectCommand($"WHERE {UNIT_TEST_COL_NAME:[]} = {testDataValueForMax:@testValue}");
            cmd.Parameters.RemoveAt("@testValue");
            cmd.Parameters.Add(testDataValueForMax.ToSqlParameter("@testValue", SpecificSQLStringType.VarChar));

            //Act
            var data = t.Translate(Execute(cmd).ToDataTable());

            //Asert
            Assert.Single(data);
        }

        [Fact]
        public void SelectDataWithLargeFilterNoType()
        {
            //setup
            var t = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<StringDataType>();
            var cmd = BasicSelectCommand($"WHERE {UNIT_TEST_COL_NAME:[]} = {testDataValueForMax:@testValue}");
            cmd.Parameters.RemoveAt("@testValue");
            cmd.Parameters.Add(testDataValueForMax.ToSqlParameter("@testValue"));

            //Act
            var data = t.Translate(Execute(cmd).ToDataTable());

            //Asert
            Assert.Single(data);
        }


        [Fact]
        public void SelectDataWithLargeSmallFilter()
        {
            //setup
            var t = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<StringDataType>();
            var cmd = BasicSelectCommand($"WHERE {UNIT_TEST_COL_NAME:[]} = {testDataValueShort:@testValue}");
            cmd.Parameters.RemoveAt("@testValue");
            cmd.Parameters.Add(testDataValueShort.ToSqlParameter("@testValue", SpecificSQLStringType.VarChar));
            //Act
            var data = t.Translate(Execute(cmd).ToDataTable());

            //Asert
            Assert.Single(data);
        }

        [Fact]
        public void SelectDataWithLargeSmallFilter_NoType()
        {
            //setup
            var t = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<StringDataType>();
            var cmd = BasicSelectCommand($"WHERE {UNIT_TEST_COL_NAME:[]} = {testDataValueShort:@testValue}");
            cmd.Parameters.RemoveAt("@testValue");
            cmd.Parameters.Add(testDataValueShort.ToSqlParameter("@testValue"));
            //Act
            var data = t.Translate(Execute(cmd).ToDataTable());

            //Asert
            Assert.Single(data);
        }

    }
}



