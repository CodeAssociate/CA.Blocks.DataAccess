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
    public class JsonConfigGetConnectionStringResolver : IDataAccessKeyToConnectionStringResolver
    {
        private readonly IConfiguration _configuration;

        public JsonConfigGetConnectionStringResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetConnectionString(string connectionStringKey)
        {
            var connectionString = _configuration.GetConnectionString(connectionStringKey);
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException($"There is no Connection String setting for {connectionStringKey}",
                    nameof(connectionStringKey));
            }
            return connectionString;
        }
    }

    public class JsonConfigGetConnectionStringResolverDataAccessConfig : DataAccessConfig
    {
        public JsonConfigGetConnectionStringResolverDataAccessConfig(IConfiguration config, string connectionStringKey) :
            base(new DataAccessConfigOptions { ConnectionStringKey = connectionStringKey },
                new JsonConfigGetConnectionStringResolver(config))
        {

        }
    }
}
