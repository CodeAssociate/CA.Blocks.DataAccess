using CA.Blocks.PostgreSQLDataAccess.Builder;
using CA.Blocks.PostgreSQLDataAccessUnitTests.Base;



namespace CA.Blocks.PostgreSQLDataAccessUnitTests.DbTypeTests
{
   
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
            public List<long> Col { get; set; }
        }

        private void InsertTestDataSQL(long[] data)
        {
            var insertCmd = new SafeSqlBuilder($"Insert into {unitTestTableName:``} (col) values({data:@Data})")
                .BuildSqlCommand();
            ExecuteNonQuery(insertCmd);
        }

        /*
        [SetUp]
        public void Setup()
        {
            DefaultDbColToTypeProviderPostgresExtensions.AddPostgresArrayTypes();
            ExecuteNonQuery(DropTestTableSQL());
            ExecuteNonQuery(CreateTestTable("bigint[] not null"));
            InsertTestDataSQL([1, 2, 3]);
            InsertTestDataSQL([1, 3, 5]);
            InsertTestDataSQL([2, 4, 8]);
            InsertTestDataSQL([(long)int.MaxValue + (long)int.MaxValue, (long)int.MaxValue]);
        }*/

        /*
        [TearDown]
        public void TearDown()
        {
            //ExecuteNonQuery(DropTestTableSQL());
        }
        */

        [Fact]
        public void SelectAllDataToDataTable()
        {
            //Setup 
            var cmd = CreateTextCommand(SelectTestDataSQL());
            //Act
            var data = this.ExecuteDataTable (cmd);
            //Assert
            Assert.Equal(4, data.Rows.Count);
            Assert.Equal(new List<long> { 1, 3, 5 }, data.Rows[1]["Col"]);
            //Assert.That(data.Rows.Count, Is.EqualTo(4));
            //Assert.That(data.Rows[1]["Col"], Is.EqualTo(new List<long> { 1, 3, 5 }));
        }

        [Fact]
        public void SelectAllDataToListOf()
        {
            //Setup 
            var cmd = CreateTextCommand(SelectTestDataSQL());
            //Act
            var data = ExecuteToListOf<BigIntArrayDataType>(cmd);
            //Assert
            //Assert.That(data.Count, Is.EqualTo(4));
            //Assert.That(data[1].Col, Is.EqualTo(new List<long> { 1, 3, 5 }));
            Assert.Equal(4, data.Count);
            Assert.Equal(new List<long> { 1, 3, 5 }, data[1].Col);
        }


    }
}
