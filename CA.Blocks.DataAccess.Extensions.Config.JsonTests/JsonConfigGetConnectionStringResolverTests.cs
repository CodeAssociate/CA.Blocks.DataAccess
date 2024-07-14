using Microsoft.Extensions.Configuration;
using System.Text;
using CA.Blocks.DataAccess.Extensions.Config.Json;
using System.Data.Common;
using System.Data;

namespace CA.Blocks.DataAccess.Extensions.Config.JsonTests
{
  public class JsonConfigGetConnectionStringResolverTests
    {

        private string TestEnvironmentVariableValue = Guid.NewGuid().ToString();

        private static IConfiguration _testConfig;

        [SetUp]
        public void Setup()
        {

            var json = @"
  {
    ""ConnectionStrings"": {       
      ""JsonConfigGetConnectionStringResolverKEY"": ""TEST_CONNECTION_STRING""        
    } 
  }
";
            json = json.Replace("TEST_CONNECTION_STRING", TestEnvironmentVariableValue);
            _testConfig = new ConfigurationBuilder().AddJsonStream(new MemoryStream(Encoding.ASCII.GetBytes(json))).Build();
        }

        [Test]
        public void JsonConfigGetConnectionStringResolverTest()
        {

            var target = new JsonConfigGetConnectionStringResolver(_testConfig);
            var result = target.GetConnectionString("JsonConfigGetConnectionStringResolverKEY");

            Assert.That(result, Is.EqualTo(TestEnvironmentVariableValue));
        }


        public class ConnectionStringDbTest : CA.Blocks.DataAccess.DataAccessCore
        {
            public ConnectionStringDbTest() : base(new JsonConfigGetConnectionStringResolverDataAccessConfig(_testConfig, "JsonConfigGetConnectionStringResolverKEY"), null)
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
            protected override DbCommand CreateSqlCommand(string sql, CommandType cmdType = CommandType.Text)
            {
	            throw new NotImplementedException();
            }
		}

        [Test]
        public void EnvironmentVariableConnectionStringResolverTestWithProvider()
        {
            var target = new ConnectionStringDbTest();
            var result = target.ExposeConnectionString();

            Assert.That(result, Is.EqualTo(TestEnvironmentVariableValue));
        }
    }
}