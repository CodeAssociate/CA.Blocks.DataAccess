using CA.Blocks.DataAccess;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Providers;
using CA.Blocks.DataAccess.Translator.Extensions;
using CA.Blocks.PostgresDataAccess;
using CA.Blocks.PostgresDataAccessTests.Base;

namespace CA.Blocks.PostgresDataAccessTests.Postgres.DbTypeTests;

[Collection("DbIntegrationTests")]
public class DbTypeEnumIntTests : UnitTestDataAccess, IDisposable
{
    public enum MyTestEnum
    {
        Foo = 1,
        Bar = 2,
        ForBar= 4,
    }

    private class StringEnumDataType
    {
        public MyTestEnum Col { get; set; }
    }

    private void InsertTestDataSQL(int data)
    {
        ExecuteNonQuery(InsertTestDataSQL(data.ToString()));
    }

    public DbTypeEnumIntTests()
    {
        ExecuteNonQuery(DropTestTableSQL());
        ExecuteNonQuery(CreateTestTable("int not null"));
        InsertTestDataSQL(1);
        InsertTestDataSQL(2);
        InsertTestDataSQL(4);
    }

    public new void Dispose()
    {
        ExecuteNonQuery(DropTestTableSQL());
        base.Dispose();
    }

    [Fact]
    public void SelectAllDataToListOf()
    {
        //Setup
        var cmd = CreateTextCommand(SelectTestDataSQL());
        //Act
        var data = Execute(cmd).ToListOf<StringEnumDataType>();
        //Assert
        Assert.Equal(3, data.Count);
        Assert.Equal(MyTestEnum.Foo, data[0].Col);
        Assert.Equal(MyTestEnum.Bar, data[1].Col);
        Assert.Equal(MyTestEnum.ForBar, data[2].Col);
    }

    [Fact]
    public void SelectAllDataWithWithTranslator()
    {
        //setup
        const MyTestEnum testValue = MyTestEnum.Bar;
        var resultOfToSqlParameter = (int)testValue;
        var cmd = CreateTextCommand(SelectTestDataSQL("Where col = @value")).WithParameter(resultOfToSqlParameter.ToPostgresParameter("@value"));
        var t = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<StringEnumDataType>();
        //Act
        var data = t.Translate(ExecuteDataRow(cmd));

        Assert.Equal(testValue, data.Col);
    }
}
