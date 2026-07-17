using CA.Blocks.DataAccess.Extensions.Translators.NUlid.DbColToType.Converters;
using CA.Blocks.DataAccessUnitTests.Translator.DbColToType.Converters;
using NUlid;

namespace CA.Blocks.DataAccess.Extensions.Translators.NUlidTests.DbColToType.Converters;

public class UlidDbColToTypeConverterUnitTests : BaseDbColToTypeConverterTests
{
    [Fact]
    public void DbColToTypeConverterTest_ValidUlid()
    {
        var ulidAsString = "01H3V724QTMH8TV1BHPE6Z5AV4";
        var expected = new Ulid(ulidAsString);
        var dt = CreateTestTable(typeof(string), ulidAsString);
        var dataRow = GetDataRow(1, dt);
        var dataReader = GetDataReader(1, dt);

        var target = new UlidDbColToTypeConverter();

        Assert.Equal(expected, target.GetDataValue(dataRow, "col"));
        Assert.Equal(expected, target.GetDataValue(dataRow, 1));
        Assert.Equal(expected, target.GetDataValue(dataReader, "col"));
        Assert.Equal(expected, target.GetDataValue(dataReader, 1));
    }

    [Fact]
    public void DbColToTypeConverterTest_WithBinaryData()
    {
        var ulidAsString = "01H3V724QTMH8TV1BHPE6Z5AV4";
        var expected = new Ulid(ulidAsString);
        var dt = CreateTestTable(typeof(byte[]), expected.ToByteArray());
        var dataRow = GetDataRow(1, dt);
        var dataReader = GetDataReader(1, dt);

        var target = new UlidDbColToTypeConverter();

        Assert.Equal(expected, target.GetDataValue(dataRow, "col"));
        Assert.Equal(expected, target.GetDataValue(dataRow, 1));
        Assert.Equal(expected, target.GetDataValue(dataReader, "col"));
        Assert.Equal(expected, target.GetDataValue(dataReader, 1));
    }
}
