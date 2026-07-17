using CA.Blocks.DataAccess.Translator.Basic;
using CA.Blocks.SqliteDataAccess;
using CA.Blocks.SqliteDataAccessUnitTests.Base;

namespace CA.Blocks.SqliteDataAccessUnitTests.SQLLite.DbTypeTests
{
    public class DbTypeNCharTests : UnitTestDataAccess, IDisposable
    {
        private void InsertTestDataSQL(char data)
        {
            ExecuteNonQuery(InsertTestDataSQL($"'{data}'"));
        }
        public DbTypeNCharTests()
{
            ExecuteNonQuery(DropTestTableSQL());
            ExecuteNonQuery(CreateTestTable("NChar not null"));
            InsertTestDataSQL('ä');
            InsertTestDataSQL('B');
            InsertTestDataSQL('C');
            InsertTestDataSQL('D');
            InsertTestDataSQL('E');
        }
        public new void Dispose()
{
            ExecuteNonQuery(DropTestTableSQL());
        }

        [Fact]
        public void SelectAllDataNChar()
        {
            //Setup 
            var cmd = CreateTextCommand(SelectTestDataSQL());
            var t = new CharTranslator(UNIT_TEST_COL_NAME);
            //Act
            var data = t.Translate(ExecuteDataTable(cmd));
            //Assert
            Assert.True(data.Count == 5);
        }

        [Fact]
        public void SelectAllDataNCharWithFilter ()
        {
            //setup
            char testvalue = 'ä';
            var t = new CharTranslator(UNIT_TEST_COL_NAME);
            var cmd = CreateTextCommand(SelectTestDataSQL(), "Where col = @testValue");
            cmd.Parameters.Add(testvalue.ToSqlParameter("@testValue"));

            //Act
            var data = t.Translate(ExecuteDataRow(cmd));

            //Asert
            Assert.Equal(testvalue, data);
        }

        [Fact]
        public void SelectAllDataNCharWithFilter1()
        {
            //setup
            char testvalue = 'B';
            var t = new CharTranslator(UNIT_TEST_COL_NAME);
            var cmd = CreateTextCommand(SelectTestDataSQL(), "Where col = @testValue");
            cmd.Parameters.Add(testvalue.ToSqlParameter("@testValue"));

            //Act
            var data = t.Translate(ExecuteDataRow(cmd));

            //Asert
            Assert.Equal(testvalue, data);
        }
    }
}





