using System;
using CA.Blocks.DataAccess.Translator.Basic;
using CA.Blocks.SqliteDataAccess;
using CA.Blocks.SqliteDataAccessUnitTests.Base;

namespace CA.Blocks.SqliteDataAccessUnitTests.SQLLite.DbTypeTests
{
    public class DbTypeVarCharTests : UnitTestDataAccess, IDisposable
    {
        private const string  TEST_DATA = "varchar data";

        private void InsertTestData(string data)
        {
            ExecuteNonQuery(InsertTestDataSQL($"'{data}'"));
        }
        public DbTypeVarCharTests()
{
            ExecuteNonQuery(DropTestTableSQL());
            ExecuteNonQuery(CreateTestTable("varchar(50) not null"));
            InsertTestData(TEST_DATA);
            InsertTestData(Guid.NewGuid().ToString());
            InsertTestData(Guid.NewGuid().ToString());
            InsertTestData(Guid.NewGuid().ToString());
            InsertTestData(Guid.NewGuid().ToString());
        }
        public new void Dispose()
{
            ExecuteNonQuery(DropTestTableSQL());
        }

        [Fact]
        public void SelectAllDataBinary()
        {
            //Setup 
            var t = new StringTranslator(UNIT_TEST_COL_NAME);
            var cmd = CreateTextCommand(SelectTestDataSQL());
            //Act
            var data = t.Translate(ExecuteDataTable(cmd));
            //Assert
            Assert.True(data.Count == 5);
            Assert.Equal(TEST_DATA, data[0]);
        }

        
        [Fact]
        public void SelectDataBinaryWithFilter()
        {
            //setup
            const string testvalue = TEST_DATA;
            var t = new StringTranslator(UNIT_TEST_COL_NAME);
            var cmd = CreateTextCommand(SelectTestDataSQL(), "Where col = @testValue");
            cmd.Parameters.Add(testvalue.ToSqlParameter("@testValue"));

            //Act
            var data = t.Translate(ExecuteDataTable(cmd));

            //Asert
            Assert.Single(data);
        }
    }
}






