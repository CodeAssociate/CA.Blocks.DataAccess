using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Linq;
using CA.Blocks.DataAccess;
using CA.Blocks.DataAccessTestDataForUnitTests.BaseTests;
using CA.Blocks.DataAccessTestDataForUnitTests.TestTypes;
using CA.Blocks.SQLServerDataAccess;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json.Linq;

namespace CA.Blocks.SQLServerDataAccessUnitTests
{
    public class SqlParameterExtensionsTests : BaseToSqlParameterTests 
    {

        public override DbParameter ToSqlParameterTypeInstanceTestMain<T>(T test, DbType expectedDbType)
        {
            return ToSqlParameterTypeTestMain<T, SqlParameter>(typeof(SqlServerParameterExtensions), test, expectedDbType);
        }



        //[Fact]
        //public void bla()
        //{
            
        //    // Setup
        //    int? target = TestDotNetTypesToSqlParameter.TestInt32;
        //    string targetName = TestDotNetTypesToSqlParameter.TestAsciiString;
        //    string Unicode = TestDotNetTypesToSqlParameter.TestUnicodeString;
        //    var tt = target.ToSqlParameter("@target");


        //    var paramValues = SqlParameterHelper.AsSqlParameters(
        //         new ParameterMap(target, nameof(target)),
        //         new ParameterMap(targetName, nameof(targetName), "varchar"),
        //         new ParameterMap(Unicode, nameof(Unicode))
        //    );

        //    //Act
        //    var sqlparam = paramValues[0];
        //    //Assert
        //    Assert.Equal(DbType.Int32, sqlparam.DbType);
        //    Assert.Equal(ParameterDirection.Input, sqlparam.Direction);
        //    Assert.Equal("@target", sqlparam.ParameterName);
        //    Assert.Equal(target, sqlparam.Value);

        //    Assert.Equal(DbType.AnsiString, paramValues[1].DbType);
        //    Assert.Equal(ParameterDirection.Input, paramValues[1].Direction);
        //    Assert.Equal("@targetName", paramValues[1].ParameterName);
        //    Assert.Equal(targetName, paramValues[1].Value);
        //}


        [Fact]
        public void ToSqlParameter_TypeTests()
        {
            var testedTypes = new List<TypeToDbParameterResult>();
            // https://learn.microsoft.com/en-us/dotnet/framework/data/adonet/sql-server-data-type-mappings
            // bigInt
            testedTypes.Add(ToSqlParameterTypeTest(TestDotNetTypesToSqlParameter.TestInt64, DbType.Int64));
            // binary
            testedTypes.Add(ToSqlParameterTypeTest(TestDotNetTypesToSqlParameter.TestByteArray, DbType.Binary));
            // bit 
            testedTypes.Add(ToSqlParameterTypeTest(TestDotNetTypesToSqlParameter.TestBool, DbType.Boolean));
            //char 
            testedTypes.Add(ToSqlParameterTypeTest(TestDotNetTypesToSqlParameter.TestChar, DbType.AnsiStringFixedLength));
            // Date default to DateTime2 
            testedTypes.Add(ToSqlParameterTypeTest(TestDotNetTypesToSqlParameter.TestDateTme, DbType.DateTime2));
            // DateTimeOffset
            testedTypes.Add(ToSqlParameterTypeTest(TestDotNetTypesToSqlParameter.TestDateTmeOffset, DbType.DateTimeOffset));

            //decimal // need to extent to money and small money test
            testedTypes.Add(ToSqlParameterTypeTest(TestDotNetTypesToSqlParameter.TestDecimal, DbType.Decimal));
            // double
            testedTypes.Add(ToSqlParameterTypeTest(TestDotNetTypesToSqlParameter.TestDouble, DbType.Double));
            // Single float
            testedTypes.Add(ToSqlParameterTypeTest(TestDotNetTypesToSqlParameter.TestSingle, DbType.Single));
            //Int16
            testedTypes.Add(ToSqlParameterTypeTest(TestDotNetTypesToSqlParameter.TestInt16, DbType.Int16));
            //Int32
            testedTypes.Add(ToSqlParameterTypeTest(TestDotNetTypesToSqlParameter.TestInt32, DbType.Int32));

            // unsigned numbers // sql server have no UShort or Uint support so you have to use the Int32 and Int64
            testedTypes.Add(ToSqlParameterTypeTest(TestDotNetTypesToSqlParameter.TestUInt16, DbType.Int32));
            testedTypes.Add(ToSqlParameterTypeTest(TestDotNetTypesToSqlParameter.TestUInt32, DbType.Int64)); 

            //time
            testedTypes.Add(ToSqlParameterTypeTest(TestDotNetTypesToSqlParameter.TestTimeSpan, DbType.Time));

            //tinyInt
            testedTypes.Add(ToSqlParameterTypeTest(TestDotNetTypesToSqlParameter.TestByte, DbType.Byte));

            testedTypes.Add(ToSqlParameterTypeTest(TestDotNetTypesToSqlParameter.TestSbyte, DbType.Int16)); // SQL server done not have sbyte


            testedTypes.Add(ToSqlParameterTypeTest(TestDotNetTypesToSqlParameter.TestUnicodeString, DbType.String));

            testedTypes.Add(ToSqlParameterTypeTest(TestDotNetTypesToSqlParameter.TestGuid, DbType.Guid));
            // To the Overload types
            // 
            // DateOnly At this point the DateOnly is passed through as a DateTime with DateOnly 
            testedTypes.Add(ToSqlParameterTypeTestDateOnly(TestDotNetTypesToSqlParameter.TestDateOnly!.Value, DbType.Date));

            testedTypes.Add(ToSqlParameterTypeTestTimeOnly(TestDotNetTypesToSqlParameter.TestTimeOnly!.Value, DbType.Time));

            var AnyUntestedTypes = GetUnTestedTypes(testedTypes);
            if (AnyUntestedTypes.Count > 0)
            {
                foreach (var type in AnyUntestedTypes)
                {
                    Console.WriteLine($"{type.FullName} is missing a ToSqlParameterTest");
                }

                Assert.Fail("There are Untested ToSqlParameter types ");


            }
        }

