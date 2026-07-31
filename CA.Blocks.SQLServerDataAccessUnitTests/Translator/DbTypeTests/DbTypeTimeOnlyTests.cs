using System;
using CA.Blocks.DataAccess;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Providers;
using CA.Blocks.DataAccess.Translator.Extensions;
using CA.Blocks.SQLServerDataAccess;
using CA.Blocks.SQLServerDataAccessUnitTests.Base;

namespace CA.Blocks.SQLServerDataAccessUnitTests.Translator.DbTypeTests;

    [Collection("DbIntegrationTests")]
public class DbTypeTimeOnlyTests : UnitTestDataAccess, IDisposable
{
    private TimeOnly _testDate;

    private class TimeOnlyDataType
    {
        public TimeOnly Col { get; set; }
    }

    private void InsertTestDataSQL(TimeOnly time)
    {
        ExecuteNonQuery(InsertTestDataSQL($"'{time.Hour}:{time.Minute}'"));
    }

    public DbTypeTimeOnlyTests()
        {
        ExecuteNonQuery(DropTestTableSQL());
        ExecuteNonQuery(CreateTestTable("time not null"));

        var dt = DateTime.Now;
        _testDate = new TimeOnly(8, 00);
        InsertTestDataSQL(_testDate);
        InsertTestDataSQL(_testDate.AddHours(1));
        InsertTestDataSQL(_testDate.AddHours(-1));
        InsertTestDataSQL(_testDate.AddHours(10));
        InsertTestDataSQL(_testDate.AddMinutes(-300));

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
        var data = ExecuteToListOf<TimeOnly>(cmd);
        //Assert
        Assert.Equal(5, data.Count);
    }


    [Fact]
    public void SelectAllDataTimeOnlyWithFilter()
    {
        //setup
        var cmd = CreateTextCommand(SelectTestDataSQL(), "Where col > @testValue");
        cmd.Parameters.Add(_testDate.ToSqlParameter("@testValue"));

        //Act
        var data = Execute(cmd).ToListOf<TimeOnlyDataType>();

        //Assert
        Assert.Equal(2, data.Count);
    }


    [Fact]
    public void SelectAllDataWithWithTranslator()
    {
        //setup
        var testValue = _testDate;
        var cmd = CreateTextCommand(SelectTestDataSQL(), "Where col = @value").WithParameter(testValue.ToSqlParameter("@value"));
        var t = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<TimeOnlyDataType>();
        //Act
        var data = t.Translate(Execute(cmd).ToDataRow());

        Assert.Equal(testValue, data.Col);
    }
}



