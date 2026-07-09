using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;
using Aspire.Hosting.Testing;
using CA.Blocks.DataAccess.Extensions.Translators.NUlid.DbColToType.Converters;
using CA.Blocks.DataAccess.Extensions.Translators.NUlid.DbColToType.Providers;
using CA.Blocks.DataAccess.Translator.DbColToType.Providers;
using CA.Blocks.PostgreSQLDataAccessUnitTests.Base;
using CA.Blocks.PostgresSQLDataAccessTests.Base;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Sdk;
using Xunit.v3;

[assembly: TestPipelineStartup(typeof(TestPipelineSetup))]


namespace CA.Blocks.PostgresSQLDataAccessTests.Base
{
    public class TestPipelineSetup : ITestPipelineStartup

    {
        private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(60);
        private IDistributedApplicationTestingBuilder? _appHost;
        private DistributedApplication? _app;

        public CancellationToken CancellationToken { get; private set; }
        public string? ConnectionString { get; private set; }


        private async Task StartPostgresDb()
        {
            // Arrange
            var cts = new CancellationTokenSource(DefaultTimeout);
            CancellationToken = cts.Token;
            
            _appHost = await DistributedApplicationTestingBuilder.CreateAsync<Projects.CA_Blocks_PostgresAppHost>(CancellationToken);
            /* We dont need logging for these tests avoid the overhead
            _appHost.Services.AddLogging(logging =>
            {
                logging.SetMinimumLevel(LogLevel.Debug);
                // Override the logging filters from the app's configuration
                logging.AddFilter(_appHost.Environment.ApplicationName, LogLevel.Debug);
                logging.AddFilter("Aspire.", LogLevel.Debug);
                // To output logs to the xUnit.net ITestOutputHelper, consider adding a package from https://www.nuget.org/packages?q=xunit+logging
            });
            */
            _app = await _appHost.BuildAsync(CancellationToken).WaitAsync(DefaultTimeout, CancellationToken);

            await _app.StartAsync(CancellationToken).WaitAsync(DefaultTimeout, CancellationToken);

            var resourceNotificationService =
                _app.Services.GetRequiredService<ResourceNotificationService>();

          
            await resourceNotificationService
                .WaitForResourceAsync("postgres", KnownResourceStates.Running)
                .WaitAsync(DefaultTimeout);

            await resourceNotificationService
                .WaitForResourceAsync("ca-blocks-db", KnownResourceStates.Running)
                .WaitAsync(DefaultTimeout);

            // we need to wait for the DB to be in a Healthy state, ie running and ready for commands
            await resourceNotificationService.WaitForResourceHealthyAsync("ca-blocks-db")
                .WaitAsync(DefaultTimeout);
  
            // Get and set the connection string 
            var db = _appHost.Resources
              .OfType<PostgresDatabaseResource>()
              .Single(r => r.Name == "ca-blocks-db");
   
            var connectionString = await db.ConnectionStringExpression.GetValueAsync(CancellationToken.None);
            ConnectionString = $"{connectionString}" ?? throw new InvalidOperationException("Failed to get postgres connection string.");
            TestConnectionStrings.TestDataBaseConnectionString = ConnectionString;
        }


        public async ValueTask StartAsync(IMessageSink diagnosticMessageSink)
        {
            await StartPostgresDb();
            DefaultDbColToTypeProviderPostgresExtensions.AddPostgresArrayTypes();
            DefaultDbColToTypeProvider.DefaultInstance.TryAdd(new UlidDbColToTypeConverter());
            await Task.CompletedTask;
        }

        public async ValueTask StopAsync()
        {
            if (_app != null)
            {
                await _app.DisposeAsync();
            }

            await Task.CompletedTask;
        }
    }
}
