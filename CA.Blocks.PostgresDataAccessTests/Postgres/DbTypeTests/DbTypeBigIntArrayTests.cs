using CA.Blocks.DataAccess.Translator.Extensions;
using CA.Blocks.PostgresDataAccess.Builder;
using CA.Blocks.PostgresDataAccessTests.Base;

namespace CA.Blocks.PostgresDataAccessTests.Postgres.DbTypeTests
{

    [Collection("DbIntegrationTests")]
    public class DbTypeBigIntArrayTests : UnitTestDataAccess, IDisposable
    {


        public DbTypeBigIntArrayTests()
        {
            ExecuteNonQuery(DropTestTableSQL());
            ExecuteNonQuery(CreateTestTable("bigint[] not null"));
            InsertTestDataSQL([1, 2, 3]);
            InsertTestDataSQL([1, 3, 5]);
            InsertTestDataSQL([2, 4, 8]);
            InsertTestDataSQL([(long)int.MaxValue + (long)int.MaxValue, (long)int.MaxValue]);
        }


        public new void Dispose()
        {

            ExecuteNonQuery(DropTestTableSQL());
            base.Dispose();
    
        }

        private class BigIntArrayDataType
        {
            public required List<long> Col { get; init; }
        }

        private void InsertTestDataSQL(long[] data)
        {
            var insertCmd = new SafeSqlBuilder($"Insert into {unitTestTableName:``} (col) values({data:@Data})")
                .BuildSqlCommand();
            ExecuteNonQuery(insertCmd);
        }


        [Fact]
        public void SelectAllDataToDataTable()
        {
            //Setup 
            var cmd = CreateTextCommand(SelectTestDataSQL());
            //Act
            var data = this.Execute(cmd).ToDataTable();
            //Assert
            Assert.Equal(4, data.Rows.Count);
            Assert.Equal(new List<long> { 1, 3, 5 }, data.Rows[1]["Col"]);
        }

        [Fact]
        public void SelectAllDataToListOf()
        {
            //Setup 
            var cmd = CreateTextCommand(SelectTestDataSQL());
            //Act
            var data = Execute(cmd).ToListOf<BigIntArrayDataType>();
            //Assert
            Assert.Equal(4, data.Count);
            Assert.Equal(new List<long> { 1, 3, 5 }, data[1].Col);
        }
    }
}
