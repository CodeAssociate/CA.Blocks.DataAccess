using System.Text;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Providers;
using CA.Blocks.DataAccess.Translator.Extensions;
using CA.Blocks.SQLServerDataAccessUnitTests.Base;

namespace CA.Blocks.SQLServerDataAccessUnitTests.Translator.DbTypeTests
{
    [Collection("DbIntegrationTests")]
    public class DbTypeBinaryTests : UnitTestDataAccess, IDisposable
    {
        private class BinaryDataType
        {
            public byte[]? Col { get; set; }
        }

        private void InsertTestDataToBinarySQL(string data)
        {
            ExecuteNonQuery(InsertTestDataCommand(Encoding.ASCII.GetBytes(data)));
        }

        public DbTypeBinaryTests()
        {
            ExecuteNonQuery(DropTestTableCommand());
            var createCommand = CreateTestTableCommand("binary");
            createCommand.CommandText = createCommand.CommandText.Replace("[binary]", "[binary](16)");
            ExecuteNonQuery(createCommand);
            InsertTestDataToBinarySQL("abc");
            InsertTestDataToBinarySQL("def");
            InsertTestDataToBinarySQL("123");
            InsertTestDataToBinarySQL("!@#");
            InsertTestDataToBinarySQL("Binary data");
        }

        public void Dispose()
        {
            ExecuteNonQuery(DropTestTableCommand());
        }

        [Fact]
        public void SelectAllData()
        {
            //Setup 
            var cmd = BasicSelectCommand();
            //Act
            var data = Execute(cmd).ToSingleNamedColumnList<byte[]>(UNIT_TEST_COL_NAME);
            //Assert
            Assert.Equal(5, data.Count);
        }

        [Fact]
        public void SelectAllDataToListOf()
        {
            //Setup 

            var cmd = BasicSelectCommand();
            //Act
            var data = Execute(cmd).ToListOf<BinaryDataType>();
            //Assert
            Assert.Equal(5, data.Count);

            Console.WriteLine();
            Assert.NotNull(data[4]);
            Assert.NotNull(data[4].Col);
            Assert.Equal("Binary data", Encoding.ASCII.GetString(data[4].Col!, 0, 11));
        }

        [Fact]
        public void SelectAllDataWithWithTranslator()
        {
            //setup
            var cmd = BasicSelectCommand();
            var t = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<BinaryDataType>();
            //Act
            var data = t.Translate(Execute(cmd).ToDataTable());
            Assert.NotNull(data[4]);
            Assert.NotNull(data[4].Col);
            Assert.Equal("Binary data", Encoding.ASCII.GetString(data[4].Col!, 0, 11));
        }

    }
}




