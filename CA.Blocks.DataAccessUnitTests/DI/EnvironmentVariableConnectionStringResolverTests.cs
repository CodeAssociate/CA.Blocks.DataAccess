using System.Data;
using System.Data.Common;
using CA.Blocks.DataAccess.DI;

namespace CA.Blocks.DataAccessUnitTests.DI
{
    [TestFixture]
    public class EnvironmentVariableConnectionStringResolverTests
    {
        private static string TestEnvironmentVariable =
            "CA.Blocks.DataAccessUnitTests.DI.EnvironmentVariableConnectionStringResolverTests";

        private string TestEnvironmentVariableValue = Guid.NewGuid().ToString();

        [SetUp]
        public void SetUpFixture()
        {
            Environment.SetEnvironmentVariable(TestEnvironmentVariable, TestEnvironmentVariableValue);
        }

        [TearDown]
        public void TearDownFixture()
        {
            Environment.SetEnvironmentVariable(TestEnvironmentVariable,null);
        }

        [Test]
        public void EnvironmentVariableConnectionStringResolverTest()
        {
            var target = new EnvironmentVariableConnectionStringResolver();
            var result = target.GetConnectionString(TestEnvironmentVariable);

            Assert.That(result, Is.EqualTo(TestEnvironmentVariableValue));
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

        [Test]
        public void EnvironmentVariableConnectionStringResolverTestWithProvider()
        {
            var target = new ConnectionStringDbTest();
            var result = target.ExposeConnectionString();

            Assert.That(result, Is.EqualTo(TestEnvironmentVariableValue));
        }
    }
}
