using System.Text;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Providers;
using CA.Blocks.DataAccess.Translator.Extensions;
using CA.Blocks.PostgresDataAccessTests.Base;

namespace CA.Blocks.PostgresDataAccessTests.Postgres.DbTypeTests
{
    //NOTE Postgres has not fix size binary
    // TODO you can use a bit(a) will need to do example...

    [Collection("DbIntegrationTests")]
    public class DbTypeBinaryTests : UnitTestDataAccess, IDisposable
    {
        private class BinaryDataType
        {
            public required byte[] Col { get; set; }
        }

        private void InsertTestDataToBinarySQL(string data)
        {
            ExecuteNonQuery(InsertTestDataSQL($"CAST( '{data}' AS bytea)"));
        }

        public DbTypeBinaryTests()
        {
            ExecuteNonQuery(DropTestTableSQL());
            ExecuteNonQuery(CreateTestTable("bytea not null"));
            InsertTestDataToBinarySQL("abc");
            InsertTestDataToBinarySQL("def");
            InsertTestDataToBinarySQL("123");
            InsertTestDataToBinarySQL("!@#");
            InsertTestDataToBinarySQL("Binary data");
        }

        public new void Dispose()
        {
            ExecuteNonQuery(DropTestTableSQL());
            base.Dispose();
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
            var data = Execute(cmd).ToListOf<BinaryDataType>();
            //Assert
            Assert.Equal(5, data.Count);
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
