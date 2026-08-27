using System.Runtime.CompilerServices;
using CA.Blocks.DataAccess.Translator.DbColToType.Providers;

namespace CA.Blocks.DataAccess.Extensions.Translators.NUlid.DbColToType.Converters
{
    public static class UlidDbColToTypeRegistration
    {
        public static void Register()
        {
            DefaultDbColToTypeProvider.DefaultInstance.TryAdd(new UlidDbColToTypeConverter());
            DefaultDbColToTypeProvider.DefaultInstance.TryAdd(new NullUlidDbColToTypeConverter());
        }

#pragma warning disable CA2255
        [ModuleInitializer]
        internal static void Initialize()
        {
            Register();
        }
#pragma warning restore CA2255
    }
}
