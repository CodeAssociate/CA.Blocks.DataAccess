using CA.Blocks.DataAccess.Translator.Basic;
using CA.Blocks.SQLServerDataAccess;
using CA.Blocks.SQLServerDataAccessUnitTests.Base;
using NUnit.Framework;

namespace CA.Blocks.SQLServerDataAccessUnitTests.SQLServer.DbTypeTests
{
    [TestFixture]
    public class DbTypeCharTests : UnitTestDataAccess
    {
        private void InsertTestDataSQL(char data)
        {
            ExecuteNonQuery(InsertTestDataSQL(string.Format("'{0}'", data)));
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
        public void SelectAllDataChar()
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
        public void SelectAllDataBigIntWithFilter ()
        {
            //setup
            const char testvalue = 'A';
            var t = new CharTranslator(UNIT_TEST_COL_NAME);
            var cmd = CreateTextCommand(SelectTestDataSQL(), "Where col = @testValue");
            cmd.Parameters.Add(testvalue.ToSqlParameter("@testValue"));

            //Act
            var data = t.Translate(ExecuteDataRow(cmd));

            //Asert
            Assert.AreEqual('A', data);
        }


    }
}
