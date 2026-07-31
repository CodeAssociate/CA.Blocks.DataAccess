using CA.Blocks.DataAccess.Translator.DbRowToObject.Providers;
using CA.Blocks.PostgresDataAccess;
using CA.Blocks.PostgresDataAccessTests.Base;

namespace CA.Blocks.PostgresDataAccessTests.Postgres.DbTypeTests;

[Collection("DbIntegrationTests")]
public class DbTypeSingleTests : UnitTestDataAccess, IDisposable
{
    private class SingleDataType
    {
        public Single Col { get; set; }
    }

    private void InsertTestDataSQL(float data)
    {
        ExecuteNonQuery(InsertTestDataSQL($"{data}"));
    }

    public DbTypeSingleTests()
    {
        ExecuteNonQuery(DropTestTableSQL());
        ExecuteNonQuery(CreateTestTable("real not null"));
        InsertTestDataSQL((float)-1.2);
        InsertTestDataSQL(0);
        InsertTestDataSQL((float)123.456);
        InsertTestDataSQL(int.MaxValue);
        InsertTestDataSQL((float)123456789.987654321);
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
        var data = ExecuteToListOf<SingleDataType>(cmd);
        //Assert
        Assert.Equal(5, data.Count);
        Assert.Equal(-(float)1.2, data[0].Col);
    }

    [Fact]
    public void SelectAllDataFilter ()
    {
        //setup
        const Single testvalue = 123;
        var cmd = CreateTextCommand(SelectTestDataSQL("Where col > @testValue"));
        cmd.Parameters.Add(testvalue.ToPostgresParameter("@testValue"));

        //Act
        var data = ExecuteToListOf<SingleDataType>(cmd);

        //Asert
        Assert.Equal(3, data.Count);
    }

    [Fact]
    public void SelectAllDataIntWithFilterWithTranslator ()
    {
        //setup
        const Single testvalue = 123;
        var t = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<SingleDataType>();
        var cmd = CreateTextCommand(SelectTestDataSQL("Where col >= @testValue"));
        cmd.Parameters.Add(testvalue.ToPostgresParameter("@testValue"));

        //Act
        var data = t.Translate(ExecuteDataTable(cmd));

        //Asert
        Assert.Equal(3, data.Count);
    }
}
