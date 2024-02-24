using CA.Blocks.DataAccess.DI;
using CA.Blocks.SQLServerDataAccess;
using CA.Blocks.SQLServerDataAccessUnitTests.Base;
using NUnit.Framework;

namespace CA.Blocks.SQLServerDataAccessUnitTests.SQLServer;

[TestFixture]
public class SqlServerDataAccessUsingExistingConnectionTests : SqlServerDataAccess
{
	public SqlServerDataAccessUsingExistingConnectionTests()
		: base(new DataAccessConfig(
			new DataAccessConfigOptions { ConnectionStringKey = "localsqlserverhost" },
			new LocalSqlServerUnitTestStringsResolver()))
	{

	}

	[Test]
	public void ExecuteExecuteScalarByte()
	{
		using (var conn = CreateSqlConnection())
		{
			// Setup
			var cmd = CreateTextCommand("Select @@SPID");
			cmd.Connection = conn;
			// act
			var spid = ExecuteScalarAs<short>(cmd);
			TestContext.WriteLine(spid);
			
			
			var cmd2 = CreateTextCommand("Select @@SPID");
			// here we execute a command but this command wil be on a managed connection 
			// as we have the connection open it will get a second connection to get the data
			// by the time we get the newSpid the connection is wrapped up and back on the pool
			// but it must not use the original  connection
			var newSpid = ExecuteScalarAs<short>(cmd2);
			TestContext.WriteLine(newSpid);
			Assert.That(spid != newSpid);

			// now execute a command using the original  connection
			var cmd3 = CreateTextCommand("Select @@SPID");
			cmd3.Connection = conn;
			// act
			var ensureSameSpid = ExecuteScalarAs<short>(cmd3);

			TestContext.WriteLine(ensureSameSpid);
			Assert.That(spid == ensureSameSpid);


			conn.Close(); // free use to be put back on the pool.
		}

	}

}