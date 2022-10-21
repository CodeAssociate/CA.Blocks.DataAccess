using System;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Providers;
using CA.Blocks.DataAccess.Translator.Extensions;
using CA.Blocks.SQLServerDataAccess;
using CA.Blocks.SQLServerDataAccessUnitTests.Base;
using NUnit.Framework;

namespace CA.Blocks.SQLServerDataAccessUnitTests.Translator.DbTypeTests;

[TestFixture]
public class DbTypeTimeOnlyTests : UnitTestDataAccess
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

    [SetUp]
    public void Setup()
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

    [TearDown]
    public void TearDown()
    {
        ExecuteNonQuery(DropTestTableSQL());
    }



    [Test]
    public void SelectAllDataToListOf()
    {
        //Setup 

        var cmd = CreateTextCommand(SelectTestDataSQL());
        //Act
        var data = ExecuteToListOf<TimeOnly>(cmd);
        //Assert
        Assert.AreEqual(5, data.Count);
    }


    [Test]
    public void SelectAllDataTimeOnlyWithFilter()
    {
        //setup
        var cmd = CreateTextCommand(SelectTestDataSQL(), "Where col > @testValue");
        cmd.Parameters.Add(_testDate.ToSqlParameter("@testValue"));

        //Act
        var data = Execute(cmd).ToListOf<TimeOnlyDataType>();

        //Assert
        Assert.AreEqual(2, data.Count);
    }


    [Test]
    public void SelectAllDataWithWithTranslator()
    {
        //setup
        var testValue = _testDate;
        var cmd = CreateTextCommand(SelectTestDataSQL(), "Where col = @value").WithParameter(testValue.ToSqlParameter("@value"));
        var t = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<TimeOnlyDataType>();
        //Act
        var data = t.Translate(ExecuteDataRow(cmd));

        Assert.AreEqual(testValue, data.Col);
    }
}