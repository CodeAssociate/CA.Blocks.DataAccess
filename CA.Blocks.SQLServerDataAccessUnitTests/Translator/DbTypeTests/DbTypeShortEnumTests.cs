using CA.Blocks.DataAccess.Translator.DbColToType.Converters;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Providers;
using CA.Blocks.SQLServerDataAccess;
using CA.Blocks.SQLServerDataAccessUnitTests.Base;
using NUnit.Framework;

namespace CA.Blocks.SQLServerDataAccessUnitTests.Translator.DbTypeTests
{
    [TestFixture]
    public class DbTypeShortEnumTests : UnitTestDataAccess
    {
        public enum MyTestEnum
        {
            Foo = 1,
            Bar = 2,
            ForBar= 4,
        }
        // in app you would write an extension for MyTestEnum called ToSqlParameter  telling it how you what to store the enum, short, int, string...

        
        [OneTimeSetUp]
        public void Init()
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

        [SetUp]
        public void Setup()
        {
            ExecuteNonQuery(DropTestTableSQL());
            ExecuteNonQuery(CreateTestTable("varchar(32) not null"));
            InsertTestDataStringSQL("Foo");
            InsertTestDataStringSQL("Bar");
            InsertTestDataStringSQL("1");
            InsertTestDataStringSQL("2");
            InsertTestDataStringSQL("ForBar");
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
            var data = this.ExecuteToListOf<StringEnumDataType>(cmd);
            //Assert
            Assert.AreEqual(5, data.Count);
            Assert.AreEqual(MyTestEnum.Foo, data[0].Col);
            Assert.AreEqual(MyTestEnum.Bar, data[3].Col);
            Assert.AreEqual(MyTestEnum.ForBar, data[4].Col);
        }
        
        [Test]
        public void SelectAllDataWithWithTranslator()
        {
            //setup
            const MyTestEnum testValue = MyTestEnum.Bar;
            var cmd = CreateTextCommand(SelectTestDataSQL(), "Where col = @value").WithParameter(testValue.ToString().ToSqlParameter("@value"));
            var t = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<StringEnumDataType>();
            //Act
            var data = t.Translate(ExecuteDataRow(cmd));
            
            Assert.AreEqual(testValue, data.Col);
        }
    }
}
