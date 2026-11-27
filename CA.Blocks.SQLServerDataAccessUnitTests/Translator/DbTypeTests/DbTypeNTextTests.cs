using System;
using CA.Blocks.DataAccess.Translator.Basic;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Providers;
using CA.Blocks.DataAccess.Translator.Extensions;
using CA.Blocks.SQLServerDataAccess;
using CA.Blocks.SQLServerDataAccessUnitTests.Base;

namespace CA.Blocks.SQLServerDataAccessUnitTests.Translator.DbTypeTests
{
    [Collection("DbIntegrationTests")]
    public class DbTypeNTextTests : UnitTestDataAccess, IDisposable
    {
        private class StringDataType
        {
            public string? Col { get; set; }
        }


        private const string  TEST_DATA = "Ntext data";

        private void InsertTestDataAsBinarySQL(string data)
        {
            ExecuteNonQuery(InsertTestDataCommand(data));
        }

        public DbTypeNTextTests()
        {
            ExecuteNonQuery(DropTestTableCommand());
            ExecuteNonQuery(CreateTestTableCommand("NText"));
            InsertTestDataAsBinarySQL(TEST_DATA);
            InsertTestDataAsBinarySQL(Guid.NewGuid().ToString());
            InsertTestDataAsBinarySQL(Guid.NewGuid().ToString());
            InsertTestDataAsBinarySQL(Guid.NewGuid().ToString());
            InsertTestDataAsBinarySQL(Guid.NewGuid().ToString());
        }

        public void Dispose()
        {
            ExecuteNonQuery(DropTestTableCommand());
        }

        [Fact]
        public void SelectAllDataText()
        {
            //Setup 
            var t = new StringTranslator(UNIT_TEST_COL_NAME);
            var cmd = BasicSelectCommand();
            //Act
            var data = t.Translate(Execute(cmd).ToDataTable());
            //Assert
            Assert.Equal(5, data.Count);
            Assert.Equal(TEST_DATA, data[0]);
        }

        [Fact]
        public void SelectAllDataToListOf()
        {
            //Setup 

            var cmd = BasicSelectCommand();
            //Act
            var data = Execute(cmd).ToListOf<StringDataType>();
            //Assert
            Assert.Equal(5, data.Count);
            Assert.Equal(TEST_DATA, data[0].Col);
        }

        [Fact]
        public void SelectDataTextWithFilter()
        {
            //setup
            const string testvalue = TEST_DATA;
            var t = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<StringDataType>();
#pragma warning disable CS0618 // Type or member is obsolete
            var parameter = testvalue.ToSqlParameter("@testValue", SpecificSQLStringType.NText);
#pragma warning restore CS0618 // Type or member is obsolete
            var cmd = BasicSelectCommand($"WHERE {UNIT_TEST_COL_NAME:[]} LIKE {parameter}");

            //Act
            var data = t.Translate(Execute(cmd).ToDataTable());

            //Asert
            Assert.Single(data);
        }
    }
}




