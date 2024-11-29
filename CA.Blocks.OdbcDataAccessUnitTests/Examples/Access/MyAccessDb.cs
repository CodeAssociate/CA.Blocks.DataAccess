using System.Data.Odbc;
using CA.Blocks.DataAccess.Translator.Extensions;
using CA.Blocks.OdbcDataAccess.Specialized;

namespace CA.Blocks.OdbcDataAccessUnitTests.Examples.Access
{
	public class Employee
	{
		public int EmployeeID { get; set; }
		public string? FirstName { get; set; }
		public string? LastName { get; set; }
		public string? EmailAddress { get; set; }
	}

	public class MyAccessDb (string sourceFileName, string password = "") : AccessAccessDb(sourceFileName, password)
	{

		// This is private to prevent injection
		private string GetEmployeesSql(string filter = "")
		{

			return
				$"SELECT Employees.EmployeeID, Employees.LastName, Employees.FirstName, Employees.EmailAddress FROM Employees {filter};";
		}

		public IList<Employee> GetEmployees()
		{
			var cmd = CreateTextCommand(GetEmployeesSql());
			return Execute(cmd).ToListOf<Employee>();
		}

		
		public Employee? GetEmployee(string email)
		{
			var cmd = CreateTextCommand(GetEmployeesSql("Where EmailAddress = ?"));
			cmd.Parameters.Add(new OdbcParameter { Value = email, OdbcType = OdbcType.NVarChar });
			return Execute(cmd).ToFirstOrDefault<Employee>();
		}
	}


	public class AccessDbTest()
	{

		[Test]
		public void SimpleGetEmployeesTest()
		{
			if (ODBC_Test_Helper.DriverExists("Microsoft Access Driver (*.mdb, *.accdb)"))
			{
				var northWindDb = TestFilePathResolver.ResolveTestFilePath("Examples\\Access\\NorthWind.accdb");

				var target = new MyAccessDb(northWindDb);
				var employeeList = target.GetEmployees();
				foreach (var employee in employeeList)
				{
					TestContext.Out.WriteLine(
						$"{employee.EmployeeID}, {employee.FirstName},{employee.LastName},{employee.EmailAddress}");
				}
			}
			else
			{
				Assert.Inconclusive("Obdc Driver not installed");
			}
		}

		[Test]
		public void SimpleGetEmployeeTest()
		{
			if (ODBC_Test_Helper.DriverExists("Microsoft Access Driver (*.mdb, *.accdb)"))
			{
				var northWindDb = TestFilePathResolver.ResolveTestFilePath("Examples\\Access\\NorthWind.accdb");
				var target = new MyAccessDb(northWindDb);
				var employee = target.GetEmployee("steven@northwindtraders.com");
				if (employee != default)
				{
					TestContext.Out.WriteLine(
						$"{employee.EmployeeID}, {employee.FirstName},{employee.LastName},{employee.EmailAddress}");
				}
			}
			else
			{
				Assert.Inconclusive("Obdc Driver not installed");
			}

		}
	}
}
