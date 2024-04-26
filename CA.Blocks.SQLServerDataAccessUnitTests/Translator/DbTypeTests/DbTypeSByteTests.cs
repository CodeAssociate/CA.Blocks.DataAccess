using CA.Blocks.DataAccess;
using CA.Blocks.DataAccess.Translator.Basic;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Providers;
using CA.Blocks.SQLServerDataAccess;
using CA.Blocks.SQLServerDataAccessUnitTests.Base;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace CA.Blocks.SQLServerDataAccessUnitTests.Translator.DbTypeTests
{
    [TestFixture]
    public class DbTypeSByteTests : UnitTestDataAccess
    {

        private class SByteDataType
        {
            public sbyte Col { get; set; }
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
            InsertTestDataSQL(-128);
            InsertTestDataSQL(0);
            InsertTestDataSQL(10);
            InsertTestDataSQL(100);
            InsertTestDataSQL(127);
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
            var data = ExecuteToListOf<SByteDataType>(cmd);
            //Assert
            ClassicAssert.AreEqual(5, data.Count);
            ClassicAssert.AreEqual(-128, data[0].Col);
            ClassicAssert.AreEqual(127, data[4].Col);
        }
        
        [Test]
        public void SelectAllDataWithWithTranslator()
        {
            //setup
            sbyte testValue = 127;
            var cmd = CreateTextCommand(SelectTestDataSQL(), "Where col = @value").WithParameter(testValue.ToSqlParameter("@value"));
            var t = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<SByteDataType>();
            //Act
            var data = t.Translate(ExecuteDataRow(cmd));
            
            ClassicAssert.AreEqual(testValue, data.Col);
        }

    }
}
