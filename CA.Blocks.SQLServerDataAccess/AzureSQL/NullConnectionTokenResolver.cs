namespace CA.Blocks.SQLServerDataAccess.AzureSQL
{
    /// <summary>
    /// provides a class for Null Token. Ie don't use tokens.   at this point this will be the same as  using the SqlServerDataAccess to access the Azure SQL database
    /// </summary>
    public class NullConnectionTokenResolver : IConnectionTokenResolver
    {

        public string GetConnectionToken(string connectionString)
        {
            return null;
        }
    }
}