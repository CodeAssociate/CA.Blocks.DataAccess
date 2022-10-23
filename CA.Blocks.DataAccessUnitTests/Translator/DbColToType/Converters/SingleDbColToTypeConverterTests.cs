
using CA.Blocks.DataAccess.Translator.DbColToType.Converters;


namespace CA.Blocks.DataAccessUnitTests.Translator.DbColToType.Converters
{
    [TestFixture]
    public class SingleDbColToTypeConverterTests : BaseDbColToTypeConverterTests
    {
        [Test]
        [TestCase(1, Single.MinValue)]
        [TestCase(1, (Single)987.456)]
        [TestCase(1, Single.MaxValue)]
        public void DbColToTypeConverterTest(int rowNumber, Single expected)
        {
            var dt = CreateTestTable(typeof(Single), expected);
            var dataRow = GetDataRow(rowNumber, dt);
            var dataReader = GetDataReader(rowNumber, dt);

            var target = new SingleDbColToTypeConverter();
            Assert.That(target.GetDataValue(dataRow, "col"), Is.EqualTo(expected));
            Assert.That(target.GetDataValue(dataReader, "col"), Is.EqualTo(expected));

            Assert.That(target.GetDataValue(dataRow, 1), Is.EqualTo(expected));
            Assert.That(target.GetDataValue(dataReader, 1), Is.EqualTo(expected));
        }

        [Test]
        [TestCase(0, null)]
        [TestCase(1, (Single)987.456)]
        public void NullDbColToTypeConverterTest(int rowNumber, Single? expected)
        {
            var dt = CreateTestTable(typeof(Single), expected);
            var dataRow = GetDataRow(rowNumber, dt);
            var dataReader = GetDataReader(rowNumber, dt);

            var target = new NullSingleDbColToTypeConverter();
            Assert.That(target.GetDataValue(dataRow, "col"), Is.EqualTo(expected));
            Assert.That(target.GetDataValue(dataReader, "col"), Is.EqualTo(expected));

            Assert.That(target.GetDataValue(dataRow, 1), Is.EqualTo(expected));
            Assert.That(target.GetDataValue(dataReader, 1), Is.EqualTo(expected));
        }
    }
}
