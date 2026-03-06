using CA.Blocks.DataAccess;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Providers;
using CA.Blocks.DataAccess.Translator.Extensions;
using CA.Blocks.PostgreSQLDataAccessUnitTests.Base;
using CA.Blocks.SQLServerDataAccess;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace CA.Blocks.PostgreSQLDataAccessUnitTests.DbTypeTests;

[TestFixture]
public class DbTypeEnumIntTests : UnitTestDataAccess
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

    [SetUp]
    public void Setup()
    {
        ExecuteNonQuery(DropTestTableSQL());
        ExecuteNonQuery(CreateTestTable("int not null"));
        InsertTestDataSQL(1);
        InsertTestDataSQL(2);
        InsertTestDataSQL(4);
    }

    [TearDown]
    public void TearDown()
    {
        ExecuteNonQuery(DropTestTableSQL());
    }


    [Test]
    public void SelectAllDataToListOf()
    {
        //Setup 
        var cmd = CreateTextCommand(SelectTestDataSQL());
        //Act
        var data = Execute(cmd).ToListOf<StringEnumDataType>();
        //Assert
        ClassicAssert.AreEqual(3, data.Count);
        ClassicAssert.AreEqual(MyTestEnum.Foo, data[0].Col);
        ClassicAssert.AreEqual(MyTestEnum.Bar, data[1].Col);
        ClassicAssert.AreEqual(MyTestEnum.ForBar, data[2].Col);
    }
        
    [Test]
    public void SelectAllDataWithWithTranslator()
    {
        //setup
        const MyTestEnum testValue = MyTestEnum.Bar;
        var resultOfToSqlParameter = (int)testValue;
        var cmd = CreateTextCommand(SelectTestDataSQL("Where col = @value")).WithParameter(resultOfToSqlParameter.ToPostgresParameter("@value"));
        var t = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<StringEnumDataType>();
        //Act
        var data = t.Translate(ExecuteDataRow(cmd));
            
        ClassicAssert.AreEqual(testValue, data.Col);
    }
}

