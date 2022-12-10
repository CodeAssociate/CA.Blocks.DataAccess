using CA.Blocks.DataAccess;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Providers;
using CA.Blocks.DataAccess.Translator.Extensions;
using CA.Blocks.SQLServerDataAccess;
using CA.Blocks.SQLServerDataAccessUnitTests.Base;
using NUnit.Framework;

namespace CA.Blocks.SQLServerDataAccessUnitTests.Translator.DbTypeTests
{
    [TestFixture]
    public class DbTypeEnumShortTests : UnitTestDataAccess
    {
        public enum MyTestEnum
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

        [SetUp]
        public void Setup()
        {
            ExecuteNonQuery(DropTestTableSQL());
            ExecuteNonQuery(CreateTestTable("smallint not null"));
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
            var data = Execute(cmd).ToListOf<ShortEnumDataType>();
            //Assert
            Assert.AreEqual(3, data.Count);
            Assert.AreEqual(MyTestEnum.Foo, data[0].Col);
            Assert.AreEqual(MyTestEnum.Bar, data[1].Col);
            Assert.AreEqual(MyTestEnum.ForBar, data[2].Col);
        }

        [Test]
        public void SelectAllDataToListOfNull()
        {
            //Setup 
            var cmd = CreateTextCommand(SelectTestDataSQL());
            //Act
            var data = Execute(cmd).ToListOf<NullShortEnumDataType>();
            //Assert
            Assert.AreEqual(3, data.Count);
            Assert.AreEqual(MyTestEnum.Foo, data[0].Col);
            Assert.AreEqual(MyTestEnum.Bar, data[1].Col);
            Assert.AreEqual(MyTestEnum.ForBar, data[2].Col);
        }


        [Test]
        public void SelectAllDataWithWithTranslator()
        {
            //setup
            const MyTestEnum testValue = MyTestEnum.Bar;
            var cmd = CreateTextCommand(SelectTestDataSQL(), "Where col = @value").WithParameter(((short)(testValue)).ToSqlParameter("@value"));
            var t = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<ShortEnumDataType>();
            //Act
            var data = t.Translate(ExecuteDataRow(cmd));

            Assert.AreEqual(testValue, data.Col);
        }
    }
}
