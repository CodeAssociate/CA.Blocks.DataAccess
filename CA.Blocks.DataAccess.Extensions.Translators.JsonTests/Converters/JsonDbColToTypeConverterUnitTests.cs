using CA.Blocks.DataAccess.Extensions.Translators.Json.Converters;
using CA.Blocks.DataAccessUnitTests.Translator.DbColToType.Converters;
using System.Text.Json;

namespace CA.Blocks.DataAccess.Extensions.Translators.JsonTests.Converters;

public class JsonDbColToTypeConverterUnitTests : BaseDbColToTypeConverterTests
{
    public static string TestData =
        @"[{""colour"": ""red"",""value"": ""#f00""},{""colour"": ""green"",""value"": ""#0f0""}]";

    private class ColourValueDataType
    {
        public string? Colour { get; set; }
        public string? Value { get; set; }
    }

    [Fact]
    public void DbColToTypeConverterTest()
    {
        var dt = CreateTestTable(typeof(string), TestData);
        var dataRow = GetDataRow(1, dt);
        var dataReader = GetDataReader(1, dt);

        var target = new JsonDbColToTypeConverter<List<ColourValueDataType>>(
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        Assert.Equal("green", target.GetDataValue(dataRow, "col")[1].Colour);
        Assert.Equal("green", target.GetDataValue(dataRow, 1)[1].Colour);
        Assert.Equal("green", target.GetDataValue(dataReader, "col")[1].Colour);
        Assert.Equal("green", target.GetDataValue(dataReader, 1)[1].Colour);
    }

    [Fact]
    public void NullDbColToTypeConverterTestWithNull()
    {
        var dt = CreateTestTable(typeof(string), TestData);
        var dataRow = GetDataRow(0, dt);
        var dataReader = GetDataReader(0, dt);

        var target = new NullJsonDbColToTypeConverter<IList<ColourValueDataType>>(
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        Assert.Null(target.GetDataValue(dataRow, "col"));
        Assert.Null(target.GetDataValue(dataRow, 1));
        Assert.Null(target.GetDataValue(dataReader, "col"));
        Assert.Null(target.GetDataValue(dataReader, 1));
    }

    [Fact]
    public void GeneralJsonDbColToTypeConverter_WithNullTest()
    {
        var dt = CreateTestTable(typeof(string), TestData);
        var dataRow = GetDataRow(0, dt);
        var dataReader = GetDataReader(0, dt);

#pragma warning disable CS8603 // Possible null reference return. we are testing for this case
        var target = new GeneralJsonDbColToTypeConverter<IList<ColourValueDataType>>(
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }, () => default);
#pragma warning restore CS8603 // Possible null reference return.

        Assert.Null(target.GetDataValue(dataRow, "col"));
        Assert.Null(target.GetDataValue(dataRow, 1));
        Assert.Null(target.GetDataValue(dataReader, "col"));
        Assert.Null(target.GetDataValue(dataReader, 1));
    }

    [Fact]
    public void NullDbColToTypeConverterTestWithDataNull()
    {
        var dt = CreateTestTable(typeof(string), TestData);
        var dataRow = GetDataRow(1, dt);
        var dataReader = GetDataReader(1, dt);

        var target = new NullJsonDbColToTypeConverter<IList<ColourValueDataType>>(
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        Assert.Equal("green", target.GetDataValue(dataRow, "col")![1].Colour);
        Assert.Equal("green", target.GetDataValue(dataRow, 1)![1].Colour);
        Assert.Equal("green", target.GetDataValue(dataReader, "col")![1].Colour);
        Assert.Equal("green", target.GetDataValue(dataReader, 1)![1].Colour);
    }

    [Fact]
    public void DefaultJsonDbColToTypeConverter_BasicTest()
    {
        var dt = CreateTestTable(typeof(string), TestData);
        var dataRow = GetDataRow(1, dt);
        var dataReader = GetDataReader(1, dt);

        var target = new GeneralJsonDbColToTypeConverter<IReadOnlyList<ColourValueDataType>>(
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true },
            () => new List<ColourValueDataType>());

        var obj = target.GetDataValue(dataRow, 1);
        Assert.IsAssignableFrom<IReadOnlyList<ColourValueDataType>>(obj);

        Assert.Equal("green", target.GetDataValue(dataRow, "col")[1].Colour);
        Assert.Equal("green", target.GetDataValue(dataRow, 1)[1].Colour);
        Assert.Equal("green", target.GetDataValue(dataReader, "col")[1].Colour);
        Assert.Equal("green", target.GetDataValue(dataReader, 1)[1].Colour);
    }

    [Fact]
    public void DefaultJsonDbColToTypeConverter_ReturnReadOnlyInterfaceListTest()
    {
        var dt = CreateTestTable(typeof(string), TestData);
        var dataRow = GetDataRow(1, dt);
        var dataReader = GetDataReader(1, dt);

        var target = new GeneralJsonDbColToTypeConverter<IReadOnlyList<ColourValueDataType>>(
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true },
            () => new List<ColourValueDataType>());

        var obj = target.GetDataValue(dataRow, 1);
        Assert.IsAssignableFrom<IReadOnlyList<ColourValueDataType>>(obj);

        Assert.Equal("green", target.GetDataValue(dataRow, "col")[1].Colour);
        Assert.Equal("green", target.GetDataValue(dataRow, 1)[1].Colour);
        Assert.Equal("green", target.GetDataValue(dataReader, "col")[1].Colour);
        Assert.Equal("green", target.GetDataValue(dataReader, 1)[1].Colour);
    }

    [Fact]
    public void DefaultJsonDbColToTypeConverter_ReturnReadOnlyInterfaceListTest_EmptySource()
    {
        var dt = CreateTestTable(typeof(string), "");
        var dataRow = GetDataRow(1, dt);
        var dataReader = GetDataReader(1, dt);

        var target = new GeneralJsonDbColToTypeConverter<IReadOnlyList<ColourValueDataType>>(
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true },
            () => new List<ColourValueDataType>());

        var obj = target.GetDataValue(dataRow, 1);
        Assert.IsAssignableFrom<IReadOnlyList<ColourValueDataType>>(obj);
    }

    [Fact]
    public void DefaultJsonDbColToTypeConverter_ReturnReadOnlyInterfaceListTest_NullSource()
    {
        var dt = CreateTestTable(typeof(string), null);
        var dataRow = GetDataRow(1, dt);
        var dataReader = GetDataReader(1, dt);

        var target = new GeneralJsonDbColToTypeConverter<IReadOnlyList<ColourValueDataType>>(
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true },
            () => new List<ColourValueDataType>());

        var obj = target.GetDataValue(dataRow, 1);
        Assert.IsAssignableFrom<IReadOnlyList<ColourValueDataType>>(obj);
    }

    [Fact]
    public void DefaultJsonDbColToTypeConverter_ReturnReadOnlyInterfaceListTest_EmptyArraySource()
    {
        var dt = CreateTestTable(typeof(string), "[]");
        var dataRow = GetDataRow(1, dt);
        var dataReader = GetDataReader(1, dt);

        var target = new GeneralJsonDbColToTypeConverter<IReadOnlyList<ColourValueDataType>>(
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true },
            () => new List<ColourValueDataType>());

        var obj = target.GetDataValue(dataRow, 1);
        Assert.IsAssignableFrom<IReadOnlyList<ColourValueDataType>>(obj);
    }
}
