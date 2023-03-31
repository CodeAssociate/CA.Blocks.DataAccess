using System;
using System.Collections.Generic;


namespace CA.Blocks.DataAccessTestDataForUnitTests.TestTypes
{
    // We can use this class as simple check for round tripping the data to the various databases, the is more a smoke test to make sure each provider can support each of the types rather than detailed tests
    // This is used to test the 
    public static class TestDotNetTypesToSqlParameter
    {
        public static bool? TestBool => true;
        public static byte? TestByte => 12;

        public static byte[] TestByteArray => new Byte[] { 123, 100 };

        public static sbyte? TestSbyte => -123;
        public static short? TestInt16 => -1234;
        public static ushort? TestUInt16 => 1234;

        public static int? TestInt32 => 12345;
        public static uint? TestUInt32 => 12345;
        public static long? TestInt64 => -12345678;

        // There is simply no logical use case for supporting Uint,  as the range for int64 is 9,223,372,036,854,775,808 to 9,223,372,036,854,775,807. 
        // if you need to support this use a NUMERIC(20) in the DB. this will allow you to support the max value of Uint 18,446,744,073,709,551,615 
        //public static ulong? TestUInt64 => 12345678;

        // Numbers ....
        public static decimal? TestDecimal = (decimal)0.345;

        public static float? TestSingle = (float)123.4567;

        public static Double? TestDouble = 123456789.0987654321;

        // strings
        public static char? TestChar => 'C';

        public static string TestAsciiString = "Test String";

        public static string TestUnicodeString = "The Blocks are 🔥 🤔";

        // DateTimes

        public static DateTime? TestDateTme => DateTime.Now;

        public static DateTimeOffset? TestDateTmeOffset => DateTimeOffset.Now;

#if NET6_0_OR_GREATER
        public static DateOnly? TestDateOnly => DateOnly.FromDateTime(DateTime.Now);

        public static TimeOnly? TestTimeOnly => new TimeOnly(15, 14, 13);
#endif
        public static TimeSpan? TestTimeSpan => new TimeSpan(1, 12, 13, 14);

        public static Guid? TestGuid => Guid.NewGuid();


        public static IList<Type> AllExpectedTypeValues()
        {
            var result = new List<Type>();
            // TODO use reflection to pick up any changes
            result.Add(typeof(bool));
            result.Add(typeof(byte));
            result.Add(typeof(byte[]));
            result.Add(typeof(sbyte));
            result.Add(typeof(short));
            result.Add(typeof(ushort));
            result.Add(typeof(int));
            result.Add(typeof(uint));
            result.Add(typeof(long));
            result.Add(typeof(Guid));
            result.Add(typeof(decimal));
            result.Add(typeof(float));
            result.Add(typeof(double));
            result.Add(typeof(char));
            result.Add(typeof(string));
            result.Add(typeof(DateTime));
            result.Add(typeof(DateTimeOffset));
#if NET6_0_OR_GREATER
            result.Add(typeof(DateOnly));

            result.Add(typeof(TimeOnly));
#endif
            result.Add(typeof(TimeSpan));

            return result;
        }
    }
}
