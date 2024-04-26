using System.Text;
using CA.Blocks.DataAccess;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Providers;
using CA.Blocks.DataAccess.Translator.Extensions;
using CA.Blocks.SqliteDataAccess;
using CA.Blocks.SqliteDataAccessUnitTests.Base;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace CA.Blocks.SqliteDataAccessUnitTests.SQLLite.DbTypeTests
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
            var cmd = CreateTextCommand(InsertTestDataSQL("@Data")).WithParameter(Encoding.ASCII.GetBytes(data).ToSqlParameter("@Data"));
            ExecuteNonQuery(cmd);
        }

        [SetUp]
        public void Setup()
        {
            ExecuteNonQuery(DropTestTableSQL());
            ExecuteNonQuery(CreateTestTable("BLOB not null"));
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
            var data = Execute(cmd).ToListOf<BinaryDataType>();
            //Assert
            ClassicAssert.AreEqual(5, data.Count);
        }

        [Test]
        public void SelectAllDataToListOf()
        {
            //Setup 

            var cmd = CreateTextCommand(SelectTestDataSQL());
            //Act
            var data = Execute(cmd).ToListOf<BinaryDataType>();
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
