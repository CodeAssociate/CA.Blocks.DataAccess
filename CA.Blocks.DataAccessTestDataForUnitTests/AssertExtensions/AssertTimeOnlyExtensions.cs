#if NET6_0_OR_GREATER
using System;
using Xunit;

namespace CA.Blocks.DataAccessTestDataForUnitTests.AssertExtensions
{
    public static class AssertTimeOnlyExtensions
    {
        public static void IsSameValueAs(this TimeOnly expected, TimeSpan source)
        {
	        Assert.Equal(expected.Hour, source.Hours);
	        Assert.Equal(expected.Minute, source.Minutes);
	        Assert.Equal(expected.Second, source.Seconds);
	        Assert.Equal(expected.Millisecond, source.Milliseconds);
        }

    }
}
#endif