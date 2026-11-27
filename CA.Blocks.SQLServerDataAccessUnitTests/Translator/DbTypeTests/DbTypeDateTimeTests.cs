using CA.Blocks.DataAccess.Translator.Extensions;
using System;
using CA.Blocks.DataAccess;
using CA.Blocks.DataAccess.Translator.Basic;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Providers;
using CA.Blocks.SQLServerDataAccess;
using CA.Blocks.SQLServerDataAccessUnitTests.Base;

namespace CA.Blocks.SQLServerDataAccessUnitTests.Translator.DbTypeTests
{
    [Collection("DbIntegrationTests")]
    public class DbTypeDateTimeTests : UnitTestDataAccess, IDisposable
    {
        private DateTime _testDate;

        private class DateTimeDataType
        {
            public DateTime Col { get; set; }
        }

        private void InsertTestDataSQL(DateTime data)
        {
            var cmd = InsertTestDataCommand(data);
            cmd.Parameters["@value"].SqlDbType = System.Data.SqlDbType.DateTime;
            ExecuteNonQuery(cmd);
        }

        public DbTypeDateTimeTests()
        {
            ExecuteNonQuery(DropTestTableCommand());
            ExecuteNonQuery(CreateTestTableCommand("DateTime"));

            DateTime dt = DateTime.Now;
            _testDate  = new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second);
            InsertTestDataSQL(_testDate);
            InsertTestDataSQL(_testDate.AddDays(1).Date);
            InsertTestDataSQL(_testDate.AddDays(-1).Date);
            InsertTestDataSQL(_testDate.AddDays(100).Date);
            InsertTestDataSQL(_testDate.AddDays(-100).Date);
        }

        public void Dispose()
        {
            ExecuteNonQuery(DropTestTableCommand());
        }

        [Fact]
        public void SelectAll()
        {
            //Setup 
            var t = new DateTimeTranslator(UNIT_TEST_COL_NAME);
            var cmd = BasicSelectCommand();
            //Act
            var data = t.Translate(Execute(cmd).ToDataTable());
            //Assert
            Assert.Equal(5, data.Count);
        }



        [Fact]
        public void SelectAllDataToListOf()
        {
            //Setup 

            var cmd = BasicSelectCommand();
            //Act
            var data = Execute(cmd).ToListOf<DateTimeDataType>();
            //Assert
            Assert.Equal(5, data.Count);
        }



        [Fact]
        public void SelectAllDataDateTimeWithFilter()
        {
            //setup
            var t = new DateTimeTranslator(UNIT_TEST_COL_NAME);
            var cmd = BasicSelectCommand($"WHERE {UNIT_TEST_COL_NAME:[]} > {_testDate:@testValue|DateTime}");

            //Act
            var data = t.Translate(Execute(cmd).ToDataTable());

            //Asert
            Assert.Equal(2, data.Count);
        }

        [Fact]
        public void SelectAllDataWithWithTranslator()
        {
            //setup
            var testValue = _testDate;
            var cmd = BasicSelectCommand($"WHERE {UNIT_TEST_COL_NAME:[]} = {testValue:@value|DateTime}");
            var t = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<DateTimeDataType>();
            //Act
            var data = t.Translate(Execute(cmd).ToDataRow());
            
            Assert.Equal(testValue, data.Col);
        }

    }
}




