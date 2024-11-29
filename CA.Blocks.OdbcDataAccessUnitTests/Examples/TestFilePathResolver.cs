using System.Runtime.InteropServices;
#pragma warning disable CA1416 // test done internally

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


    private static void TryClose(Microsoft.Win32.RegistryKey reg)
    {
        try
        {
            reg.Close();
        }
        catch
        {
            /* ignore this exception if we couldn't close */
        }
    }

    private static List<String> GetSystemDriverList()
	{
		if (_installedOdbcDrivers == null)
		{
			List<string> names = new List<string>();
			// get system dsn's
			Microsoft.Win32.RegistryKey? sreg = (Microsoft.Win32.Registry.LocalMachine).OpenSubKey("Software");
			if (sreg != null)
			{
				var oreg = sreg.OpenSubKey("ODBC");
				if (oreg != null)
				{
                    var oiReg = oreg.OpenSubKey("ODBCINST.INI");
					if (oiReg != null)
					{

                        var oidReg = oiReg.OpenSubKey("ODBC Drivers");
						if (oidReg != null)
                        {
                            // Get all DSN entries defined in DSN_LOC_IN_REGISTRY.
                            names.AddRange(oidReg.GetValueNames());
                            TryClose(oidReg);
                        }
                        TryClose(oiReg);
                    }
                    TryClose(oreg);
                }
                TryClose(sreg);
            }

			_installedOdbcDrivers = names;
		}

		return _installedOdbcDrivers;
	}

	public static bool DriverExists(string driverName)
	{
       
        var list = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? GetSystemDriverList() : new List<String>();
        
        return list.Any(x => x == driverName);
	}
}
