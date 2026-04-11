using CA.Blocks.DataAccess;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Providers;
using CA.Blocks.PostgreSQLDataAccessUnitTests.Base;
using CA.Blocks.SQLServerDataAccess;

namespace CA.Blocks.PostgreSQLDataAccessUnitTests.DbTypeTests;

// Note The fractional precision is determined by the database's lc_monetary setting.
// Default is two fractional digits
public class DbTypeMoneyTests : UnitTestDataAccess, IDisposable
{
    private class MoneyDataType
    {
        public decimal Col { get; set; }
    }

    private void InsertTestDataSQL(double data)
    {
        ExecuteNonQuery(InsertTestDataSQL($"{data}"));
    }

    public DbTypeMoneyTests()
    {
        ExecuteNonQuery(DropTestTableSQL());
        ExecuteNonQuery(CreateTestTable("money not null"));
        InsertTestDataSQL(-1.2);
        InsertTestDataSQL(0);
        InsertTestDataSQL(123.456);
        InsertTestDataSQL(int.MaxValue);
        InsertTestDataSQL(123456789.9876);
    }

    public new void Dispose()
    {
        ExecuteNonQuery(DropTestTableSQL());
        base.Dispose();
    }

    [Fact]
    public void SelectAllData()
    {
        //Setup
        var cmd = CreateTextCommand(SelectTestDataSQL());
        //Act
        var data = ExecuteDataTable(cmd);
        //Assert
        Assert.Equal(5, data.Rows.Count);
    }

    [Fact]
    public void SelectAllDataToListOf()
    {
        //Setup
        var cmd = CreateTextCommand(SelectTestDataSQL());
        //Act
        var data = ExecuteToListOf<MoneyDataType>(cmd);
        //Assert
        Assert.Equal(5, data.Count);
        Assert.Equal(-1.2M, data[0].Col);
        Assert.Equal(123456789.99M, data[4].Col);
    }

    [Fact]
    public void SelectAllDataFilter ()
    {
        //setup
        const decimal testvalue = 123M;
        var cmd = CreateTextCommand(SelectTestDataSQL( "Where col > @testValue"));
        cmd.Parameters.Add(testvalue.ToPostgresParameter("@testValue", SpecificSQLDecimalType.Money));

        //Act
        var data = ExecuteToListOf<MoneyDataType>(cmd);

        //Asert
        Assert.Equal(3, data.Count);
    }

    [Fact]
    public void SelectAllDataWithWithTranslator()
    {
        //setup
        const Decimal testValue = 123.46M;
        var cmd = CreateTextCommand(SelectTestDataSQL("Where col = @value")).WithParameter(testValue.ToPostgresParameter("@value", SpecificSQLDecimalType.Money));
        var t = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<MoneyDataType>();
        //Act
        var data = t.Translate(ExecuteDataRow(cmd));

        Assert.Equal(testValue, data.Col);
    }
}
