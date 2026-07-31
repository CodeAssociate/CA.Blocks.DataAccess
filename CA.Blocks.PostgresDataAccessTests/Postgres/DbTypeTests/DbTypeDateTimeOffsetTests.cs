using CA.Blocks.DataAccess.Translator.Extensions;
namespace CA.Blocks.PostgresDataAccessTests.Postgres.DbTypeTests
{
}


// PostgreSQL does not have a direct equivalent to SQL Server's DATETIMEOFFSET type.
// TIMESTAMP WITH TIME ZONE(timestamptz) stores an absolute UTC instant and normalizes all input to UTC internally — it does not preserve the original offset.
// If you need to store and retrieve the original offset(like 2026-01-18 15:30:00+13:00), you must store the offset separately.


//public class DbTypeDateTimeOffsetTests : UnitTestDataAccess, IDisposable
//{
//    private DateTimeOffset _testDate;

//    private class DateTimeOffSetDataType
//    {
//        public DateTimeOffset Col { get; set; }
//    }

//    private void InsertTestDataSQL(DateTimeOffset data)
//    {
//        ExecuteNonQuery(InsertTestDataSQL(string.Format("'{0}'", data.ToString("yyyy MMMM dd HH:mm:sszzz"))));
//    }

//    public DbTypeDateTimeOffsetTests()
//    {
//        ExecuteNonQuery(DropTestTableSQL());
//        ExecuteNonQuery(CreateTestTable("TIMESTAMP WITH TIME ZONE not null"));

//        var dt = DateTimeOffset.Now;
//        _testDate = new DateTimeOffset(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, dt.Offset);
//        InsertTestDataSQL(_testDate);
//        InsertTestDataSQL(_testDate.AddDays(1).Date);
//        InsertTestDataSQL(_testDate.AddDays(-1).Date);
//        InsertTestDataSQL(_testDate.AddDays(100).Date);
//        InsertTestDataSQL(_testDate.AddDays(-100).Date);

//    }

//    public new void Dispose()
//    {
//        ExecuteNonQuery(DropTestTableSQL());
//        base.Dispose();
//    }



//    [Fact]
//    public void SelectAllDataToListOf()
//    {
//        //Setup

//        var cmd = CreateTextCommand(SelectTestDataSQL());
//        //Act
//        var data = ExecuteToListOf<DateTimeOffSetDataType>(cmd);
//        //Assert
//        Assert.Equal(5, data.Count);
//    }


//    [Fact]
//    public void SelectAllDataDateTimeOffsetWithFilter()
//    {
//        //setup
//        var t = new DateTimeOffsetTranslator(UNIT_TEST_COL_NAME);
//        var cmd = CreateTextCommand(SelectTestDataSQL("Where col > @testValue"));
//        cmd.Parameters.Add(_testDate.ToPostgresParameter("@testValue"));

//        //Act
//        var data = t.Translate(Execute(cmd).ToDataTable());

//        //Assert
//        Assert.Equal(2, data.Count);
//    }


//    [Fact]
//    public void SelectAllDataWithWithTranslator()
//    {
//        //setup
//        var testValue = _testDate;
//        var cmd = CreateTextCommand(SelectTestDataSQL("Where col = @value")).WithParameter(testValue.ToPostgresParameter("@value"));
//        var t = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<DateTimeOffSetDataType>();
//        //Act
//        var data = t.Translate(Execute(cmd).ToDataRow());

//        Assert.Equal(testValue, data.Col);
//    }
//}
