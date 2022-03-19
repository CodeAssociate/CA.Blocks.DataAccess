using CA.Blocks.DataAccess.Translator.DbColToType.Converters;
using NUnit.Framework;

namespace CA.Blocks.SQLLiteDataAccessUnitTests.Translator.DbColToType.Converters
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
            Assert.AreEqual(expected, target.GetDataValue(dataRow, "col"));
            Assert.AreEqual(expected, target.GetDataValue(dataReader, "col"));

            Assert.AreEqual(expected, target.GetDataValue(dataRow, 1));
            Assert.AreEqual(expected, target.GetDataValue(dataReader, 1));
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
            Assert.AreEqual(expected, target.GetDataValue(dataRow, "col"));
            Assert.AreEqual(expected, target.GetDataValue(dataReader, "col"));

            Assert.AreEqual(expected, target.GetDataValue(dataRow, 1));
            Assert.AreEqual(expected, target.GetDataValue(dataReader, 1));
        }
    }
}
