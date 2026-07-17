using CA.Blocks.SQLServerDataAccess.Builder;
using System;

namespace CA.Blocks.SQLServerDataAccessUnitTests.Builder
{
    public class SafeSqlBuilderTests
    {

        [Fact]
        public void Basic_Test()
        {
            var target = new SafeSqlBuilder();
            int id = 123;
            target.AddSql($"Select * From Table1 where Id = {id:@id}");
            var resultCommnet = target.BuildSqlCommand();
            Assert.Equal("Select * From Table1 where Id = @id", resultCommnet.CommandText);
            Assert.Equal(1, resultCommnet.Parameters.Count);
        }

        [Fact]
        public void Basic_SQlNameTest()
        {
            var target = new SafeSqlBuilder();
            int id = 123;
            string scehma = "test";
            target.AddSql($"Select * From {scehma:[]}.[Table1] where Id = {id:@id}");
            var resultCommnet = target.BuildSqlCommand();
            Assert.Equal("Select * From [test].[Table1] where Id = @id", resultCommnet.CommandText);
            Assert.Equal(1, resultCommnet.Parameters.Count);
        }

        [Fact]
        public void Basic_Test_not_Suported()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                var target = new SafeSqlBuilder();
                int id = 123;
                target.AddSql($"Select * From Table1 where Id = {id}");
                var resultCommnet = target.BuildSqlCommand();
            });
        }
    }
}




