using System;
using CA.Blocks.DataAccess.Translator.DbColToType.Converters;
using CA.Blocks.DataAccessUnitTests.Translator.DbColToType.Converters;
using NUnit.Framework;

namespace CA.Blocks.DataAccessUnitTests.Translator.DbColToType.Converters
{
    [TestFixture]
    public class ShortDbColToTypeConverterTests : BaseDbColToTypeConverterTests
    {
        [Test]
        [TestCase(1, short.MinValue)]
        [TestCase(1, 12345)]
        [TestCase(1, short.MaxValue)]
        public void DbColToTypeConverterTest(int rowNumber, int expected)
        {
            var dt = CreateTestTable(typeof(short), expected);
            var dataRow = GetDataRow(rowNumber, dt);
            var dataReader = GetDataReader(rowNumber, dt);

            var target = new ShortDbColToTypeConverter();
            Assert.AreEqual(expected, target.GetDataValue(dataRow, "col"));
            Assert.AreEqual(expected, target.GetDataValue(dataReader, "col"));

            Assert.AreEqual(expected, target.GetDataValue(dataRow, 1));
            Assert.AreEqual(expected, target.GetDataValue(dataReader, 1));
        }

        [Test]
        [TestCase(0, null)]
        [TestCase(1, 123)]
        public void NullDbColToTypeConverterTest(int rowNumber, short? expected)
        {
            var dt = CreateTestTable(typeof(short), expected);
            var dataRow = GetDataRow(rowNumber, dt);
            var dataReader = GetDataReader(rowNumber, dt);

            var target = new NullShortDbColToTypeConverter();
            Assert.AreEqual(expected, target.GetDataValue(dataRow, "col"));
            Assert.AreEqual(expected, target.GetDataValue(dataReader, "col"));

            Assert.AreEqual(expected, target.GetDataValue(dataRow, 1));
            Assert.AreEqual(expected, target.GetDataValue(dataReader, 1));
        }
    }
}
