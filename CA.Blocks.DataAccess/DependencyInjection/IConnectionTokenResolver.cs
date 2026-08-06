namespace CA.Blocks.DataAccess.DependencyInjection
{

    /// <summary>
    /// Used when connecting to a database that can use a token for Authentication    
    /// </summary>
    public interface IConnectionTokenResolver
    {
        /// <summary>
        /// Will Provide a Token that can be used for the given Connection String
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns> A Token that can we used on a DBConnection </returns>
        string? GetConnectionToken(string connectionString);
    }
    
    /// <summary>
    /// Provides a class for Null Token. Ie don't use tokens. Used for default DependencyInjection
    /// </summary>
    public class NullConnectionTokenResolver : IConnectionTokenResolver
    {
        public string? GetConnectionToken(string connectionString)
        {
            return null;
        }
    }
}