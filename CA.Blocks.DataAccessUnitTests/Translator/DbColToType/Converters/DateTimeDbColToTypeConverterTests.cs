using CA.Blocks.DataAccess.Translator.DbColToType.Converters;


namespace CA.Blocks.DataAccessUnitTests.Translator.DbColToType.Converters
{


    [TestFixture]
    public class DateTimeDbColToTypeConverterTests : BaseDbColToTypeConverterTests
    {
        [Test]
        [TestCase(1, "now")]
        [TestCase(1, "2-Jan-2019")]
        [TestCase(1, "2-Jan-2019 12:39:22")]
        [TestCase(1, "2-Jan-2019 23:59:59")]
        [TestCase(1, "2-Jan-2019 00:00:01.333")]
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
            Assert.That(target.GetDataValue(dataRow, "col"), Is.EqualTo(expected));
            Assert.That(target.GetDataValue(dataReader, "col"), Is.EqualTo(expected));

            Assert.That(target.GetDataValue(dataRow, 1), Is.EqualTo(expected));
            Assert.That(target.GetDataValue(dataReader, 1), Is.EqualTo(expected));
        }


        [Test]
        [TestCase(0, null)]
        [TestCase(1, "1-Jan-2019")]
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
            Assert.That(target.GetDataValue(dataRow, "col"), Is.EqualTo(expected));
            Assert.That(target.GetDataValue(dataReader, "col"), Is.EqualTo(expected));

            Assert.That(target.GetDataValue(dataRow, 1), Is.EqualTo(expected));
            Assert.That(target.GetDataValue(dataReader, 1), Is.EqualTo(expected));
        }
    }
}