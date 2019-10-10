using System.Configuration;
using Microsoft.Extensions.Configuration;

namespace CA.Blocks.DataAccess.DI
{
    // the interface that does the lookup ie could be from app.config or appsettings.json or whatever takes you fancy
    public interface IDataAccessKeyToConnectionStringResolver
    {
        string GetConnectionString(string connectionStringKey);
    }
    
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
        public IConfiguration Configuration { get; }

        public JsonConfigConnectionStringsResolver(IConfiguration configuration)
        {
            Configuration = configuration;

        }

        public string GetConnectionString(string connectionStringKey)
        {
            return Configuration.GetConnectionString(connectionStringKey);
        }
    }
}
