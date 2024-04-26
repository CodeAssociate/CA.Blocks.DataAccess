using System.Text;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Providers;
using CA.Blocks.SQLServerDataAccessUnitTests.Base;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace CA.Blocks.SQLServerDataAccessUnitTests.Translator.DbTypeTests
{
    [TestFixture]
    public class DbTypeBinaryTests : UnitTestDataAccess
    {
        private class BinaryDataType
        {
            public byte[] Col { get; set; }
        }

        private void InsertTestDataToBinarySQL(string data)
        {
            ExecuteNonQuery(InsertTestDataSQL($"CAST( '{data}' AS BINARY(16))"));
        }

        [SetUp]
        public void Setup()
        {
            ExecuteNonQuery(DropTestTableSQL());
            ExecuteNonQuery(CreateTestTable("binary(16) not null"));
            InsertTestDataToBinarySQL("abc");
            InsertTestDataToBinarySQL("def");
            InsertTestDataToBinarySQL("123");
            InsertTestDataToBinarySQL("!@#");
            InsertTestDataToBinarySQL("Binary data");
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
            var data = ExecuteToListOf<BinaryDataType>(cmd);
            //Assert
            ClassicAssert.AreEqual(5, data.Count);

            TestContext.WriteLine();

            ClassicAssert.AreEqual("Binary data", Encoding.ASCII.GetString(data[4].Col, 0, 11));
        }

        [Test]
        public void SelectAllDataWithWithTranslator()
        {
            //setup
            var cmd = CreateTextCommand(SelectTestDataSQL());
            var t = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<BinaryDataType>();
            //Act
            var data = t.Translate(ExecuteDataTable(cmd));
            
            ClassicAssert.AreEqual("Binary data", Encoding.ASCII.GetString(data[4].Col, 0, 11));
        }

    }
}
