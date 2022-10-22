using System;
using CA.Blocks.DataAccess.Translator.DbColToType.Converters;
using CA.Blocks.DataAccessUnitTests.Translator.DbColToType.Converters;
using NUnit.Framework;

namespace CA.Blocks.DataAccessUnitTests.Translator.DbColToType.Converters
{
    [TestFixture]
    public class LongDbColToTypeConverterTests : BaseDbColToTypeConverterTests
    {
        [Test]
        [TestCase(1, long.MinValue)]
        [TestCase(1, 12345)]
        [TestCase(1, long.MaxValue)]
        public void DbColToTypeConverterTest(int rowNumber, long expected)
        {
            var dt = CreateTestTable(typeof(long), expected);
            var dataRow = GetDataRow(rowNumber, dt);
            var dataReader = GetDataReader(rowNumber, dt);

            var target = new LongDbColToTypeConverter();
            Assert.AreEqual(expected, target.GetDataValue(dataRow, "col"));
            Assert.AreEqual(expected, target.GetDataValue(dataReader, "col"));

            Assert.AreEqual(expected, target.GetDataValue(dataRow, 1));
            Assert.AreEqual(expected, target.GetDataValue(dataReader, 1));
        }

        [Test]
        [TestCase(0, null)]
        [TestCase(1, 123)]
        public void NullDbColToTypeConverterTest(int rowNumber, long? expected)
        {
            var dt = CreateTestTable(typeof(long), expected);
            var dataRow = GetDataRow(rowNumber, dt);
            var dataReader = GetDataReader(rowNumber, dt);

            var target = new NullLongDbColToTypeConverter();
            Assert.AreEqual(expected, target.GetDataValue(dataRow, "col"));
            Assert.AreEqual(expected, target.GetDataValue(dataReader, "col"));

            Assert.AreEqual(expected, target.GetDataValue(dataRow, 1));
            Assert.AreEqual(expected, target.GetDataValue(dataReader, 1));
        }
    }
}
