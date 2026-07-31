using System;
using CA.Blocks.DataAccess.Translator.Basic;
using CA.Blocks.DataAccess.Translator.Extensions;
using CA.Blocks.MySQLDataAccess;
using CA.Blocks.MySQLDataAccessUnitTests.Base;
using Xunit;

namespace CA.Blocks.MySQLDataAccessUnitTests.MySQL.DbTypeTests
{
[Collection("MySQLDbTypeTests")]
public class DbTypeDateTimeTests : UnitTestDataAccess, IDisposable
    {
        private DateTime _testDate;

        private class DateTimeDataType
        {
            public DateTime Col { get; set; }
        }

        private void InsertTestDataSQL(DateTime data)
        {
            ExecuteNonQuery(InsertTestDataSQL(string.Format("'{0}'",data.ToString("yyyy-MM-dd HH:mm:ss"))));
        }

        public DbTypeDateTimeTests()
        {
            ExecuteNonQuery(DropTestTableSQL());
            ExecuteNonQuery(CreateTestTable("DateTime not null"));

            _testDate = DateTime.Now;
            InsertTestDataSQL(_testDate);
            InsertTestDataSQL(_testDate.AddDays(1));
            InsertTestDataSQL(_testDate.AddDays(-1));
            InsertTestDataSQL(_testDate.AddDays(100));
            InsertTestDataSQL(_testDate.AddDays(-100));
        }

        public void Dispose()
        {
           ExecuteNonQuery(DropTestTableSQL());
        }
        [Fact]
public void SelectAll()
        {
            //Setup 
            var t = new DateTimeTranslator(UNIT_TEST_COL_NAME);
            var cmd = CreateTextCommand(SelectTestDataSQL());
            //Act
            var data = t.Translate(Execute(cmd).ToDataTable());
            //Assert
            Assert.Equal(5, data.Count);
        }
        [Fact]
public void SelectAllDataToListOf()
        {
            //Setup 

            var cmd = CreateTextCommand(SelectTestDataSQL());
            //Act
            var data = ExecuteToListOf<DateTimeDataType>(cmd);
            //Assert
            Assert.Equal(5, data.Count);
        }
        [Fact]
public void SelectAllDataDateTimeWithFilter()
        {
            //setup
            var t = new DateTimeTranslator(UNIT_TEST_COL_NAME);
            var cmd = CreateTextCommand(SelectTestDataSQL(), "Where col > @testValue");
            cmd.Parameters.Add(_testDate.ToSqlParameter("@testValue"));

            //Act
            var data = t.Translate(Execute(cmd).ToDataTable());

            //Asert
            Assert.Equal(2, data.Count);
        }


    }
}



