using System;
using CA.Blocks.DataAccess.Translator.DbColToType.Converters;
using CA.Blocks.DataAccessUnitTests.Translator.DbColToType.Converters;
using NUnit.Framework;

namespace CA.Blocks.DataAccessUnitTests.Translator.DbColToType.Converters
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
            Assert.That(target.GetDataValue(dataRow, "col"), Is.EqualTo(expected));
            Assert.That(target.GetDataValue(dataReader, "col"), Is.EqualTo(expected));

            Assert.That(target.GetDataValue(dataRow, 1), Is.EqualTo(expected));
            Assert.That(target.GetDataValue(dataReader, 1), Is.EqualTo(expected));
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
            Assert.That(target.GetDataValue(dataRow, "col"), Is.EqualTo(expected));
            Assert.That(target.GetDataValue(dataReader, "col"), Is.EqualTo(expected));

            Assert.That(target.GetDataValue(dataRow, 1), Is.EqualTo(expected));
            Assert.That(target.GetDataValue(dataReader, 1), Is.EqualTo(expected));
        }
    }
}
