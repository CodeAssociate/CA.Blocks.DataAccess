using CA.Blocks.DataAccess.Translator.Basic;
using CA.Blocks.PostgresDataAccess;
using CA.Blocks.PostgresDataAccessTests.Base;
using CA.Blocks.DataAccess.Translator.Extensions;

namespace CA.Blocks.PostgresDataAccessTests.Postgres.DbTypeTests;

[Collection("DbIntegrationTests")]
public class DbTypeNCharTests : UnitTestDataAccess, IDisposable
{
    private void InsertTestDataSQL(char data)
    {
        ExecuteNonQuery(InsertTestDataSQL(string.Format("'{0}'", data)));
    }

    public DbTypeNCharTests()
    {
        ExecuteNonQuery(DropTestTableSQL());
        ExecuteNonQuery(CreateTestTable("Char(1) not null"));
        InsertTestDataSQL('ä');
        InsertTestDataSQL('B');
        InsertTestDataSQL('C');
        InsertTestDataSQL('D');
        InsertTestDataSQL('E');
    }

    public new void Dispose()
    {
        ExecuteNonQuery(DropTestTableSQL());
        base.Dispose();
    }

    [Fact]
    public void SelectAllDataNChar()
    {
        //Setup
        var cmd = CreateTextCommand(SelectTestDataSQL());
        var t = new CharTranslator(UNIT_TEST_COL_NAME);
        //Act
        var data = t.Translate(ExecuteDataTable(cmd));
        //Assert
        Assert.Equal(5, data.Count);
    }

    [Fact]
    public void SelectAllDataNCharWithFilter ()
    {
        //setup
        char testvalue = 'ä';
        var t = new CharTranslator(UNIT_TEST_COL_NAME);
        var cmd = CreateTextCommand(SelectTestDataSQL("Where col = @testValue"));
        cmd.Parameters.Add(testvalue.ToPostgresParameter("@testValue"));

        //Act
        var data = t.Translate(Execute(cmd).ToDataRow());

        //Asert
        Assert.Equal(testvalue, data);
    }
}
