using CA.Blocks.DataAccess.Translator.DbColToType.Converters;

namespace CA.Blocks.DataAccessUnitTests.Translator.DbColToType.Converters
{
        public class GuidDbColToTypeConverterTests : BaseDbColToTypeConverterTests
    {
        [Theory]
        [InlineData(1, "new")]
        [InlineData(1, "empty")]
        [InlineData(1, "7009B509-098F-4AF4-97C8-CF354B4E0D77")]
        public void DbColToTypeConverterTest(int rowNumber, string testDate)
        {
            Guid? expected = null;
            if (!string.IsNullOrWhiteSpace(testDate))
            {
                if (testDate == "new")
                {
                    expected = Guid.NewGuid();
                }
                else if (testDate == "empty")
                {
                    expected = Guid.Empty;
                }
                else
                {
                    expected = Guid.Parse(testDate);
                }
            }

            var dt = CreateTestTable(typeof(Guid), expected);
            var dataRow = GetDataRow(rowNumber, dt);
            var dataReader = GetDataReader(rowNumber, dt);

            var target = new GuidDbColToTypeConverter();
            Assert.Equal(expected, target.GetDataValue(dataRow, "col"));
            Assert.Equal(expected, target.GetDataValue(dataReader, "col"));

            Assert.Equal(expected, target.GetDataValue(dataRow, 1));
            Assert.Equal(expected, target.GetDataValue(dataReader, 1));
        }

        [Theory]
        [InlineData(0, null)]
        [InlineData(1, "7009B509-098F-4AF4-97C8-CF354B4E0D76")]
        public void NullDbColToTypeConverterTest(int rowNumber, string? testDate)
        {
            Guid? expected = null;
            if (!string.IsNullOrWhiteSpace(testDate))
            {
                expected = Guid.Parse(testDate);
            }

            var dt = CreateTestTable(typeof(Guid), expected);
            var dataRow = GetDataRow(rowNumber, dt);
            var dataReader = GetDataReader(rowNumber, dt);

            var target = new NullGuidDbColToTypeConverter();
            Assert.Equal(expected, target.GetDataValue(dataRow, "col"));
            Assert.Equal(expected, target.GetDataValue(dataReader, "col"));

            Assert.Equal(expected, target.GetDataValue(dataRow, 1));
            Assert.Equal(expected, target.GetDataValue(dataReader, 1));
        }
    }
}
