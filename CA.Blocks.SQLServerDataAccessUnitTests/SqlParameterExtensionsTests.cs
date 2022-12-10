using System;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using CA.Blocks.SQLServerDataAccess;
using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic;
using NUnit.Framework;
using NUnit.Framework.Internal;
using static CA.Blocks.SQLServerDataAccessUnitTests.SqlParameterExtensionsTests;

namespace CA.Blocks.SQLServerDataAccessUnitTests
{

    [TestFixture]
    public class SqlParameterExtensionsTests
    {


        public static class TestDotNetTypesToToSqlParameter
        {
            public static bool? TestBool => true;
            public static byte? TestByte => 12;

            public static byte[] TestByteArray => new Byte[] {123, 100};

            public static sbyte? TestSbyte => -123;
            public static short? TestInt16 => -1234;
            public static ushort? TestuInt16 => 1234;

            public static int? TestInt32 => 12345;
            public static uint? TestUInt32 => 12345;
            public static long? TestInt64 => -12345678;
            public static ulong? TestUInt64 => 12345678;
            
            // Numbers ....
            public static decimal? TestDecimal = (decimal)0.345;

            public static float? TestSingle = (float)123.4567;

            public static Double? TestDouble = 123456789.0987654321;

            // strings
            public static char? TestChar => 'C';

            public static string? TestAsciiString = "Test String";

            public static string? TestUnicodeString = "The Blocks are 🔥 🤔";

            // DateTimes

            public static DateTime? TestDateTme => DateTime.Now;

            public static DateTimeOffset? TestDateTmeOffset => DateTimeOffset.Now;

            public static DateOnly? TestDateOnly => DateOnly.FromDateTime(DateTime.Now);

            public static TimeOnly? TestTimeOnly => new TimeOnly(15, 14,13);

            public static TimeSpan? TestTimeSpan => new TimeSpan(1, 12, 13, 14);

            public static Guid? TestGuid => Guid.NewGuid();
        }


        public void AssertSame(DateOnly expected, DateTime source)
        {
            var anyTime = source.Subtract(source.Date); 
            Assert.AreEqual(anyTime.TotalMilliseconds, 0);
            Assert.AreEqual(expected.Year, source.Year);
            Assert.AreEqual(expected.Month, source.Month);
            Assert.AreEqual(expected.Day, source.Day);
        }

        public void AssertSame(TimeOnly expected, TimeSpan source)
        {
            Assert.AreEqual(expected.Hour, source.Hours);
            Assert.AreEqual(expected.Minute, source.Minutes);
            Assert.AreEqual(expected.Second, source.Seconds);
            Assert.AreEqual(expected.Millisecond, source.Milliseconds);
        }

        public object ToSqlParameterTypeTestMain<T>(T test, DbType expectedDbType) 
        {
            //typeof(SqlServerParameterExtensions).GetMethod("ToSqlParameter")
            // Act
            var methods = typeof(SqlServerParameterExtensions).GetMethods().Where(x => x.Name == "ToSqlParameter");
            var methodForType = methods.Where(x => x.GetParameters()[0].ParameterType.FullName == typeof(T).FullName);
      
            var target = methodForType.FirstOrDefault();
            Assert.IsNotNull(target, $"Type - {typeof(T).FullName}");
            var targetParameters = target.GetParameters();
            var sqlParam = new SqlParameter();
            // Invoke
            if (targetParameters.Length == 2)
            {
                sqlParam = (SqlParameter)target.Invoke(null, new object[] { test, "@paramName" });
            }
            if (targetParameters.Length == 3)
            {
                sqlParam = (SqlParameter)target.Invoke(null, new object[] { test, "@paramName", 
                    targetParameters[2].DefaultValue });
            }
            if (targetParameters.Length == 4)
            {
                sqlParam = (SqlParameter)target.Invoke(null, new object[] { test, "@paramName", 
                    targetParameters[2].DefaultValue, 
                    targetParameters[3].DefaultValue });
            }
            if (targetParameters.Length == 5)
            {
                sqlParam = (SqlParameter)target.Invoke(null, new object[] { test, "@paramName",
                    targetParameters[2].DefaultValue, 
                    targetParameters[3].DefaultValue,
                    targetParameters[4].DefaultValue
                });
            }

            // assert

            Assert.AreEqual(expectedDbType, sqlParam.DbType);
            Assert.AreEqual(ParameterDirection.Input, sqlParam.Direction);
            Assert.AreEqual(false, sqlParam.IsNullable);
            Assert.AreEqual("@paramName", sqlParam.ParameterName);
            return sqlParam.Value;
        }

        public bool IsNullable<T>(T value)
        {
            return Nullable.GetUnderlyingType(typeof(T)) != null;
        }

