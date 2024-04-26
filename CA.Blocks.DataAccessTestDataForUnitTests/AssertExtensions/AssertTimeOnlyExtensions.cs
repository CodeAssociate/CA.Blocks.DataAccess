#if NET6_0_OR_GREATER
using NUnit.Framework;
using System;
using NUnit.Framework.Legacy;

namespace CA.Blocks.DataAccessTestDataForUnitTests.AssertExtensions
{
    public static class AssertTimeOnlyExtensions
    {
        public static void IsSameValueAs(this TimeOnly expected, TimeSpan source)
        {
	        ClassicAssert.AreEqual(expected.Hour, source.Hours);
	        ClassicAssert.AreEqual(expected.Minute, source.Minutes);
	        ClassicAssert.AreEqual(expected.Second, source.Seconds);
	        ClassicAssert.AreEqual(expected.Millisecond, source.Milliseconds);
        }

    }
}
#endif