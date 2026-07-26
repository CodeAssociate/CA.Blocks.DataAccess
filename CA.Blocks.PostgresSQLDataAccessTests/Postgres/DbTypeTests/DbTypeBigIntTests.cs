using CA.Blocks.DataAccess.Translator.Basic;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Providers;
using CA.Blocks.PostgreSQLDataAccess.Builder;
using CA.Blocks.PostgresSQLDataAccessTests.Base;
using CA.Blocks.SQLServerDataAccess;
using Npgsql;

namespace CA.Blocks.PostgreSQLDataAccessUnitTests.DbTypeTests
{
    [Collection("DbIntegrationTests")]
    public class DbTypeBigIntTests : UnitTestDataAccess, IDisposable
    {
        private class BigIntDataType
        {
            public long Col { get; set; }
        }

        private void InsertTestDataSQL(long data)
        {
            var insertCmd = new SafeSqlBuilder($"Insert into {unitTestTableName:``}(col) values({data:@Data})")
                .BuildSqlCommand();
            ExecuteNonQuery(insertCmd);
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
            base.Dispose();
        }

        [Fact]
        public void SelectAllData()
        {
            //Setup
            var t = new LongTranslator(UNIT_TEST_COL_NAME);
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
            var data = ExecuteToListOf<BigIntDataType>(cmd);
            //Assert
            Assert.Equal(5, data.Count);
            Assert.Equal(-1, data[0].Col);
        }


        [Fact]
        public void SelectAllDataBigIntWithFilter ()
        {
            //setup
            const long testvalue = 123;
            var t = new LongTranslator(UNIT_TEST_COL_NAME);
            var cmd = CreateTextCommand(SelectTestDataSQL("Where col >= @testValue"));
            cmd.Parameters.Add(testvalue.ToPostgresParameter("@testValue"));

            //Act
            var data = t.Translate(ExecuteDataTable(cmd));

            //Asert
            Assert.Equal(3, data.Count);
        }


        [Fact]
        public void SelectAllDataBigIntWithFilterWithParameters()
        {
            //setup
            const long testvalue = 123;
            var t = new LongTranslator(UNIT_TEST_COL_NAME);
            var cmd = CreateTextCommand(SelectTestDataSQL("Where col >= @testValue"))
                .WithParameters(new List<NpgsqlParameter>
                {
                    testvalue.ToPostgresParameter("@testValue")
                });

            //Act
            var data = t.Translate(ExecuteDataTable(cmd));

            //Assert
            Assert.Equal(3, data.Count);
        }

        [Fact]
        public void SelectAllDataWithWithTranslator()
        {
            //setup
            const long testvalue = 123;
            var cmd = CreateTextCommand(SelectTestDataSQL("Where col = @testvalue")).WithParameter(testvalue.ToPostgresParameter("testvalue"));
            var t = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<BigIntDataType>();
            //Act
            var data = t.Translate(ExecuteDataRow(cmd));

            Assert.Equal(123, data.Col);
        }
    }
}