        public void ToSqlParameterTypeTest<T>(T test, DbType expectedDbType)
        {
            var input = ToSqlParameterTypeTestMain(test, expectedDbType);
            Assert.AreEqual(test, input, $"{test.GetType().FullName}");
            if (IsNullable(test))
            {
                // Test Null values
                T nullTest = default(T);
                var nullInput = ToSqlParameterTypeTestMain<T>((T)nullTest, expectedDbType);
                Assert.AreEqual(DBNull.Value, nullInput, $"{test.GetType().FullName}");
            }
        }

        public void ToSqlParameterTypeTestDateOnly(DateOnly test, DbType expectedDbType)
        {
            var input = ToSqlParameterTypeTestMain(test, expectedDbType);
            AssertSame(test, (DateTime)input);
            // Test Null values
            DateOnly? nullTest = null;
            var nullInput = ToSqlParameterTypeTestMain<DateOnly?>((DateOnly?)nullTest, expectedDbType);
            Assert.AreEqual(DBNull.Value, nullInput, $"{test.GetType().FullName}");

        }

        public void ToSqlParameterTypeTestTimeOnly(TimeOnly test, DbType expectedDbType)
        {
            var input = ToSqlParameterTypeTestMain(test, expectedDbType);
            AssertSame(test, (TimeSpan)input);
            // Test Null values
            TimeOnly? nullTest = null;
            var nullInput = ToSqlParameterTypeTestMain<TimeOnly?>((TimeOnly?)nullTest, expectedDbType);
            Assert.AreEqual(DBNull.Value, nullInput, $"{test.GetType().FullName}");

        }




        [Test]
        public void ToSqlParameter_structs()
        {
            // https://learn.microsoft.com/en-us/dotnet/framework/data/adonet/sql-server-data-type-mappings
            // bigint
            ToSqlParameterTypeTest(TestDotNetTypesToToSqlParameter.TestInt64, DbType.Int64);
            // binary
            ToSqlParameterTypeTest(TestDotNetTypesToToSqlParameter.TestByteArray, DbType.Binary);
            // bit 
            ToSqlParameterTypeTest(TestDotNetTypesToToSqlParameter.TestBool, DbType.Boolean);
            //char 
            ToSqlParameterTypeTest(TestDotNetTypesToToSqlParameter.TestChar, DbType.AnsiStringFixedLength);
            // Date default to DateTime2 
            ToSqlParameterTypeTest(TestDotNetTypesToToSqlParameter.TestDateTme, DbType.DateTime2);
            // DateTimeOffset
            ToSqlParameterTypeTest(TestDotNetTypesToToSqlParameter.TestDateTmeOffset, DbType.DateTimeOffset);
            // DateOnly At this point the DateOnly is passed through as a DateTime with DateOnly 
            ToSqlParameterTypeTestDateOnly(TestDotNetTypesToToSqlParameter.TestDateOnly.Value, DbType.Date);

            ToSqlParameterTypeTestTimeOnly(TestDotNetTypesToToSqlParameter.TestTimeOnly.Value, DbType.Time);
            //decimal // need to extent to money and small money test
            ToSqlParameterTypeTest(TestDotNetTypesToToSqlParameter.TestDecimal, DbType.Decimal);
            // double
            ToSqlParameterTypeTest(TestDotNetTypesToToSqlParameter.TestDouble, DbType.Double);
            // Single float
            ToSqlParameterTypeTest(TestDotNetTypesToToSqlParameter.TestSingle, DbType.Single);
            //Int16
            ToSqlParameterTypeTest(TestDotNetTypesToToSqlParameter.TestInt16, DbType.Int16);
            //Int32
            ToSqlParameterTypeTest(TestDotNetTypesToToSqlParameter.TestInt32, DbType.Int32);

            //Int64
            ToSqlParameterTypeTest(TestDotNetTypesToToSqlParameter.TestInt64, DbType.Int64);

            //time
            ToSqlParameterTypeTest(TestDotNetTypesToToSqlParameter.TestTimeSpan, DbType.Time);

            //tinyint
            ToSqlParameterTypeTest(TestDotNetTypesToToSqlParameter.TestByte, DbType.Byte);

            ToSqlParameterTypeTest(TestDotNetTypesToToSqlParameter.TestSbyte, DbType.Int16); // SQL server done not have sbyte


            ToSqlParameterTypeTest(TestDotNetTypesToToSqlParameter.TestUnicodeString, DbType.String);

            ToSqlParameterTypeTest(TestDotNetTypesToToSqlParameter.TestGuid, DbType.Guid);
            // To the Overload types
            // 

            //TODO Monry 
            //TODO SMallmoney
        }


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