        // Note this is long form of the test above ToSqlParameterTypeTest(TestDotNetTypesToSqlParameter.TestInt32, DbType.Int32)
        // leave this test to help explain the intention of the the generic abstract code
        [Fact]
        public void ToSqlParameterInt32()
        {
            // Setup
            int target = 123;
            // Act
            var sqlparam = target.ToSqlParameter("@target");
            //Assert
            Assert.Equal(DbType.Int32, sqlparam.DbType);
            Assert.Equal(ParameterDirection.Input, sqlparam.Direction);
            Assert.Equal("@target", sqlparam.ParameterName);
            Assert.Equal(target, sqlparam.Value);
        }


        // Note this is second test if value is nullable type ToSqlParameterTypeTest(TestDotNetTypesToSqlParameter.TestInt32, DbType.Int32)
        [Fact]
        public void ToSqlParameterInt32NullValue()
        {
            // Setup
            int? target = null;
            // Act
            var sqlparam = target.ToSqlParameter("@target");
            //Assert
            Assert.Equal(DbType.Int32, sqlparam.DbType);
            Assert.Equal(ParameterDirection.Input, sqlparam.Direction);
            Assert.Equal("@target", sqlparam.ParameterName);
            Assert.Equal(DBNull.Value, sqlparam.Value);
        }


        [Fact]
        public void ToSqlParameterStringTestTrim()
        {
            // Setup
            string testdata = "01234567890123456789";
            // Act
            var sqlparam = testdata.ToSqlParameter("@test", trimInputTo:15);
            //Assert
            Assert.Equal(DbType.String, sqlparam.DbType);
            Assert.Equal(ParameterDirection.Input, sqlparam.Direction);
            Assert.False(sqlparam.IsNullable);
            Assert.Equal("@test", sqlparam.ParameterName);
            Assert.Equal("012345678901234", sqlparam.Value);
        }


        [Fact]
        public void ToSqlParameterStringTestTrimEmpty()
        {
            // Setup
            string testdata = "";
            // Act
            var sqlparam = testdata.ToSqlParameter("@test", trimInputTo: 15);
            //Assert
            Assert.Equal(DbType.String, sqlparam.DbType);
            Assert.Equal(ParameterDirection.Input, sqlparam.Direction);
            Assert.False(sqlparam.IsNullable);
            Assert.Equal("@test", sqlparam.ParameterName);
            Assert.Equal("", sqlparam.Value);
        }

        // TODO the Overriders with optional parameters 
        // String Specific Types ( varchar rather than NVarChar ) 
        // DateTime Specific Types 
        // Decimal Specific Types Money and Small Money

    }
}




