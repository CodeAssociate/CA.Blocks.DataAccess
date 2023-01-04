using NUnit.Framework;
using System;

namespace CA.Blocks.DataAccessTestDataForUnitTests.AssertExtensions
{
    public static class AssertTimeOnlyExtensions
    {

#if NET6_0_OR_GREATER

        public static void IsSameValueAs(this TimeOnly expected, TimeSpan source)
        {
            Assert.AreEqual(expected.Hour, source.Hours);
            Assert.AreEqual(expected.Minute, source.Minutes);
            Assert.AreEqual(expected.Second, source.Seconds);
            Assert.AreEqual(expected.Millisecond, source.Milliseconds);
        }
#endif
    }
}