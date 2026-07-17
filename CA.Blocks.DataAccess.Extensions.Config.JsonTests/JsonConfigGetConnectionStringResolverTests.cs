using CA.Blocks.DataAccess.Extensions.Config.Json;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.Common;
using System.Text;

namespace CA.Blocks.DataAccess.Extensions.Config.JsonTests;

public class JsonConfigGetConnectionStringResolverTests
{
    private readonly string _testEnvironmentVariableValue = Guid.NewGuid().ToString();
    private readonly IConfiguration _testConfig;

    public JsonConfigGetConnectionStringResolverTests()
    {
        var json = @"
{
  ""ConnectionStrings"": {
    ""JsonConfigGetConnectionStringResolverKEY"": ""TEST_CONNECTION_STRING""
  }
}
";

        json = json.Replace("TEST_CONNECTION_STRING", _testEnvironmentVariableValue);
        _testConfig = new ConfigurationBuilder()
            .AddJsonStream(new MemoryStream(Encoding.ASCII.GetBytes(json)))
            .Build();
    }

    [Fact]
    public void JsonConfigGetConnectionStringResolverTest()
    {
        var target = new JsonConfigGetConnectionStringResolver(_testConfig);
        var result = target.GetConnectionString("JsonConfigGetConnectionStringResolverKEY");

        Assert.Equal(_testEnvironmentVariableValue, result);
    }

    private sealed class ConnectionStringDbTest : DataAccessCore
    {
        public ConnectionStringDbTest(IConfiguration configuration)
            : base(new JsonConfigGetConnectionStringResolverDataAccessConfig(configuration, "JsonConfigGetConnectionStringResolverKEY"), null)
        {
        }

        public string ExposeConnectionString()
        {
            return this.ConnectionString;
        }

        protected override DbDataAdapter GetDataAdapter(IDbCommand cmd)
        {
            throw new NotImplementedException();
        }

        protected override bool PrepCommand(IDbCommand cmd)
        {
            throw new NotImplementedException();
        }

        protected override bool IsTransientError(DbException dbEx)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand CreateDbCommand(string sql, CommandType cmdType = CommandType.Text)
        {
            throw new NotImplementedException();
        }
    }

    [Fact]
    public void EnvironmentVariableConnectionStringResolverTestWithProvider()
    {
        var target = new ConnectionStringDbTest(_testConfig);
        var result = target.ExposeConnectionString();

        Assert.Equal(_testEnvironmentVariableValue, result);
    }
}
