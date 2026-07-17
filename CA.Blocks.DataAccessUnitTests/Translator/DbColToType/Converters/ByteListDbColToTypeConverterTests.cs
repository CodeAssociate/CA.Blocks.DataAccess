using System.Collections.Generic;
using CA.Blocks.DataAccess.Translator.DbColToType.Converters;
using CA.Blocks.DataAccessUnitTests.Translator.DbColToType.Converters;

namespace CA.Blocks.DataAccessUnitTests.Translator.DbColToType.Converters
{
        public class ByteListDbColToTypeConverterTests : BaseDbColToTypeConverterTests
    {
        [Theory]
        [InlineData(0, null, new byte[] { 0 })]
        [InlineData(1, "", (byte)0)]
        [InlineData(1, "1,2,3,4,5",(byte)1, (byte)2, (byte)3, (byte)4, (byte)5) ]
        [InlineData(1, "1, 2,3, 4 , 5", (byte)1, (byte)2, (byte)3, (byte)4, (byte)5)]
        public void DbColToTypeConverterTest(int rowNumber, string? dbValue, params byte[] numbers)
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
            Assert.Equal(expected, target.GetDataValue(dataRow, "col"));
            Assert.Equal(expected, target.GetDataValue(dataReader, "col"));

            Assert.Equal(expected, target.GetDataValue(dataRow, 1));
            Assert.Equal(expected, target.GetDataValue(dataReader, 1));
        }
    }
}
