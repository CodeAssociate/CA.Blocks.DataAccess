using CA.Blocks.DataAccess.Translator.DbColToType.Providers;
using CA.Blocks.PostgresDataAccess.Translator.DbColToType.Converters;

namespace CA.Blocks.PostgresDataAccess.Translator.DbColToType.Providers
{
    public static class DefaultDbColToTypeProviderPostgresExtensions
    {
  
        public static void AddPostgresArrayTypes()
        {
            // Short
            DefaultDbColToTypeProvider.DefaultInstance.TryAdd(new PostgresArrayDbColToTypeConverter<short[]>());
            DefaultDbColToTypeProvider.DefaultInstance.TryAdd(new PostgresArrayDbColToTypeConverter<List<short>>());
            //Int 
            DefaultDbColToTypeProvider.DefaultInstance.TryAdd(new PostgresArrayDbColToTypeConverter<int[]>());
            DefaultDbColToTypeProvider.DefaultInstance.TryAdd(new PostgresArrayDbColToTypeConverter<List<int>>());
            // Long
            DefaultDbColToTypeProvider.DefaultInstance.TryAdd(new PostgresArrayDbColToTypeConverter<long[]>());
            DefaultDbColToTypeProvider.DefaultInstance.TryAdd(new PostgresArrayDbColToTypeConverter<List<long>>());

            // real
            DefaultDbColToTypeProvider.DefaultInstance.TryAdd(new PostgresArrayDbColToTypeConverter<float[]>());
            DefaultDbColToTypeProvider.DefaultInstance.TryAdd(new PostgresArrayDbColToTypeConverter<List<float>>());
            // double
            DefaultDbColToTypeProvider.DefaultInstance.TryAdd(new PostgresArrayDbColToTypeConverter<double[]>());
            DefaultDbColToTypeProvider.DefaultInstance.TryAdd(new PostgresArrayDbColToTypeConverter<List<double>>());
            // Decimal
            DefaultDbColToTypeProvider.DefaultInstance.TryAdd(new PostgresArrayDbColToTypeConverter<decimal[]>());
            DefaultDbColToTypeProvider.DefaultInstance.TryAdd(new PostgresArrayDbColToTypeConverter<List<decimal>>());
        }
    }
}
