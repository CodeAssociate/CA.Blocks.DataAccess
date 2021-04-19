using System.Collections.Generic;
using CA.Blocks.DataAccess.Translator.DbColToType.Converters;
using NUnit.Framework;

namespace CA.Blocks.SQLLiteDataAccessUnitTests.Translator.DbColToType.Converters
{
    [TestFixture]
    public class ByteListDbColToTypeConverterTests : BaseDbColToTypeConverterTests
    {
        [Test]
        [TestCase(0, null, (byte)0)]
        [TestCase(1, "", (byte)0)]
        [TestCase(1, "1,2,3,4,5",(byte)1, (byte)2, (byte)3, (byte)4, (byte)5) ]
        [TestCase(1, "1, 2,3, 4 , 5", (byte)1, (byte)2, (byte)3, (byte)4, (byte)5)]
        public void DbColToTypeConverterTest(int rowNumber, string dbValue, params byte[] numbers)
        {
            var dt = CreateTestTable(typeof(string), dbValue);
            var dataRow = GetDataRow(rowNumber, dt);
            var dataReader = GetDataReader(rowNumber, dt);
            var expected = new List<byte>();
            if (!(numbers.Length == 1 && numbers[0] == 0))
            {
                expected.AddRange(numbers);
            }

            var target = new ByteListDbColToTypeConverter();
            Assert.AreEqual(expected, target.GetDataValue(dataRow, "col"));
            Assert.AreEqual(expected, target.GetDataValue(dataReader, "col"));
        }
    }
}
