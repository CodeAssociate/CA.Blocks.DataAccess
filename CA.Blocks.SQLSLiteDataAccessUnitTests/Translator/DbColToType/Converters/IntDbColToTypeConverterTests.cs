using System;
using CA.Blocks.DataAccess.Translator.DbColToType.Converters;
using NUnit.Framework;

namespace CA.Blocks.SQLLiteDataAccessUnitTests.Translator.DbColToType.Converters
{
    [TestFixture]
    public class IntDbColToTypeConverterTests : BaseDbColToTypeConverterTests
    {
        [Test]
        [TestCase(1, int.MinValue)]
        [TestCase(1, 123456)]
        [TestCase(1, int.MaxValue)]
        public void DbColToTypeConverterTest(int rowNumber, int expected)
        {
            var dt = CreateTestTable(typeof(int), expected);
            var dataRow = GetDataRow(rowNumber, dt);
            var dataReader = GetDataReader(rowNumber, dt);

            var target = new IntDbColToTypeConverter();
            Assert.AreEqual(expected, target.GetDataValue(dataRow, "col"));
            Assert.AreEqual(expected, target.GetDataValue(dataReader, "col"));
        }

        [Test]
        [TestCase(0, null)]
        [TestCase(1, 123)]
        public void NullDbColToTypeConverterTest(int rowNumber, int? expected)
        {
            var dt = CreateTestTable(typeof(int), expected);
            var dataRow = GetDataRow(rowNumber, dt);
            var dataReader = GetDataReader(rowNumber, dt);

            var target = new NullIntDbColToTypeConverter();
            Assert.AreEqual(expected, target.GetDataValue(dataRow, "col"));
            Assert.AreEqual(expected, target.GetDataValue(dataReader, "col"));
        }
    }
}
