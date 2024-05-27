using CA.Blocks.DataAccess.DI;

namespace CA.Blocks.OdbcDataAccess.Specialized
{


	public class TextFilePathConnectionStringResolver : IDataAccessKeyToConnectionStringResolver
	{
		public static string BuildConnectionString(string sourcePath)
		{
			return $"Driver={{Microsoft Text Driver (*.txt; *.csv)}};Dbq={sourcePath};Extensions=asc,csv,tab,txt;";
		}

		private readonly string _sourcePath;

		public TextFilePathConnectionStringResolver(string sourcePath)
		{
			_sourcePath = sourcePath;
		}

		public string GetConnectionString(string _)
		{
			return BuildConnectionString(_sourcePath);
		}
	}

	public class TextFileDataAccess : OdbcDataAccess
	{
		public TextFileDataAccess(string sourcePath) : base(
			new DirectConnectionStringDataAccessConfig(new TextFilePathConnectionStringResolver(sourcePath)))
		{

		}
	}
}