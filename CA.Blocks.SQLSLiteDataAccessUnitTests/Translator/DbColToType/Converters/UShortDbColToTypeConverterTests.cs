using System;
using CA.Blocks.DataAccess.Translator.DbColToType.Converters;
using NUnit.Framework;

namespace CA.Blocks.SQLLiteDataAccessUnitTests.Translator.DbColToType.Converters
{
    [TestFixture]
    public class UShortDbColToTypeConverterTests : BaseDbColToTypeConverterTests
    {
        [Test]
        [TestCase(1, ushort.MinValue)]
        [TestCase(1, (ushort)12345)]
        [TestCase(1, ushort.MaxValue)]
        public void DbColToTypeConverterTest(int rowNumber, ushort expected)
        {
            var dt = CreateTestTable(typeof(ushort), expected);
            var dataRow = GetDataRow(rowNumber, dt);
            var dataReader = GetDataReader(rowNumber, dt);

            var target = new UShortDbColToTypeConverter();
            Assert.AreEqual(expected, target.GetDataValue(dataRow, "col"));
            Assert.AreEqual(expected, target.GetDataValue(dataReader, "col"));
        }

        [Test]
        [TestCase(0, null)]
        [TestCase(1, (ushort)1234)]
        public void NullDbColToTypeConverterTest(int rowNumber, ushort? expected)
        {
            var dt = CreateTestTable(typeof(ushort), expected);
            var dataRow = GetDataRow(rowNumber, dt);
            var dataReader = GetDataReader(rowNumber, dt);

            var target = new NullUShortDbColToTypeConverter();
            Assert.AreEqual(expected, target.GetDataValue(dataRow, "col"));
            Assert.AreEqual(expected, target.GetDataValue(dataReader, "col"));
        }
    }
}
