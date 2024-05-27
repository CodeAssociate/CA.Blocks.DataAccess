namespace CA.Blocks.OdbcDataAccessUnitTests.Examples;

public static class TestFilePathResolver
{
    public static string ResolveTestFilePath(string relativePath)
    {
        var baseDir = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
        return !string.IsNullOrWhiteSpace(baseDir) ? Path.Combine(baseDir, relativePath) : "";
    }
}

public static class ODBC_Test_Helper
{
	private static List<string>? _installedOdbcDrivers = null;


	public static List<String> GetSystemDriverList()
	{
		if (_installedOdbcDrivers == null)
		{
			List<string> names = new List<string>();
			// get system dsn's
			Microsoft.Win32.RegistryKey? reg = (Microsoft.Win32.Registry.LocalMachine).OpenSubKey("Software");
			if (reg != null)
			{
				reg = reg.OpenSubKey("ODBC");
				if (reg != null)
				{
					reg = reg.OpenSubKey("ODBCINST.INI");
					if (reg != null)
					{

						reg = reg.OpenSubKey("ODBC Drivers");
						if (reg != null)
						{
							// Get all DSN entries defined in DSN_LOC_IN_REGISTRY.
							foreach (string sName in reg.GetValueNames())
							{
								names.Add(sName);
							}
						}

						try
						{
							reg.Close();
						}
						catch
						{
							/* ignore this exception if we couldn't close */
						}
					}
				}
			}

			_installedOdbcDrivers = names;
		}

		return _installedOdbcDrivers;
	}

	public static bool DriverExists(string driverName)
	{
		var list = GetSystemDriverList();
		return list.Any(x => x == driverName);
	}
}
