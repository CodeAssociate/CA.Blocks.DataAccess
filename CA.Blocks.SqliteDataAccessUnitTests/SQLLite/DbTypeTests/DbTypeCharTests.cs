using System.Linq;
using CA.Blocks.DataAccess.Translator.Basic;
using CA.Blocks.DataAccess.Translator.Extensions;
using CA.Blocks.SqliteDataAccess;
using CA.Blocks.SqliteDataAccessUnitTests.Base;
using NUnit.Framework;

namespace CA.Blocks.SqliteDataAccessUnitTests.SQLLite.DbTypeTests
{
    [TestFixture]
    public class DbTypeCharTests : UnitTestDataAccess
    {
        private void InsertTestDataSQL(char data)
        {
            ExecuteNonQuery(InsertTestDataSQL($"'{data}'"));
        }

        [SetUp]
        public void Setup()
        {
            ExecuteNonQuery(DropTestTableSQL());
            ExecuteNonQuery(CreateTestTable("char(1) not null"));
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
            var data = Execute(cmd).ToSingleNamedColumnList<char>(UNIT_TEST_COL_NAME);
            //Assert
            Assert.AreEqual(5, data.Count);
        }

        [Test]
        public void SelectAllDataBigIntWithFilter ()
        {
            //setup
            const char testvalue = 'A';
            var cmd = CreateTextCommand(SelectTestDataSQL(), "Where col = @testValue");
            cmd.Parameters.Add(testvalue.ToSqlParameter("@testValue"));

            //Act
            var data = Execute(cmd).ToSingleNamedColumnList<char>(UNIT_TEST_COL_NAME).FirstOrDefault();

            //Asert
            Assert.AreEqual('A', data);
        }


    }
}
