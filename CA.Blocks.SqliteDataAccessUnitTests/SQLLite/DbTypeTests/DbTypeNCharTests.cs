using CA.Blocks.DataAccess.Translator.Basic;
using CA.Blocks.SqliteDataAccess;
using CA.Blocks.SqliteDataAccessUnitTests.Base;
using NUnit.Framework;

namespace CA.Blocks.SqliteDataAccessUnitTests.SQLLite.DbTypeTests
{
    [TestFixture]
    public class DbTypeNCharTests : UnitTestDataAccess
    {
        private void InsertTestDataSQL(char data)
        {
            ExecuteNonQuery(InsertTestDataSQL($"'{data}'"));
        }

        [SetUp]
        public void Setup()
        {
            ExecuteNonQuery(DropTestTableSQL());
            ExecuteNonQuery(CreateTestTable("NChar not null"));
            InsertTestDataSQL('ä');
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
        public void SelectAllDataNChar()
        {
            //Setup 
            var cmd = CreateTextCommand(SelectTestDataSQL());
            var t = new CharTranslator(UNIT_TEST_COL_NAME);
            //Act
            var data = t.Translate(ExecuteDataTable(cmd));
            //Assert
            Assert.AreEqual(5, data.Count);
        }

        [Test]
        public void SelectAllDataNCharWithFilter ()
        {
            //setup
            char testvalue = 'ä';
            var t = new CharTranslator(UNIT_TEST_COL_NAME);
            var cmd = CreateTextCommand(SelectTestDataSQL(), "Where col = @testValue");
            cmd.Parameters.Add(testvalue.ToSqlParameter("@testValue", SpecificSQLCharType.NChar));

            //Act
            var data = t.Translate(ExecuteDataRow(cmd));

            //Asert
            Assert.AreEqual(testvalue, data);

 
        }


    }
}
