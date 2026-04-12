using CA.Blocks.DataAccess.Translator.Basic;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Providers;
using CA.Blocks.DataAccess.Translator.Extensions;
using CA.Blocks.PostgreSQLDataAccessUnitTests.Base;
using CA.Blocks.SQLServerDataAccess;

namespace CA.Blocks.PostgreSQLDataAccessUnitTests.DbTypeTests;

[Collection("DbTypeTests")]
public class DbTypeTextTests : UnitTestDataAccess, IDisposable
{
    private class StringDataType
    {
        public required string Col { get; set; }
    }

    private const string  TEST_DATA = "text data";

    private void InsertTestDataAsBinarySQL(string data)
    {
        ExecuteNonQuery(InsertTestDataSQL(string.Format("'{0}'", data)));
    }

    public DbTypeTextTests()
    {
        ExecuteNonQuery(DropTestTableSQL());
        ExecuteNonQuery(CreateTestTable("Text not null"));
        InsertTestDataAsBinarySQL(TEST_DATA);
        InsertTestDataAsBinarySQL(Guid.NewGuid().ToString());
        InsertTestDataAsBinarySQL(Guid.NewGuid().ToString());
        InsertTestDataAsBinarySQL(Guid.NewGuid().ToString());
        InsertTestDataAsBinarySQL(Guid.NewGuid().ToString());
    }

    public new void Dispose()
    {
        ExecuteNonQuery(DropTestTableSQL());
        base.Dispose();
    }

    [Fact]
    public void SelectAllDataText()
    {
        //Setup
        var t = new StringTranslator(UNIT_TEST_COL_NAME);
        var cmd = CreateTextCommand(SelectTestDataSQL());
        //Act
        var data = t.Translate(ExecuteDataTable(cmd));
        //Assert
        Assert.Equal(5, data.Count);
        Assert.Equal(TEST_DATA, data[0]);
    }

    [Fact]
    public void SelectAllDataToListOf()
    {
        //Setup
        var cmd = CreateTextCommand(SelectTestDataSQL());
        //Act
        var data = Execute(cmd).ToListOf<StringDataType>();
        //Assert
        Assert.Equal(5, data.Count);
        Assert.Equal(TEST_DATA, data[0].Col);
    }

    [Fact]
    public void SelectDataTextWithFilter()
    {
        //setup
        const string testvalue = TEST_DATA;
        var t = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<StringDataType>();
        var cmd = CreateTextCommand(SelectTestDataSQL("Where col like @testValue"));
        cmd.Parameters.Add(testvalue.ToPostgresParameter("@testValue", SpecificSQLStringType.Text));

        //Act
        var data = t.Translate(ExecuteDataTable(cmd));

        //Asert
        Assert.Single(data);
    }
}
