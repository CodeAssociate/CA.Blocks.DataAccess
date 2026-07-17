using CA.Blocks.DataAccess.Translator.DbColToType.Converters;
using CA.Blocks.MySQLDataAccessUnitTests.Base;
using Xunit;

namespace CA.Blocks.MySQLDataAccessUnitTests.MySQL.DbTypeTests
{
public class DbTypeStringEnumTests : UnitTestDataAccess, IDisposable
    {
        public enum MyTestEnum
        {
            Foo = 1,
            Bar = 2,
            ForBar= 4,
        }

        static DbTypeStringEnumTests()
        {
            CA.Blocks.DataAccess.Translator.DbColToType.Providers.DefaultDbColToTypeProvider.DefaultInstance.Add(new EnumDbColToTypeConverter<MyTestEnum>());
        }

        private class StringEnumDataType
        {
            public MyTestEnum Col { get; set; }
        }

        private void InsertTestDataStringSQL(string data)
        {
            ExecuteNonQuery(InsertTestDataSQL($"'{data}'"));
        }

        public DbTypeStringEnumTests()
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
    }
}


