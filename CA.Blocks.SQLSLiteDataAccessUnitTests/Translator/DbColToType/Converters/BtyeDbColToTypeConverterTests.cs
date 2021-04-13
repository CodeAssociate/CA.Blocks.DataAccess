using CA.Blocks.DataAccess.Translator.DbColToType.Converters;
using NUnit.Framework;

namespace CA.Blocks.SQLLiteDataAccessUnitTests.Translator.DbColToType.Converters
{
    [TestFixture]
    public class CharDbColToTypeConverterTests : BaseDbColToTypeConverterTests
    {
        [Test]
        [TestCase(1, 'a')]
        [TestCase(1, 'B')]
        [TestCase(1, 'Z')]
        public void DbColToTypeConverterTest(int rowNumber, char expected)
        {
            var dt = CreateTestTable(typeof(char), expected);
            var dataRow = GetDataRow(rowNumber, dt);
            var dataReader = GetDataReader(rowNumber, dt);

            var target = new CharDbColToTypeConverter();
            Assert.AreEqual(expected, target.GetDataValue(dataRow, "col"));
            Assert.AreEqual(expected, target.GetDataValue(dataReader, "col"));
        }

        [Test]
        [TestCase(0, null)]
        [TestCase(1, 'a')]
        [TestCase(1, 'B')]
        [TestCase(1, 'Z')]
        public void NullDbColToTypeConverterTest(int rowNumber, char? expected)
        {
            var dt = CreateTestTable(typeof(char), expected);
            var dataRow = GetDataRow(rowNumber, dt);
            var dataReader = GetDataReader(rowNumber, dt);

            var target = new NullCharDbColToTypeConverter();
            Assert.AreEqual(expected, target.GetDataValue(dataRow, "col"));
            Assert.AreEqual(expected, target.GetDataValue(dataReader, "col"));
        }
    }
}
