using System;
using CA.Blocks.DataAccess.Translator.Basic;
using CA.Blocks.DataAccess.Translator.Extensions;
using CA.Blocks.SqliteDataAccessUnitTests.Base;
using NUnit.Framework;
using NUnit.Framework.Legacy;

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
            var cmd = CreateTextCommand(SelectTestDataSQL());
            //Act
            var data = Execute(cmd).ToSingleNamedColumnList<DateTime>(UNIT_TEST_COL_NAME);
            //Assert
            TestContext.Write(DataTableToText(ExecuteDataTable(cmd)));
            ClassicAssert.AreEqual(5, data.Count);
        }

        [Test]
        public void SelectAllDataDateTimeWithFilter()
        {
            //setup
            DateTime testvalue = DateTime.Now;
            //var cmd = CreateTextCommand(SelectTestDataSQL(), "Where col >= @testValue");
            var cmd = CreateTextCommand(SelectTestDataSQL(), $"Where col >= datetime('{testvalue:o}')");
            //cmd.Parameters.Add(testvalue.ToSqlParameter("@testValue"));

            //Act
            var data = Execute(cmd).ToSingleNamedColumnList<DateTime>(UNIT_TEST_COL_NAME);

            //Asert
            ClassicAssert.AreEqual(3, data.Count);
        }


    }
}
