using System.Data;
using System.Data.Common;
using CA.Blocks.DataAccess.DI;

namespace CA.Blocks.DataAccessUnitTests.DI
{
    public class EnvironmentVariableConnectionStringResolverTests : IDisposable
    {
        private static string TestEnvironmentVariable =
            "CA.Blocks.DataAccessUnitTests.DI.EnvironmentVariableConnectionStringResolverTests";

        private string TestEnvironmentVariableValue = Guid.NewGuid().ToString();

        public EnvironmentVariableConnectionStringResolverTests()
        {
            Environment.SetEnvironmentVariable(TestEnvironmentVariable, TestEnvironmentVariableValue);
        }

        public void Dispose()
        {
            Environment.SetEnvironmentVariable(TestEnvironmentVariable, null);
        }

        [Fact]
        public void EnvironmentVariableConnectionStringResolverTest()
        {
            var target = new EnvironmentVariableConnectionStringResolver();
            var result = target.GetConnectionString(TestEnvironmentVariable);

            Assert.Equal(TestEnvironmentVariableValue, result);
        }

        public class ConnectionStringDbTest : CA.Blocks.DataAccess.DataAccessCore
        {
            public ConnectionStringDbTest() : base(new EnvironmentVariableDataAccessConfig(TestEnvironmentVariable), null)
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
            var target = new ConnectionStringDbTest();
            var result = target.ExposeConnectionString();

            Assert.Equal(TestEnvironmentVariableValue, result);
        }
    }
}
