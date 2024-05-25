
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.Common;
using CA.Blocks.DataAccessTestDataForUnitTests.BaseTests;
using CA.Blocks.DataAccessTestDataForUnitTests.TestTypes;
using CA.Blocks.SqliteDataAccess;
using Microsoft.Data.Sqlite;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace CA.Blocks.SqliteDataAccessUnitTests
{
	[TestFixture]
    public class SqlParameterExtensionsTests : BaseToSqlParameterTests
    {
        public override DbParameter ToSqlParameterTypeInstanceTestMain<T>(T test, DbType expectedDbType)
        {
            return ToSqlParameterTypeTestMain<T, SqliteParameter>(typeof(SqliteParameterExtensions), test, expectedDbType);
        }

        // Due to SQLite's dynamic type system, parameter values are not converted. So will always be DbType.String  however we can test the SqliteType type
        public TypeToDbParameterResult Verify(TypeToDbParameterResult result, SqliteType expectedSqliteType)
        {
            ClassicAssert.AreEqual(((SqliteParameter)result.DbParameter).SqliteType, expectedSqliteType);

            return result;
        }

        private void IsDateTimeDbParameterExpectedResult(DateTime? expecteDateTime, DbParameter generatedParameter)
        {
            ClassicAssert.IsNotNull(generatedParameter);
            if (expecteDateTime.HasValue)
            {
                ClassicAssert.IsNotNull(generatedParameter.Value);
                DateTime actualDateTime = DateTime.Parse(generatedParameter.Value.ToString());
                ClassicAssert.AreEqual(expecteDateTime, actualDateTime);
            }
            else
            {
                ClassicAssert.IsNull(generatedParameter.Value);
            }
        }

        private void IsDateTimeOffsetDbParameterExpectedResult(DateTimeOffset? expecteDateTime, DbParameter generatedParameter)
        {
            ClassicAssert.IsNotNull(generatedParameter);
            if (expecteDateTime.HasValue)
            {
                ClassicAssert.IsNotNull(generatedParameter.Value);
                DateTimeOffset actualDateTime = DateTimeOffset.Parse(generatedParameter.Value.ToString());
                ClassicAssert.AreEqual(expecteDateTime, actualDateTime);
            }
            else
            {
                ClassicAssert.IsNull(generatedParameter.Value);
            }
        }

        private void IsDateOnlyDbParameterExpectedResult(DateOnly? expecteDate, DbParameter generatedParameter)
        {
            ClassicAssert.IsNotNull(generatedParameter);
            if (expecteDate.HasValue)
            {
                ClassicAssert.IsNotNull(generatedParameter.Value);
                DateOnly actualDate = DateOnly.Parse(generatedParameter.Value.ToString());
                ClassicAssert.AreEqual(expecteDate, actualDate);
            }
            else
            {
                ClassicAssert.IsNull(generatedParameter.Value);
            }
        }

        private void IsTimeOnlyDbParameterExpectedResult(TimeOnly? expecteTime, DbParameter generatedParameter)
        {
            ClassicAssert.IsNotNull(generatedParameter);
            if (expecteTime.HasValue)
            {
                ClassicAssert.IsNotNull(generatedParameter.Value);
                TimeOnly actualTime = TimeOnly.Parse(generatedParameter.Value.ToString());
                ClassicAssert.AreEqual(expecteTime, actualTime);
            }
            else
            {
                ClassicAssert.IsNull(generatedParameter.Value);
            }
        }

        /*
        /// https://learn.microsoft.com/en-gb/dotnet/standard/data/sqlite/types
    
Boolean			INTEGER	
Byte			INTEGER	
Byte[]			BLOB	
Char			TEXT	
DateOnly		TEXT	yyyy-MM-dd
DateTime		TEXT	yyyy-MM-dd HH:mm:ss.FFFFFFF
DateTimeOffset	TEXT	yyyy-MM-dd HH:mm:ss.FFFFFFFzzz
Decimal			TEXT	0.0########################### format. REAL would be lossy.
Double			REAL	
Guid			TEXT	00000000-0000-0000-0000-000000000000
Int16			INTEGER	
Int32			INTEGER	
Int64			INTEGER	
SByte			INTEGER	
Single			REAL	
String			TEXT	UTF-8
TimeOnly		TEXT	HH:mm:ss.fffffff
TimeSpan		TEXT	d.hh:mm:ss.fffffff
UInt16			INTEGER	
UInt32			INTEGER	
UInt64			INTEGER	Large values overflow
        */

        [Test]
        public void ToSqlParameter_TypeTests()
        {
            
            
            var testedTypes = new List<TypeToDbParameterResult>();

            // Boolean => INTEGER
            testedTypes.Add(Verify(ToSqlParameterTypeTest(TestDotNetTypesToSqlParameter.TestBool, DbType.String), SqliteType.Integer));
            // Byte => INTEGER	
            testedTypes.Add(Verify(ToSqlParameterTypeTest(TestDotNetTypesToSqlParameter.TestByte, DbType.String), SqliteType.Integer));
            //Byte[] =>  BLOB
            testedTypes.Add(Verify(ToSqlParameterTypeTest(TestDotNetTypesToSqlParameter.TestByteArray, DbType.Binary), SqliteType.Blob));
            //Char => TEXT
            testedTypes.Add(Verify(ToSqlParameterTypeTest(TestDotNetTypesToSqlParameter.TestChar, DbType.String), SqliteType.Text));
            // DateOnly	=> TEXT	yyyy-MM-dd
            testedTypes.Add(Verify(ToSqlParameterTypeTestDateOnly(TestDotNetTypesToSqlParameter.TestDateOnly.Value, DbType.String, IsDateOnlyDbParameterExpectedResult), SqliteType.Text));
            // DateTime	=> TEXT	yyyy-MM-dd HH:mm:ss.FFFFFFF
            testedTypes.Add(Verify(ToSqlParameterTypeTest(TestDotNetTypesToSqlParameter.TestDateTme, DbType.String, IsDateTimeDbParameterExpectedResult), SqliteType.Text));
            // DateTimeOffset => TEXT	yyyy-MM-dd HH:mm:ss.FFFFFFFzzz
            testedTypes.Add(Verify(ToSqlParameterTypeTest(TestDotNetTypesToSqlParameter.TestDateTmeOffset, DbType.String, IsDateTimeOffsetDbParameterExpectedResult), SqliteType.Text));
            //Decimal => TEXT	0.0########################### format. REAL would be lossy.
            testedTypes.Add(Verify(ToSqlParameterTypeTest(TestDotNetTypesToSqlParameter.TestDecimal, DbType.String), SqliteType.Text));
            // Double | Single  => REAL
            testedTypes.Add(Verify(ToSqlParameterTypeTest(TestDotNetTypesToSqlParameter.TestDouble, DbType.String), SqliteType.Real));
            testedTypes.Add(Verify(ToSqlParameterTypeTest(TestDotNetTypesToSqlParameter.TestSingle, DbType.String), SqliteType.Real));
            //Guid => TEXT	00000000-0000-0000-0000-000000000000
            testedTypes.Add(Verify(ToSqlParameterTypeTest(TestDotNetTypesToSqlParameter.TestGuid, DbType.String) , SqliteType.Text));

            // Int16, Int32, Int64=> INTEGER	
            testedTypes.Add(Verify(ToSqlParameterTypeTest(TestDotNetTypesToSqlParameter.TestInt16, DbType.String), SqliteType.Integer));
            testedTypes.Add(Verify(ToSqlParameterTypeTest(TestDotNetTypesToSqlParameter.TestInt32, DbType.String), SqliteType.Integer));
            testedTypes.Add(Verify(ToSqlParameterTypeTest(TestDotNetTypesToSqlParameter.TestInt64, DbType.String), SqliteType.Integer));

            testedTypes.Add(Verify(ToSqlParameterTypeTest(TestDotNetTypesToSqlParameter.TestUInt16, DbType.String), SqliteType.Integer));
            testedTypes.Add(Verify(ToSqlParameterTypeTest(TestDotNetTypesToSqlParameter.TestUInt32, DbType.String), SqliteType.Integer));
            testedTypes.Add(Verify(ToSqlParameterTypeTest(TestDotNetTypesToSqlParameter.TestSbyte, DbType.String), SqliteType.Integer));

            // String
            testedTypes.Add(Verify(ToSqlParameterTypeTest(TestDotNetTypesToSqlParameter.TestAsciiString, DbType.String), SqliteType.Text));
            testedTypes.Add(Verify(ToSqlParameterTypeTest(TestDotNetTypesToSqlParameter.TestUnicodeString, DbType.String), SqliteType.Text));

            // Time and TimeSpan
            testedTypes.Add(Verify(ToSqlParameterTypeTestTimeOnly(TestDotNetTypesToSqlParameter.TestTimeOnly.Value, DbType.String, IsTimeOnlyDbParameterExpectedResult), SqliteType.Text));
            testedTypes.Add(Verify(ToSqlParameterTypeTest(TestDotNetTypesToSqlParameter.TestTimeSpan, DbType.String), SqliteType.Text));



            var AnyUntestesTypes = GetUnTestedTypes(testedTypes);
            if (AnyUntestesTypes.Count > 0)
            {
                foreach (var type in AnyUntestesTypes)
                {
                    TestContext.WriteLine($"{type.FullName} is missing a ToSqlParameterTest");
                }

                Assert.Warn("There are Untested ToSqlParameter types ");
            }
        }

   
    }
}
