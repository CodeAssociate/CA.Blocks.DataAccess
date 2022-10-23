using CA.Blocks.DataAccess.Translator.DbColToType.Converters;


namespace CA.Blocks.DataAccessUnitTests.Translator.DbColToType.Converters
{
    [TestFixture]
    public class DecimalDbColToTypeConverterTests : BaseDbColToTypeConverterTests
    {
        [Test]
        [TestCase(1, 987.456)]
        public void DbColToTypeConverterTest(int rowNumber, decimal expected)
        {
            var dt = CreateTestTable(typeof(decimal), expected);
            var dataRow = GetDataRow(rowNumber, dt);
            var dataReader = GetDataReader(rowNumber, dt);

            var target = new DecimalDbColToTypeConverter();
            Assert.That(target.GetDataValue(dataRow, "col"), Is.EqualTo(expected));
            Assert.That(target.GetDataValue(dataReader, "col"), Is.EqualTo(expected));

            Assert.That(target.GetDataValue(dataRow, 1), Is.EqualTo(expected));
            Assert.That(target.GetDataValue(dataReader, 1), Is.EqualTo(expected));
        }

        [Test]
        [TestCase(0, null)]
        [TestCase(1, 987.456)]
        public void NullDbColToTypeConverterTest(int rowNumber, decimal? expected)
        {
            var dt = CreateTestTable(typeof(decimal), expected);
            var dataRow = GetDataRow(rowNumber, dt);
            var dataReader = GetDataReader(rowNumber, dt);

            var target = new NullDecimalDbColToTypeConverter();
            Assert.That(target.GetDataValue(dataRow, "col"), Is.EqualTo(expected));
            Assert.That(target.GetDataValue(dataReader, "col"), Is.EqualTo(expected));

            Assert.That(target.GetDataValue(dataRow, 1), Is.EqualTo(expected));
            Assert.That(target.GetDataValue(dataReader, 1), Is.EqualTo(expected));
        }
    }
}
