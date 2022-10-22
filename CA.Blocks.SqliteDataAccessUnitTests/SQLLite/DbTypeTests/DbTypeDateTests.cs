using System;
using CA.Blocks.DataAccess.Translator.Basic;
using CA.Blocks.SqliteDataAccessUnitTests.Base;
using NUnit.Framework;

namespace CA.Blocks.SqliteDataAccessUnitTests.SQLLite.DbTypeTests
{
    [TestFixture]
    public class DbTypeDateTests : UnitTestDataAccess
    {
        // SQLite does not have a storage class set aside for storing dates and/or times.Instead, the built-in Date And Time Functions of SQLite are capable of storing dates and times as TEXT, REAL, or INTEGER values:
        // TEXT as ISO8601 strings ("YYYY-MM-DD HH:MM:SS.SSS").
        private void InsertTestDataSQL(DateTime data)
        {
            ExecuteNonQuery(InsertTestDataSQL($" date('{data:o}') "));
        }

        [SetUp]
        public void Setup()
        {
            ExecuteNonQuery(DropTestTableSQL());
            ExecuteNonQuery(CreateTestTable("Date not null"));
            InsertTestDataSQL(DateTime.Now.Date);
            InsertTestDataSQL(DateTime.Now.AddDays(1).Date);
            InsertTestDataSQL(DateTime.Now.AddDays(-1).Date);
            InsertTestDataSQL(DateTime.Now.AddDays(100).Date);
            InsertTestDataSQL(DateTime.Now.AddDays(-100).Date);
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
            DateTime testvalue = DateTime.Now.Date;
            var t = new DateTimeTranslator(UNIT_TEST_COL_NAME);
            var cmd = CreateTextCommand(SelectTestDataSQL(), $"Where col = date('{testvalue:o}')");
            //cmd.Parameters.Add(testvalue.ToSqlParameter("@testValue"));

            //Act
            var data = t.Translate(ExecuteDataTable(cmd));

            //Asert
            Assert.AreEqual(1, data.Count);
        }


    }
}
