using System;
using CA.Blocks.DataAccess;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Providers;
using CA.Blocks.DataAccess.Translator.Extensions;
using CA.Blocks.SQLServerDataAccess;
using CA.Blocks.SQLServerDataAccessUnitTests.Base;
using NUnit.Framework;

namespace CA.Blocks.SQLServerDataAccessUnitTests.Translator.DbTypeTests;

[TestFixture]
public class DbTypeDateOnlyTests : UnitTestDataAccess
{
    private DateOnly _testDate;

    private class DateOnlyDataType
    {
        public DateOnly Col { get; set; }
    }

    private void InsertTestDataSQL(DateOnly data)
    {
        ExecuteNonQuery(InsertTestDataSQL(string.Format("'{0}'", data.ToString("yyyy MMMM dd"))));
    }

    [SetUp]
    public void Setup()
    {
        ExecuteNonQuery(DropTestTableSQL());
        ExecuteNonQuery(CreateTestTable("Date not null"));

        var dt = DateTime.Now;
        _testDate = new DateOnly(dt.Year, dt.Month, dt.Day);
        InsertTestDataSQL(_testDate);
        InsertTestDataSQL(_testDate.AddDays(1));
        InsertTestDataSQL(_testDate.AddDays(-1));
        InsertTestDataSQL(_testDate.AddDays(100));
        InsertTestDataSQL(_testDate.AddDays(-100));

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
        var data = ExecuteToListOf<DateOnly>(cmd);
        //Assert
        Assert.AreEqual(5, data.Count);
    }


    [Test]
    public void SelectAllDataDateOnlyWithFilter()
    {
        //setup
        var cmd = CreateTextCommand(SelectTestDataSQL(), "Where col > @testValue");
        cmd.Parameters.Add(_testDate.ToSqlParameter("@testValue"));

        //Act
        var data = Execute(cmd).ToListOf<DateOnlyDataType>();

        //Assert
        Assert.AreEqual(2, data.Count);
    }


    [Test]
    public void SelectAllDataWithWithTranslator()
    {
        //setup
        var testValue = _testDate;
        var cmd = CreateTextCommand(SelectTestDataSQL(), "Where col = @value").WithParameter(testValue.ToSqlParameter("@value"));
        var t = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<DateOnlyDataType>();
        //Act
        var data = t.Translate(ExecuteDataRow(cmd));

        Assert.AreEqual(testValue, data.Col);
    }
}