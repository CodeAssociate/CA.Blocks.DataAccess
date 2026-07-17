using System.Text;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Providers;
using CA.Blocks.SQLServerDataAccessUnitTests.Base;

namespace CA.Blocks.SQLServerDataAccessUnitTests.Translator.DbTypeTests
{
    public class DbTypeBinaryTests : UnitTestDataAccess, IDisposable
    {
        private class BinaryDataType
        {
            public byte[] Col { get; set; }
        }

        private void InsertTestDataToBinarySQL(string data)
        {
            ExecuteNonQuery(InsertTestDataSQL($"CAST( '{data}' AS BINARY(16))"));
        }

        public DbTypeBinaryTests()
        {
            ExecuteNonQuery(DropTestTableSQL());
            ExecuteNonQuery(CreateTestTable("binary(16) not null"));
            InsertTestDataToBinarySQL("abc");
            InsertTestDataToBinarySQL("def");
            InsertTestDataToBinarySQL("123");
            InsertTestDataToBinarySQL("!@#");
            InsertTestDataToBinarySQL("Binary data");
        }

        public new void Dispose()
        {
            ExecuteNonQuery(DropTestTableSQL());
        }

        [Fact]
        public void SelectAllData()
        {
            //Setup 
            var cmd = CreateTextCommand(SelectTestDataSQL());
            //Act
            var data = ExecuteDataTable(cmd);
            //Assert
            Assert.Equal(5, data.Rows.Count);
        }

        [Fact]
        public void SelectAllDataToListOf()
        {
            //Setup 

            var cmd = CreateTextCommand(SelectTestDataSQL());
            //Act
            var data = ExecuteToListOf<BinaryDataType>(cmd);
            //Assert
            Assert.Equal(5, data.Count);

            Console.WriteLine();

            Assert.Equal("Binary data", Encoding.ASCII.GetString(data[4].Col, 0, 11));
        }

        [Fact]
        public void SelectAllDataWithWithTranslator()
        {
            //setup
            var cmd = CreateTextCommand(SelectTestDataSQL());
            var t = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<BinaryDataType>();
            //Act
            var data = t.Translate(ExecuteDataTable(cmd));
            
            Assert.Equal("Binary data", Encoding.ASCII.GetString(data[4].Col, 0, 11));
        }

    }
}




