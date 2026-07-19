using System.Text;
using CA.Blocks.DataAccess;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Providers;
using CA.Blocks.DataAccess.Translator.Extensions;
using CA.Blocks.SqliteDataAccess;
using CA.Blocks.SqliteDataAccessUnitTests.Base;

namespace CA.Blocks.SqliteDataAccessUnitTests.SQLLite.DbTypeTests
{
    public class DbTypeBinaryTests : UnitTestDataAccess, IDisposable
    {
        private class BinaryDataType
        {
            public required byte[] Col { get; set; }
        }

        private void InsertTestDataToBinarySQL(string data)
        {
            var cmd = CreateTextCommand(InsertTestDataSQL("@Data")).WithParameter(Encoding.ASCII.GetBytes(data).ToSqlParameter("@Data"));
            ExecuteNonQuery(cmd);
        }

        public DbTypeBinaryTests()
        {
            ExecuteNonQuery(DropTestTableSQL());
            ExecuteNonQuery(CreateTestTable("BLOB not null"));
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
            var data = Execute(cmd).ToListOf<BinaryDataType>();
            //Assert
            Assert.True(data.Count == 5);
        }

        [Fact]
        public void SelectAllDataToListOf()
        {
            //Setup 

            var cmd = CreateTextCommand(SelectTestDataSQL());
            //Act
            var data = Execute(cmd).ToListOf<BinaryDataType>();
            //Assert
            Assert.True(data.Count == 5);

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




