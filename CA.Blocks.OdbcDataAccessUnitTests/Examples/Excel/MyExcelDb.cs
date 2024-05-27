using CA.Blocks.OdbcDataAccess.Specialized;
using System.Data.Odbc;
using CA.Blocks.DataAccess.Translator.Extensions;

namespace CA.Blocks.OdbcDataAccessUnitTests.Examples.Excel
{
	public class Employee
	{
		public int EmployeeID { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }

		public string EmailAddress { get; set; }
	}

	internal class MyExcelDb(string connectionString) : ExcelDataAccess(connectionString)
	{
		private string GetEmployeesSql(string filter = "")
		{

			return
				$"SELECT * FROM [Employees$] {filter}";
		}

		public IList<Employee> GetEmployees()
		{
			var cmd = CreateTextCommand(GetEmployeesSql());
			return Execute(cmd).ToListOf<Employee>();
		}


		public Employee? GetEmployee(string email)
		{
			var cmd = CreateTextCommand(GetEmployeesSql($"Where EmailAddress = ?"));
			cmd.Parameters.Add(new OdbcParameter { Value = email, OdbcType = OdbcType.VarChar });
			return Execute(cmd).ToFirstOrDefault<Employee>();
		}
	}

	public class ExcelDataAccessTest()
	{
		[Test]
		public void SimpleGetEmployeesTest()
		{

			if (ODBC_Test_Helper.DriverExists("Microsoft Excel Driver (*.xls, *.xlsx, *.xlsm, *.xlsb)"))
			{
				var employeesXlsxs = TestFilePathResolver.ResolveTestFilePath("Examples\\Excel\\Employees.xlsx");

				var target = new MyExcelDb(employeesXlsxs);
				var employeeList = target.GetEmployees();
				foreach (var employee in employeeList)
				{
					TestContext.WriteLine(
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
			if (ODBC_Test_Helper.DriverExists("Microsoft Excel Driver (*.xls, *.xlsx, *.xlsm, *.xlsb)"))
			{

				var employeesXlsxs = TestFilePathResolver.ResolveTestFilePath("Examples\\Excel\\Employees.xlsx");


				var target = new MyExcelDb(employeesXlsxs);
				var employee = target.GetEmployee("steven@northwindtraders.com");
				if (employee != default)
				{
					TestContext.WriteLine(
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
