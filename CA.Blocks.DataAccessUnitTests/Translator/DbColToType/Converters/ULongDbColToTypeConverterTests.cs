using System;
using CA.Blocks.DataAccess.Translator.DbColToType.Converters;
using CA.Blocks.DataAccessUnitTests.Translator.DbColToType.Converters;

namespace CA.Blocks.DataAccessUnitTests.Translator.DbColToType.Converters
{
        public class ULongDbColToTypeConverterTests : BaseDbColToTypeConverterTests
    {
        [Theory]
        [InlineData(1, (ulong)0)]
        [InlineData(1, (ulong)12345)]
        [InlineData(1, ulong.MaxValue)]
        public void DbColToTypeConverterTest(int rowNumber, ulong expected)
        {
            var dt = CreateTestTable(typeof(ulong), expected);
            var dataRow = GetDataRow(rowNumber, dt);
            var dataReader = GetDataReader(rowNumber, dt);

            var target = new ULongDbColToTypeConverter();
            Assert.Equal(expected, target.GetDataValue(dataRow, "col"));
            Assert.Equal(expected, target.GetDataValue(dataReader, "col"));

            Assert.Equal(expected, target.GetDataValue(dataRow, 1));
            Assert.Equal(expected, target.GetDataValue(dataReader, 1));
        }

        [Theory]
        [InlineData(0, null)]
        [InlineData(1, (ulong)123)]
        public void NullDbColToTypeConverterTest(int rowNumber, ulong? expected)
        {
            var dt = CreateTestTable(typeof(ulong), expected);
            var dataRow = GetDataRow(rowNumber, dt);
            var dataReader = GetDataReader(rowNumber, dt);

            var target = new NullULongDbColToTypeConverter();
            Assert.Equal(expected, target.GetDataValue(dataRow, "col"));
            Assert.Equal(expected, target.GetDataValue(dataReader, "col"));

            Assert.Equal(expected, target.GetDataValue(dataRow, 1));
            Assert.Equal(expected, target.GetDataValue(dataReader, 1));
        }
    }
}
