using CA.Blocks.DataAccess.DI;
using Microsoft.Extensions.Configuration;

namespace CA.Blocks.DataAccess.Extensions.Config.Json
{

    /// <summary>
    /// This will get the Connection String from Json where the configuration is the ConnectionStrings Section
    ///   {
    ///    "ConnectionStrings": {       
    ///             "connectionStringKey": "...Connection String Value ...."} 
    ///   }
    /// </summary>
    public class JsonConfigGetConnectionStringResolver(IConfiguration configuration)
        : IDataAccessKeyToConnectionStringResolver
    {
        public string GetConnectionString(string connectionStringKey)
        {
            var connectionString = configuration.GetConnectionString(connectionStringKey);
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException($"There is no Connection String setting for {connectionStringKey}",
                    nameof(connectionStringKey));
            }
            return connectionString;
        }
    }

    public class JsonConfigGetConnectionStringResolverDataAccessConfig(
        IConfiguration config,
        string connectionStringKey) : DataAccessConfig(
        new DataAccessConfigOptions { ConnectionStringKey = connectionStringKey },
        new JsonConfigGetConnectionStringResolver(config));
}
