using CA.Blocks.MySQLDataAccess;
using CA.Blocks.MySQLDataAccessUnitTests.Base;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace CA.Blocks.MySQLDataAccessUnitTests.MySQL.DbTypeTests
{
    [TestFixture]
    public class DbTypeUIntTests : UnitTestDataAccess
    {

        private class UIntDataType
        {
            public uint Col { get; set; }
        }

        private void InsertTestDataSQL(uint data)
        {
            ExecuteNonQuery(InsertTestDataSQL(data.ToString()));
        }

        [SetUp]
        public void Setup()
        {
            ExecuteNonQuery(DropTestTableSQL());
            ExecuteNonQuery(CreateTestTable("int UNSIGNED not null")); 
            InsertTestDataSQL(0);
            InsertTestDataSQL(0);
            InsertTestDataSQL(123);
            InsertTestDataSQL(246);
            InsertTestDataSQL(uint.MaxValue);
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
            var data = ExecuteDataTable(cmd);
            //Assert
            ClassicAssert.AreEqual(5, data.Rows.Count);
        }

        [Test]
        public void SelectAllDataToListOf()
        {
            //Setup 

            var cmd = CreateTextCommand(SelectTestDataSQL());
            //Act
            var data = ExecuteToListOf<UIntDataType>(cmd);
            //Assert
            ClassicAssert.AreEqual(5, data.Count);
            ClassicAssert.AreEqual(uint.MaxValue, data[4].Col);
        }


        [Test]
        public void SelectAllDataWithFilter ()
        {
            //setup
            const int testvalue = 123;
            var cmd = CreateTextCommand(SelectTestDataSQL(), "Where col >= @testValue");
            cmd.Parameters.Add(testvalue.ToSqlParameter("@testValue"));

            //Act
            var data = ExecuteToListOf<UIntDataType>(cmd);

            //Asert
            ClassicAssert.AreEqual(3, data.Count);
        }


    }
}
