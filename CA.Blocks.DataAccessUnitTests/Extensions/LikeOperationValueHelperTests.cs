using CA.Blocks.DataAccess.Extensions;

namespace CA.Blocks.DataAccessUnitTests.Extensions
{

    public class LikeOperationValueHelperTests
    {
        [Theory]
        [InlineData(null, null)]
        [InlineData("", "")]
        [InlineData("a", "%a%")]
        [InlineData("LikeValue", "%LikeValue%")]
        [InlineData("Like Value", "%Like Value%")]
        [InlineData("LikeValue_", "%LikeValue[_]%")] // escape the wildcard _
        [InlineData("Like_Value", "%Like[_]Value%")] // escape the wildcard _
        [InlineData("Like%Value", "%Like[%]Value%")] // escape the wildcard %
        [InlineData("%Like Value", "%[%]Like Value%")] // escape the wildcard %
        [InlineData("Like Value%", "%Like Value[%]%")] // escape the wildcard %
        [InlineData("_", "%[_]%")]
        [InlineData("%", "%[%]%")]
        [InlineData("__", "%[_][_]%")]
        [InlineData("[1]", "%[[]1]%")] // a little funky with SQL but you only need to escape the opening sequence of an escape,
        [InlineData("[1", "%[[]1%")]
        [InlineData("1]", "%1]%")]
        [InlineData("Like[abc]Value", "%Like[[]abc]Value%")]
        public void PrepContainsValueTests(string? input, string? expected)
        {
            var result = LikeOperationValueHelper.PrepContainsValue(input);
            Assert.Equal(expected, result);
        }
        
        [Theory]
        [InlineData(null, null)]
        [InlineData("", "")]
        [InlineData("a", "a%")]
        [InlineData("LikeValue", "LikeValue%")]
        [InlineData("Like Value", "Like Value%")]
        [InlineData("LikeValue_", "LikeValue[_]%")] // escape the wildcard _
        [InlineData("Like_Value", "Like[_]Value%")] // escape the wildcard _
        [InlineData("Like%Value", "Like[%]Value%")] // escape the wildcard %
        [InlineData("%Like Value", "[%]Like Value%")] // escape the wildcard %
        [InlineData("Like Value%", "Like Value[%]%")] // escape the wildcard %
        [InlineData("_", "[_]%")]
        [InlineData("%", "[%]%")]
        [InlineData("__", "[_][_]%")]
        [InlineData("[1]", "[[]1]%")] // a little funky with SQL but you only need to escape the opening sequence of an escape,
        [InlineData("[1", "[[]1%")]
        [InlineData("1]", "1]%")]
        [InlineData("Like[abc]Value", "Like[[]abc]Value%")]
        public void PrepStartsWithValueTests(string? input, string? expected)
        {
            var result = LikeOperationValueHelper.PrepStartsWithValue(input);
            Assert.Equal(expected, result);
        }
        
        [Theory]
        [InlineData(null, null)]
        [InlineData("", "")]
        [InlineData("a", "%a")]
        [InlineData("LikeValue", "%LikeValue")]
        [InlineData("Like Value", "%Like Value")]
        [InlineData("LikeValue_", "%LikeValue[_]")] // escape the wildcard _
        [InlineData("Like_Value", "%Like[_]Value")] // escape the wildcard _
        [InlineData("Like%Value", "%Like[%]Value")] // escape the wildcard %
        [InlineData("%Like Value", "%[%]Like Value")] // escape the wildcard %
        [InlineData("Like Value%", "%Like Value[%]")] // escape the wildcard %
        [InlineData("_", "%[_]")]
        [InlineData("%", "%[%]")]
        [InlineData("__", "%[_][_]")]
        [InlineData("[1]", "%[[]1]")] // a little funky with SQL but you only need to escape the opening sequence of an escape,
        [InlineData("[1", "%[[]1")]
        [InlineData("1]", "%1]")]
        [InlineData("Like[abc]Value", "%Like[[]abc]Value")]
        public void PrepEndsWithValueTests(string? input, string? expected)
        {
            var result = LikeOperationValueHelper.PrepEndsWithValue(input);
            Assert.Equal(expected, result);
        }
        
        [Theory]
        [InlineData(null, null)]
        [InlineData("", "")]
        [InlineData("a", "a")]
        [InlineData("LikeValue", "LikeValue")]
        [InlineData("Like Value", "Like Value")]
        [InlineData("LikeValue_", "LikeValue[_]")] // escape the wildcard _
        [InlineData("Like_Value", "Like[_]Value")] // escape the wildcard _
        [InlineData("Like%Value", "Like[%]Value")] // escape the wildcard %
        [InlineData("%Like Value", "[%]Like Value")] // escape the wildcard %
        [InlineData("Like Value%", "Like Value[%]")] // escape the wildcard %
        [InlineData("_", "[_]")]
        [InlineData("%", "[%]")]
        [InlineData("__", "[_][_]")]
        [InlineData("[1]", "[[]1]")] // a little funky with SQL but you only need to escape the opening sequence of an escape,
        [InlineData("[1", "[[]1")]
        [InlineData("1]", "1]")]
        [InlineData("Like[abc]Value", "Like[[]abc]Value")]
        public void EscapeWildcardTests(string? input, string? expected)
        {
            var result = LikeOperationValueHelper.EscapeWildcards(input);
            Assert.Equal(expected, result);
        }
    }
}