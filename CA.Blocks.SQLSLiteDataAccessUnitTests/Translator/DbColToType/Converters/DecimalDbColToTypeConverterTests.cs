using CA.Blocks.DataAccess.Translator.DbColToType.Converters;
using NUnit.Framework;

namespace CA.Blocks.SQLLiteDataAccessUnitTests.Translator.DbColToType.Converters
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
            Assert.AreEqual(expected, target.GetDataValue(dataRow, "col"));
            Assert.AreEqual(expected, target.GetDataValue(dataReader, "col"));
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
            Assert.AreEqual(expected, target.GetDataValue(dataRow, "col"));
            Assert.AreEqual(expected, target.GetDataValue(dataReader, "col"));
        }
    }
}
