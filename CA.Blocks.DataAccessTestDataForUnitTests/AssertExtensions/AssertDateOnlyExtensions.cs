#if NET6_0_OR_GREATER
using NUnit.Framework;
using System;
using NUnit.Framework.Legacy;

namespace CA.Blocks.DataAccessTestDataForUnitTests.AssertExtensions
{
    public static class AssertDateOnlyExtensions
    {
        public static void IsSameValueAs(this DateOnly expected, DateTime source)
        {
            var anyTime = source.Subtract(source.Date);
            ClassicAssert.AreEqual(anyTime.TotalMilliseconds, 0);
            ClassicAssert.AreEqual(expected.Year, source.Year);
            ClassicAssert.AreEqual(expected.Month, source.Month);
            ClassicAssert.AreEqual(expected.Day, source.Day);
        }
    }
}
#endif
