using CA.Blocks.DataAccess;
using CA.Blocks.DataAccess.Translator.Basic;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Providers;
using CA.Blocks.DataAccess.Translator.Extensions;
using CA.Blocks.PostgresDataAccess;
using CA.Blocks.PostgresDataAccessTests.Base;

namespace CA.Blocks.PostgresDataAccessTests.Postgres.DbTypeTests;

[Collection("DbIntegrationTests")]
public class DbTypeDateTests : UnitTestDataAccess, IDisposable
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

    public DbTypeDateTests()
    {
        ExecuteNonQuery(DropTestTableSQL());
        ExecuteNonQuery(CreateTestTable("timestamp not null"));
        _testDate = DateTime.Now.Date;
        InsertTestDataSQL(_testDate);
        InsertTestDataSQL(_testDate.AddDays(1).Date);
        InsertTestDataSQL(_testDate.AddDays(-1).Date);
        InsertTestDataSQL(_testDate.AddDays(100).Date);
        InsertTestDataSQL(_testDate.AddDays(-100).Date);
    }

    public new void Dispose()
    {
        ExecuteNonQuery(DropTestTableSQL());
        base.Dispose();
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
        Assert.Equal(_testDate, data[0].Col);
    }

    [Fact]
    public void SelectAllDataDateTimeWithFilter()
    {
        //setup
        var t = new DateTimeTranslator(UNIT_TEST_COL_NAME);
        var cmd = CreateTextCommand(SelectTestDataSQL("Where col = @testValue"));
        cmd.Parameters.Add(_testDate.ToPostgresParameter("@testValue"));

        //Act
        var data = t.Translate(Execute(cmd).ToDataTable());

        //Asert
        Assert.Single(data);
    }

    [Fact]
    public void SelectAllDataWithWithTranslator()
    {
        //setup
        var testValue = _testDate;
        var cmd = CreateTextCommand(SelectTestDataSQL("Where col = @value")).WithParameter(testValue.ToPostgresParameter("@value"));
        var t = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<DateTimeDataType>();
        //Act
        var data = t.Translate(Execute(cmd).ToDataRow());

        Assert.Equal(testValue, data.Col);
    }
}
