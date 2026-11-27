using System.Text;
using CA.Blocks.DataAccess.Translator.Basic;
using CA.Blocks.DataAccess.Translator.Extensions;
using CA.Blocks.SQLServerDataAccess;
using CA.Blocks.SQLServerDataAccessUnitTests.Base;

namespace CA.Blocks.SQLServerDataAccessUnitTests.Translator.DbTypeTests
{
    [Collection("DbIntegrationTests")]
    public class DbTypeVarBinaryTests : UnitTestDataAccess, IDisposable
    {
        private const string  TEST_DATA = "Binary Data 1";

        private void InsertTestDataAsBinarySQL(string data)
        {
            ExecuteNonQuery(InsertTestDataCommand(Encoding.ASCII.GetBytes(data)));
        }

        public DbTypeVarBinaryTests()
        {
            ExecuteNonQuery(DropTestTableCommand());
            var create = CreateTestTableCommand("varbinary");
            create.CommandText = create.CommandText.Replace("[varbinary]", "[varbinary](50)");
            ExecuteNonQuery(create);
            InsertTestDataAsBinarySQL(TEST_DATA);
            InsertTestDataAsBinarySQL("Binary Data 2");
            InsertTestDataAsBinarySQL("Binary Data 3");
            InsertTestDataAsBinarySQL("Binary Data 4");
            InsertTestDataAsBinarySQL("Binary Data 5");
        }

        public void Dispose()
        {
            ExecuteNonQuery(DropTestTableCommand());
        }

        [Fact]
        public void SelectAllDataBinary()
        {
            //Setup 
            var t = new BinaryTranslator(UNIT_TEST_COL_NAME);
            var cmd = BasicSelectCommand();
            //Act
            var data = t.Translate(Execute(cmd).ToDataTable());
            //Assert
            Assert.Equal(5, data.Count);
            Assert.Equal(TEST_DATA, Encoding.ASCII.GetString(data[0]));
            var streamed = Execute(BasicSelectCommand()).ToSingleNamedColumnList<byte[]>(UNIT_TEST_COL_NAME);
            Assert.Equal(TEST_DATA, Encoding.ASCII.GetString(streamed[0]));
        }

        
        [Fact]
        public void SelectDataBinaryWithFilter()
        {
            //setup
            byte[] testvalue = Encoding.ASCII.GetBytes(TEST_DATA);
            var t = new BinaryTranslator(UNIT_TEST_COL_NAME);
            var cmd = BasicSelectCommand($"WHERE {UNIT_TEST_COL_NAME:[]} = {testvalue:@testValue}");

            //Act
            var data = t.Translate(Execute(cmd).ToDataTable());

            //Asert
            Assert.Single(data);
        }


    }
}




