using CA.Blocks.DataAccess.Translator.DbColToType.Converters;

namespace CA.Blocks.DataAccessUnitTests.Translator.DbColToType.Converters;

public class DateOnlyDbColToTypeConverterTests : BaseDbColToTypeConverterTests
{
    [Theory]
    [InlineData(1, "now")]
    [InlineData(1, "2-Jan-2019")]
    public void DbColToTypeConverterTest(int rowNumber, string testDate)
    {
        DateTime expecteddt = testDate == "now" ? DateTime.Now.Date : DateTime.Parse(testDate);
        DateOnly expected = DateOnly.FromDateTime(expecteddt);

        var dt = CreateTestTable(typeof(DateTime), expecteddt);
        var dataRow = GetDataRow(rowNumber, dt);
        var dataReader = GetDataReader(rowNumber, dt);

        var target = new DateOnlyDbColToTypeConverter();
        Assert.Equal(expected, target.GetDataValue(dataRow, "col"));
        Assert.Equal(expected, target.GetDataValue(dataReader, "col"));

        Assert.Equal(expected, target.GetDataValue(dataRow, 1));
        Assert.Equal(expected, target.GetDataValue(dataReader, 1));
    }


    [Theory]
    [InlineData(0, null)]
    [InlineData(1, "1-Jan-2019")]
    public void NullDbColToTypeConverterTest(int rowNumber, string? testDate)
    {
        DateTime? expectedDt = null;
        if (!string.IsNullOrWhiteSpace(testDate))
        {
            expectedDt = DateTime.Parse(testDate);
        }

        DateOnly? expected = expectedDt.HasValue ? DateOnly.FromDateTime(expectedDt.Value) : null;

        var dt = CreateTestTable(typeof(DateTime), expectedDt);
        var dataRow = GetDataRow(rowNumber, dt);
        var dataReader = GetDataReader(rowNumber, dt);

        var target = new NullDateOnlyDbColToTypeConverter();
        Assert.Equal(expected, target.GetDataValue(dataRow, "col"));
        Assert.Equal(expected, target.GetDataValue(dataReader, "col"));

        Assert.Equal(expected, target.GetDataValue(dataRow, 1));
        Assert.Equal(expected, target.GetDataValue(dataReader, 1));
    }
}
