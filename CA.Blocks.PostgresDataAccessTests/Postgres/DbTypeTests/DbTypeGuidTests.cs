using CA.Blocks.DataAccess;
using CA.Blocks.DataAccess.Translator.Basic;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Providers;
using CA.Blocks.PostgresDataAccess;
using CA.Blocks.PostgresDataAccessTests.Base;

namespace CA.Blocks.PostgresDataAccessTests.Postgres.DbTypeTests;

[Collection("DbIntegrationTests")]
public class DbTypeGuidTests : UnitTestDataAccess, IDisposable
{
    private const string TestGuidValue = "CE69B300-F9EA-4F3B-BBA8-676D12737E3E";
    private class GuidDataType
    {
        public Guid Col { get; set; }
    }

    private void InsertTestDataSQL(Guid data)
    {
        ExecuteNonQuery(InsertTestDataSQL($"'{data.ToString()}'"));
    }

    public DbTypeGuidTests()
    {
        ExecuteNonQuery(DropTestTableSQL());
        ExecuteNonQuery(CreateTestTable("uuid not null"));
        InsertTestDataSQL(Guid.Empty);
        InsertTestDataSQL(Guid.NewGuid());
        InsertTestDataSQL(Guid.NewGuid());
        InsertTestDataSQL(Guid.NewGuid());
        InsertTestDataSQL(Guid.Parse(TestGuidValue));
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
        var data = ExecuteToListOf<GuidDataType>(cmd);
        //Assert
        Assert.Equal(5, data.Count);
        Assert.Equal(Guid.Parse(TestGuidValue), data[4].Col);
    }

    [Fact]
    public void SelectAllDataTimeWithFilter()
    {
        //setup
        Guid testvalue = Guid.Parse(TestGuidValue);
        var t = new DateTimeTranslator(UNIT_TEST_COL_NAME);
        var cmd = CreateTextCommand(SelectTestDataSQL("Where col = @testValue"));
        cmd.Parameters.Add(testvalue.ToPostgresParameter("@testValue"));

        //Act
        var data = ExecuteTo<GuidDataType>(cmd);

        //Asert
        Assert.Equal(testvalue, data.Col);
    }

    [Fact]
    public void SelectAllDataWithWithTranslator()
    {
        //setup
        Guid testValue = Guid.Parse(TestGuidValue);
        var cmd = CreateTextCommand(SelectTestDataSQL("Where col = @value")).WithParameter(testValue.ToPostgresParameter("@value"));
        var t = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<GuidDataType>();
        //Act
        var data = t.Translate(ExecuteDataRow(cmd));

        Assert.Equal(testValue, data.Col);
    }
}
