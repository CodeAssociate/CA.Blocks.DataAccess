using CA.Blocks.DataAccess.Translator.DbColToType.Converters;

namespace CA.Blocks.DataAccessUnitTests.Translator.DbColToType.Converters;

[TestFixture]
public class DateOnlyDbColToTypeConverterTests : BaseDbColToTypeConverterTests
{
    [Test]
    [TestCase(1, "now")]
    [TestCase(1, "2-Jan-2019")]
    public void DbColToTypeConverterTest(int rowNumber, string testDate)
    {
        DateTime expecteddt = testDate == "now" ? DateTime.Now.Date : DateTime.Parse(testDate);
        DateOnly expected = DateOnly.FromDateTime(expecteddt);

        var dt = CreateTestTable(typeof(DateTime), expecteddt);
        var dataRow = GetDataRow(rowNumber, dt);
        var dataReader = GetDataReader(rowNumber, dt);

        var target = new DateOnlyDbColToTypeConverter();
        Assert.That(target.GetDataValue(dataRow, "col"), Is.EqualTo(expected));
        Assert.That(target.GetDataValue(dataReader, "col"), Is.EqualTo(expected));

        Assert.That(target.GetDataValue(dataRow, 1), Is.EqualTo(expected));
        Assert.That(target.GetDataValue(dataReader, 1), Is.EqualTo(expected));
    }


    [Test]
    [TestCase(0, null)]
    [TestCase(1, "1-Jan-2019")]
    public void NullDbColToTypeConverterTest(int rowNumber, string testDate)
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
        Assert.That(target.GetDataValue(dataRow, "col"), Is.EqualTo(expected));
        Assert.That(target.GetDataValue(dataReader, "col"), Is.EqualTo(expected));

        Assert.That(target.GetDataValue(dataRow, 1), Is.EqualTo(expected));
        Assert.That(target.GetDataValue(dataReader, 1), Is.EqualTo(expected));
    }
}