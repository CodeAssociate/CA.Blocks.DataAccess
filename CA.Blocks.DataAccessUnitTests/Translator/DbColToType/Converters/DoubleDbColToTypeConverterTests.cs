using CA.Blocks.DataAccess.Translator.DbColToType.Converters;

namespace CA.Blocks.DataAccessUnitTests.Translator.DbColToType.Converters
{
    [TestFixture]
    public class DoubleDbColToTypeConverterTests : BaseDbColToTypeConverterTests
    {
        [Test]
        [TestCase(1, double.MinValue)]
        [TestCase(1, 987.456)]
        [TestCase(1, double.MaxValue)]
        public void DbColToTypeConverterTest(int rowNumber, double expected)
        {
            var dt = CreateTestTable(typeof(double), expected);
            var dataRow = GetDataRow(rowNumber, dt);
            var dataReader = GetDataReader(rowNumber, dt);

            var target = new DoubleDbColToTypeConverter();
            Assert.That(target.GetDataValue(dataRow, "col"), Is.EqualTo(expected));
            Assert.That(target.GetDataValue(dataReader, "col"), Is.EqualTo(expected));

            Assert.That(target.GetDataValue(dataRow, 1), Is.EqualTo(expected));
            Assert.That(target.GetDataValue(dataReader, 1), Is.EqualTo(expected));
        }

        [Test]
        [TestCase(0, null)]
        [TestCase(1, 987.456)]
        public void NullDbColToTypeConverterTest(int rowNumber, double? expected)
        {
            var dt = CreateTestTable(typeof(double), expected);
            var dataRow = GetDataRow(rowNumber, dt);
            var dataReader = GetDataReader(rowNumber, dt);

            var target = new NullDoubleDbColToTypeConverter();
            Assert.That(target.GetDataValue(dataRow, "col"), Is.EqualTo(expected));
            Assert.That(target.GetDataValue(dataReader, "col"), Is.EqualTo(expected));

            Assert.That(target.GetDataValue(dataRow, 1), Is.EqualTo(expected));
            Assert.That(target.GetDataValue(dataReader, 1), Is.EqualTo(expected));
        }
    }
}
