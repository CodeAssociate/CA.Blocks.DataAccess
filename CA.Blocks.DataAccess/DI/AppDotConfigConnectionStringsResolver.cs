using System.Configuration;

namespace CA.Blocks.DataAccess.DI
{
    /// <summary>
    /// Uses the App.Config ConnectionStrings, this is common on .NET 1-4.8 frameworks. It will be using the App.config or web.config ConnectionStrings setting
    /// </summary>
    public class AppDotConfigConnectionStringsResolver : IDataAccessKeyToConnectionStringResolver
    {
        /// <summary>
        /// Provides the mapping from the name in code to the name in the Connection string Stored in the app.config or web.config file
        /// </summary>
        /// <param name="connectionStringKey">The Connection string known to the code</param>
        /// <returns> The Connection string to be used by the ADO.NET provider.</returns>
        public string GetConnectionString(string connectionStringKey)
        {
            return ConfigurationManager.ConnectionStrings[connectionStringKey].ConnectionString;
        }
    }
}