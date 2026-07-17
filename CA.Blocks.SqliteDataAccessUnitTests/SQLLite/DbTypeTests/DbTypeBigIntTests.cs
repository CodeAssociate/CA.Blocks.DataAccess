using System.Collections.Generic;
using CA.Blocks.DataAccess.Translator.Extensions;
using CA.Blocks.SqliteDataAccess;
using CA.Blocks.SqliteDataAccessUnitTests.Base;
using Microsoft.Data.Sqlite;

namespace CA.Blocks.SqliteDataAccessUnitTests.SQLLite.DbTypeTests
{
    public class DbTypeBigIntTests : UnitTestDataAccess, IDisposable
    {
        private void InsertTestDataSQL(long data)
        {
            ExecuteNonQuery(InsertTestDataSQL(data.ToString()));
        }
        public DbTypeBigIntTests()
{

            ExecuteNonQuery(DropTestTableSQL());
            ExecuteNonQuery(CreateTestTable("bigint not null"));
            InsertTestDataSQL(-1);
            InsertTestDataSQL(0);
            InsertTestDataSQL(123);
            InsertTestDataSQL(246);
            InsertTestDataSQL((long)int.MaxValue + (long)int.MaxValue);
        }
        public new void Dispose()
{
            ExecuteNonQuery(DropTestTableSQL());
        }

        [Fact]
        public void SelectAllDataBigInt()
        {
            //Setup 
            var cmd = CreateTextCommand(SelectTestDataSQL());
            //Act
            var data = Execute(cmd).ToSingleNamedColumnList<long>(UNIT_TEST_COL_NAME);
            //Assert
            Assert.True(data.Count == 5);
        }

        [Fact]
        public void SelectAllDataBigIntWithFilter ()
        {
            //setup
            const long testvalue = 123;
            var cmd = CreateTextCommand(SelectTestDataSQL(), "Where col >= @testValue");
            cmd.Parameters.Add(testvalue.ToSqlParameter("@testValue"));

            //Act
            var data = Execute(cmd).ToSingleNamedColumnList<long>(UNIT_TEST_COL_NAME);

            //Asert
            Assert.True(data.Count == 3);
        }

        
        [Fact]
        public void SelectAllDataBigIntWithFilterAndSugger()
        {
            //setup
            const long testvalue = 123;
            var cmd = CreateTextCommand(SelectTestDataSQL(), "Where col >= @testValue")
                .WithParameters(new List<SqliteParameter>
                {
                    testvalue.ToSqlParameter("@testValue")
                });

            //Act
            var data = Execute(cmd).ToSingleNamedColumnList<long>(UNIT_TEST_COL_NAME);

            //Asert
            Assert.True(data.Count == 3);
        }
        
        
    }
}





