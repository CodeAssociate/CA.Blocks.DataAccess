using CA.Blocks.DataAccess;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Providers;
using CA.Blocks.DataAccess.Translator.Extensions;
using CA.Blocks.PostgresDataAccess;
using CA.Blocks.PostgresDataAccessTests.Base;

namespace CA.Blocks.PostgresDataAccessTests.Postgres.DbTypeTests;

[Collection("DbIntegrationTests")]
public class DbTypeEnumShortTests : UnitTestDataAccess, IDisposable
{
    public enum MyTestEnum : short
    {
        Foo = 1,
        Bar = 2,
        ForBar = 4,
    }

    private class ShortEnumDataType
    {
        public MyTestEnum Col { get; set; }
    }

    private class NullShortEnumDataType
    {
        public MyTestEnum? Col { get; set; }
    }

    private void InsertTestDataSQL(short data)
    {
        ExecuteNonQuery(InsertTestDataSQL(data.ToString()));
    }

    public DbTypeEnumShortTests()
    {
        ExecuteNonQuery(DropTestTableSQL());
        ExecuteNonQuery(CreateTestTable("smallint not null"));
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
        var data = Execute(cmd).ToListOf<ShortEnumDataType>();
        //Assert
        Assert.Equal(3, data.Count);
        Assert.Equal(MyTestEnum.Foo, data[0].Col);
        Assert.Equal(MyTestEnum.Bar, data[1].Col);
        Assert.Equal(MyTestEnum.ForBar, data[2].Col);
    }

    [Fact]
    public void SelectAllDataToListOfNull()
    {
        //Setup
        var cmd = CreateTextCommand(SelectTestDataSQL());
        //Act
        var data = Execute(cmd).ToListOf<NullShortEnumDataType>();
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
        var cmd = CreateTextCommand(SelectTestDataSQL("Where col = @value")).WithParameter(((short)(testValue))
            .ToPostgresParameter("@value"));
        var t = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<ShortEnumDataType>();
        //Act
        var data = t.Translate(Execute(cmd).ToDataRow());

        Assert.Equal(testValue, data.Col);
    }
}
