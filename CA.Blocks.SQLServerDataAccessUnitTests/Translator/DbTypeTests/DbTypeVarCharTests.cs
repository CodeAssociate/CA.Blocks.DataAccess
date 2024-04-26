using System;
using CA.Blocks.DataAccess.Translator.Basic;
using CA.Blocks.SQLServerDataAccess;
using CA.Blocks.SQLServerDataAccessUnitTests.Base;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace CA.Blocks.SQLServerDataAccessUnitTests.Translator.DbTypeTests
{
    [TestFixture]
    public class DbTypeVarCharTests : UnitTestDataAccess
    {
        private const string  TEST_DATA = "varchar data";

        private void InsertTestData(string data)
        {
            ExecuteNonQuery(InsertTestDataSQL($"'{data}'"));
        }

        [SetUp]
        public void Setup()
        {
            ExecuteNonQuery(DropTestTableSQL());
            ExecuteNonQuery(CreateTestTable("varchar(50) not null"));
            InsertTestData(TEST_DATA);
            InsertTestData(Guid.NewGuid().ToString());
            InsertTestData(Guid.NewGuid().ToString());
            InsertTestData(Guid.NewGuid().ToString());
            InsertTestData(Guid.NewGuid().ToString());
        }

        [TearDown]
        public void TearDown()
        {
            ExecuteNonQuery(DropTestTableSQL());
        }

        [Test]
        public void SelectAllDataBinary()
        {
            //Setup 
            var t = new StringTranslator(UNIT_TEST_COL_NAME);
            var cmd = CreateTextCommand(SelectTestDataSQL());
            //Act
            var data = t.Translate(ExecuteDataTable(cmd));
            //Assert
            ClassicAssert.AreEqual(5, data.Count);
            ClassicAssert.AreEqual(TEST_DATA, data[0]);
        }

        
        [Test]
        public void SelectDataBinaryWithFilter()
        {
            //setup
            const string testvalue = TEST_DATA;
            var t = new StringTranslator(UNIT_TEST_COL_NAME);
            var cmd = CreateTextCommand(SelectTestDataSQL(), "Where col = @testValue");
            cmd.Parameters.Add(testvalue.ToSqlParameter("@testValue"));

            //Act
            var data = t.Translate(ExecuteDataTable(cmd));

            //Asert
            ClassicAssert.AreEqual(1, data.Count);
        }
    }
}
