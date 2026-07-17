using CA.Blocks.DataAccess.Translator.DbColToType.Converters;
using CA.Blocks.DataAccessUnitTests.Translator.DbColToType.Converters;

namespace CA.Blocks.DataAccessUnitTests.Translator.DbColToType.Converters
{
        public class BoolDbColToTypeConverterTests : BaseDbColToTypeConverterTests
    {
        [Theory]
        [InlineData(1, true)]
        [InlineData(1, false)]
        public void BoolDbColToTypeConverterTest(int rowNumber, bool expected)
        {
            var dt = CreateTestTable(typeof(bool), expected);
            var dataRow = GetDataRow(rowNumber, dt);
            var dataReader = GetDataReader(rowNumber, dt);

            var target = new BoolDbColToTypeConverter();
            Assert.Equal(expected, target.GetDataValue(dataRow, "col"));
            Assert.Equal(expected, target.GetDataValue(dataReader, "col"));

            Assert.Equal(expected, target.GetDataValue(dataRow, 1));
            Assert.Equal(expected, target.GetDataValue(dataReader, 1));
        }

        [Theory]
        [InlineData(0, null)]
        [InlineData(1, true)]
        [InlineData(1, false)]
        public void NullBoolDbColToTypeConverterTest(int rowNumber, bool? expected)
        {
            var dt = CreateTestTable(typeof(bool), expected);
            var dataRow = GetDataRow(rowNumber, dt);
            var dataReader = GetDataReader(rowNumber, dt);

            var target = new NullBoolDbColToTypeConverter();
            Assert.Equal(expected, target.GetDataValue(dataRow, "col"));
            Assert.Equal(expected, target.GetDataValue(dataReader, "col"));

            Assert.Equal(expected, target.GetDataValue(dataRow, 1));
            Assert.Equal(expected, target.GetDataValue(dataReader, 1));
        }
    }
}
