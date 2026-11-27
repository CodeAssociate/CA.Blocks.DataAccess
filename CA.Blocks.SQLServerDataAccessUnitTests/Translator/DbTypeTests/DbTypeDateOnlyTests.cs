using System;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Providers;
using CA.Blocks.DataAccess.Translator.Extensions;
using CA.Blocks.SQLServerDataAccess;
using CA.Blocks.SQLServerDataAccessUnitTests.Base;

namespace CA.Blocks.SQLServerDataAccessUnitTests.Translator.DbTypeTests;

    [Collection("DbIntegrationTests")]
public class DbTypeDateOnlyTests : UnitTestDataAccess, IDisposable
{
    private DateOnly _testDate;

    private class DateOnlyDataType
    {
        public DateOnly Col { get; set; }
    }

    private void InsertTestDataSQL(DateOnly data)
    {
        ExecuteNonQuery(InsertTestDataCommand(data));
    }

    public DbTypeDateOnlyTests()
        {
        ExecuteNonQuery(DropTestTableCommand());
        ExecuteNonQuery(CreateTestTableCommand("Date"));

        var dt = DateTime.Now;
        _testDate = new DateOnly(dt.Year, dt.Month, dt.Day);
        InsertTestDataSQL(_testDate);
        InsertTestDataSQL(_testDate.AddDays(1));
        InsertTestDataSQL(_testDate.AddDays(-1));
        InsertTestDataSQL(_testDate.AddDays(100));
        InsertTestDataSQL(_testDate.AddDays(-100));

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
        var data = Execute(cmd).ToListOf<DateOnly>();
        //Assert
        Assert.Equal(5, data.Count);
    }


    [Fact]
    public void SelectAllDataDateOnlyWithFilter()
    {
        //setup
        var cmd = BasicSelectCommand($"WHERE {UNIT_TEST_COL_NAME:[]} > {_testDate:@testValue}");

        //Act
        var data = Execute(cmd).ToListOf<DateOnlyDataType>();

        //Assert
        Assert.Equal(2, data.Count);
    }


    [Fact]
    public void SelectAllDataWithWithTranslator()
    {
        //setup
        var testValue = _testDate;
        var cmd = BasicSelectCommand($"WHERE {UNIT_TEST_COL_NAME:[]} = {testValue:@value}");
        var t = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<DateOnlyDataType>();
        //Act
        var data = t.Translate(Execute(cmd).ToDataRow());

        Assert.Equal(testValue, data.Col);
    }
}



