using System;
using CA.Blocks.DataAccess.Translator.Basic;
using CA.Blocks.SQLServerDataAccess;
using CA.Blocks.SQLServerDataAccessUnitTests.Base;
using NUnit.Framework;

namespace CA.Blocks.SQLServerDataAccessUnitTests.SQLServer.DbTypeTests
{
    [TestFixture]
    public class DbTypeDateTime2Tests : UnitTestDataAccess
    {
        private DateTime _testDate;

        private class DateTimeDataType
        {
            public DateTime Col { get; set; }
        }

        private void InsertTestDataSQL(DateTime data)
        {
            ExecuteNonQuery(InsertTestDataSQL(string.Format("'{0}'",data.ToString("yyyy MMMM dd HH:mm:ss"))));
        }

        [SetUp]
        public void Setup()
        {
            ExecuteNonQuery(DropTestTableSQL());
            ExecuteNonQuery(CreateTestTable("DateTime2 not null"));


            _testDate = DateTime.Now;
            InsertTestDataSQL(_testDate);
            InsertTestDataSQL(_testDate.AddDays(1).Date);
            InsertTestDataSQL(_testDate.AddDays(-1).Date);
            InsertTestDataSQL(_testDate.AddDays(100).Date);
            InsertTestDataSQL(_testDate.AddDays(-100).Date);

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
            Assert.AreEqual(5, data.Count);
        }


        [Test]
        public void SelectAllDataToListOf()
        {
            //Setup 

            var cmd = CreateTextCommand(SelectTestDataSQL());
            //Act
            var data = ExecuteToListOf<DateTimeDataType>(cmd);
            //Assert
            Assert.AreEqual(5, data.Count);
        }


        [Test]
        public void SelectAllDataDateTimeWithFilter()
        {
            //setup
            var t = new DateTimeTranslator(UNIT_TEST_COL_NAME);
            var cmd = CreateTextCommand(SelectTestDataSQL(), "Where col > @testValue");
            cmd.Parameters.Add(_testDate.ToSqlParameter("@testValue"));

            //Act
            var data = t.Translate(ExecuteDataTable(cmd));

            //Asert
            Assert.AreEqual(2, data.Count);
        }


    }
}
