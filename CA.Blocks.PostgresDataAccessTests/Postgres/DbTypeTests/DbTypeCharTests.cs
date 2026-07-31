using CA.Blocks.DataAccess.Translator.Basic;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Providers;
using CA.Blocks.DataAccess.Translator.Extensions;
using CA.Blocks.PostgresDataAccess;
using CA.Blocks.PostgresDataAccessTests.Base;

namespace CA.Blocks.PostgresDataAccessTests.Postgres.DbTypeTests
{
    [Collection("DbIntegrationTests")]
    public class DbTypeCharTests : UnitTestDataAccess, IDisposable
    {
        private class CharDataType
        {
            public char Col { get; set; }
        }

        private void InsertTestDataSQL(char data)
        {
            ExecuteNonQuery(InsertTestDataSQL($"'{data}'"));
        }

        public DbTypeCharTests()
        {
            ExecuteNonQuery(DropTestTableSQL());
            ExecuteNonQuery(CreateTestTable("char not null"));
            InsertTestDataSQL('A');
            InsertTestDataSQL('B');
            InsertTestDataSQL('C');
            InsertTestDataSQL('D');
            InsertTestDataSQL('E');
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
            var t = new CharTranslator(UNIT_TEST_COL_NAME);
            //Act
            var data = t.Translate(Execute(cmd).ToDataTable());
            //Assert
            Assert.Equal(5, data.Count);
        }

        [Fact]
        public void SelectAllDataToListOf()
        {
            //Setup
            var cmd = CreateTextCommand(SelectTestDataSQL());
            //Act
            var data = ExecuteToListOf<CharDataType>(cmd);
            //Assert
            Assert.Equal(5, data.Count);
            Assert.Equal('E', data[4].Col);
        }

        [Fact]
        public void SelectAllDataBigIntWithFilter ()
        {
            //setup
            const char testvalue = 'A';
            var t = new CharTranslator(UNIT_TEST_COL_NAME);
            var cmd = CreateTextCommand(SelectTestDataSQL("Where col = @testValue"));
            cmd.Parameters.Add(testvalue.ToPostgresParameter("@testValue"));

            //Act
            var data = t.Translate(Execute(cmd).ToDataRow());

            //Asert
            Assert.Equal('A', data);
        }

        [Fact]
        public void SelectAllDataWithWithTranslator()
        {
            //setup
            const char testValue = 'A';
            var cmd = CreateTextCommand(SelectTestDataSQL("Where col = @value")).WithParameter(testValue.ToPostgresParameter("@value"));
            var t = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<CharDataType>();
            //Act
            var data = t.Translate(Execute(cmd).ToDataRow());

            Assert.Equal(testValue, data.Col);
        }
    }
}
