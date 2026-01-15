using CA.Blocks.SQLServerDataAccess.Builder;
using NUnit.Framework;
using System;

namespace CA.Blocks.SQLServerDataAccessUnitTests.Builder
{
    [TestFixture]
    public class SafeSqlBuilderTests
    {

        [Test]
        public void Basic_Test()
        {
            var target = new SafeSqlBuilder();
            int id = 123;
            target.AddSql($"Select * From Table1 where Id = {id:@id}");
            var resultCommnet = target.BuildSqlCommand();
            Assert.That(resultCommnet.CommandText, Is.EqualTo("Select * From Table1 where Id = @id"));
            Assert.That(resultCommnet.Parameters.Count, Is.EqualTo(1));
        }

        [Test]
        public void Basic_SQlNameTest()
        {
            var target = new SafeSqlBuilder();
            int id = 123;
            string scehma = "test";
            target.AddSql($"Select * From {scehma:[]}.[Table1] where Id = {id:@id}");
            var resultCommnet = target.BuildSqlCommand();
            Assert.That(resultCommnet.CommandText, Is.EqualTo("Select * From [test].[Table1] where Id = @id"));
            Assert.That(resultCommnet.Parameters.Count, Is.EqualTo(1));
        }

        [Test]
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
