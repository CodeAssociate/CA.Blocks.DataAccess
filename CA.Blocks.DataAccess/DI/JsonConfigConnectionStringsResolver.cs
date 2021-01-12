using Microsoft.Extensions.Configuration;

namespace CA.Blocks.DataAccess.DI
{
    /// <summary>
    /// Uses the JsonConfig  this is common on .NET core frameworks 
    /// </summary>
    public class JsonConfigConnectionStringsResolver : IDataAccessKeyToConnectionStringResolver
    {
        private readonly IConfiguration _configuration;

        public JsonConfigConnectionStringsResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetConnectionString(string connectionStringKey)
        {
            return _configuration.GetConnectionString(connectionStringKey);
        }
    }
}