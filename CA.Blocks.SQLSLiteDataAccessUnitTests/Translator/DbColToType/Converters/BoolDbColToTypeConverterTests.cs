using CA.Blocks.DataAccess.Translator.DbColToType.Converters;
using NUnit.Framework;

namespace CA.Blocks.SQLLiteDataAccessUnitTests.Translator.DbColToType.Converters
{
    [TestFixture]
    public class BoolDbColToTypeConverterTests : BaseDbColToTypeConverterTests
    {
        [Test]
        [TestCase(1, true)]
        [TestCase(1, false)]
        public void BoolDbColToTypeConverterTest(int rowNumber, bool expected)
        {
            var dt = CreateTestTable(typeof(bool), expected);
            var dataRow = GetDataRow(rowNumber, dt);
            var dataReader = GetDataReader(rowNumber, dt);

            var target = new BoolDbColToTypeConverter();
            Assert.AreEqual(expected, target.GetDataValue(dataRow, "col"));
            Assert.AreEqual(expected, target.GetDataValue(dataReader, "col"));

            Assert.AreEqual(expected, target.GetDataValue(dataRow, 1));
            Assert.AreEqual(expected, target.GetDataValue(dataReader, 1));
        }

        [Test]
        [TestCase(0, null)]
        [TestCase(1, true)]
        [TestCase(1, false)]
        public void NullBoolDbColToTypeConverterTest(int rowNumber, bool? expected)
        {
            var dt = CreateTestTable(typeof(bool), expected);
            var dataRow = GetDataRow(rowNumber, dt);
            var dataReader = GetDataReader(rowNumber, dt);

            var target = new NullBoolDbColToTypeConverter();
            Assert.AreEqual(expected, target.GetDataValue(dataRow, "col"));
            Assert.AreEqual(expected, target.GetDataValue(dataReader, "col"));

            Assert.AreEqual(expected, target.GetDataValue(dataRow, 1));
            Assert.AreEqual(expected, target.GetDataValue(dataReader, 1));
        }
    }
}
