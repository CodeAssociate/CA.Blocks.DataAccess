using System;
using CA.Blocks.DataAccess;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Providers;
using CA.Blocks.PostgreSQLDataAccessUnitTests.Base;
using CA.Blocks.SQLServerDataAccess;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace CA.Blocks.PostgreSQLDataAccessUnitTests.DbTypeTests;

// Note The fractional precision is determined by the database's lc_monetary setting. 
// Default is two fractional digits
[TestFixture]
public class DbTypeMoneyTests : UnitTestDataAccess
{

    private class MoneyDataType
    {
        public decimal Col { get; set; }
    }


    private void InsertTestDataSQL(double data)
    {
        ExecuteNonQuery(InsertTestDataSQL($"{data}"));
    }

    [SetUp]
    public void Setup()
    {
        ExecuteNonQuery(DropTestTableSQL());
        ExecuteNonQuery(CreateTestTable("money not null"));
        InsertTestDataSQL(-1.2);
        InsertTestDataSQL(0);
        InsertTestDataSQL(123.456);
        InsertTestDataSQL(int.MaxValue);
        InsertTestDataSQL(123456789.9876);
    }

    [TearDown]
    public void TearDown()
    {
        ExecuteNonQuery(DropTestTableSQL());
    }

    [Test]
    public void SelectAllData()
    {
        //Setup 
        var cmd = CreateTextCommand(SelectTestDataSQL());
        //Act
        var data = ExecuteDataTable(cmd);
        //Assert
        ClassicAssert.AreEqual(5, data.Rows.Count);
    }


    [Test]
    public void SelectAllDataToListOf()
    {
        //Setup 
        var cmd = CreateTextCommand(SelectTestDataSQL());
        //Act
        var data = ExecuteToListOf<MoneyDataType>(cmd);
        //Assert
        ClassicAssert.AreEqual(5, data.Count);
        ClassicAssert.AreEqual(-1.2, data[0].Col);
        ClassicAssert.AreEqual(123456789.99, data[4].Col);
    }

    [Test]
    public void SelectAllDataFilter ()
    {
        //setup
        const decimal testvalue = 123M;
        var cmd = CreateTextCommand(SelectTestDataSQL( "Where col > @testValue"));
        cmd.Parameters.Add(testvalue.ToPostgresParameter("@testValue", SpecificSQLDecimalType.Money));

        //Act
        var data = ExecuteToListOf<MoneyDataType>(cmd);

        //Asert
        ClassicAssert.AreEqual(3, data.Count);
    }

    [Test]
    public void SelectAllDataWithWithTranslator()
    {
        //setup
        const Decimal testValue = 123.46M;
        var cmd = CreateTextCommand(SelectTestDataSQL("Where col = @value")).WithParameter(testValue.ToPostgresParameter("@value", SpecificSQLDecimalType.Money));
        var t = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<MoneyDataType>();
        //Act
        var data = t.Translate(ExecuteDataRow(cmd));
            
        ClassicAssert.AreEqual(testValue, data.Col);
    }

}
