using Microsoft.Extensions.Configuration;

namespace CA.Blocks.DataAccess.DI
{
    /// <summary>
    /// Uses the JsonConfig  this is common on .NET core frameworks 
    /// </summary>
    ///
    [System.Obsolete("We are going remove class to reduce the number of dependencies taken on the CA.Blocks.DataAccess. The Configuration used is a Hosting app concern. See https://www.codeassociate.com/Blocks/DataAccess/Samples/Connection/UsingJsonConfigConnectionStringsResolver.html for a fix")]
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