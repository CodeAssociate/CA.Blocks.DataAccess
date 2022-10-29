using CA.Blocks.DataAccess.Translator.DbColToType.Converters;

namespace CA.Blocks.DataAccessUnitTests.Translator.DbColToType.Converters;

[TestFixture]
public class TimeOnlyDbColToTypeConverterTests : BaseDbColToTypeConverterTests
{
    [Test]
    [TestCase(1, 9, 10, 11)]
    [TestCase(1, 13, 15, 0)]
    public void DbColToTypeConverterTest(int rowNumber, int hour, int min, int sec)
    {

        TimeSpan expectedts = new TimeSpan(hour, min, sec);
        TimeOnly expected = TimeOnly.FromTimeSpan(expectedts);

        var dt = CreateTestTable(typeof(TimeSpan), expectedts);
        var dataRow = GetDataRow(rowNumber, dt);
        var dataReader = GetDataReader(rowNumber, dt);

        var target = new TimeOnlyDbColToTypeConverter();
        Assert.That(target.GetDataValue(dataRow, "col"), Is.EqualTo(expected));
        Assert.That(target.GetDataValue(dataReader, "col"), Is.EqualTo(expected));

        Assert.That(target.GetDataValue(dataRow, 1), Is.EqualTo(expected));
        Assert.That(target.GetDataValue(dataReader, 1), Is.EqualTo(expected));
    }


    [Test]
    [TestCase(0, -1, 0, 0)]
    [TestCase(1, 1, 2, 3)]
    public void NullDbColToTypeConverterTest(int rowNumber, int hour, int min, int sec)
    {
        TimeSpan? expectedts = null;
        if (hour != -1)
        {
            expectedts = new TimeSpan(hour, min, sec);
        }

        TimeOnly? expected = expectedts.HasValue ? TimeOnly.FromTimeSpan(expectedts.Value) : null;

        var dt = CreateTestTable(typeof(TimeSpan), expectedts);
        var dataRow = GetDataRow(rowNumber, dt);
        var dataReader = GetDataReader(rowNumber, dt);

        var target = new NullTimeOnlyDbColToTypeConverter();
        Assert.That(target.GetDataValue(dataRow, "col"), Is.EqualTo(expected));
        Assert.That(target.GetDataValue(dataReader, "col"), Is.EqualTo(expected));

        Assert.That(target.GetDataValue(dataRow, 1), Is.EqualTo(expected));
        Assert.That(target.GetDataValue(dataReader, 1), Is.EqualTo(expected));
    }
}