using CA.Blocks.DataAccess.DI;

namespace CA.Blocks.SqliteDataAccessUnitTests.Base;

public class UnitTestBadConnection() : SqliteDataAccess.SqliteDataAccess(new DataAccessConfig(
        new DataAccessConfigOptions { TraceExceptions = true, ConnectionStringKey = "BAD_CONNECTION" },
        new UnitTRestInMemDBResolver()),
    null);