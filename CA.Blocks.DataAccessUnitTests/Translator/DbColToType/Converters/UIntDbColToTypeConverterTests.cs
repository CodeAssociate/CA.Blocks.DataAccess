using CA.Blocks.DataAccess.Translator.DbColToType.Converters;


namespace CA.Blocks.DataAccessUnitTests.Translator.DbColToType.Converters
{
    [TestFixture]
    public class UIntDbColToTypeConverterTests : BaseDbColToTypeConverterTests
    {
        [Test]
        [TestCase(1, uint.MinValue)]
        [TestCase(1, (uint)123456)]
        [TestCase(1, uint.MaxValue)]
        public void DbColToTypeConverterTest(int rowNumber, uint expected)
        {
            var dt = CreateTestTable(typeof(uint), expected);
            var dataRow = GetDataRow(rowNumber, dt);
            var dataReader = GetDataReader(rowNumber, dt);

            var target = new UIntDbColToTypeConverter();
            Assert.That(target.GetDataValue(dataRow, "col"), Is.EqualTo(expected));
            Assert.That(target.GetDataValue(dataReader, "col"), Is.EqualTo(expected));

            Assert.That(target.GetDataValue(dataRow, 1), Is.EqualTo(expected));
            Assert.That(target.GetDataValue(dataReader, 1), Is.EqualTo(expected));
        }

        [Test]
        [TestCase(0, null)]
        [TestCase(1, (uint)123)]
        public void NullDbColToTypeConverterTest(int rowNumber, uint? expected)
        {
            var dt = CreateTestTable(typeof(uint), expected);
            var dataRow = GetDataRow(rowNumber, dt);
            var dataReader = GetDataReader(rowNumber, dt);

            var target = new NullUIntDbColToTypeConverter();
            Assert.That(target.GetDataValue(dataRow, "col"), Is.EqualTo(expected));
            Assert.That(target.GetDataValue(dataReader, "col"), Is.EqualTo(expected));

            Assert.That(target.GetDataValue(dataRow, 1), Is.EqualTo(expected));
            Assert.That(target.GetDataValue(dataReader, 1), Is.EqualTo(expected));
        }
    }
}
