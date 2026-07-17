using CA.Blocks.DataAccess.Translator.DbColToType.Converters;


namespace CA.Blocks.DataAccessUnitTests.Translator.DbColToType.Converters
{


        public class DateTimeDbColToTypeConverterTests : BaseDbColToTypeConverterTests
    {
        [Theory]
        [InlineData(1, "now")]
        [InlineData(1, "2-Jan-2019")]
        [InlineData(1, "2-Jan-2019 12:39:22")]
        [InlineData(1, "2-Jan-2019 23:59:59")]
        [InlineData(1, "2-Jan-2019 00:00:01.333")]
        public void DbColToTypeConverterTest(int rowNumber, string testDate)
        {
            DateTime? expected = null;
            if (!string.IsNullOrWhiteSpace(testDate))
            {
                expected = testDate == "now" ? DateTime.Now : DateTime.Parse(testDate);
            }

            var dt = CreateTestTable(typeof(DateTime), expected);
            var dataRow = GetDataRow(rowNumber, dt);
            var dataReader = GetDataReader(rowNumber, dt);

            var target = new DateTimeDbColToTypeConverter();
            Assert.Equal(expected, target.GetDataValue(dataRow, "col"));
            Assert.Equal(expected, target.GetDataValue(dataReader, "col"));

            Assert.Equal(expected, target.GetDataValue(dataRow, 1));
            Assert.Equal(expected, target.GetDataValue(dataReader, 1));
        }


        [Theory]
        [InlineData(0, null)]
        [InlineData(1, "1-Jan-2019")]
        public void NullDbColToTypeConverterTest(int rowNumber, string? testDate)
        {
            DateTime? expected = null;
            if (!string.IsNullOrWhiteSpace(testDate))
            {
                expected = DateTime.Parse(testDate);
            }

            var dt = CreateTestTable(typeof(DateTime), expected);
            var dataRow = GetDataRow(rowNumber, dt);
            var dataReader = GetDataReader(rowNumber, dt);

            var target = new NullDateTimeDbColToTypeConverter();
            Assert.Equal(expected, target.GetDataValue(dataRow, "col"));
            Assert.Equal(expected, target.GetDataValue(dataReader, "col"));

            Assert.Equal(expected, target.GetDataValue(dataRow, 1));
            Assert.Equal(expected, target.GetDataValue(dataReader, 1));
        }
    }
}
