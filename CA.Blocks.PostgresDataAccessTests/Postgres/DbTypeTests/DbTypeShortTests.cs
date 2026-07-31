using CA.Blocks.DataAccess.Translator.Basic;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Providers;
using CA.Blocks.DataAccess.Translator.Extensions;
using CA.Blocks.PostgresDataAccess;
using CA.Blocks.PostgresDataAccessTests.Base;

namespace CA.Blocks.PostgresDataAccessTests.Postgres.DbTypeTests;

[Collection("DbIntegrationTests")]
public class DbTypeShortTests : UnitTestDataAccess, IDisposable
{
    private class ShortDataType
    {
        public short Col { get; set; }
    }

    private void InsertTestDataSQL(short data)
    {
        ExecuteNonQuery(InsertTestDataSQL(data.ToString()));
    }

    public DbTypeShortTests()
    {
        ExecuteNonQuery(DropTestTableSQL());
        ExecuteNonQuery(CreateTestTable("smallint not null"));
        InsertTestDataSQL(-1);
        InsertTestDataSQL(0);
        InsertTestDataSQL(123);
        InsertTestDataSQL(246);
        InsertTestDataSQL(short.MaxValue);
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
        var t = new ShortTranslator(UNIT_TEST_COL_NAME);
        var cmd = CreateTextCommand(SelectTestDataSQL());
        //Act
        var data = t.Translate(Execute(cmd).ToDataTable());
        //Assert
        Assert.Equal(5, data.Count);
    }

    [Fact]
    public void SelectAllDataToListOf()
    {
        //Setup
        var cmd = CreateTextCommand(SelectTestDataSQL());
        //Act
        var data = ExecuteToListOf<ShortDataType>(cmd);
        //Assert
        Assert.Equal(5, data.Count);
        Assert.Equal(short.MaxValue, data[4].Col);
    }

    [Fact]
    public void SelectAllDataIntWithFilter ()
    {
        //setup
        const short testvalue = 123;
        var t = new ShortTranslator(UNIT_TEST_COL_NAME);
        var cmd = CreateTextCommand(SelectTestDataSQL("Where col >= @testValue"));
        cmd.Parameters.Add(testvalue.ToPostgresParameter("@testValue"));


        //Act
        var data = t.Translate(Execute(cmd).ToDataTable());

        //Asert
        Assert.Equal(3, data.Count);
    }

    [Fact]
    public void SelectAllDataIntWithFilterWithTranslator ()
    {
        //setup
        const short testvalue = 123;
        var t = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<ShortDataType>();
        var cmd = CreateTextCommand(SelectTestDataSQL("Where col >= @testValue"));
        cmd.Parameters.Add(testvalue.ToPostgresParameter("@testValue"));

        //Act
        var data = t.Translate(Execute(cmd).ToDataTable());

        //Asert
        Assert.Equal(3, data.Count);
    }
}
