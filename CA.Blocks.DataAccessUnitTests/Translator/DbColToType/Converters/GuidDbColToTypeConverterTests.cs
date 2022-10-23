using CA.Blocks.DataAccess.Translator.DbColToType.Converters;

namespace CA.Blocks.DataAccessUnitTests.Translator.DbColToType.Converters
{
    [TestFixture]
    public class GuidDbColToTypeConverterTests : BaseDbColToTypeConverterTests
    {
        [Test]
        [TestCase(1, "new")]
        [TestCase(1, "empty")]
        [TestCase(1, "7009B509-098F-4AF4-97C8-CF354B4E0D77")]
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
            Assert.That(target.GetDataValue(dataRow, "col"), Is.EqualTo(expected));
            Assert.That(target.GetDataValue(dataReader, "col"), Is.EqualTo(expected));

            Assert.That(target.GetDataValue(dataRow, 1), Is.EqualTo(expected));
            Assert.That(target.GetDataValue(dataReader, 1), Is.EqualTo(expected));
        }

        [Test]
        [TestCase(0, null)]
        [TestCase(1, "7009B509-098F-4AF4-97C8-CF354B4E0D76")]
        public void NullDbColToTypeConverterTest(int rowNumber, string testDate)
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
            Assert.That(target.GetDataValue(dataRow, "col"), Is.EqualTo(expected));
            Assert.That(target.GetDataValue(dataReader, "col"), Is.EqualTo(expected));

            Assert.That(target.GetDataValue(dataRow, 1), Is.EqualTo(expected));
            Assert.That(target.GetDataValue(dataReader, 1), Is.EqualTo(expected));
        }
    }
}
