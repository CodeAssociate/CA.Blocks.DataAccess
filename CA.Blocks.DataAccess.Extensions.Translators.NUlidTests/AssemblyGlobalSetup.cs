// This will run exactly once when the entire test assembly starts up on  Linux & Windows & MacOS
using CA.Blocks.DataAccess.Extensions.Translators.NUlid.DbColToType.Converters;
using CA.Blocks.DataAccess.Extensions.Translators.NUlidTests;

[assembly: AssemblyFixture(typeof(AssemblyGlobalSetup))]

namespace CA.Blocks.DataAccess.Extensions.Translators.NUlidTests;

public class AssemblyGlobalSetup : IAsyncDisposable
{
    public AssemblyGlobalSetup()
    {
        // Place initialization logic here
        UlidDbColToTypeRegistration.Register();
    }

    public ValueTask DisposeAsync()
    {
        // Clean up global resources here
        return ValueTask.CompletedTask;
    }
}