using CA.Blocks.DataAccess.Extensions.Translators.Json.Converters;
using CA.Blocks.DataAccess.Translator.DbColToType.Providers;
using CA.Blocks.DataAccess.Translator.Extensions;
using CA.Blocks.PostgreSQLDataAccess.Builder;
using CA.Blocks.PostgreSQLDataAccessUnitTests.Base;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using System.Text.Json;

namespace CA.Blocks.PostgreSQLDataAccessUnitTests.DbTypeTests;

[TestFixture]
public class DbTypeJsonbTests : UnitTestDataAccess
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
        var insertCmd = new SafeSqlBuilder($"Insert into {unitTestTableName:``}(col) values({data:@Data|json})")
        .BuildSqlCommand();
        ExecuteNonQuery(insertCmd);
    }

    [SetUp]
    public void Setup()
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

    [TearDown]
    public void TearDown()
    {
        ExecuteNonQuery(DropTestTableSQL());
    }


    [Test]
    public void SelectAllData()
    {
        //Setup 
        var cmd = CreateTextCommand(SelectTestDataSQL());

        //Act
        var result = Execute(cmd).ToListOf<JsonDataExample>();

        ClassicAssert.AreEqual(2, result.Count);
        ClassicAssert.AreEqual("black", result[1].col[6].Color );
    }

    // TODO Json patamenerr and jsonb
}