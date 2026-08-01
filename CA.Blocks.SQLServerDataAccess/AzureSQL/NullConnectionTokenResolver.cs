namespace CA.Blocks.SQLServerDataAccess.AzureSQL
{
    /// <summary>
    /// provides a class for Null Token. Ie don't use tokens.   at this point this will be the same as  using the SqlServerDataAccess to access the Azure SQL database
    /// </summary>
    [System.Obsolete("Moved to CA.Blocks.DataAccess.DependencyInjection.NullConnectionTokenResolver can you use that directly")]
    public class NullConnectionTokenResolver : CA.Blocks.DataAccess.DependencyInjection.NullConnectionTokenResolver
    {
    }
}