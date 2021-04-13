using System;
using CA.Blocks.DataAccess.Translator.DbColToType.Converters;
using NUnit.Framework;

namespace CA.Blocks.SQLLiteDataAccessUnitTests.Translator.DbColToType.Converters
{
    [TestFixture]
    public class ULongDbColToTypeConverterTests : BaseDbColToTypeConverterTests
    {
        [Test]
        [TestCase(1, (ulong)0)]
        [TestCase(1, (ulong)12345)]
        [TestCase(1, ulong.MaxValue)]
        public void DbColToTypeConverterTest(int rowNumber, ulong expected)
        {
            var dt = CreateTestTable(typeof(ulong), expected);
            var dataRow = GetDataRow(rowNumber, dt);
            var dataReader = GetDataReader(rowNumber, dt);

            var target = new ULongDbColToTypeConverter();
            Assert.AreEqual(expected, target.GetDataValue(dataRow, "col"));
            Assert.AreEqual(expected, target.GetDataValue(dataReader, "col"));
        }

        [Test]
        [TestCase(0, null)]
        [TestCase(1, (ulong)123)]
        public void NullDbColToTypeConverterTest(int rowNumber, ulong? expected)
        {
            var dt = CreateTestTable(typeof(ulong), expected);
            var dataRow = GetDataRow(rowNumber, dt);
            var dataReader = GetDataReader(rowNumber, dt);

            var target = new NullULongDbColToTypeConverter();
            Assert.AreEqual(expected, target.GetDataValue(dataRow, "col"));
            Assert.AreEqual(expected, target.GetDataValue(dataReader, "col"));
        }
    }
}
