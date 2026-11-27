using CA.Blocks.DataAccess.Translator.Basic;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Providers;
using CA.Blocks.DataAccess.Translator.Extensions;
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
            ExecuteNonQuery(InsertTestDataCommand(data));
        }

        public DbTypeSmallDateTimeTests()
        {
            ExecuteNonQuery(DropTestTableCommand());
            ExecuteNonQuery(CreateTestTableCommand("SmallDateTime"));


            var now = DateTime.Now;
            _testDate = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0);
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
        public void SelectAllDataDateTime()
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
            // SmallDateTime is now to min
            Assert.Equal(_testDate.ToString("yyyy MMMM dd HH:mm"), data[0].Col.ToString("yyyy MMMM dd HH:mm"));
        }


        [Fact]
        public void SelectAllDataDateTimeWithFilter()
        {
            //setup

            var t = new DateTimeTranslator(UNIT_TEST_COL_NAME);
            var cmd = BasicSelectCommand($"WHERE {UNIT_TEST_COL_NAME:[]} > {_testDate:@testValue}");

            //Act
            var data = t.Translate(Execute(cmd).ToDataTable());

            //Asert
            Assert.Equal(2, data.Count);
        }
        
        [Fact]
        public void SelectAllDataWithFilterWithTranslator ()
        {
            //setup
            var t = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<DateTimeDataType>();
            var cmd = BasicSelectCommand($"WHERE {UNIT_TEST_COL_NAME:[]} >= {_testDate:@testValue}");
            //Act
            var data = t.Translate(Execute(cmd).ToDataTable());
            //Asert
            Assert.Equal(3, data.Count);
        }

        
    }
}




