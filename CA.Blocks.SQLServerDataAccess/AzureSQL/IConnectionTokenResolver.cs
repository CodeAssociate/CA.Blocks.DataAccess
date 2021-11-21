namespace CA.Blocks.SQLServerDataAccess.AzureSQL
{

    /// <summary>
    /// When connecting to a Azure Database we have the option of using a Token for Auth   
    /// </summary>
    public interface IConnectionTokenResolver
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        string GetConnectionToken(string connectionString);
    }
}
