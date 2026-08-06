using System;

namespace CA.Blocks.DataAccess.DI
{
    /// <summary>
    /// This is a useful block when working with frameworks that run from console, this will include most of the modern cloud
    /// serverless instances like Azure functions and AWS Lambda   
    /// </summary>
    /// <remarks>
    /// This Connection String Resolver method retrieves an environment variable from the environment block of the current process only
    /// This is the https://learn.microsoft.com/en-us/dotnet/api/system.environment.getenvironmentvariable
    /// </remarks>>
    public class EnvironmentVariableConnectionStringResolver : IDataAccessKeyToConnectionStringResolver
    {
        private string? _connectionString;

        public string GetConnectionString(string connectionStringKey)
        {
            _connectionString ??= Environment.GetEnvironmentVariable(connectionStringKey);
            return _connectionString ?? throw new InvalidOperationException($"The connection String for '{connectionStringKey}' is not set.");
        }
    }

    public class EnvironmentVariableDataAccessConfig(string connectionStringKey) : DataAccessConfig(
        new DataAccessConfigOptions { ConnectionStringKey = connectionStringKey },
        new EnvironmentVariableConnectionStringResolver());
}