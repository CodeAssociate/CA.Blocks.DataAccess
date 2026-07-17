using CA.Blocks.DataAccess.Translator.Basic;
using CA.Blocks.SqliteDataAccess;
using CA.Blocks.SqliteDataAccessUnitTests.Base;

namespace CA.Blocks.SqliteDataAccessUnitTests.SQLLite.DbTypeTests
{
    public class DbTypeShortTests : UnitTestDataAccess, IDisposable
    {
        private void InsertTestDataSQL(short data)
        {
            ExecuteNonQuery(InsertTestDataSQL(data.ToString()));
        }
        public DbTypeShortTests()
{
            ExecuteNonQuery(DropTestTableSQL());
            ExecuteNonQuery(CreateTestTable("smallint not null"));
            InsertTestDataSQL(-1);
            InsertTestDataSQL(0);
            InsertTestDataSQL(123);
            InsertTestDataSQL(246);
            InsertTestDataSQL(short.MaxValue);
        }
        public new void Dispose()
{
            ExecuteNonQuery(DropTestTableSQL());
        }

        [Fact]
        public void SelectAllDataInt()
        {
            //Setup 
            var t = new ShortTranslator(UNIT_TEST_COL_NAME);
            var cmd = CreateTextCommand(SelectTestDataSQL());
            //Act
            var data = t.Translate(ExecuteDataTable(cmd));
            //Assert
            Assert.True(data.Count == 5);
        }

        [Fact]
        public void SelectAllDataIntWithFilter ()
        {
            //setup
            const int testvalue = 123; 
            var t = new ShortTranslator(UNIT_TEST_COL_NAME);
            var cmd = CreateTextCommand(SelectTestDataSQL(), "Where col >= @testValue");
            cmd.Parameters.Add(testvalue.ToSqlParameter("@testValue"));

            //Act
            var data = t.Translate(ExecuteDataTable(cmd));

            //Asert
            Assert.True(data.Count == 3);
        }


    }
}





