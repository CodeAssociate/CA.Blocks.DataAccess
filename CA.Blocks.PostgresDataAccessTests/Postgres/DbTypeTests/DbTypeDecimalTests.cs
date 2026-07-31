using CA.Blocks.DataAccess;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Providers;
using CA.Blocks.PostgresDataAccess;
using CA.Blocks.PostgresDataAccessTests.Base;

namespace CA.Blocks.PostgresDataAccessTests.Postgres.DbTypeTests;

[Collection("DbIntegrationTests")]
public class DbTypeDecimalTests : UnitTestDataAccess, IDisposable
{
    private class DecimalDataType
    {
        public Decimal Col { get; set; }
    }

    private void InsertTestDataSQL(decimal data)
    {
        ExecuteNonQuery(InsertTestDataSQL($"{data}"));
    }

    public DbTypeDecimalTests()
    {
        ExecuteNonQuery(DropTestTableSQL());
        ExecuteNonQuery(CreateTestTable("NUMERIC(24,12) not null"));
        InsertTestDataSQL(-1.2M);
        InsertTestDataSQL(0);
        InsertTestDataSQL(123.456M);
        InsertTestDataSQL(int.MaxValue);
        InsertTestDataSQL(123456789.987654321M);
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
        var data = ExecuteToListOf<DecimalDataType>(cmd);
        //Assert
        Assert.Equal(5, data.Count);
        Assert.Equal(-1.2M, data[0].Col);
        Assert.Equal(123.456M, data[2].Col);
        Assert.Equal(123456789.987654321M, data[4].Col);
    }

    [Fact]
    public void SelectAllDataFilter ()
    {
        //setup
        const decimal testvalue = 123M;
        var cmd = CreateTextCommand(SelectTestDataSQL("Where col > @testValue"));
        cmd.Parameters.Add(testvalue.ToPostgresParameter("@testValue"));

        //Act
        var data = ExecuteToListOf<DecimalDataType>(cmd);

        //Asert
        Assert.Equal(3, data.Count);
    }

    [Fact]
    public void SelectAllDataWithWithTranslator()
    {
        //setup
        const Decimal testValue = 123.456M;
        var cmd = CreateTextCommand(SelectTestDataSQL("Where col = @value")).WithParameter(testValue.ToPostgresParameter("@value"));
        var t = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<DecimalDataType>();
        //Act
        var data = t.Translate(ExecuteDataRow(cmd));

        Assert.Equal(testValue, data.Col);
    }
}
