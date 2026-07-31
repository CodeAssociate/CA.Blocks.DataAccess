using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;
using Aspire.Hosting.Testing;
using CA.Blocks.DataAccess.Extensions.Translators.NUlid.DbColToType.Converters;
using CA.Blocks.DataAccess.Translator.DbColToType.Providers;
using CA.Blocks.PostgresDataAccess.Translator.DbColToType.Providers;
using CA.Blocks.PostgresDataAccessTests.Base;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Sdk;
using Xunit.v3;

[assembly: TestPipelineStartup(typeof(TestPipelineSetup))]


namespace CA.Blocks.PostgresDataAccessTests.Base
{
    [CollectionDefinition("DbIntegrationTests")]
    public class DbTypeTestsCollection : ICollectionFixture<TestPipelineSetup> { }

    public class TestPipelineSetup : ITestPipelineStartup, IAsyncLifetime

    {
        private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(90);
        private static IDistributedApplicationTestingBuilder? _appHost;
        private static DistributedApplication? _app;
        private static readonly SemaphoreSlim _lock = new(1, 1);
        private static bool _initialized = false;

        public CancellationToken CancellationToken { get; private set; }
        public string? ConnectionString { get; private set; }

        public async ValueTask InitializeAsync() => await EnsureInitializedAsync();

        public async ValueTask DisposeAsync() => await StopAsync();

        private async Task EnsureInitializedAsync()
        {
            if (_initialized) return;
            await _lock.WaitAsync();
            try
            {
                if (_initialized) return;
                await StartPostgresDb();
                DefaultDbColToTypeProviderPostgresExtensions.AddPostgresArrayTypes();
                DefaultDbColToTypeProvider.DefaultInstance.TryAdd(new UlidDbColToTypeConverter());
                _initialized = true;
            }
            finally
            {
                _lock.Release();
            }
        }


        private async Task StartPostgresDb()
        {
            // Arrange
            var cts = new CancellationTokenSource(DefaultTimeout);
            CancellationToken = cts.Token;
            
            _appHost = await DistributedApplicationTestingBuilder.CreateAsync<Projects.CA_Blocks_PostgresAppHost>(CancellationToken);
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
