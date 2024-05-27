using CA.Blocks.DataAccess.DI;

namespace CA.Blocks.OdbcDataAccess.Specialized
{

	public class ExcelDbConnectionStringResolver : IDataAccessKeyToConnectionStringResolver
	{
		public static string BuildConnectionString(string sourceFile)
		{
			return $"Driver={{Microsoft Excel Driver (*.xls, *.xlsx, *.xlsm, *.xlsb)}};DBQ={sourceFile};";
		}

		private readonly string _sourceFile;

		public ExcelDbConnectionStringResolver(string sourceFile)
		{
			_sourceFile = sourceFile;
		}

		public string GetConnectionString(string _)
		{
			return BuildConnectionString(_sourceFile);
		}
	}


	public class ExcelDataAccess : OdbcDataAccess
	{
		public ExcelDataAccess(string sourceFile) : base(
			new DirectConnectionStringDataAccessConfig(new ExcelDbConnectionStringResolver(sourceFile)))
		{

		}

	}
}