using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CA.Blocks.DataAccessTestDataForUnitTests.TestTypes;
using CA.Blocks.SQLServerDataAccess;

namespace CA.Blocks.SQLServerDataAccessUnitTests
{
    public class SqlParameterHelperTests
    {
        [Theory]
        [InlineData("test", "@test")]
        [InlineData("@test", "@test")]
        public void CreateNewParameterFor_Int(string paramName, string expectedParamName)
        {
            // setup 
            var target = TestDotNetTypesToSqlParameter.TestInt32;

            // Act
            var sqlParm = SqlParameterHelper.CreateNewParameterFor(target, paramName);
            // Assert
            Assert.Equal(SqlDbType.Int, sqlParm.SqlDbType);
            Assert.Equal(DbType.Int32, sqlParm.DbType);
            Assert.Equal(ParameterDirection.Input, sqlParm.Direction);
            Assert.Equal(expectedParamName, sqlParm.ParameterName);
            Assert.Equal(target, sqlParm.Value);
        }

        [Theory]
        [InlineData("test", "@test")]
        [InlineData("@test", "@test")]
        public void CreateNewParameterFor_StringAnsi(string paramName, string expectedParamName)
        {
            // setup 
            var target = TestDotNetTypesToSqlParameter.TestAsciiString;

            // Act
            var sqlParm = SqlParameterHelper.CreateNewParameterFor(target, paramName, "varchar");
            // Assert
            Assert.Equal(SqlDbType.VarChar, sqlParm.SqlDbType);
            Assert.Equal(DbType.AnsiString, sqlParm.DbType);
            Assert.Equal(ParameterDirection.Input, sqlParm.Direction);
            Assert.Equal(expectedParamName, sqlParm.ParameterName);
            Assert.Equal(target, sqlParm.Value);
        }

        [Theory]
        [InlineData("test", "@test")]
        [InlineData("@test", "@test")]
        public void CreateNewParameterFor_StringUnicode(string paramName, string expectedParamName)
        {
            // setup 
            var target = TestDotNetTypesToSqlParameter.TestUnicodeString;

            // Act
            var sqlParm = SqlParameterHelper.CreateNewParameterFor(target, paramName);
            // Assert
            Assert.Equal(SqlDbType.NVarChar, sqlParm.SqlDbType);
            Assert.Equal(DbType.String, sqlParm.DbType);
            Assert.Equal(ParameterDirection.Input, sqlParm.Direction);
            Assert.Equal(expectedParamName, sqlParm.ParameterName);
            Assert.Equal(target, sqlParm.Value);
        }



        [Fact]
        public void AsSqlParameters_Test()
        {
            // Setup
                int? tint = TestDotNetTypesToSqlParameter.TestInt32;
                string tansiString = TestDotNetTypesToSqlParameter.TestAsciiString;
                string tString = TestDotNetTypesToSqlParameter.TestUnicodeString;
            // act
            var paramValues = SqlParameterHelper.AsSqlParameters(
                     new ParameterMap(tint!, nameof(tint)),
                     new ParameterMap(tansiString, nameof(tansiString), "varchar"),
                     new ParameterMap(tString, nameof(tString))
                );
            // Assert
            Assert.Equal(SqlDbType.Int, paramValues[0].SqlDbType);
            Assert.Equal(DbType.Int32, paramValues[0].DbType);
            Assert.Equal(ParameterDirection.Input, paramValues[0].Direction);
            Assert.Equal("@tint", paramValues[0].ParameterName);
            Assert.Equal(tint, paramValues[0].Value);


            Assert.Equal(SqlDbType.VarChar, paramValues[1].SqlDbType);
            Assert.Equal(DbType.AnsiString, paramValues[1].DbType);
            Assert.Equal(ParameterDirection.Input, paramValues[1].Direction);
            Assert.Equal("@tansiString", paramValues[1].ParameterName);
            Assert.Equal(tansiString, paramValues[1].Value);

            Assert.Equal(SqlDbType.NVarChar, paramValues[2].SqlDbType);
            Assert.Equal(DbType.String, paramValues[2].DbType);
            Assert.Equal(ParameterDirection.Input, paramValues[2].Direction);
            Assert.Equal("@tString", paramValues[2].ParameterName);
            Assert.Equal(tString, paramValues[2].Value);
        }
    }
}




