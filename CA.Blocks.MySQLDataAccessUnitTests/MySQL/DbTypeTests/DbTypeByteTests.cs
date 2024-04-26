using CA.Blocks.DataAccess.Translator.Basic;
using CA.Blocks.MySQLDataAccess;
using CA.Blocks.MySQLDataAccessUnitTests.Base;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace CA.Blocks.MySQLDataAccessUnitTests.MySQL.DbTypeTests
{
    [TestFixture]
    public class DbTypeByteTests : UnitTestDataAccess
    {
        private class ByteDataType
        {
            public sbyte Col { get; set; }
        }

        private void InsertTestDataSQL(sbyte data)
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
            InsertTestDataSQL(127);
            InsertTestDataSQL(-128);
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
            //Act
            var data = ExecuteToListOf<ByteDataType>(cmd);
            //Assert
            ClassicAssert.AreEqual(6, data.Count);
        }

        [Test]
        public void SelectAllDataToListOf()
        {
            //Setup 

            var cmd = CreateTextCommand(SelectTestDataSQL());
            //Act
            var data = ExecuteToListOf<ByteDataType>(cmd);
            //Assert
            ClassicAssert.AreEqual(6, data.Count);
            ClassicAssert.AreEqual(127, data[4].Col);
            ClassicAssert.AreEqual(-128, data[5].Col);
        }

        [Test]
        public void SelectAllDataByteWithFilter ()
        {
            //setup
            const byte testvalue = 123;
            var cmd = CreateTextCommand(SelectTestDataSQL(), "Where col >= @testValue");
            cmd.Parameters.Add(testvalue.ToSqlParameter("@testValue"));

            //Act
            var data = ExecuteToListOf<ByteDataType>(cmd);

            //Asert
            ClassicAssert.AreEqual(1, data.Count);
        }


    }
}
