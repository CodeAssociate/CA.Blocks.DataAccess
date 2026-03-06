using CA.Blocks.DataAccess;
using CA.Blocks.DataAccess.Translator.Basic;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Providers;
using CA.Blocks.PostgreSQLDataAccessUnitTests.Base;
using CA.Blocks.SQLServerDataAccess;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace CA.Blocks.PostgreSQLDataAccessUnitTests.DbTypeTests
{
    [TestFixture]
    public class DbTypeCharTests : UnitTestDataAccess
    {
        private class CharDataType
        {
            public char Col { get; set; }
        }

        private void InsertTestDataSQL(char data)
        {
            ExecuteNonQuery(InsertTestDataSQL($"'{data}'"));
        }

        [SetUp]
        public void Setup()
        {
            ExecuteNonQuery(DropTestTableSQL());
            ExecuteNonQuery(CreateTestTable("char not null"));
            InsertTestDataSQL('A');
            InsertTestDataSQL('B');
            InsertTestDataSQL('C');
            InsertTestDataSQL('D');
            InsertTestDataSQL('E');

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
            var cmd = CreateTextCommand(SelectTestDataSQL());
            var t = new CharTranslator(UNIT_TEST_COL_NAME);
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
            var data = ExecuteToListOf<CharDataType>(cmd);
            //Assert
            ClassicAssert.AreEqual(5, data.Count);
            ClassicAssert.AreEqual('E', data[4].Col);
        }



        [Test]
        public void SelectAllDataBigIntWithFilter ()
        {
            //setup
            const char testvalue = 'A';
            var t = new CharTranslator(UNIT_TEST_COL_NAME);
            var cmd = CreateTextCommand(SelectTestDataSQL("Where col = @testValue"));
            cmd.Parameters.Add(testvalue.ToPostgresParameter("@testValue"));

            //Act
            var data = t.Translate(ExecuteDataRow(cmd));

            //Asert
            ClassicAssert.AreEqual('A', data);
        }

        [Test]
        public void SelectAllDataWithWithTranslator()
        {
            //setup
            const char testValue = 'A';
            var cmd = CreateTextCommand(SelectTestDataSQL("Where col = @value")).WithParameter(testValue.ToPostgresParameter("@value"));
            var t = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<CharDataType>();
            //Act
            var data = t.Translate(ExecuteDataRow(cmd));
            
            ClassicAssert.AreEqual(testValue, data.Col);
        }

    }
}
