using System.Text.Json;
using CA.Blocks.DataAccess.Extensions.Translators.Json.Converters;
using CA.Blocks.DataAccess.Translator.DbColToType.Providers;
using CA.Blocks.DataAccess.Translator.Extensions;
using CA.Blocks.PostgresDataAccess.Builder;
using CA.Blocks.PostgresDataAccessTests.Base;

namespace CA.Blocks.PostgresDataAccessTests.Postgres.DbTypeTests;

[Collection("DbIntegrationTests")]
public class DbTypeJsonbTests : UnitTestDataAccess, IDisposable
{
    private class ColourValueDataType
    {
        public required string Color { get; init; }
        public required string Value { get; init; }
    }

    private class JsonDataExample
    {
        public required IList<ColourValueDataType> col { get; set; }
    }

    private void InsertTestDataAsText(string data)
    {
        var insertCmd = new SafeSqlBuilder($"Insert into {unitTestTableName:``}(col) values({data:@Data|json})")
        .BuildSqlCommand();
        ExecuteNonQuery(insertCmd);
    }

    public DbTypeJsonbTests()
    {
        DefaultDbColToTypeProvider.DefaultInstance.Add(new NullJsonDbColToTypeConverter<IList<ColourValueDataType>>(new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        }));
        ExecuteNonQuery(DropTestTableSQL());
        ExecuteNonQuery(CreateTestTable("jsonb not null"));



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

    public new void Dispose()
    {
        ExecuteNonQuery(DropTestTableSQL());
        base.Dispose();
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

    // TODO Json patamenerr and jsonb
}
