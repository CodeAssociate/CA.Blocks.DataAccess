using System;
using System.Text;
using CA.Blocks.DataAccess.Translator.Basic;
using CA.Blocks.MySQLDataAccess;
using CA.Blocks.MySQLDataAccessUnitTests.Base;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace CA.Blocks.MySQLDataAccessUnitTests.MySQL.DbTypeTests
{
    [TestFixture]
    public class DbTypeVarBinaryTests : UnitTestDataAccess
    {
        private const string  TEST_DATA = "Binary Data 1";

        private void InsertTestDataAsBinarySQL(string data)
        {
            var bytes = Encoding.ASCII.GetBytes(data);
            string hexdata = BitConverter.ToString(bytes);
            ExecuteNonQuery(InsertTestDataSQL($"x'{hexdata.Replace("-", string.Empty)}'"));
        }

        [SetUp]
        public void Setup()
        {
            ExecuteNonQuery(DropTestTableSQL());
            ExecuteNonQuery(CreateTestTable("varbinary(50) not null"));
            InsertTestDataAsBinarySQL(TEST_DATA);
            InsertTestDataAsBinarySQL("Binary Data 2");
            InsertTestDataAsBinarySQL("Binary Data 3");
            InsertTestDataAsBinarySQL("Binary Data 4");
            InsertTestDataAsBinarySQL("Binary Data 5");
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
            var t = new BinaryTranslator(UNIT_TEST_COL_NAME);
            var cmd = CreateTextCommand(SelectTestDataSQL());
            //Act
            var data = t.Translate(ExecuteDataTable(cmd));
            //Assert
            ClassicAssert.AreEqual(5, data.Count);
            ClassicAssert.AreEqual(TEST_DATA, Encoding.ASCII.GetString(data[0]));
        }

        
        [Test]
        public void SelectDataBinaryWithFilter()
        {
            //setup
            byte[] testvalue = Encoding.ASCII.GetBytes(TEST_DATA);
            var t = new BinaryTranslator(UNIT_TEST_COL_NAME);
            var cmd = CreateTextCommand(SelectTestDataSQL(), "Where col = @testValue");
            cmd.Parameters.Add(testvalue.ToSqlParameter("@testValue"));

            //Act
            var data = t.Translate(ExecuteDataTable(cmd));

            //Asert
            ClassicAssert.AreEqual(1, data.Count);
            ClassicAssert.AreEqual(testvalue, data[0]);
        }


    }
}
