using CA.Blocks.DataAccess.DI;

namespace CA.Blocks.OdbcDataAccess.Specialized
{
	public class AccessDbConnectionStringResolver : IDataAccessKeyToConnectionStringResolver
	{
		public static string BuildConnectionString(string sourceFile, string password = "")
		{
			return $"Driver={{Microsoft Access Driver (*.mdb, *.accdb)}};Dbq={sourceFile};Uid=Admin;Pwd={password}";
		}

		private readonly string _sourceFile;
		private readonly string _password;

		public AccessDbConnectionStringResolver(string sourceFile, string password = "")
		{
			_sourceFile = sourceFile;
			_password = password;
		}

		public string GetConnectionString(string _)
		{
			return BuildConnectionString(_sourceFile, _password);
		}
	}


	public class AccessAccessDb : OdbcDataAccess
	{
		public AccessAccessDb(string sourceFileName, string password) : base(new DirectConnectionStringDataAccessConfig( new AccessDbConnectionStringResolver(sourceFileName, password)))
		{

		}

	}
}
