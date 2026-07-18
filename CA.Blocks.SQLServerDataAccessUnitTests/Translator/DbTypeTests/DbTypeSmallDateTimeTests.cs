using System;
using CA.Blocks.DataAccess.Translator.Basic;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Providers;
using CA.Blocks.SQLServerDataAccess;
using CA.Blocks.SQLServerDataAccessUnitTests.Base;

namespace CA.Blocks.SQLServerDataAccessUnitTests.Translator.DbTypeTests
{
    [Collection("DbIntegrationTests")]
    public class DbTypeSmallDateTimeTests : UnitTestDataAccess, IDisposable
    {
        private DateTime _testDate;

        private class DateTimeDataType
        {
            public DateTime Col { get; set; }
        }


        private void InsertTestDataSQL(DateTime data)
        {
            ExecuteNonQuery(InsertTestDataSQL(string.Format("'{0}'",data.ToString("yyyy MMMM dd HH:mm"))));
        }

        public DbTypeSmallDateTimeTests()
        {
            ExecuteNonQuery(DropTestTableSQL());
            ExecuteNonQuery(CreateTestTable("SmallDateTime not null"));


            _testDate = DateTime.Now;
            InsertTestDataSQL(_testDate);
            InsertTestDataSQL(_testDate.AddDays(1).Date);
            InsertTestDataSQL(_testDate.AddDays(-1).Date);
            InsertTestDataSQL(_testDate.AddDays(100).Date);
            InsertTestDataSQL(_testDate.AddDays(-100).Date);

        }

        public new void Dispose()
        {
            ExecuteNonQuery(DropTestTableSQL());
        }

        [Fact]
        public void SelectAllDataDateTime()
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
            // SmallDateTime is now to min
            Assert.Equal(_testDate.ToString("yyyy MMMM dd HH:mm"), data[0].Col.ToString("yyyy MMMM dd HH:mm"));
        }


        [Fact]
        public void SelectAllDataDateTimeWithFilter()
        {
            //setup

            var t = new DateTimeTranslator(UNIT_TEST_COL_NAME);
            var cmd = CreateTextCommand(SelectTestDataSQL(), "Where col >  @testValue");
            cmd.Parameters.Add(_testDate.ToSqlParameter("@testValue"));

            //Act
            var data = t.Translate(ExecuteDataTable(cmd));

            //Asert
            Assert.Equal(2, data.Count);
        }
        
        [Fact]
        public void SelectAllDataWithFilterWithTranslator ()
        {
            //setup
            var t = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<DateTimeDataType>();
            var cmd = CreateTextCommand(SelectTestDataSQL(), "Where col >= @testValue");
            cmd.Parameters.Add(_testDate.ToSqlParameter("@testValue"));
            //Act
            var data = t.Translate(ExecuteDataTable(cmd));
            //Asert
            Assert.Equal(2, data.Count);
        }

        
    }
}




