using CA.Blocks.DataAccess.Translator.Extensions;
using CA.Blocks.OdbcDataAccess.Specialized;
using System.Data.Odbc;
using Xunit;

namespace CA.Blocks.OdbcDataAccessUnitTests.Examples.TextFile
{
	public class Employee
	{
		public int EmployeeID { get; set; }
		public string? FirstName { get; set; }
		public string? LastName { get; set; }
		public string? EmailAddress { get; set; }
	}

	internal class MyTextFileDataAccess(string connectionString) : TextFileDataAccess(connectionString)
	{
		private string GetEmployeesSql(string filter = "")
		{
			return $"SELECT * FROM Employees.csv {filter}";
		}

		public IList<Employee> GetEmployees()
		{
			var cmd = CreateTextCommand(GetEmployeesSql());
			return Execute(cmd).ToListOf<Employee>();
		}

		public Employee? GetEmployee(string email)
		{
			var cmd = CreateTextCommand(GetEmployeesSql("Where EmailAddress = ?"));
			cmd.Parameters.Add(new OdbcParameter { Value = email, OdbcType = OdbcType.VarChar });
			return Execute(cmd).ToFirstOrDefault<Employee>();
		}
	}

	public class MyTextFileDataAccessTest
	{
		[Fact]
		public void SimpleGetEmployeesTest()
		{
			if (ODBC_Test_Helper.DriverExists("Microsoft Text Driver (*.txt; *.csv)"))
			{
				var sourcePath = TestFilePathResolver.ResolveTestFilePath("Examples\\TextFile");
				Console.WriteLine(sourcePath);
				var target = new MyTextFileDataAccess(sourcePath);
				var employeeList = target.GetEmployees();
				foreach (var employee in employeeList)
				{
					Console.WriteLine($"{employee.EmployeeID}, {employee.FirstName},{employee.LastName},{employee.EmailAddress}");
				}
			}
		}

		[Fact]
		public void SimpleGetEmployeeTest()
		{
			if (ODBC_Test_Helper.DriverExists("Microsoft Text Driver (*.txt; *.csv)"))
			{
				var sourcePath = TestFilePathResolver.ResolveTestFilePath("Examples\\TextFile");
				Console.WriteLine(sourcePath);
				var target = new MyTextFileDataAccess(sourcePath);
				var employee = target.GetEmployee("steven@northwindtraders.com");
				if (employee != default)
				{
					Console.WriteLine($"{employee.EmployeeID}, {employee.FirstName},{employee.LastName},{employee.EmailAddress}");
				}
			}
		}
	}
}
