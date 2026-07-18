using System;
using CA.Blocks.DataAccess;
using CA.Blocks.DataAccess.Translator.Basic;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Providers;
using CA.Blocks.SQLServerDataAccess;
using CA.Blocks.SQLServerDataAccessUnitTests.Base;

namespace CA.Blocks.SQLServerDataAccessUnitTests.Translator.DbTypeTests
{
    [Collection("DbIntegrationTests")]
    public class DbTypeDateTime2Tests : UnitTestDataAccess, IDisposable
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

        public DbTypeDateTime2Tests()
        {
            ExecuteNonQuery(DropTestTableSQL());
            ExecuteNonQuery(CreateTestTable("DateTime2 not null"));

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
            ExecuteNonQuery(DropTestTableSQL());
        }

        [Fact]
        public void SelectAll()
        {
            //Setup 
            var t = new DateTimeTranslator(UNIT_TEST_COL_NAME);
            var cmd = CreateTextCommand(SelectTestDataSQL());
            //Act
            var data = t.Translate(ExecuteDataTable(cmd));
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
            var data = t.Translate(ExecuteDataTable(cmd));

            //Asert
            Assert.Equal(2, data.Count);
        }


        [Fact]
        public void SelectAllDataWithWithTranslator()
        {
            //setup
            var testValue = _testDate;
            var cmd = CreateTextCommand(SelectTestDataSQL(), "Where col = @value").WithParameter(testValue.ToSqlParameter("@value"));
            var t = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<DateTimeDataType>();
            //Act
            var data = t.Translate(ExecuteDataRow(cmd));
            
            Assert.Equal(testValue, data.Col);
        }
    }
}




