using CA.Blocks.DataAccess;
using CA.Blocks.DataAccess.Translator.Basic;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Providers;
using CA.Blocks.PostgresSQLDataAccessTests.Base;
using CA.Blocks.SQLServerDataAccess;

namespace CA.Blocks.PostgreSQLDataAccessUnitTests.DbTypeTests
{
    [Collection("DbIntegrationTests")]
    public class DbTypeByteTests : UnitTestDataAccess, IDisposable
    {
        private class ByteDataType
        {
            public byte Col { get; set; }
        }

        private void InsertTestDataSQL(byte data)
        {
            ExecuteNonQuery(InsertTestDataSQL(data.ToString()));
        }

        public DbTypeByteTests()
        {
            // Test using smallint as Postgres does not have a byte type
            ExecuteNonQuery(DropTestTableSQL());
            ExecuteNonQuery(CreateTestTable("smallint not null"));
            InsertTestDataSQL(0);
            InsertTestDataSQL(1);
            InsertTestDataSQL(2);
            InsertTestDataSQL(4);
            InsertTestDataSQL(byte.MaxValue);
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
            var t = new ByteTranslator(UNIT_TEST_COL_NAME);
            var cmd = CreateTextCommand(SelectTestDataSQL());
            //Act
            var data = t.Translate(ExecuteDataTable(cmd));
            //Assert
            Assert.Equal(5, data.Count);
        }

        [Fact]
        public void SelectAllDataToListOf()
        {
            //Setup
            var cmd = CreateTextCommand(SelectTestDataSQL());
            //Act
            var data = ExecuteToListOf<ByteDataType>(cmd);
            //Assert
            Assert.Equal(5, data.Count);
            Assert.Equal(byte.MaxValue, data[4].Col);
        }

        [Fact]
        public void SelectAllDataByteWithFilter ()
        {
            //setup
            const byte testvalue = 123;
            var t = new ByteTranslator(UNIT_TEST_COL_NAME);
            var cmd = CreateTextCommand(SelectTestDataSQL("Where col >= @testValue"));
            cmd.Parameters.Add(testvalue.ToPostgresParameter("@testValue"));

            //Act
            var data = t.Translate(ExecuteDataTable(cmd));

            //Asert
            Assert.Single(data);
        }

        [Fact]
        public void SelectAllDataWithWithTranslator()
        {
            //setup
            const byte testValue = 1;
            var cmd = CreateTextCommand(SelectTestDataSQL("Where col = @value")).WithParameter(testValue.ToPostgresParameter("@value"));
            var t = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<ByteDataType>();
            //Act
            var data = t.Translate(ExecuteDataRow(cmd));

            Assert.Equal(testValue, data.Col);
        }
    }
}
