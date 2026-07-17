using System;
using CA.Blocks.DataAccess.Translator.Basic;
using CA.Blocks.DataAccess.Translator.Extensions;
using CA.Blocks.SqliteDataAccess;
using CA.Blocks.SqliteDataAccessUnitTests.Base;

namespace CA.Blocks.SqliteDataAccessUnitTests.SQLLite.DbTypeTests
{
    public class DbTypeNVarCharTests : UnitTestDataAccess, IDisposable
    {
        private const string  TEST_DATA = "nvarchar data";

        private void InsertTestDataAsTextSQL(string data)
        {
            ExecuteNonQuery(InsertTestDataSQL($"'{data}'"));
        }
        public DbTypeNVarCharTests()
{
            ExecuteNonQuery(DropTestTableSQL());
            ExecuteNonQuery(CreateTestTable("NVarChar(50) not null"));
            InsertTestDataAsTextSQL(TEST_DATA);
            InsertTestDataAsTextSQL(Guid.NewGuid().ToString());
            InsertTestDataAsTextSQL(Guid.NewGuid().ToString());
            InsertTestDataAsTextSQL(Guid.NewGuid().ToString());
            InsertTestDataAsTextSQL(Guid.NewGuid().ToString());
            InsertTestDataAsTextSQL("ä");
        }
        public new void Dispose()
{
            ExecuteNonQuery(DropTestTableSQL());
        }

        [Fact]
        public void SelectAllDataBinary()
        {
            //Setup 
            var cmd = CreateTextCommand(SelectTestDataSQL());
            //Act
            var data = Execute(cmd).ToSingleNamedColumnList<string>(UNIT_TEST_COL_NAME);
            //Assert
            Assert.True(data.Count == 6);
            Assert.Equal(TEST_DATA, data[0]);
            Assert.Equal("ä", data[5]);
        }

        
        [Fact]
        public void SelectDataBinaryWithFilter()
        {
            //setup
            const string testvalue = TEST_DATA;
            var cmd = CreateTextCommand(SelectTestDataSQL(), "Where col = @testValue");
            cmd.Parameters.Add(testvalue.ToSqlParameter("@testValue"));

            //Act
            var data = Execute(cmd).ToSingleNamedColumnList<string>(UNIT_TEST_COL_NAME);

            //Asert
            Assert.Single(data);
        }

    }
}






