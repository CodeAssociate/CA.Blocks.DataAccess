using System;
using CA.Blocks.DataAccess.Translator.Basic;
using CA.Blocks.SqliteDataAccessUnitTests.Base;
using NUnit.Framework;

namespace CA.Blocks.SqliteDataAccessUnitTests.SQLLite.DbTypeTests
{
    [TestFixture]
    public class DbTypeDateTimeTests : UnitTestDataAccess
    {
        private void InsertTestDataSQL(DateTime data)
        {
            ExecuteNonQuery(InsertTestDataSQL($" datetime('{data:o}') "));
        }

        [SetUp]
        public void Setup()
        {
            ExecuteNonQuery(DropTestTableSQL());
            ExecuteNonQuery(CreateTestTable("DateTime not null"));
            InsertTestDataSQL(DateTime.Now.AddMinutes(1));
            InsertTestDataSQL(DateTime.Now.AddDays(1));
            InsertTestDataSQL(DateTime.Now.AddDays(-1));
            InsertTestDataSQL(DateTime.Now.AddDays(100));
            InsertTestDataSQL(DateTime.Now.AddDays(-100));
        }

        [TearDown]
        public void TearDown()
        {
            ExecuteNonQuery(DropTestTableSQL());
        }

        [Test]
        public void SelectAllDataDateTime()
        {
            //Setup 
            var t = new DateTimeTranslator(UNIT_TEST_COL_NAME);
            var cmd = CreateTextCommand(SelectTestDataSQL());
            //Act
            var data = t.Translate(ExecuteDataTable(cmd));
            //Assert
            TestContext.Write(DataTableToText(ExecuteDataTable(cmd)));
            Assert.AreEqual(5, data.Count);
        }

        [Test]
        public void SelectAllDataDateTimeWithFilter()
        {
            //setup
            DateTime testvalue = DateTime.Now;
            var t = new DateTimeTranslator(UNIT_TEST_COL_NAME);
            //var cmd = CreateTextCommand(SelectTestDataSQL(), "Where col >= @testValue");
            var cmd = CreateTextCommand(SelectTestDataSQL(), $"Where col >= datetime('{testvalue:o}')");
            //cmd.Parameters.Add(testvalue.ToSqlParameter("@testValue"));

            //Act
            var data = t.Translate(ExecuteDataTable(cmd));

            //Asert
            Assert.AreEqual(3, data.Count);
        }


    }
}
