using CA.Blocks.DataAccess.Translator.DbRowToObject.Providers;
using CA.Blocks.DataAccess.Translator.Extensions;
using CA.Blocks.SQLServerDataAccessUnitTests.Base;

namespace CA.Blocks.SQLServerDataAccessUnitTests.Translator.DbTypeTests
{
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
            ExecuteNonQuery(InsertTestDataCommand(data));
        }

        public DbTypeEnumIntTests()
        {
            ExecuteNonQuery(DropTestTableCommand());
            ExecuteNonQuery(CreateTestTableCommand("int"));
            InsertTestDataSQL(1);
            InsertTestDataSQL(2);
            InsertTestDataSQL(4);
        }

        public void Dispose()
        {
            ExecuteNonQuery(DropTestTableCommand());
        }


        [Fact]
        public void SelectAllDataToListOf()
        {
            //Setup 
            var cmd = BasicSelectCommand();
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
            var cmd = BasicSelectCommand($"WHERE {UNIT_TEST_COL_NAME:[]} = {resultOfToSqlParameter:@value}");
            var t = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<StringEnumDataType>();
            //Act
            var data = t.Translate(Execute(cmd).ToDataRow());
            
            Assert.Equal(testValue, data.Col);
        }
    }
}




