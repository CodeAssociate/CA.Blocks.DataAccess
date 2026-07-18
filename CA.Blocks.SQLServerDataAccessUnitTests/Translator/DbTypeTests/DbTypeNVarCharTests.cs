using System;
using System.Collections;
using CA.Blocks.DataAccess.Translator.Basic;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Providers;
using CA.Blocks.SQLServerDataAccess;
using CA.Blocks.SQLServerDataAccessUnitTests.Base;

namespace CA.Blocks.SQLServerDataAccessUnitTests.Translator.DbTypeTests
{
    [Collection("DbIntegrationTests")]
    public class DbTypeNVarCharTests : UnitTestDataAccess, IDisposable
    {
        private class StringDataType
        {
            public string Col { get; set; }
        }

        private const string TEST_DATA = "nvarchar data";
        private const string TEST_UTF8DATA = "????"; //"????" (which means "language processing" in Chinese):

        private void InsertTestDataAsText(string data)
        {
            ExecuteNonQuery(InsertTestDataSQL(string.Format("N'{0}'", data)));
        }

        public DbTypeNVarCharTests()
        {
            ExecuteNonQuery(DropTestTableSQL());
            ExecuteNonQuery(CreateTestTable("NVarChar(50) not null"));
            InsertTestDataAsText(TEST_DATA);
            InsertTestDataAsText(Guid.NewGuid().ToString());
            InsertTestDataAsText(Guid.NewGuid().ToString());
            InsertTestDataAsText(Guid.NewGuid().ToString());
            InsertTestDataAsText(Guid.NewGuid().ToString());
            InsertTestDataAsText(TEST_UTF8DATA);
        }

        public new void Dispose()
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
            Assert.Equal(6, data.Count);
            Assert.Equal(TEST_DATA, data[0]);
            Assert.Equal(TEST_UTF8DATA, data[5]);
        }


        [Fact]
        public void SelectAllDataToListOf()
        {
            //Setup 

            var cmd = CreateTextCommand(SelectTestDataSQL());
            //Act
            var data = ExecuteToListOf<StringDataType>(cmd);
            //Assert
            Assert.Equal(6, data.Count);
            Assert.Equal(TEST_DATA, data[0].Col);
            Assert.Equal(TEST_UTF8DATA, data[5].Col);
        }

        [Fact]
        public void SelectDataBinaryWithFilter()
        {
            //setup
            const string testvalue = TEST_DATA;
            var t = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<StringDataType>();
            var cmd = CreateTextCommand(SelectTestDataSQL(), "Where col = @testValue");
            cmd.Parameters.Add(testvalue.ToSqlParameter("@testValue"));

            //Act
            var data = t.Translate(ExecuteDataTable(cmd));

            //Asert
            Assert.Equal(1, data.Count);
        }


        [Fact]
        public void SelectDataBinaryWithUTF8Filter()
        {
            //setup
            const string testvalue = TEST_UTF8DATA;
            var t = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<StringDataType>();
            var cmd = CreateTextCommand(SelectTestDataSQL(), "Where col = @testValue");
            cmd.Parameters.Add(testvalue.ToSqlParameter("@testValue"));

            //Act
            var data = t.Translate(ExecuteDataTable(cmd));

            //Asert
            Assert.Equal(1, data.Count);
        }
    }
}




