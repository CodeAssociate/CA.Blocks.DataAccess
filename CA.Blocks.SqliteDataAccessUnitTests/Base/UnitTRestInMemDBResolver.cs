using System.IO;
using CA.Blocks.DataAccess.DI;

namespace CA.Blocks.SqliteDataAccessUnitTests.Base;

public class UnitTRestInMemDBResolver : IDataAccessKeyToConnectionStringResolver
{
    public string GetConnectionString(string connectionStringKey)
    {
        // we have to build a valid path to the base file name use Path to avoid Os issues
        var tempPath = Path.GetTempPath();
        var badFileName = Path.Combine(tempPath, "bad_path", "badfile.db"); 
        
        return connectionStringKey != "BAD_CONNECTION" ? 
            "Data Source=ca_blocks_unittest;mode=memory;cache=shared" 
            : $"Data Source={badFileName}"; // used to simulate connection errors 
    }
}