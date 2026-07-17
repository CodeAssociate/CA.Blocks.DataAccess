using CA.Blocks.DataAccess.Translator.DbColToType.Converters;


namespace CA.Blocks.DataAccessUnitTests.Translator.DbColToType.Converters
{
        public class DecimalDbColToTypeConverterTests : BaseDbColToTypeConverterTests
    {
        [Theory]
        [InlineData(1, 987.456d)]
        public void DbColToTypeConverterTest(int rowNumber, double expected)
        {
            var expectedDecimal = (decimal)expected;
            var dt = CreateTestTable(typeof(decimal), expectedDecimal);
            var dataRow = GetDataRow(rowNumber, dt);
            var dataReader = GetDataReader(rowNumber, dt);

            var target = new DecimalDbColToTypeConverter();
            Assert.Equal(expectedDecimal, target.GetDataValue(dataRow, "col"));
            Assert.Equal(expectedDecimal, target.GetDataValue(dataReader, "col"));

            Assert.Equal(expectedDecimal, target.GetDataValue(dataRow, 1));
            Assert.Equal(expectedDecimal, target.GetDataValue(dataReader, 1));
        }

        [Theory]
        [InlineData(0, null)]
        [InlineData(1, 987.456d)]
        public void NullDbColToTypeConverterTest(int rowNumber, double? expected)
        {
            decimal? expectedDecimal = expected.HasValue ? (decimal?)expected.Value : null;
            var dt = CreateTestTable(typeof(decimal), expectedDecimal);
            var dataRow = GetDataRow(rowNumber, dt);
            var dataReader = GetDataReader(rowNumber, dt);

            var target = new NullDecimalDbColToTypeConverter();
            Assert.Equal(expectedDecimal, target.GetDataValue(dataRow, "col"));
            Assert.Equal(expectedDecimal, target.GetDataValue(dataReader, "col"));

            Assert.Equal(expectedDecimal, target.GetDataValue(dataRow, 1));
            Assert.Equal(expectedDecimal, target.GetDataValue(dataReader, 1));
        }
    }
}
