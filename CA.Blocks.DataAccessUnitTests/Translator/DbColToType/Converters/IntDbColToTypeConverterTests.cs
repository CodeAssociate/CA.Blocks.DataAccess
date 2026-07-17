using CA.Blocks.DataAccess.Translator.DbColToType.Converters;


namespace CA.Blocks.DataAccessUnitTests.Translator.DbColToType.Converters
{
        public class IntDbColToTypeConverterTests : BaseDbColToTypeConverterTests
    {
        [Theory]
        [InlineData(1, int.MinValue)]
        [InlineData(1, 123456)]
        [InlineData(1, int.MaxValue)]
        public void DbColToTypeConverterTest(int rowNumber, int expected)
        {
            var dt = CreateTestTable(typeof(int), expected);
            var dataRow = GetDataRow(rowNumber, dt);
            var dataReader = GetDataReader(rowNumber, dt);

            var target = new IntDbColToTypeConverter();
            Assert.Equal(expected, target.GetDataValue(dataRow, "col"));
            Assert.Equal(expected, target.GetDataValue(dataReader, "col"));

            Assert.Equal(expected, target.GetDataValue(dataRow, 1));
            Assert.Equal(expected, target.GetDataValue(dataReader, 1));
        }

        [Theory]
        [InlineData(0, null)]
        [InlineData(1, 123)]
        public void NullDbColToTypeConverterTest(int rowNumber, int? expected)
        {
            var dt = CreateTestTable(typeof(int), expected);
            var dataRow = GetDataRow(rowNumber, dt);
            var dataReader = GetDataReader(rowNumber, dt);

            var target = new NullIntDbColToTypeConverter();
            Assert.Equal(expected, target.GetDataValue(dataRow, "col"));
            Assert.Equal(expected, target.GetDataValue(dataReader, "col"));

            Assert.Equal(expected, target.GetDataValue(dataRow, 1));
            Assert.Equal(expected, target.GetDataValue(dataReader, 1));
        }



        [Theory]
        [InlineData(1, int.MinValue)]
        [InlineData(1, 123456)]
        [InlineData(1, int.MaxValue)]
        public void DbColToTypeConverterTestWithStringSource(int rowNumber, int expected)
        {
            var dt = CreateTestTable(typeof(string), expected.ToString());
            var dataRow = GetDataRow(rowNumber, dt);
            var dataReader = GetDataReader(rowNumber, dt);

            var target = new IntDbColToTypeConverter();
            Assert.Equal(expected, target.GetDataValue(dataRow, "col"));
            Assert.Equal(expected, target.GetDataValue(dataReader, "col"));

            Assert.Equal(expected, target.GetDataValue(dataRow, 1));
            Assert.Equal(expected, target.GetDataValue(dataReader, 1));
        }


        [Theory]
        [InlineData(0, null)]
        [InlineData(1, 123)]
        public void NullDbColToTypeConverterTestWithStringSource(int rowNumber, int? expected)
        {
            var dt = CreateTestTable(typeof(string), expected);
            var dataRow = GetDataRow(rowNumber, dt);
            var dataReader = GetDataReader(rowNumber, dt);

            var target = new NullIntDbColToTypeConverter();
            Assert.Equal(expected, target.GetDataValue(dataRow, "col"));
            Assert.Equal(expected, target.GetDataValue(dataReader, "col"));

            Assert.Equal(expected, target.GetDataValue(dataRow, 1));
            Assert.Equal(expected, target.GetDataValue(dataReader, 1));
        }

    }
}
