using System;
using CA.Blocks.DataAccess;
using CA.Blocks.DataAccess.Translator.Basic;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Providers;
using CA.Blocks.SQLServerDataAccess;
using CA.Blocks.SQLServerDataAccessUnitTests.Base;

namespace CA.Blocks.SQLServerDataAccessUnitTests.Translator.DbTypeTests
{
    [Collection("DbIntegrationTests")]
    public class DbTypeDateTimeOffsetTests : UnitTestDataAccess, IDisposable
    {
        private DateTimeOffset _testDate;

        private class DateTimeOffSetDataType
        {
            public DateTimeOffset Col { get; set; }
        }

        private void InsertTestDataSQL(DateTimeOffset data)
        {
            ExecuteNonQuery(InsertTestDataSQL(string.Format("'{0}'", data.ToString("yyyy MMMM dd HH:mm:ss zzz"))));
        }

        public DbTypeDateTimeOffsetTests()
        {
            ExecuteNonQuery(DropTestTableSQL());
            ExecuteNonQuery(CreateTestTable("DateTimeOffSet not null"));

            var dt = DateTimeOffset.Now;
            _testDate = new DateTimeOffset(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, dt.Offset);
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
        public void SelectAllDataToListOf()
        {
            //Setup 

            var cmd = CreateTextCommand(SelectTestDataSQL());
            //Act
            var data = ExecuteToListOf<DateTimeOffSetDataType>(cmd);
            //Assert
            Assert.Equal(5, data.Count);
        }


        [Fact]
        public void SelectAllDataDateTimeOffsetWithFilter()
        {
            //setup
            var t = new DateTimeOffsetTranslator(UNIT_TEST_COL_NAME);
            var cmd = CreateTextCommand(SelectTestDataSQL(), "Where col > @testValue");
            cmd.Parameters.Add(_testDate.ToSqlParameter("@testValue"));

            //Act
            var data = t.Translate(ExecuteDataTable(cmd));

            //Assert
            Assert.Equal(2, data.Count);
        }


        [Fact]
        public void SelectAllDataWithWithTranslator()
        {
            //setup
            var testValue = _testDate;
            var cmd = CreateTextCommand(SelectTestDataSQL(), "Where col = @value").WithParameter(testValue.ToSqlParameter("@value"));
            var t = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<DateTimeOffSetDataType>();
            //Act

            var data = t.Translate(ExecuteDataRow(cmd));

            Assert.Equal(testValue, data.Col);
        }
    }
}



