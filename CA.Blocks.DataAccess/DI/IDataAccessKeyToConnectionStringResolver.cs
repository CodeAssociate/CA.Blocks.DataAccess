using System.Configuration;
using Microsoft.Extensions.Configuration;

namespace CA.Blocks.DataAccess.DI
{
    // the interface that does the lookup ie could be from app.config or appsettings.json or whatever takes you fancy
    public interface IDataAccessKeyToConnectionStringResolver
    {
        string GetConnectionString(string connectionStringKey);
    }

    /* below are two simple and common config providers.. but the most likely is the client implementing the IDataAccessKeyToConnectionStringResolver then they can get the connection string from the apps provider */
    //public class ExampleConnectionStringResolver : IDataAccessKeyToConnectionStringResolver
    //{
    //    public string GetConnectionString(string connectionStringKey)
    //    {
    //         return /*place your code here to get the connectionString*/ "";
    //    }
    //}
    
    // App.Config provider using when the connection string is is app.config file
    public class AppDotConfigConfigConnectionStringsResolver : IDataAccessKeyToConnectionStringResolver
    {
        public string GetConnectionString(string connectionStringKey)
        {
            return ConfigurationManager.ConnectionStrings[connectionStringKey].ConnectionString;
        }
    }

    // App.Config provider using when the connection string is in appsettings.json file
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
