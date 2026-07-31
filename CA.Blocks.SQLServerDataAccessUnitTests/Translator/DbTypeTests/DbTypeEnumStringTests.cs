using CA.Blocks.DataAccess.Translator.Extensions;
using CA.Blocks.DataAccess;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Providers;
using CA.Blocks.SQLServerDataAccess;
using CA.Blocks.SQLServerDataAccessUnitTests.Base;

namespace CA.Blocks.SQLServerDataAccessUnitTests.Translator.DbTypeTests
{
    [Collection("DbIntegrationTests")]
    public class DbTypeEnumStringTests : UnitTestDataAccess, IDisposable
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

        private void InsertTestDataStringSQL(string data)
        {
            ExecuteNonQuery(InsertTestDataSQL($"'{data}'"));
        }

        public DbTypeEnumStringTests()
        {
            ExecuteNonQuery(DropTestTableSQL());
            ExecuteNonQuery(CreateTestTable("varchar(32) not null"));
            InsertTestDataStringSQL("Foo");
            InsertTestDataStringSQL("Bar");
            InsertTestDataStringSQL("1");
            InsertTestDataStringSQL("2");
            InsertTestDataStringSQL("ForBar");

        }

        public void Dispose()
        {
            ExecuteNonQuery(DropTestTableSQL());
        }


        [Fact]
        public void SelectAllDataToListOf()
        {
            //Setup 
            var cmd = CreateTextCommand(SelectTestDataSQL());
            //Act
            var data = this.ExecuteToListOf<StringEnumDataType>(cmd);
            //Assert
            Assert.Equal(5, data.Count);
            Assert.Equal(MyTestEnum.Foo, data[0].Col);
            Assert.Equal(MyTestEnum.Bar, data[3].Col);
            Assert.Equal(MyTestEnum.ForBar, data[4].Col);
        }
        
        [Fact]
        public void SelectAllDataWithWithTranslator()
        {
            //setup
            const MyTestEnum testValue = MyTestEnum.Bar;
            var cmd = CreateTextCommand(SelectTestDataSQL(), "Where col = @value").WithParameter(testValue.ToString().ToSqlParameter("@value"));
            var t = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<StringEnumDataType>();
            //Act
            var data = t.Translate(Execute(cmd).ToDataRow());
            
            Assert.Equal(testValue, data.Col);
        }
    }
}




