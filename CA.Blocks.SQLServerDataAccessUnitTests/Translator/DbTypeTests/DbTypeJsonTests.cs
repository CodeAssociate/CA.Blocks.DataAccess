using System.Collections.Generic;
using System.Text.Json;
using CA.Blocks.DataAccess.Extensions.Translators.Json.Converters;
using CA.Blocks.DataAccess.Translator.DbColToType.Providers;
using CA.Blocks.DataAccess.Translator.Extensions;
using CA.Blocks.SQLServerDataAccessUnitTests.Base;

namespace CA.Blocks.SQLServerDataAccessUnitTests.Translator.DbTypeTests;

    [Collection("DbIntegrationTests")]
public class DbTypeJsonTests : UnitTestDataAccess, IDisposable
{
    private class ColourValueDataType
    {
        public string Color { get; set; }
        public string Value { get; set; }
    }

    private class JsonDataExample
    {
        public IList<ColourValueDataType> col { get; set; }
    }

    private void InsertTestDataAsText(string data)
    {
        ExecuteNonQuery(InsertTestDataSQL(string.Format("N'{0}'", data)));
    }

    public DbTypeJsonTests()
        {
        DefaultDbColToTypeProvider.DefaultInstance.Add(new NullJsonDbColToTypeConverter<IList<ColourValueDataType>>(new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        }));
        ExecuteNonQuery(DropTestTableSQL());
        ExecuteNonQuery(CreateTestTable("NVarChar(512) not null"));
        InsertTestDataAsText(@"
[
{""color"": ""red"",""value"": ""#f00""},
{""color"": ""green"",""value"": ""#0f0""}
]
");

        InsertTestDataAsText(@"
[
{""color"": ""red"",""value"": ""#f00""},
{""color"": ""green"",""value"": ""#0f0""},
{""color"": ""blue"",""value"": ""#00f""},
{""color"": ""cyan"",""value"": ""#0ff""},
{""color"": ""magenta"",""value"": ""#f0f""},
{""color"": ""yellow"",""value"": ""#ff0""},
{""color"": ""black"",""value"": ""#000""}
]
");
    }

    public void Dispose()
        {
        ExecuteNonQuery(DropTestTableSQL());
    }


    [Fact]
    public void SelectAllData()
    {
        //Setup 
        var cmd = CreateTextCommand(SelectTestDataSQL());

        //Act
        var result = Execute(cmd).ToListOf<JsonDataExample>();

        Assert.Equal(2, result.Count);
        Assert.Equal("black", result[1].col[6].Color );
    }
}



