using CA.Blocks.DataAccess.DI;

namespace CA.Blocks.SqliteDataAccessUnitTests.Base;

public class UnitTRestInMemDBResolver : IDataAccessKeyToConnectionStringResolver
{
    public string GetConnectionString(string connectionStringKey)
    {
        return connectionStringKey != "BAD_CONNECTION" ? 
            "Data Source=ca_blocks_unittest;mode=memory;cache=shared" 
            : "Data Source=C\\BadPath\\badfile.db"; // used to simulate connection errors 
    }
}