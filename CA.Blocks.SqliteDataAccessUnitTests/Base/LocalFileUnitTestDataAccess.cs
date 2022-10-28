using CA.Blocks.DataAccess.DI;

namespace CA.Blocks.SqliteDataAccessUnitTests.Base;

public class LocalFileUnitTestDataAccess : SqliteDataAccess.SqliteDataAccess
{
    public LocalFileUnitTestDataAccess(): base(new SimpleConnectionStringDataAccessConfig("Data Source=.\\cablockstest.db"))
    {
    }



}