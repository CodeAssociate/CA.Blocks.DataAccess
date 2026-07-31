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
            ExecuteNonQuery(InsertTestDataSQL(string.Format("'{0}'", data)));
        }

        public DbTypeNTextTests()
        {
            ExecuteNonQuery(DropTestTableSQL());
            ExecuteNonQuery(CreateTestTable("NText not null"));
            InsertTestDataAsBinarySQL(TEST_DATA);
            InsertTestDataAsBinarySQL(Guid.NewGuid().ToString());
            InsertTestDataAsBinarySQL(Guid.NewGuid().ToString());
            InsertTestDataAsBinarySQL(Guid.NewGuid().ToString());
            InsertTestDataAsBinarySQL(Guid.NewGuid().ToString());
        }

        public void Dispose()
        {
            ExecuteNonQuery(DropTestTableSQL());
        }

        [Fact]
        public void SelectAllDataText()
        {
            //Setup 
            var t = new StringTranslator(UNIT_TEST_COL_NAME);
            var cmd = CreateTextCommand(SelectTestDataSQL());
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

            var cmd = CreateTextCommand(SelectTestDataSQL());
            //Act
            var data = ExecuteToListOf<StringDataType>(cmd);
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
            var cmd = CreateTextCommand(SelectTestDataSQL(), "Where col like @testValue");
#pragma warning disable CS0618 // Type or member is obsolete
            cmd.Parameters.Add(testvalue.ToSqlParameter("@testValue", SpecificSQLStringType.NText));
#pragma warning restore CS0618 // Type or member is obsolete

            //Act
            var data = t.Translate(Execute(cmd).ToDataTable());

            //Asert
            Assert.Single(data);
        }
    }
}




