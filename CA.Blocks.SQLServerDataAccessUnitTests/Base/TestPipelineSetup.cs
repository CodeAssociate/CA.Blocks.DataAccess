using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;
using Aspire.Hosting.Testing;
using CA.Blocks.SQLServer.AppHost;
using CA.Blocks.SQLServerDataAccessUnitTests.Base;
using Microsoft.Testing.Platform.Services;
using Xunit.Sdk;
using Xunit.v3;

[assembly: TestPipelineStartup(typeof(TestPipelineSetup))]


namespace CA.Blocks.SQLServerDataAccessUnitTests.Base
{
    [CollectionDefinition("DbIntegrationTests")]
    public class SQLServerDbTypeTestsCollection : ICollectionFixture<TestPipelineSetup> { }

    public class TestPipelineSetup : ITestPipelineStartup, IAsyncLifetime

    {
        private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(60);
        private static IDistributedApplicationTestingBuilder? _appHost;
        private static DistributedApplication? _app;
        private static readonly SemaphoreSlim _lock = new(1, 1);
        private static bool _initialized = false;

        private CancellationToken CancellationToken { get; set; }
        private string? ConnectionString { get; set; }

        public async ValueTask InitializeAsync() => await EnsureInitializedAsync();

        public async ValueTask DisposeAsync() => await StopAsync();

        private async Task EnsureInitializedAsync()
        {
            var cts = new CancellationTokenSource(DefaultTimeout);
            CancellationToken = cts.Token;
            
            if (_initialized) return;
            await _lock.WaitAsync();
            try
            {
                if (_initialized) return;
                await StartDb();
                // you can use this for local db debugging 
                //await StartDbLocal();
                _initialized = true;
            }
            finally
            {
                _lock.Release();
            }
        }

        private async Task StartDbLocal()
        {
            TestConnectionStrings.TestDataBaseConnectionString = "Server=(local);Database=tempdb;Integrated Security=SSPI;TrustServerCertificate=True";
            await Task.Delay(1, CancellationToken);
        }

        private async Task StartDb()
        {
            _appHost = await DistributedApplicationTestingBuilder.CreateAsync<Projects.CA_Blocks_SQLServer_AppHost>(CancellationToken);
            _app = await _appHost.BuildAsync(CancellationToken).WaitAsync(DefaultTimeout, CancellationToken);

            await _app.StartAsync(CancellationToken).WaitAsync(DefaultTimeout, CancellationToken);

            var resourceNotificationService =
                _app.Services.GetRequiredService<ResourceNotificationService>();

          
            await resourceNotificationService
                .WaitForResourceAsync(ConfigSettings.SERVER_SERVICE_NAME, KnownResourceStates.Running, CancellationToken)
                .WaitAsync(DefaultTimeout, CancellationToken);

            await resourceNotificationService
                .WaitForResourceAsync(ConfigSettings.DB_NAME, KnownResourceStates.Running, CancellationToken)
                .WaitAsync(DefaultTimeout, CancellationToken);

            // we need to wait for the DB to be in a Healthy state, ie running and ready for commands
            await resourceNotificationService.WaitForResourceHealthyAsync(ConfigSettings.DB_NAME, CancellationToken)
                .WaitAsync(DefaultTimeout, CancellationToken);
  
            // Get and set the connection string 
            var db = _appHost.Resources
              .OfType<SqlServerDatabaseResource>()
              .Single(r => r.Name == ConfigSettings.DB_NAME);
   
            var connectionString = await db.ConnectionStringExpression.GetValueAsync(CancellationToken.None);
            ConnectionString = $"{connectionString}" ?? throw new InvalidOperationException("Failed to get mssql connection string.");
            TestConnectionStrings.TestDataBaseConnectionString = ConnectionString;
        }


        public async ValueTask StartAsync(IMessageSink diagnosticMessageSink)
        {
            await EnsureInitializedAsync();
        }

        public async ValueTask StopAsync()
        {
            await _lock.WaitAsync();
            try
            {
                if (_app != null)
                {
                    await _app.DisposeAsync();
                    _app = null;
                    _appHost = null;
                    _initialized = false;
                }
            }
            finally
            {
                _lock.Release();
            }
        }
    }
}
