using CA.Blocks.DataAccess.DI;

namespace CA.Blocks.SqliteDataAccessUnitTests.Base;

public class UnitTestBadConnection : SqliteDataAccess.SqliteDataAccess
{
    public UnitTestBadConnection() : 
        base ( new DataAccessConfig( "BAD_CONNECTION", 
                new DataAccessConfigOptions{TraceExceptions = true, ConnectionStringKey = "BAD_CONNECTION"}, 
                new UnitTRestInMemDBResolver() ), 
            null)
    {

    } 
}