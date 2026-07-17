using CA.Blocks.DataAccess.Translator.DbColToType.Converters;

namespace CA.Blocks.DataAccessUnitTests.Translator.DbColToType.Converters
{
        public class IntListDbColToTypeConverterTests : BaseDbColToTypeConverterTests
    {
        [Theory]
        [InlineData(0, null, 0)]
        [InlineData(1, "", 0)]
        [InlineData(1, "1,2,3,4,5",1,2,3,4,5) ]
        [InlineData(1, "1, 2,3, 4 , 5", 1, 2, 3, 4, 5)]
        public void DbColToTypeConverterTest(int rowNumber, string? dbValue, params int[] numbers)
        {
            var dt = CreateTestTable(typeof(string), dbValue);
            var dataRow = GetDataRow(rowNumber, dt);
            var dataReader = GetDataReader(rowNumber, dt);
            var expected = new List<int>();
            if (!(numbers.Length == 1 && numbers[0] == 0))
            {
                expected.AddRange(numbers);
            }

            var target = new IntListDbColToTypeConverter();
            Assert.Equal(expected, target.GetDataValue(dataRow, "col"));
            Assert.Equal(expected, target.GetDataValue(dataReader, "col"));

            Assert.Equal(expected, target.GetDataValue(dataRow, 1));
            Assert.Equal(expected, target.GetDataValue(dataReader, 1));
        }
    }
}
