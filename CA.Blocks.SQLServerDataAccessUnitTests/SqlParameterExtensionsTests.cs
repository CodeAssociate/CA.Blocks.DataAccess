using System;
using System.Data;
using CA.Blocks.SQLServerDataAccess;
using NUnit.Framework;

namespace CA.Blocks.SQLServerDataAccessUnitTests
{
    [TestFixture]
    public class SqlParameterExtensionsTests
    {

        [Test]
        public void ToSqlParameterInt32()
        {
            // Setup
            int target = 123;
            // Act
            var sqlparam = target.ToSqlParameter("@target");
            //Asert
            Assert.AreEqual(DbType.Int32, sqlparam.DbType);
            Assert.AreEqual(ParameterDirection.Input, sqlparam.Direction);
            Assert.AreEqual(false, sqlparam.IsNullable);
            Assert.AreEqual("@target", sqlparam.ParameterName);
            Assert.AreEqual(target, sqlparam.Value);
        }

        [Test]
        public void ToSqlParameterSameNameInt32()
        {
            // Setup
            int? target = 123;
            // Act
            var sqlparam = target.ToSqlParameter("@target");
            //Asert
            Assert.AreEqual(DbType.Int32, sqlparam.DbType);
            Assert.AreEqual(ParameterDirection.Input, sqlparam.Direction);
            Assert.AreEqual(false, sqlparam.IsNullable);
            Assert.AreEqual("@target", sqlparam.ParameterName);
            Assert.AreEqual(target, sqlparam.Value);
        }

        [Test]
        public void ToSqlParameterStringTest()
        {
            // Setup
            string testdata = "01234567890123456789";
            // Act
            var sqlparam = testdata.ToSqlParameter("@test");
            //Asert
            Assert.AreEqual(DbType.String, sqlparam.DbType);
            Assert.AreEqual(ParameterDirection.Input, sqlparam.Direction);
            Assert.AreEqual(false, sqlparam.IsNullable);
            Assert.AreEqual("@test", sqlparam.ParameterName);
            Assert.AreEqual(testdata, sqlparam.Value);
        }

        [Test]
        public void ToSqlParameterStringTestTrim()
        {
            // Setup
            string testdata = "01234567890123456789";
            // Act
            var sqlparam = testdata.ToSqlParameter("@test", trimInputTo:15);
            //Asert
            Assert.AreEqual(DbType.String, sqlparam.DbType);
            Assert.AreEqual(ParameterDirection.Input, sqlparam.Direction);
            Assert.AreEqual(false, sqlparam.IsNullable);
            Assert.AreEqual("@test", sqlparam.ParameterName);
            Assert.AreEqual("012345678901234", sqlparam.Value);
        }


        [Test]
        public void ToSqlParameterStringTestTrimEmpty()
        {
            // Setup
            string testdata = "";
            // Act
            var sqlparam = testdata.ToSqlParameter("@test", trimInputTo: 15);
            //Asert
            Assert.AreEqual(DbType.String, sqlparam.DbType);
            Assert.AreEqual(ParameterDirection.Input, sqlparam.Direction);
            Assert.AreEqual(false, sqlparam.IsNullable);
            Assert.AreEqual("@test", sqlparam.ParameterName);
            Assert.AreEqual("", sqlparam.Value);
        }

        [Test]
        public void ToSqlParameterLong()
        {
            // Setup
            long target = 123;
            // Act
            var sqlparam = target.ToSqlParameter("@target");
            //Asert
            Assert.AreEqual(DbType.Int64, sqlparam.DbType);
            Assert.AreEqual(ParameterDirection.Input, sqlparam.Direction);
            Assert.AreEqual(false, sqlparam.IsNullable);
            Assert.AreEqual("@target", sqlparam.ParameterName);
            Assert.AreEqual(target, sqlparam.Value);
        }

        [Test]
        public void ToSqlParameterNullLong()
        {
            // Setup
            long? target = null;
            // Act
            var sqlparam = target.ToSqlParameter("@target");
            //Asert
            Assert.AreEqual(DbType.Int64, sqlparam.DbType);
            Assert.AreEqual(ParameterDirection.Input, sqlparam.Direction);
            Assert.AreEqual(false, sqlparam.IsNullable);
            Assert.AreEqual("@target", sqlparam.ParameterName);
            Assert.AreEqual(DBNull.Value, sqlparam.Value);
        }
    }
}
