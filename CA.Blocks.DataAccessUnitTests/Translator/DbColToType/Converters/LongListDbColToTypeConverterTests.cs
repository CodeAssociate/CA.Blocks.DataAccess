using CA.Blocks.DataAccess.Translator.DbColToType.Converters;


namespace CA.Blocks.DataAccessUnitTests.Translator.DbColToType.Converters
{
        public class LongListDbColToTypeConverterTests : BaseDbColToTypeConverterTests
    {
        [Theory]
        [InlineData(0, null, new long[] { 0 })]
        [InlineData(1, "", new long[] { 0 })]
        [InlineData(1, "1,2,3,4,5", new long[] { 1, 2, 3, 4, 5 })]
        [InlineData(1, "1, 2,3, 4 , 5", new long[] { 1, 2, 3, 4, 5 })]
        public void DbColToTypeConverterTest(int rowNumber, string? dbValue, params long[] numbers)
        {
            var dt = CreateTestTable(typeof(string), dbValue);
            var dataRow = GetDataRow(rowNumber, dt);
            var dataReader = GetDataReader(rowNumber, dt);
            var expected = new List<long>();
            if (!(numbers.Length == 1 && numbers[0] == 0))
            {
                expected.AddRange(numbers);
            }
            
            var target = new LongListDbColToTypeConverter();
            Assert.Equal(expected, target.GetDataValue(dataRow, "col"));
            Assert.Equal(expected, target.GetDataValue(dataReader, "col"));

            Assert.Equal(expected, target.GetDataValue(dataRow, 1));
            Assert.Equal(expected, target.GetDataValue(dataReader, 1));
        }
    }
}
