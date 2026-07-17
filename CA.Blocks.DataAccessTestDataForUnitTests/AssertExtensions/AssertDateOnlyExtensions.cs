#if NET6_0_OR_GREATER
using System;
using Xunit;


namespace CA.Blocks.DataAccessTestDataForUnitTests.AssertExtensions
{
    public static class AssertDateOnlyExtensions
    {
        public static void IsSameValueAs(this DateOnly expected, DateTime source)
        {
            var anyTime = source.Subtract(source.Date);
            Assert.Equal(anyTime.TotalMilliseconds, 0);
            Assert.Equal(expected.Year, source.Year);
            Assert.Equal(expected.Month, source.Month);
            Assert.Equal(expected.Day, source.Day);
        }
    }
}
#endif
