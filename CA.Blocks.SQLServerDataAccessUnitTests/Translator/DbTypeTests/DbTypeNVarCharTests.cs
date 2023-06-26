using System;
using System.Collections;
using CA.Blocks.DataAccess.Translator.Basic;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Providers;
using CA.Blocks.SQLServerDataAccess;
using CA.Blocks.SQLServerDataAccessUnitTests.Base;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace CA.Blocks.SQLServerDataAccessUnitTests.Translator.DbTypeTests
{
    [TestFixture]
    public class DbTypeNVarCharTests : UnitTestDataAccess
    {
        private class StringDataType
        {
            public string Col { get; set; }
        }

        private const string TEST_DATA = "nvarchar data";
        private const string TEST_UTF8DATA = "语言处理"; //"语言处理" (which means "language processing" in Chinese):

        private void InsertTestDataAsText(string data)
        {
            ExecuteNonQuery(InsertTestDataSQL(string.Format("N'{0}'", data)));
        }

        [SetUp]
        public void Setup()
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

        [TearDown]
        public void TearDown()
        {
            ExecuteNonQuery(DropTestTableSQL());
        }

        [Test]
        public void SelectAllData()
        {
            //Setup 
            var t = new StringTranslator(UNIT_TEST_COL_NAME);
            var cmd = CreateTextCommand(SelectTestDataSQL());
            //Act
            var data = t.Translate(ExecuteDataTable(cmd));
            //Assert
            Assert.AreEqual(6, data.Count);
            Assert.AreEqual(TEST_DATA, data[0]);
            Assert.AreEqual(TEST_UTF8DATA, data[5]);
        }


        [Test]
        public void SelectAllDataToListOf()
        {
            //Setup 

            var cmd = CreateTextCommand(SelectTestDataSQL());
            //Act
            var data = ExecuteToListOf<StringDataType>(cmd);
            //Assert
            Assert.AreEqual(6, data.Count);
            Assert.AreEqual(TEST_DATA, data[0].Col);
            Assert.AreEqual(TEST_UTF8DATA, data[5].Col);
        }

        [Test]
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
            Assert.AreEqual(1, data.Count);
        }


        [Test]
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
            Assert.AreEqual(1, data.Count);
        }
    }
}
