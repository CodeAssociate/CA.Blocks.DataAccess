using CA.Blocks.DataAccess.Translator.Basic;
using CA.Blocks.SQLServerDataAccess;
using CA.Blocks.SQLServerDataAccessUnitTests.Base;
using NUnit.Framework;

namespace CA.Blocks.SQLServerDataAccessUnitTests.SQLServer.DbTypeTests
{
    [TestFixture]
    public class DbTypeByteTests : UnitTestDataAccess
    {
        private class ByteDataType
        {
            public byte Col { get; set; }
        }

        private void InsertTestDataSQL(byte data)
        {
            ExecuteNonQuery(InsertTestDataSQL(data.ToString()));
        }

        [SetUp]
        public void Setup()
        {
            ExecuteNonQuery(DropTestTableSQL());
            ExecuteNonQuery(CreateTestTable("tinyint not null"));
            InsertTestDataSQL(0);
            InsertTestDataSQL(1);
            InsertTestDataSQL(2);
            InsertTestDataSQL(4);
            InsertTestDataSQL(byte.MaxValue);
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
            var t = new ByteTranslator(UNIT_TEST_COL_NAME);
            var cmd = CreateTextCommand(SelectTestDataSQL());
            //Act
            var data = t.Translate(ExecuteDataTable(cmd));
            //Assert
            Assert.AreEqual(5, data.Count);
        }

        [Test]
        public void SelectAllDataToListOf()
        {
            //Setup 

            var cmd = CreateTextCommand(SelectTestDataSQL());
            //Act
            var data = ExecuteToListOf<ByteDataType>(cmd);
            //Assert
            Assert.AreEqual(5, data.Count);
            Assert.AreEqual(byte.MaxValue, data[4].Col);
        }

        [Test]
        public void SelectAllDataByteWithFilter ()
        {
            //setup
            const byte testvalue = 123; 
            var t = new ByteTranslator(UNIT_TEST_COL_NAME);
            var cmd = CreateTextCommand(SelectTestDataSQL(), "Where col >= @testValue");
            cmd.Parameters.Add(testvalue.ToSqlParameter("@testValue"));

            //Act
            var data = t.Translate(ExecuteDataTable(cmd));

            //Asert
            Assert.AreEqual(1, data.Count);
        }


    }
}
