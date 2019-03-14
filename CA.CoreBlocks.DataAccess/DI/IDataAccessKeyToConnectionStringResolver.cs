using System.Configuration;
using Microsoft.Extensions.Configuration;

namespace CA.Blocks.DataAccess.DI
{
    // the interface that do the lookup ie could be from app.config or appsettings.json or  other
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

    // App.Config provider using when the connection string is is app.config file
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
