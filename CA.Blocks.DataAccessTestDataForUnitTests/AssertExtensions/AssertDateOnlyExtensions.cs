using NUnit.Framework;
using System;

namespace CA.Blocks.DataAccessTestDataForUnitTests.AssertExtensions
{
    public static class AssertDateOnlyExtensions
    {

#if NET6_0_OR_GREATER

        public static void IsSameValueAs(this DateOnly expected, DateTime source)
        {
            var anyTime = source.Subtract(source.Date);
            Assert.AreEqual(anyTime.TotalMilliseconds, 0);
            Assert.AreEqual(expected.Year, source.Year);
            Assert.AreEqual(expected.Month, source.Month);
            Assert.AreEqual(expected.Day, source.Day);
        }
#endif
    }
}
