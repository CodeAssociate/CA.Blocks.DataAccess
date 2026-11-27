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
        var cmd = InsertTestDataCommand(time);
        cmd.Parameters["@value"].Value = time.ToTimeSpan();
        ExecuteNonQuery(cmd);
    }

    public DbTypeTimeOnlyTests()
        {
        ExecuteNonQuery(DropTestTableCommand());
        ExecuteNonQuery(CreateTestTableCommand("time"));

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
        ExecuteNonQuery(DropTestTableCommand());
    }



    [Fact]
    public void SelectAllDataToListOf()
    {
        //Setup 

        var cmd = BasicSelectCommand();
        //Act
        var data = Execute(cmd).ToSingleNamedColumnList<TimeOnly>(UNIT_TEST_COL_NAME);
        //Assert
        Assert.Equal(5, data.Count);
        Assert.Equal(5, ExecuteToListOf<TimeOnly>(BasicSelectCommand()).Count);
    }


    [Fact]
    public void SelectAllDataTimeOnlyWithFilter()
    {
        //setup
        var cmd = BasicSelectCommand($"WHERE {UNIT_TEST_COL_NAME:[]} > {_testDate:@testValue}");
        cmd.Parameters["@testValue"].Value = _testDate.ToTimeSpan();

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
        var cmd = BasicSelectCommand($"WHERE {UNIT_TEST_COL_NAME:[]} = {testValue:@value}");
        cmd.Parameters["@value"].Value = testValue.ToTimeSpan();
        var t = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<TimeOnlyDataType>();
        //Act
        var data = t.Translate(Execute(cmd).ToDataRow());

        Assert.Equal(testValue, data.Col);
    }
}



