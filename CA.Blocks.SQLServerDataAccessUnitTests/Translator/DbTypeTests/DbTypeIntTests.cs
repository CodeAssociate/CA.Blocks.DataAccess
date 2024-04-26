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
    public class DbTypeIntTests : UnitTestDataAccess
    {

        private class IntDataType
        {
            public int Col { get; set; }
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
            InsertTestDataSQL(-1);
            InsertTestDataSQL(0);
            InsertTestDataSQL(123);
            InsertTestDataSQL(246);
            InsertTestDataSQL(int.MaxValue);
        }

        [TearDown]
        public void TearDown()
        {
            ExecuteNonQuery(DropTestTableSQL());
        }

        [Test]
        public void SelectAllData()
        {
            //Setup 
            var t = new IntTranslator(UNIT_TEST_COL_NAME);
            var cmd = CreateTextCommand(SelectTestDataSQL());
            //Act
            var data = t.Translate(ExecuteDataTable(cmd));
            //Assert
            ClassicAssert.AreEqual(5, data.Count);
        }


        [Test]
        public void SelectAllDataToListOf()
        {
            //Setup 
            var cmd = CreateTextCommand(SelectTestDataSQL());
            //Act
            var data = this.ExecuteToListOf<IntDataType>(cmd);
            //Assert
            ClassicAssert.AreEqual(5, data.Count);
            ClassicAssert.AreEqual(-1, data[0].Col);
        }

        [Test]
        public void SelectAllDataFilter ()
        {
            //setup
            const int testvalue = 123; 
            var t = new IntTranslator(UNIT_TEST_COL_NAME);
            var cmd = CreateTextCommand(SelectTestDataSQL(), "Where col >= @testValue");
            cmd.Parameters.Add(testvalue.ToSqlParameter("@testValue"));

            //Act
            var data = t.Translate(ExecuteDataTable(cmd));

            //Asert
            ClassicAssert.AreEqual(3, data.Count);
        }

        [Test]
        public void SelectAllDataWithWithTranslator()
        {
            //setup
            int testValue = 123;
            var cmd = CreateTextCommand(SelectTestDataSQL(), "Where col = @value").WithParameter(testValue.ToSqlParameter("@value"));
            var t = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<IntDataType>();
            //Act
            var data = t.Translate(ExecuteDataRow(cmd));
            
            ClassicAssert.AreEqual(testValue, data.Col);
        }
    }
}
