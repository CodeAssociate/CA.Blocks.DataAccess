using CA.Blocks.DataAccess;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Providers;
using CA.Blocks.DataAccess.Translator.Extensions;
using CA.Blocks.PostgresDataAccess;
using CA.Blocks.PostgresDataAccessTests.Base;

namespace CA.Blocks.PostgresDataAccessTests.Postgres.DbTypeTests;

[Collection("DbIntegrationTests")]
public class DbTypeDoubleTests : UnitTestDataAccess, IDisposable
{
    private class DoubleDataType
    {
        public Double Col { get; set; }
    }

    private void InsertTestDataSQL(double data)
    {
        ExecuteNonQuery(InsertTestDataSQL($"{data}"));
    }

    public DbTypeDoubleTests()
    {
        ExecuteNonQuery(DropTestTableSQL());
        ExecuteNonQuery(CreateTestTable("double precision not null"));
        InsertTestDataSQL(-1.2);
        InsertTestDataSQL(0);
        InsertTestDataSQL(123.456);
        InsertTestDataSQL(int.MaxValue);
        InsertTestDataSQL(123456789.987654321);
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
        var data = ExecuteToListOf<DoubleDataType>(cmd);
        //Assert
        Assert.Equal(5, data.Count);
        Assert.Equal(-1.2, data[0].Col);
        Assert.Equal(123456789.987654321, data[4].Col);
    }

    [Fact]
    public void SelectAllDataFilter ()
    {
        //setup
        const double testvalue = 123;
        var cmd = CreateTextCommand(SelectTestDataSQL("Where col > @testValue"));
        cmd.Parameters.Add(testvalue.ToPostgresParameter("@testValue"));

        //Act
        var data = ExecuteToListOf<DoubleDataType>(cmd);

        //Asert
        Assert.Equal(3, data.Count);
    }

    [Fact]
    public void SelectAllDataWithWithTranslator()
    {
        //setup
        const double testValue = 123.456;
        var cmd = CreateTextCommand(SelectTestDataSQL("Where col = @value")).WithParameter(testValue.ToPostgresParameter("@value"));
        var t = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<DoubleDataType>();
        //Act
        var data = t.Translate(Execute(cmd).ToDataRow());

        Assert.Equal(testValue, data.Col);
    }
}
