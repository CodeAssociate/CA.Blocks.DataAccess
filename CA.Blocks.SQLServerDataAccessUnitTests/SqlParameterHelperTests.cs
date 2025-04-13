using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CA.Blocks.DataAccessTestDataForUnitTests.TestTypes;
using CA.Blocks.SQLServerDataAccess;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace CA.Blocks.SQLServerDataAccessUnitTests
{
    [TestFixture]
    public class SqlParameterHelperTests
    {

        [Test]
        [TestCase("test", "@test")]
        [TestCase("@test", "@test")]
        public void CreateNewParameterFor_Int(string paramName, string expectedParamName)
        {
            // setup 
            var target = TestDotNetTypesToSqlParameter.TestInt32;

            // Act
            var sqlParm = SqlParameterHelper.CreateNewParameterFor(target, paramName);
            // Assert
            Assert.That(sqlParm.SqlDbType, Is.EqualTo(SqlDbType.Int));
            Assert.That(sqlParm.DbType, Is.EqualTo(DbType.Int32));
            Assert.That(sqlParm.Direction, Is.EqualTo(ParameterDirection.Input));
            Assert.That(sqlParm.ParameterName, Is.EqualTo(expectedParamName));
            Assert.That(sqlParm.Value, Is.EqualTo(target));
        }

        [Test]
        [TestCase("test", "@test")]
        [TestCase("@test", "@test")]
        public void CreateNewParameterFor_StringAnsi(string paramName, string expectedParamName)
        {
            // setup 
            var target = TestDotNetTypesToSqlParameter.TestAsciiString;

            // Act
            var sqlParm = SqlParameterHelper.CreateNewParameterFor(target, paramName, "varchar");
            // Assert
            Assert.That(sqlParm.SqlDbType, Is.EqualTo(SqlDbType.VarChar));
            Assert.That(sqlParm.DbType, Is.EqualTo(DbType.AnsiString));
            Assert.That(sqlParm.Direction, Is.EqualTo(ParameterDirection.Input));
            Assert.That(sqlParm.ParameterName, Is.EqualTo(expectedParamName));
            Assert.That(sqlParm.Value, Is.EqualTo(target));
        }

        [Test]
        [TestCase("test", "@test")]
        [TestCase("@test", "@test")]
        public void CreateNewParameterFor_StringUnicode(string paramName, string expectedParamName)
        {
            // setup 
            var target = TestDotNetTypesToSqlParameter.TestUnicodeString;

            // Act
            var sqlParm = SqlParameterHelper.CreateNewParameterFor(target, paramName);
            // Assert
            Assert.That(sqlParm.SqlDbType, Is.EqualTo(SqlDbType.NVarChar));
            Assert.That(sqlParm.DbType, Is.EqualTo(DbType.String));
            Assert.That(sqlParm.Direction, Is.EqualTo(ParameterDirection.Input));
            Assert.That(sqlParm.ParameterName, Is.EqualTo(expectedParamName));
            Assert.That(sqlParm.Value, Is.EqualTo(target));
        }



        [Test]
        public void AsSqlParameters_Test()
        {
            // Setup
                int? tint = TestDotNetTypesToSqlParameter.TestInt32;
                string tansiString = TestDotNetTypesToSqlParameter.TestAsciiString;
                string tString = TestDotNetTypesToSqlParameter.TestUnicodeString;
            // act
            var paramValues = SqlParameterHelper.AsSqlParameters(
                     new ParameterMap(tint, nameof(tint)),
                     new ParameterMap(tansiString, nameof(tansiString), "varchar"),
                     new ParameterMap(tString, nameof(tString))
                );
            // Assert
            Assert.That(paramValues[0].SqlDbType, Is.EqualTo(SqlDbType.Int));
            Assert.That(paramValues[0].DbType, Is.EqualTo(DbType.Int32));
            Assert.That(paramValues[0].Direction, Is.EqualTo(ParameterDirection.Input));
            Assert.That(paramValues[0].ParameterName, Is.EqualTo("@tint"));
            Assert.That(paramValues[0].Value, Is.EqualTo(tint));


            Assert.That(paramValues[1].SqlDbType, Is.EqualTo(SqlDbType.VarChar));
            Assert.That(paramValues[1].DbType, Is.EqualTo(DbType.AnsiString));
            Assert.That(paramValues[1].Direction, Is.EqualTo(ParameterDirection.Input));
            Assert.That(paramValues[1].ParameterName, Is.EqualTo("@tansiString"));
            Assert.That(paramValues[1].Value, Is.EqualTo(tansiString));

            Assert.That(paramValues[2].SqlDbType, Is.EqualTo(SqlDbType.NVarChar));
            Assert.That(paramValues[2].DbType, Is.EqualTo(DbType.String));
            Assert.That(paramValues[2].Direction, Is.EqualTo(ParameterDirection.Input));
            Assert.That(paramValues[2].ParameterName, Is.EqualTo("@tString"));
            Assert.That(paramValues[2].Value, Is.EqualTo(tString));
        }
    }
}
