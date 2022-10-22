using System.Collections.Generic;
using CA.Blocks.DataAccess.Translator.DbColToType.Converters;
using CA.Blocks.DataAccessUnitTests.Translator.DbColToType.Converters;
using NUnit.Framework;

namespace CA.Blocks.DataAccessUnitTests.Translator.DbColToType.Converters
{
    [TestFixture]
    public class IntListDbColToTypeConverterTests : BaseDbColToTypeConverterTests
    {
        [Test]
        [TestCase(0, null, 0)]
        [TestCase(1, "", 0)]
        [TestCase(1, "1,2,3,4,5",1,2,3,4,5) ]
        [TestCase(1, "1, 2,3, 4 , 5", 1, 2, 3, 4, 5)]
        public void DbColToTypeConverterTest(int rowNumber, string dbValue, params int[] numbers)
        {
            var dt = CreateTestTable(typeof(string), dbValue);
            var dataRow = GetDataRow(rowNumber, dt);
            var dataReader = GetDataReader(rowNumber, dt);
            var expected = new List<int>();
            if (!(numbers.Length == 1 && numbers[0] == 0))
            {
                expected.AddRange(numbers);
            }

            var target = new IntListDbColToTypeConverter();
            Assert.AreEqual(expected, target.GetDataValue(dataRow, "col"));
            Assert.AreEqual(expected, target.GetDataValue(dataReader, "col"));

            Assert.AreEqual(expected, target.GetDataValue(dataRow, 1));
            Assert.AreEqual(expected, target.GetDataValue(dataReader, 1));
        }
    }
}
