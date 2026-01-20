using CA.Blocks.DataAccess.Translator.DbRowToObject.Providers;
using CA.Blocks.DataAccess.Translator.Extensions;
using CA.Blocks.PostgreSQLDataAccessUnitTests.Base;
using CA.Blocks.SQLServerDataAccess;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace CA.Blocks.PostgreSQLDataAccessUnitTests.DbTypeTests;

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
        ExecuteNonQuery(InsertTestDataSQL(string.Format("'{0}'", data.ToString("yyyy-MM-dd"))));
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
        ClassicAssert.AreEqual(5, data.Count);
    }


    [Test]
    public void SelectAllDataDateOnlyWithFilter()
    {
        //setup
        var cmd = CreateTextCommand(SelectTestDataSQL("Where col > @testValue"));
        cmd.Parameters.Add(_testDate.ToPostgresParameter("@testValue"));

        //Act
        var data = Execute(cmd).ToListOf<DateOnlyDataType>();

        //Assert
        ClassicAssert.AreEqual(2, data.Count);
    }


    [Test]
    public void SelectAllDataWithWithTranslator()
    {
        //setup
        var testValue = _testDate;
        var cmd = CreateTextCommand(SelectTestDataSQL("Where col = @value")).WithParameter(testValue.ToPostgresParameter("@value"));
        var t = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<DateOnlyDataType>();
        //Act
        var data = t.Translate(ExecuteDataRow(cmd));

        ClassicAssert.AreEqual(testValue, data.Col);
    }
}