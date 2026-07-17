using CA.Blocks.DataAccess.Translator.DbColToType.Converters;

namespace CA.Blocks.DataAccessUnitTests.Translator.DbColToType.Converters
{
        public class CharDbColToTypeConverterTests : BaseDbColToTypeConverterTests
    {
        [Theory]
        [InlineData(1, 'a')]
        [InlineData(1, 'B')]
        [InlineData(1, 'Z')]
        public void DbColToTypeConverterTest(int rowNumber, char expected)
        {
            var dt = CreateTestTable(typeof(char), expected);
            var dataRow = GetDataRow(rowNumber, dt);
            var dataReader = GetDataReader(rowNumber, dt);

            var target = new CharDbColToTypeConverter();
            Assert.Equal(expected, target.GetDataValue(dataRow, "col"));
            Assert.Equal(expected, target.GetDataValue(dataReader, "col"));

            Assert.Equal(expected, target.GetDataValue(dataRow, 1));
            Assert.Equal(expected, target.GetDataValue(dataReader, 1));
        }

        [Theory]
        [InlineData(0, null)]
        [InlineData(1, 'a')]
        [InlineData(1, 'B')]
        [InlineData(1, 'Z')]
        public void NullDbColToTypeConverterTest(int rowNumber, char? expected)
        {
            var dt = CreateTestTable(typeof(char), expected);
            var dataRow = GetDataRow(rowNumber, dt);
            var dataReader = GetDataReader(rowNumber, dt);

            var target = new NullCharDbColToTypeConverter();
            Assert.Equal(expected, target.GetDataValue(dataRow, "col"));
            Assert.Equal(expected, target.GetDataValue(dataReader, "col"));

            Assert.Equal(expected, target.GetDataValue(dataRow, 1));
            Assert.Equal(expected, target.GetDataValue(dataReader, 1));
        }
    }
}
