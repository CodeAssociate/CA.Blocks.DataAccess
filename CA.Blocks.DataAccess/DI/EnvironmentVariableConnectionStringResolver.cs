using System;

namespace CA.Blocks.DataAccess.DI
{
    /// <summary>
    /// This is a useful block when working with frameworks that run from console, this will include most of the modern based cloud
    /// serverles instances like Azure functions and AWS Lambda   
    /// </summary>
    /// <remarks>
    /// This Connection String Resolver method retrieves an environment variable from the environment block of the current process only
    /// This is the https://learn.microsoft.com/en-us/dotnet/api/system.environment.getenvironmentvariable
    /// </remarks>>
    public class EnvironmentVariableConnectionStringResolver : IDataAccessKeyToConnectionStringResolver
    {
        private readonly string _connectionString;

        public string GetConnectionString(string connectionStringKey)
        {
            return _connectionString ?? Environment.GetEnvironmentVariable(connectionStringKey);
        }
    }

    public class EnvironmentVariableDataAccessConfig : DataAccessConfig
    {
        public EnvironmentVariableDataAccessConfig(string connectionStringKey) :
            base(new DataAccessConfigOptions { ConnectionStringKey = connectionStringKey },
                new EnvironmentVariableConnectionStringResolver())
        {

        }
    }
}